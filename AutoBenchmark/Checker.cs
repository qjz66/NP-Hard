using System;
using System.Collections.Generic;
using System.Linq;


namespace AutoBenchmark {
    public delegate void Check(string[] input, string output, Statistic statistic);


    public class Checker {
        public static readonly char[] InlineDelimiters = new char[] { ' ', '\t' };
        public static readonly char[] LineDelimiters = new char[] { '\r', '\n' };
        public static readonly char[] WhiteSpaceChars = new char[] { ' ', '\t', '\r', '\n' };


        struct Edge {
            public int src;
            public int dst;
        };
        public static void coloring(string[] input, string output, Statistic statistic) {
            int nodeNum = 0;
            List<Edge> edges = new List<Edge>();
            try { // load instance.
                string[] cells = input[0].Split(InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                nodeNum = int.Parse(cells[0]);
                for (int l = 1; l < input.Length; ++l) {
                    cells = input[l].Split(InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                    edges.Add(new Edge { src = int.Parse(cells[0]), dst = int.Parse(cells[1]) });
                }
            } catch (Exception) { }

            HashSet<string> colors = new HashSet<string>();
            List<string> nodeColors = new List<string>();
            try { // load solution.
                string[] cells = output.Split(WhiteSpaceChars, StringSplitOptions.RemoveEmptyEntries);
                for (int c = 0; c < cells.Length; ++c) {
                    nodeColors.Add(cells[c]);
                    colors.Add(cells[c]);
                }
            } catch (Exception) { }

            int conflictNum = 0;
            try { // check.
                foreach (Edge edge in edges) {
                    if (nodeColors[edge.src] == nodeColors[edge.dst]) { ++conflictNum; }
                }
            } catch (Exception) { }

            bool feasible = (conflictNum == 0) && (nodeColors.Count == nodeNum);
            statistic.obj = feasible ? colors.Count : Problem.MaxObjValue;
            statistic.info = conflictNum.ToString();
        }


        public static void pCenter(string[] input, string output, Statistic statistic) {
            int nodeNum = 0;
            int centerNum = 0;
            int maxRank = 0;
            int minRank = 0;
            List<List<int>> sets = new List<List<int>>();
            List<List<int>> setsWithdrops = new List<List<int>>();
            try { // load instance.
                string[] cells = input[0].Split(InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                nodeNum = int.Parse(cells[0]);
                centerNum = int.Parse(cells[1]);
                int l = 1;
                for (sets.Capacity = nodeNum; (l < input.Length) && (sets.Count < nodeNum); ++l) {
                    int coveredItemNum = int.Parse(input[l]);
                    cells = input[++l].Split(InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                    List<int> set = new List<int>(coveredItemNum);
                    foreach (var cell in cells) { set.Add(int.Parse(cell)); }
                    sets.Add(set);
                }

                cells = input[l++].Split(InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                maxRank = int.Parse(cells[0]);
                minRank = int.Parse(cells[1]);
                for (setsWithdrops.Capacity = maxRank - minRank; l < input.Length; ++l) {
                    cells = input[l].Split(InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                    List<int> setsWithdrop = new List<int>(cells.Length - 1);
                    for (int c = 1; c < cells.Length; ++c) { setsWithdrop.Add(int.Parse(cells[c])); }
                    setsWithdrops.Add(setsWithdrop);
                }
            } catch (Exception) { }

            List<int> pickedSets = new List<int>(centerNum);
            List<bool> notPickedSet = new List<bool>(Enumerable.Repeat(true, nodeNum));
            try { // load solution.
                string[] cells = output.Split(WhiteSpaceChars, StringSplitOptions.RemoveEmptyEntries);
                foreach (string cell in cells) {
                    int s = int.Parse(cell);
                    pickedSets.Add(s);
                    notPickedSet[s] = false;
                }
            } catch (Exception) { }

            int uncoveredItemNum = nodeNum;
            int rank = maxRank + 1;
            try { // check.
                List<int> coveringSetNumOfItems = new List<int>(Enumerable.Repeat(0, nodeNum));
                foreach (var s in pickedSets) {
                    foreach (var item in sets[s]) {
                        if (coveringSetNumOfItems[item]++ < 1) { --uncoveredItemNum; }
                    }
                }

                if (uncoveredItemNum == 0) {
                    for (int r = 0; (r < setsWithdrops.Count) && (rank > maxRank); ++r) {
                        foreach (var s in setsWithdrops[r]) {
                            if (notPickedSet[s]) { continue; }
                            int dropItem = sets[s].Last();
                            if (--coveringSetNumOfItems[dropItem] < 1) { rank = maxRank - r; break; }
                            sets[s].pop();
                        }
                    }
                }
            } catch (Exception) { }

            bool feasible = (uncoveredItemNum == 0) && (pickedSets.Count == centerNum);
            statistic.obj = feasible ? rank : Problem.MaxObjValue;
            statistic.info = pickedSets.Count.ToString() + BenchmarkCfg.LogDelim + uncoveredItemNum.ToString();
        }


        // name mapping:
        // ---------------------------------------
        //  job shop scheduling | task scheduling
        // ---------------------+-----------------
        //  job                 | batch
        //  operation           | job
        //  machine             | worker
        // ---------------------+-----------------
        class Job {
            // `succeedingJobs[j]` is the jobs which can only begin after job `j` finishes.
            public List<int> succeedingJobs = new List<int>();
            // `candidateWorkers[w]` is the processing time of this job by worker `w`.
            public Dictionary<int, int> candidateWorkers = new Dictionary<int, int>();
        }
        public static void jobshop(string[] input, string output, Statistic statistic) {
            int batchNum = 0;
            int workerNum = 0;
            int maxCandidateWorkerNum = 0;
            List<Job> jobs = new List<Job>();
            List<List<int>> jobIdMap = new List<List<int>>(); // `jobIdMap[b][k]` is the `k`_th job (operation) in batch `b`.
            try { // load instance.
                string[] cells = input[0].Split(InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                batchNum = int.Parse(cells[0]);
                workerNum = int.Parse(cells[1]);
                maxCandidateWorkerNum = int.Parse(cells[2]);
                for (int l = 1; l < input.Length; ++l) { // for each batch.
                    cells = input[l].Split(InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                    int opNum = int.Parse(cells[0]);
                    List<int> idMap = new List<int>(opNum);
                    for (int c = 1; c < cells.Length; ++c) { // for each job in the batch.
                        Job job = new Job();
                        int candidateWorkerNum = int.Parse(cells[c]);
                        for (int w = 0; w < candidateWorkerNum; ++w) { // for each candidate worker.
                            int worker = int.Parse(cells[++c]);
                            int duration = int.Parse(cells[++c]);
                            job.candidateWorkers.Add(worker, duration);
                        }
                        idMap.Add(jobs.Count);
                        jobs.Add(job);
                        job.succeedingJobs.Add(jobs.Count); // succeeding job of the same batch.
                    }
                    jobs.Last().succeedingJobs.pop();
                    jobIdMap.Add(idMap);
                }
            } catch (Exception) { }

            List<List<int>> jobsOnWorkers = new List<List<int>>(workerNum);
            try { // load solution.
                string[] lines = output.Split(LineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                for (int l = 0; l < lines.Length; ++l) {
                    string[] cells = lines[l].Split(WhiteSpaceChars, StringSplitOptions.RemoveEmptyEntries);
                    int jobNumOnWorker = int.Parse(cells[0]);
                    List<int> jobsOnWorker = new List<int>(jobNumOnWorker);
                    for (int c = 1; c < cells.Length; ++c) {
                        int batch = int.Parse(cells[c]);
                        int job = int.Parse(cells[++c]);
                        jobsOnWorker.Add(jobIdMap[batch][job]);
                    }
                    jobsOnWorkers.Add(jobsOnWorker);
                }
            } catch (Exception) { }

            int makespan = (int)Problem.MaxObjValue;
            int restJobNum = jobs.Count;
            try { // check.
                List<int> jobExeDurations = new List<int>(Enumerable.Repeat(0, jobs.Count));
                for (int w = 0; w < workerNum; ++w) {
                    if (jobsOnWorkers[w].Count <= 0) { continue; }
                    int prevJob = jobsOnWorkers[w][0];
                    jobExeDurations[prevJob] = jobs[prevJob].candidateWorkers[w];
                    for (int j = 1; j < jobsOnWorkers[w].Count; ++j) {
                        int thisJob = jobsOnWorkers[w][j];
                        jobs[prevJob].succeedingJobs.Add(thisJob); // succeeding job on the same worker.
                        jobExeDurations[thisJob] = jobs[thisJob].candidateWorkers[w];
                        prevJob = thisJob;
                    }
                }
                List<int> preceedingJobNums = new List<int>(Enumerable.Repeat(0, jobs.Count));
                foreach (var job in jobs) {
                    foreach (var succeedingJob in job.succeedingJobs) {
                        ++preceedingJobNums[succeedingJob];
                    }
                }

                Queue<int> freeJobs = new Queue<int>(jobs.Count);
                List<int> earliestFinishTimes = new List<int>(Enumerable.Repeat(0, jobs.Count));
                for (int j = 0; j < jobs.Count; ++j) {
                    if (preceedingJobNums[j] > 0) { continue; }
                    freeJobs.Enqueue(j);
                    earliestFinishTimes[j] = jobExeDurations[j];
                }
                for (; freeJobs.Count > 0; --restJobNum) {
                    int j = freeJobs.Dequeue();
                    foreach (var succeedingJob in jobs[j].succeedingJobs) {
                        int newFinishTime = earliestFinishTimes[j] + jobExeDurations[succeedingJob];
                        if (earliestFinishTimes[succeedingJob] < newFinishTime) { earliestFinishTimes[succeedingJob] = newFinishTime; }
                        if (--preceedingJobNums[succeedingJob] <= 0) { freeJobs.Enqueue(succeedingJob); }
                    }
                }

                makespan = earliestFinishTimes.Max();
            } catch (Exception) { }

            bool feasible = (restJobNum == 0);
            statistic.obj = feasible ? makespan : Problem.MaxObjValue;
            statistic.info = restJobNum.ToString();
        }


        public static void rwa(string[] input, string output, Statistic statistic) {
            Dictionary<string, Dictionary<string, HashSet<string>>> edges = new Dictionary<string, Dictionary<string, HashSet<string>>>();
            List<string[]> traffics = new List<string[]>();
            try { // load instance.
                string[] nums = input[0].Split(InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                int nodeNum = int.Parse(nums[0]);
                int edgeNum = int.Parse(nums[1]);
                int trafficNum = int.Parse(nums[2]);
                int l = 1;
                for (int n = 0; (n < edgeNum) && (l < input.Length); ++l, ++n) {
                    string[] nodes = input[l].Split(InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                    edges.TryAdd(nodes[0], new Dictionary<string, HashSet<string>>());
                    edges[nodes[0]].Add(nodes[1], new HashSet<string>());
                }

                traffics.Capacity = trafficNum;
                for (int t = 0; (t < trafficNum) && (l < input.Length); ++l, ++t) {
                    traffics.Add(input[l].Split(InlineDelimiters, StringSplitOptions.RemoveEmptyEntries));
                }
            } catch (Exception) { }

            int brokenPathNum = 0;
            int conflictNum = 0;
            HashSet<string> colors = new HashSet<string>();
            try { // load solution and check.
                string[] lines = output.Split(LineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                for (int l = 0; l < lines.Length; ++l) {
                    List<string> nums = lines[l].Split(InlineDelimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
                    nums.Add(traffics[l][1]);
                    string color = nums[0];
                    colors.Add(color);
                    string src = traffics[l][0];
                    for (int i = 2; i < nums.Count; ++i) {
                        string dst = nums[i];
                        if (dst == src) { continue; }
                        if (!edges.ContainsKey(src) || !edges[src].ContainsKey(dst)) { ++brokenPathNum; break; }
                        if (edges[src][dst].Contains(color)) { ++conflictNum; }
                        edges[src][dst].Add(color);
                        src = nums[i];
                    }
                }
            } catch (Exception) { }

            bool feasible = (brokenPathNum == 0) && (conflictNum == 0);
            statistic.obj = feasible ? colors.Count : Problem.MaxObjValue;
            statistic.info = brokenPathNum.ToString() + BenchmarkCfg.LogDelim + conflictNum.ToString();
        }


        public static void rectPacking(string[] input, string output, Statistic statistic) {

        }


        public static void vrp(string[] input, string output, Statistic statistic) {

        }
    }
}
