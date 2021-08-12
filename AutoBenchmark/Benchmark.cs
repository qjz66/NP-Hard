using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;


namespace AutoBenchmark {
    delegate double NormalizeObj(double obj);
    delegate void SaveOutput(string output, double obj);


    public class Benchmark {
        static Queue<Submission> q = new Queue<Submission>();
        static Dictionary<string, int> emailNums = new Dictionary<string, int>();
        static Dictionary<string, int> authorNums = new Dictionary<string, int>();


        public static void run() {
            try {
                foreach (var problem in BenchmarkCfg.rank.problems) {
                    Directory.CreateDirectory(Path.Combine(problem.Key, CommonCfg.SolutionSubDir));
                }
            } catch (Exception) { }

            for (; ; Thread.Sleep(CommonCfg.PollIntervalInMillisecond)) {
                while (testSubmission(pop())) { }
            }
        }

        public static void push(Submission s) {
            q.Enqueue(s);
            Util.tryInc(emailNums, s.email);
            Util.tryInc(authorNums, s.author);
        }

        public static int queueSize {
            get { return q.Count; }
        }

        static Submission pop() {
            if (!EmailFetcher.fetch()) { return null; }
            Submission s;
            do {
                s = q.Dequeue();
                Util.tryDec(emailNums, s.email);
                Util.tryDec(authorNums, s.author);
            } while (emailNums.ContainsKey(s.email) || authorNums.ContainsKey(s.author));
            return s;
        }


        static bool testSubmission(Submission s) {
            if (s == null) { return false; }

            Problem problem = BenchmarkCfg.rank.problems[s.problem];
            Check check = BenchmarkCfg.Checkers[s.problem];

            StringBuilder reply = new StringBuilder();
            reply.AppendLine(BenchmarkCfg.LogBasicHeader + BenchmarkCfg.LogHeaders[s.problem].Substring(BenchmarkCfg.LogCommonHeader.Length));

            string logPath = Path.Combine(s.problem, CommonCfg.LogFilePrefix + s.date.Substring(0, 4) + CommonCfg.LogFileExt);
            foreach (var dataset in problem.datasets) {
                int feasibleCount = 0;
                int optCount = 0;
                int timeoutCount = 0;
                Util.scrambleForTasks(BenchmarkCfg.ParallelBenchmarkNum, (isTaskTaken) => {
                    foreach (var instance in dataset.instances) {
                        if (isTaskTaken()) { continue; }

                        Instance i = instance.Value;
                        string inputPath = Path.Combine(s.problem, CommonCfg.InstanceSubDir, instance.Key);
                        if (i.data == null) { i.data = File.ReadAllLines(inputPath); }

                        List<Statistic> statistics = testInstance(s.exePath, i, check, (output, obj) => {
                            if (!i.isNewRecord(obj)) { return; }
                            string slnPath = Path.Combine(s.problem, CommonCfg.SolutionSubDir, instance.Key + obj);
                            File.WriteAllText(slnPath, output); // save the solution if the record is refreshed.
                        }, obj => problem.normalizeObj(obj));

                        List<string> lines = new List<string>(statistics.Count);
                        foreach (var line in statistics) {
                            if (line.obj < Problem.MaxObjValue) { Interlocked.Increment(ref feasibleCount); }
                            if (i.matchRecord(line.obj)) { Interlocked.Increment(ref optCount); }
                            if (line.duration > i.secTimeout) { Interlocked.Increment(ref timeoutCount); }
                            lines.Add(s.author.ToString() + BenchmarkCfg.LogDelim + line.seed.ToString() + BenchmarkCfg.LogDelim
                                + instance.Key + BenchmarkCfg.LogDelim + line.obj + BenchmarkCfg.LogDelim
                                + line.duration.ToString() + BenchmarkCfg.LogDelim + line.info);
                        }
                        lock (logPath) {
                            if (!File.Exists(logPath)) { Util.appendLine(logPath, BenchmarkCfg.LogHeaders[s.problem]); }
                            Util.appendLines(logPath, lines);
                            foreach (var line in statistics) {
                                reply.AppendLine(instance.Key + BenchmarkCfg.LogDelim + line.obj + BenchmarkCfg.LogDelim
                                    + line.duration.ToString() + BenchmarkCfg.LogDelim + line.info);
                            }
                        }

                        Result bestResult = new Result { obj = Problem.MaxObjValue, author = s.author, date = s.date };
                        foreach (var statistic in statistics) {
                            if (statistic.obj >= bestResult.obj) { continue; }
                            bestResult.obj = statistic.obj;
                            bestResult.duration = statistic.duration;
                        }
                        i.results.Add(bestResult);

                        if (i.results.Count <= CommonCfg.MaxResultsCountPerInstance) { continue; }
                        i.results.Remove(i.results.Max); // drop the worst one if the limit is exceeded.
                    }
                });
                // stop testing next dataset if the results are poor.
                if (feasibleCount < (int)(dataset.instances.Count * dataset.minFeasibleRate)) { break; }
                if (optCount < (int)(dataset.instances.Count * dataset.minOptRate)) { break; }
                if (timeoutCount > (int)(dataset.instances.Count * dataset.maxTimeoutRate)) { break; }
            }

            StdSmtp.send(s.email, "Statistics of " + s.exePath, reply.ToString());

            Util.Json.save(CommonCfg.RankPath, BenchmarkCfg.rank);
            PageGenerator.generateMarkdown(BenchmarkCfg.rank);
            return true;
        }

