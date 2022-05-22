using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace AutoBenchmark {
    public class PageGenerator {
        public static void generateMarkdown(string problemName, Problem problem) {
            string filename = $"{problemName}/{CommonCfg.RankMarkdownPath}";
            using (StreamWriter sw = File.CreateText(filename)) {
                sw.WriteLine($"# {problemName} Results");
                foreach (var dataset in problem.datasets) {
                    foreach (var instance in dataset.instances) {
                        sw.WriteLine($"## {instance.Key}");
                        sw.WriteLine("| Rank |    Author    |    Obj    |       Date       |   Duration (s)  |");
                        sw.WriteLine("| ---- | ------------ | --------- | ---------------- | --------------- |");
                        int count = 0;
                        foreach (var r in instance.Value.results) {
                            sw.WriteLine($"| {count} | {r.author} | {problem.restoreObj(r.obj)} | {r.date} | {r.duration} |");
                            ++count;
                        }
                        sw.WriteLine();
                    }
                }
            }

            Util.run("git", "pull origin data");
            //Util.run("git", $"add {filename}");
            Util.run("git", "commit -a -m a");
            Util.run("git", "push origin data");
        }
        public static void generateMarkdowns(Rank rank) {
            using (StreamWriter sw = File.CreateText(CommonCfg.RankMarkdownPath)) {
                sw.WriteLine("# NPBenchmark Results");
                foreach (var problem in rank.problems) {
                    sw.WriteLine($"- [{problem.Key}]({problem.Key}/{CommonCfg.RankMarkdownPath})");
                    generateMarkdown(problem.Key, problem.Value);
                }
            }

            Util.run("git", "pull origin data");
            //Util.run("git", "add " + CommonCfg.RankMarkdownPath);
            Util.run("git", "commit -a -m a");
            Util.run("git", "push origin data");
        }

        public static void generateHtml(Rank rank) {
            using (StreamWriter sw = File.CreateText(CommonCfg.RankPagePath)) {
                sw.WriteLine("<!DOCTYPE html>");
                sw.WriteLine("<html>");
                sw.WriteLine("<head>");
                sw.WriteLine("<meta charset='utf-8' />");
                sw.WriteLine("<title>NPBenchmark Results</title>");
                sw.WriteLine("<link rel='stylesheet' href='base.css' />");
                sw.WriteLine("</head>");
                sw.WriteLine("<body>");
                sw.WriteLine($"<h1>NPBenchmark Results</h1>");
                sw.WriteLine("<ol>");
                foreach (var problem in rank.problems) {
                    sw.WriteLine($"<li id='{problem.Key}'><a href='#{problem.Key}'>{problem.Key}</a><ol>");
                    foreach (var dataset in problem.Value.datasets) {
                        foreach (var instance in dataset.instances) {
                            sw.WriteLine($"<li id='{problem.Key}-{instance.Key}'><a href='#{problem.Key}-{instance.Key}'>{instance.Key}</a><table>");
                            sw.WriteLine("<tr><th>Rank</th><th>Author</th><th>Obj</th><th>Date</th><th>Duration</th></tr>");
                            int count = 0;
                            foreach (var r in instance.Value.results) {
                                sw.WriteLine($"<tr><td>{count}</td><td id='auth'>{r.author}</td><td>{problem.Value.restoreObj(r.obj)}</td><td>{r.date}</td><td>{r.duration}</td></tr>");
                                ++count;
                            }
                            sw.WriteLine("</table></li>");
                        }
                    }
                    sw.WriteLine("</ol></li>");
                }
                sw.WriteLine("</ol>");
                sw.WriteLine("</body>");
                sw.WriteLine("</html>");
            }

            Util.run("git", "pull origin data");
            //Util.run("git", "add " + CommonCfg.RankPagePath);
            Util.run("git", "commit -a -m a");
            Util.run("git", "push origin data");
        }
    }
}
