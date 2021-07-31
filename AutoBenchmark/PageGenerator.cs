using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace Analyzer {
    public class PageGenerator {
        public const int MaxDateWidth = 19;


        public void generate(Rank rank) {
            using (StreamWriter sw = File.CreateText(CommonCfg.RankPagePath)) {
                sw.WriteLine("<!DOCTYPE html>");
                sw.WriteLine("<html>");
                sw.WriteLine("<head>");
                sw.WriteLine("<meta charset='utf-8' />");
                sw.WriteLine("<title>Benchmark Results</title>");
                sw.WriteLine("<link rel='stylesheet' href='base.css' />");
                sw.WriteLine("</head>");
                sw.WriteLine("<body>");
                //sw.WriteLine($"<h1>Instruction</h1><p>{CommonCfg.AuthorInstruction}</p>");
                //sw.WriteLine($"<h1>Specification</h1><p>{CommonCfg.SdkSpecification}</p>");
                sw.WriteLine($"<h1>Results</h1>");
                sw.WriteLine("<ol>");
                foreach (var problem in rank.problems) {
                    sw.WriteLine($"<li id='{problem.Key}'><a href='#{problem.Key}'>{problem.Key}</a><ol>");
                    foreach (var dataset in problem.Value.datasets) {
                        foreach (var instance in dataset.instances) {
                            sw.WriteLine($"<li id='{problem.Key}-{instance.Key}'><a href='#{problem.Key}-{instance.Key}'>{instance.Key}</a><table>");
                            sw.WriteLine("<thead><tr><th>Rank</th><th>Author</th><th>Objective</th><th>Date</th><th>Duration</th><th>Algorithm</th><th>Thread</th><th>CPU</th><th>RAM</th><th>Language</th><th>Compiler</th><th>OS</th><th>Email</th><th>Solution</th></tr></thead><tbody>");
                            int count = 0;
                            var results = problem.Value.minimize ? instance.Value.results : instance.Value.results.Reverse();
                            foreach (var result in results) {
                                string date = result.date.Substring(0, Math.Min(result.date.Length, MaxDateWidth));
                                sw.WriteLine($"<tr><td>{count}</td><td id='auth'>{result.author}</td><td>{result.obj}</td><td>{date}</td><td>{result.duration}</td></tr>");
                                ++count;
                            }
                            sw.WriteLine("</tbody></table></li>");
                        }
                    }
                    sw.WriteLine("</ol></li>");
                }
                sw.WriteLine("</ol>");
                sw.WriteLine("</body>");
                sw.WriteLine("</html>");
            }
            
            Util.run("git", "add " + CommonCfg.RankPath);
            Util.run("git", "add " + CommonCfg.RankPagePath);
            //Util.run("git", "add " + CommonCfg.RankCssPath);
            Util.run("git", "commit -m a");
            Util.run("git", "push origin gh-pages");
        }
    }
}