        static List<Statistic> testInstance(string exePath, Instance instance, Check check, SaveOutput saveOutput, NormalizeObj normalizeObj) {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = exePath;
            psi.WorkingDirectory = Environment.CurrentDirectory;
            psi.UseShellExecute = false;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            int seed = 0;
            long msTimeout = instance.secTimeout * 1000;
            List<Statistic> statistics = new List<Statistic>(instance.repeat);
            for (int i = 0; i < instance.repeat; ++i) {
                Statistic statistic = new Statistic();
                statistic.seed = (seed = nextSeed(seed));
                psi.Arguments = instance.secTimeout.ToString() + " " + statistic.seed.ToString();

                Stopwatch sw = new Stopwatch();
                StringBuilder output = new StringBuilder();

                Process p = new Process();
                p.StartInfo = psi;
                p.ErrorDataReceived += (object sendingProcess, DataReceivedEventArgs line) => { }; // drop all.
                p.OutputDataReceived += (s, l) => { lock (output) { output.AppendLine(l.Data); } };
                try {
                    p.Start();

                    p.BeginErrorReadLine();
                    p.BeginOutputReadLine();
                    foreach (var line in instance.data) { p.StandardInput.WriteLine(line); }
                    p.StandardInput.Flush();

                    sw.Start();
                    while (!p.WaitForExit(BenchmarkCfg.MillisecondCheckInterval)
                        && (p.PrivateMemorySize64 < BenchmarkCfg.ByteMemoryLimit)
                        && (sw.ElapsedMilliseconds < msTimeout)) { }
                    sw.Stop();

                    if (p.HasExited) {
                        p.WaitForExit();
                    } else {
                        try { p.Kill(); } catch (Exception) { }
                    }

                    check(instance.data, output.ToString(), statistic);
                    saveOutput(output.ToString(), statistic.obj = normalizeObj(statistic.obj));

                    statistic.duration = sw.ElapsedMilliseconds / 1000.0;
                    statistics.Add(statistic);
                } catch (Exception e) {
                    Util.log("[error] test instance fail due to " + e.ToString());
                }
            }

            return statistics;
        }


        static int nextSeed(int seed) {
            return ((seed * BenchmarkCfg.RandSeedMul) + BenchmarkCfg.RandSeedInc) & 0xffff;
        }
    }
}
