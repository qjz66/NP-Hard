using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace AutoBenchmark {
    public class InstanceConverter {
        public class GCP {
            public static void convertAll() {
                convertDimacs(@"DSJC125.1.col", @"DSJC0125.1.txt", 5);
                convertDimacs(@"DSJC125.5.col", @"DSJC0125.5.txt", 17);
                convertDimacs(@"DSJC125.9.col", @"DSJC0125.9.txt", 44);
                convertDimacs(@"DSJC250.1.col", @"DSJC0250.1.txt", 8);
                convertDimacs(@"DSJC250.9.col", @"DSJC0250.9.txt", 72);
                convertDimacs(@"DSJC250.5.col", @"DSJC0250.5.txt", 28);
                convertDimacs(@"DSJC500.1.col", @"DSJC0500.1.txt", 12);
                convertDimacs(@"DSJC500.5.col", @"DSJC0500.5.txt", 49);
                convertDimacs(@"DSJC500.9.col", @"DSJC0500.9.txt", 126);
                convertDimacs(@"DSJC1000.1.col", @"DSJC1000.1.txt", 20);
                convertDimacs(@"DSJC1000.5.col", @"DSJC1000.5.txt", 83);
                convertDimacs(@"DSJC1000.9.col", @"DSJC1000.9.txt", 224);
            }

            class Arc {
                public int src;
                public int dst;
            }
            static void convertDimacs(string oldPath, string newPath, int colorNum) {
                int nodeNum = 0;
                List<Arc> edges = new List<Arc>();
                string[] lines = File.ReadAllLines(oldPath);

                foreach (string line in lines) {
                    if (line.Length <= 0) { continue; }
                    if (line[0] == 'c') { continue; }

                    string[] words = line.Split(Checker.InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (line[0] == 'p') {
                        nodeNum = int.Parse(words[2]);
                        edges.Capacity = int.Parse(words[3]);
                    } else if (line[0] == 'e') {
                        edges.Add(new Arc { src = int.Parse(words[1]) - 1, dst = int.Parse(words[2]) - 1 });
                    }
                }

                StringBuilder sb = new StringBuilder();
                sb.Append(nodeNum).Append(' ').Append(edges.Count).Append(' ').Append(colorNum).Append('\n');
                foreach (var edge in edges) {
                    sb.Append(edge.src).Append(' ').Append(edge.dst).Append('\n');
                }

                File.WriteAllText(newPath, sb.ToString());
            }
        }

        public class PCP {
            public static void convertAll() {
                convertTsplib(@"pcb3038.tsp", @"pcb3038p010r729.txt", 10, 729);
                convertTsplib(@"pcb3038.tsp", @"pcb3038p020r494.txt", 20, 494);
                convertTsplib(@"pcb3038.tsp", @"pcb3038p030r394.txt", 30, 394);
                convertTsplib(@"pcb3038.tsp", @"pcb3038p040r337.txt", 40, 337);
                convertTsplib(@"pcb3038.tsp", @"pcb3038p050r299.txt", 50, 299);
                convertTsplib(@"pcb3038.tsp", @"pcb3038p100r207.txt", 100, 207);
                convertTsplib(@"pcb3038.tsp", @"pcb3038p150r165.txt", 150, 165);
                convertTsplib(@"pcb3038.tsp", @"pcb3038p200r141.txt", 200, 141);
                convertTsplib(@"pcb3038.tsp", @"pcb3038p250r123.txt", 250, 123);
                convertTsplib(@"pcb3038.tsp", @"pcb3038p300r116.txt", 300, 116);
                convertTsplib(@"pcb3038.tsp", @"pcb3038p350r105.txt", 350, 105);
                convertTsplib(@"pcb3038.tsp", @"pcb3038p400r97.txt", 400, 97);
                convertTsplib(@"pcb3038.tsp", @"pcb3038p450r89.txt", 450, 89);
                convertTsplib(@"pcb3038.tsp", @"pcb3038p500r85.txt", 500, 85);

                update(@"pmed01.n100p5.txt");
                update(@"pmed02.n100p10.txt");
                update(@"pmed03.n100p10.txt");
                update(@"pmed04.n100p20.txt");
                update(@"pmed05.n100p33.txt");
                update(@"pmed06.n200p5.txt");
                update(@"pmed07.n200p10.txt");
                update(@"pmed08.n200p20.txt");
                update(@"pmed09.n200p40.txt");
                update(@"pmed10.n200p67.txt");
                update(@"pmed11.n300p5.txt");
                update(@"pmed12.n300p10.txt");
                update(@"pmed13.n300p30.txt");
                update(@"pmed14.n300p60.txt");
                update(@"pmed15.n300p100.txt");
                update(@"pmed16.n400p5.txt");
                update(@"pmed17.n400p10.txt");
                update(@"pmed18.n400p40.txt");
                update(@"pmed19.n400p80.txt");
                update(@"pmed20.n400p133.txt");
                update(@"pmed21.n500p5.txt");
                update(@"pmed22.n500p10.txt");
                update(@"pmed23.n500p50.txt");
                update(@"pmed24.n500p100.txt");
                update(@"pmed25.n500p167.txt");
                update(@"pmed26.n600p5.txt");
                update(@"pmed27.n600p10.txt");
                update(@"pmed28.n600p60.txt");
                update(@"pmed29.n600p120.txt");
                update(@"pmed30.n600p200.txt");
                update(@"pmed31.n700p5.txt");
                update(@"pmed32.n700p10.txt");
                update(@"pmed33.n700p70.txt");
                update(@"pmed34.n700p140.txt");
                update(@"pmed35.n800p5.txt");
                update(@"pmed36.n800p10.txt");
                update(@"pmed37.n800p80.txt");
                update(@"pmed38.n900p5.txt");
                update(@"pmed39.n900p10.txt");
                update(@"pmed40.n900p90.txt");
                update(@"rl1323p010r3077.30.txt");
                update(@"rl1323p020r2016.40.txt");
                update(@"rl1323p030r1631.50.txt");
                update(@"rl1323p040r1352.36.txt");
                update(@"rl1323p050r1187.27.txt");
                update(@"rl1323p060r1063.01.txt");
                update(@"rl1323p070r971.93.txt");
                update(@"rl1323p080r895.06.txt");
                update(@"rl1323p090r832.00.txt");
                update(@"rl1323p100r789.70.txt");
                update(@"u1060p010r2273.08.txt");
                update(@"u1060p020r1580.80.txt");
                update(@"u1060p030r1207.77.txt");
                update(@"u1060p040r1020.56.txt");
                update(@"u1060p050r904.92.txt");
                update(@"u1060p060r781.17.txt");
                update(@"u1060p070r710.75.txt");
                update(@"u1060p080r652.16.txt");
                update(@"u1060p090r607.87.txt");
                update(@"u1060p100r570.01.txt");
                update(@"u1060p110r538.84.txt");
                update(@"u1060p120r510.27.txt");
                update(@"u1060p130r499.65.txt");
                update(@"u1060p140r452.46.txt");
                update(@"u1060p150r447.01.txt");
                update(@"u1817p010r457.91.txt");
                update(@"u1817p020r309.01.txt");
                update(@"u1817p030r240.99.txt");
                update(@"u1817p040r209.45.txt");
                update(@"u1817p050r184.91.txt");
                update(@"u1817p060r162.64.txt");
                update(@"u1817p070r148.11.txt");
                update(@"u1817p080r136.77.txt");
                update(@"u1817p090r129.51.txt");
                update(@"u1817p100r126.99.txt");
                update(@"u1817p110r109.25.txt");
                update(@"u1817p120r107.76.txt");
                update(@"u1817p130r104.73.txt");
                update(@"u1817p140r101.60.txt");
                update(@"u1817p150r91.60.txt");
            }

            static double[,] loadNodeCoords(string geoGraphPath) {
                string[] lines = File.ReadAllLines(geoGraphPath);

                int nodeNum = lines.Length;
                int l = 0;

                string[] header = lines[0].Split(Checker.InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                if (header.Length == 1) {
                    nodeNum = int.Parse(header[0]);
                    l = 1;
                }

                double[,] coords = new double[nodeNum, 2];
                for (int n = 0; l < lines.Length; ++l, ++n) {
                    string[] words = lines[l].Split(Checker.InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length < 2) { continue; }
                    coords[n, 0] = double.Parse(words[words.Length - 2]);
                    coords[n, 1] = double.Parse(words[words.Length - 1]);
                }

                return coords;
            }

            static double[,] calcAdjMat(double[,] nodeCoords) {
                int nodeNum = nodeCoords.GetLength(0);
                double[,] adjMat = new double[nodeNum, nodeNum];
                for (int s = 0; s < nodeNum; ++s) {
                    for (int d = 0; d < s; ++d) {
                        double dx = nodeCoords[s, 0] - nodeCoords[d, 0];
                        double dy = nodeCoords[s, 1] - nodeCoords[d, 1];
                        adjMat[s, d] = adjMat[d, s] = Math.Sqrt(dx * dx + dy * dy);
                    }
                }
                return adjMat;
            }

            static double[] calcDistances(double[,] adjMat) {
                int nodeNum = adjMat.GetLength(0);
                SortedSet<double> distanceSet = new SortedSet<double>();
                for (int s = 0; s < nodeNum; ++s) {
                    for (int d = 0; d < s; ++d) {
                        distanceSet.Add(adjMat[s, d]);
                    }
                }
                return distanceSet.ToArray();
            }

            static void tryInc(SortedDictionary<int, int> dictionary, int key) {
                dictionary.TryAdd(key, 0);
                ++dictionary[key];
            }

            // there exist `centerNum` nodes that the number of covered nodes exceeds the `nodeNum`.
            static int estimateMinDistanceRankConservative(double[] distances, double[,] adjMat, List<List<int>> adjList, int centerNum) {
                int nodeNum = adjList.Count;
                int r = 0;
                int[] adjNodeNums = new int[nodeNum];
                for (int adjNodeNumSum = 0; adjNodeNumSum < nodeNum; ++r) {
                    double radius = distances[r];
                    for (int s = 0; s < nodeNum; ++s) {
                        for (int d = adjNodeNums[s]; (d < adjList[s].Count) && (adjMat[s, adjList[s][d]] <= radius); ++d) { ++adjNodeNums[s]; }
                    }

                    SortedDictionary<int, int> topAdjNodeNums = new SortedDictionary<int, int>();
                    for (int n = 0; n < centerNum; ++n) { tryInc(topAdjNodeNums, adjNodeNums[n]); }
                    int tail = topAdjNodeNums.First().Key;
                    for (int n = centerNum; n < nodeNum; ++n) {
                        if (adjNodeNums[n] <= tail) { continue; }
                        if (--topAdjNodeNums[tail] <= 0) { topAdjNodeNums.Remove(tail); }
                        tryInc(topAdjNodeNums, adjNodeNums[n]);
                        tail = topAdjNodeNums.First().Key;
                    }

                    adjNodeNumSum = 0;
                    foreach (var n in topAdjNodeNums) { adjNodeNumSum += (n.Key * n.Value); }
                }
                return r;
            }
            // the average number of nodes covered by each node exceeds `nodeNum / centerNum`.
            static int estimateMinDistanceRankAggressive(double[] distances, double[,] adjMat, List<List<int>> adjList, int centerNum) {
                int nodeNum = adjList.Count;
                int r = 0;
                int[] adjNodeNums = new int[nodeNum];
                int totalAdjNodeNum = nodeNum * nodeNum / centerNum; // average node number covered by a node times node number.
                for (int adjNodeNumSum = 0; adjNodeNumSum < totalAdjNodeNum; ++r) {
                    double radius = distances[r];
                    for (int s = 0; s < nodeNum; ++s) {
                        for (int d = adjNodeNums[s]; (d < nodeNum) && (adjMat[s, adjList[s][d]] <= radius); ++d) {
                            ++adjNodeNums[s];
                            ++adjNodeNumSum;
                        }
                    }
                }
                return r;
            }

            static void convertTsplib(string geoGraphPath, string uscpPath, int centerNum, double maxRadius) {
                double[,] nodeCoords = loadNodeCoords(geoGraphPath);
                int nodeNum = nodeCoords.GetLength(0);
                double[,] adjMat = calcAdjMat(nodeCoords);
                double[] distances = calcDistances(adjMat);

                List<List<int>> adjList = new List<List<int>>(nodeNum);
                for (int s = 0; s < nodeNum; ++s) {
                    List<int> adjNodes = new List<int>(nodeNum);
                    for (int d = 0; d < nodeNum; ++d) {
                        if ((adjMat[s, d] <= maxRadius) || (s == d)) { adjNodes.Add(d); }
                    }
                    adjNodes.Sort((int u, int v) => { return adjMat[s, u].CompareTo(adjMat[s, v]); });
                    adjList.Add(adjNodes);
                }

                int maxRank = distances.Length - 1;
                for (; (maxRank >= 0) && (distances[maxRank] > maxRadius); --maxRank) { }
                maxRadius = distances[maxRank];
                int minRank = estimateMinDistanceRankConservative(distances, adjMat, adjList, centerNum);

                StringBuilder sb = new StringBuilder();
                sb.Append(nodeNum).Append(' ').Append(centerNum).Append('\n');
                for (int s = 0; s < nodeNum; ++s) {
                    sb.Append(adjList[s].Count).Append('\n');
                    for (int d = 0; d < adjList[s].Count; ++d) {
                        sb.Append(adjList[s][d]).Append(' ');
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append('\n');
                }

                sb.Append(maxRank).Append(' ').Append(minRank).Append('\n');
                for (int r = maxRank - 1; r >= minRank; --r) {
                    double radius = distances[r];
                    StringBuilder rsb = new StringBuilder();
                    int dropNodeNumSum = 0;
                    for (int s = 0; s < nodeNum; ++s) {
                        int dropNodeNum = 0;
                        for (int d = adjList[s].Count - 1; (d >= 0) && (adjMat[s, adjList[s][d]] > radius); --d) { ++dropNodeNum; }
                        if (dropNodeNum == 0) { continue; }
                        adjList[s].RemoveRange(adjList[s].Count - dropNodeNum, dropNodeNum);
                        dropNodeNumSum += dropNodeNum;
                        while (dropNodeNum-- > 0) { rsb.Append(s).Append(' '); }
                    }
                    rsb.Remove(rsb.Length - 1, 1);
                    sb.Append(dropNodeNumSum).Append('\t').AppendLine(rsb.ToString());
                }

                File.WriteAllText(uscpPath, sb.ToString());
            }

            static void update(string path) {
                string[] lines = File.ReadAllLines(path);

                StringBuilder sb = new StringBuilder();
                for (int l = 0; l < lines.Length; ++l) { sb.AppendLine(lines[l].TrimEnd()); }
                sb.AppendLine("0 0");

                File.WriteAllText(path, sb.ToString());
            }
        }

        public class FJSP {
            public static void convertAll() {
                convertJspTa("tai15_15.txt", 1);
                convertJspTa("tai20_15.txt", 11);
                convertJspTa("tai20_20.txt", 21);
                convertJspTa("tai30_15.txt", 31);
                convertJspTa("tai30_20.txt", 41);
                convertJspTa("tai50_15.txt", 51);
                convertJspTa("tai50_20.txt", 61);
                convertJspTa("tai100_20.txt", 71);
            }

            static void convertJspTa(string path, int index) {
                string[] lines = File.ReadAllLines(path);
                for (int l = 0; l < lines.Length; ++l) {
                    if (lines[l++].Length <= 0) { break; }
                    string[] words = lines[l++].Split(Checker.InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length <= 0) { break; }
                    int batchNum = int.Parse(words[0]);
                    int workerNum = int.Parse(words[1]);
                    int ub = int.Parse(words[4]);
                    int lb = int.Parse(words[5]);
                    string[][] times = new string[batchNum][];
                    for (int b = 0; b < batchNum; ++b) {
                        times[b] = lines[++l].Split(Checker.InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                    }
                    ++l;
                    string[][] workers = new string[batchNum][];
                    for (int b = 0; b < batchNum; ++b) {
                        workers[b] = lines[++l].Split(Checker.InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.Append(batchNum).Append(' ').Append(workerNum).AppendLine(" 1");
                    for (int b = 0; b < batchNum; ++b) {
                        sb.Append(workerNum);
                        for (int w = 0; w < workerNum; ++w) {
                            sb.Append("    1  ").Append(int.Parse(workers[b][w]) - 1)
                                .Append(' ').Append(times[b][w]);
                        }
                        sb.Append('\n');
                    }

                    path = "jsp.TA" + index++ + ".m" + workerNum + "j" + batchNum + "c1";
                    Console.WriteLine(path + " " + ub + " " + lb);
                    File.WriteAllText(path + ".txt", sb.ToString());
                }
            }
        }

        public class RWA {
            public static void convertAll() {
                convertXyz("ATT.net", "ATT.trf", "ATT");
                convertXyz("ATT2.net", "ATT2.trf", "ATT2");
                convertXyz("brasil.net", "brasil.trf", "brasil");
                convertXyz("EON.net", "EON.trf", "EON");
                convertXyz("Finland.net", "Finland.trf", "Finland");
                convertXyz("NSF.net", "NSF.1.trf", "NSF-01");
                convertXyz("NSF.net", "NSF.3.trf", "NSF-03");
                convertXyz("NSF.net", "NSF.12.trf", "NSF-12");
                convertXyz("NSF.net", "NSF.48.trf", "NSF-48");
                convertXyz("NSF2.net", "NSF.1.trf", "NSF2-01");
                convertXyz("NSF2.net", "NSF.3.trf", "NSF2-03");
                convertXyz("NSF2.net", "NSF.12.trf", "NSF2-12");
                convertXyz("NSF2.net", "NSF.48.trf", "NSF2-48");

                convertXyz("Y.3_seed=1.net", "Y.3.20_seed=1.trf", "Y3-020-1");
                convertXyz("Y.3_seed=2.net", "Y.3.20_seed=2.trf", "Y3-020-2");
                convertXyz("Y.3_seed=3.net", "Y.3.20_seed=3.trf", "Y3-020-3");
                convertXyz("Y.3_seed=4.net", "Y.3.20_seed=4.trf", "Y3-020-4");
                convertXyz("Y.3_seed=5.net", "Y.3.20_seed=5.trf", "Y3-020-5");
                convertXyz("Y.3_seed=1.net", "Y.3.40_seed=1.trf", "Y3-040-1");
                convertXyz("Y.3_seed=2.net", "Y.3.40_seed=2.trf", "Y3-040-2");
                convertXyz("Y.3_seed=3.net", "Y.3.40_seed=3.trf", "Y3-040-3");
                convertXyz("Y.3_seed=4.net", "Y.3.40_seed=4.trf", "Y3-040-4");
                convertXyz("Y.3_seed=5.net", "Y.3.40_seed=5.trf", "Y3-040-5");
                convertXyz("Y.3_seed=1.net", "Y.3.60_seed=1.trf", "Y3-060-1");
                convertXyz("Y.3_seed=2.net", "Y.3.60_seed=2.trf", "Y3-060-2");
                convertXyz("Y.3_seed=3.net", "Y.3.60_seed=3.trf", "Y3-060-3");
                convertXyz("Y.3_seed=4.net", "Y.3.60_seed=4.trf", "Y3-060-4");
                convertXyz("Y.3_seed=5.net", "Y.3.60_seed=5.trf", "Y3-060-5");
                convertXyz("Y.3_seed=1.net", "Y.3.80_seed=1.trf", "Y3-080-1");
                convertXyz("Y.3_seed=2.net", "Y.3.80_seed=2.trf", "Y3-080-2");
                convertXyz("Y.3_seed=3.net", "Y.3.80_seed=3.trf", "Y3-080-3");
                convertXyz("Y.3_seed=4.net", "Y.3.80_seed=4.trf", "Y3-080-4");
                convertXyz("Y.3_seed=5.net", "Y.3.80_seed=5.trf", "Y3-080-5");
                convertXyz("Y.3_seed=1.net", "Y.3.100_seed=1.trf", "Y3-100-1");
                convertXyz("Y.3_seed=2.net", "Y.3.100_seed=2.trf", "Y3-100-2");
                convertXyz("Y.3_seed=3.net", "Y.3.100_seed=3.trf", "Y3-100-3");
                convertXyz("Y.3_seed=4.net", "Y.3.100_seed=4.trf", "Y3-100-4");
                convertXyz("Y.3_seed=5.net", "Y.3.100_seed=5.trf", "Y3-100-5");
                convertXyz("Y.4_seed=1.net", "Y.4.20_seed=1.trf", "Y4-020-1");
                convertXyz("Y.4_seed=2.net", "Y.4.20_seed=2.trf", "Y4-020-2");
                convertXyz("Y.4_seed=3.net", "Y.4.20_seed=3.trf", "Y4-020-3");
                convertXyz("Y.4_seed=4.net", "Y.4.20_seed=4.trf", "Y4-020-4");
                convertXyz("Y.4_seed=5.net", "Y.4.20_seed=5.trf", "Y4-020-5");
                convertXyz("Y.4_seed=1.net", "Y.4.40_seed=1.trf", "Y4-040-1");
                convertXyz("Y.4_seed=2.net", "Y.4.40_seed=2.trf", "Y4-040-2");
                convertXyz("Y.4_seed=3.net", "Y.4.40_seed=3.trf", "Y4-040-3");
                convertXyz("Y.4_seed=4.net", "Y.4.40_seed=4.trf", "Y4-040-4");
                convertXyz("Y.4_seed=5.net", "Y.4.40_seed=5.trf", "Y4-040-5");
                convertXyz("Y.4_seed=1.net", "Y.4.60_seed=1.trf", "Y4-060-1");
                convertXyz("Y.4_seed=2.net", "Y.4.60_seed=2.trf", "Y4-060-2");
                convertXyz("Y.4_seed=3.net", "Y.4.60_seed=3.trf", "Y4-060-3");
                convertXyz("Y.4_seed=4.net", "Y.4.60_seed=4.trf", "Y4-060-4");
                convertXyz("Y.4_seed=5.net", "Y.4.60_seed=5.trf", "Y4-060-5");
                convertXyz("Y.4_seed=1.net", "Y.4.80_seed=1.trf", "Y4-080-1");
                convertXyz("Y.4_seed=2.net", "Y.4.80_seed=2.trf", "Y4-080-2");
                convertXyz("Y.4_seed=3.net", "Y.4.80_seed=3.trf", "Y4-080-3");
                convertXyz("Y.4_seed=4.net", "Y.4.80_seed=4.trf", "Y4-080-4");
                convertXyz("Y.4_seed=5.net", "Y.4.80_seed=5.trf", "Y4-080-5");
                convertXyz("Y.4_seed=1.net", "Y.4.100_seed=1.trf", "Y4-100-1");
                convertXyz("Y.4_seed=2.net", "Y.4.100_seed=2.trf", "Y4-100-2");
                convertXyz("Y.4_seed=3.net", "Y.4.100_seed=3.trf", "Y4-100-3");
                convertXyz("Y.4_seed=4.net", "Y.4.100_seed=4.trf", "Y4-100-4");
                convertXyz("Y.4_seed=5.net", "Y.4.100_seed=5.trf", "Y4-100-5");
                convertXyz("Y.5_seed=1.net", "Y.5.20_seed=1.trf", "Y5-020-1");
                convertXyz("Y.5_seed=2.net", "Y.5.20_seed=2.trf", "Y5-020-2");
                convertXyz("Y.5_seed=3.net", "Y.5.20_seed=3.trf", "Y5-020-3");
                convertXyz("Y.5_seed=4.net", "Y.5.20_seed=4.trf", "Y5-020-4");
                convertXyz("Y.5_seed=5.net", "Y.5.20_seed=5.trf", "Y5-020-5");
                convertXyz("Y.5_seed=1.net", "Y.5.40_seed=1.trf", "Y5-040-1");
                convertXyz("Y.5_seed=2.net", "Y.5.40_seed=2.trf", "Y5-040-2");
                convertXyz("Y.5_seed=3.net", "Y.5.40_seed=3.trf", "Y5-040-3");
                convertXyz("Y.5_seed=4.net", "Y.5.40_seed=4.trf", "Y5-040-4");
                convertXyz("Y.5_seed=5.net", "Y.5.40_seed=5.trf", "Y5-040-5");
                convertXyz("Y.5_seed=1.net", "Y.5.60_seed=1.trf", "Y5-060-1");
                convertXyz("Y.5_seed=2.net", "Y.5.60_seed=2.trf", "Y5-060-2");
                convertXyz("Y.5_seed=3.net", "Y.5.60_seed=3.trf", "Y5-060-3");
                convertXyz("Y.5_seed=4.net", "Y.5.60_seed=4.trf", "Y5-060-4");
                convertXyz("Y.5_seed=5.net", "Y.5.60_seed=5.trf", "Y5-060-5");
                convertXyz("Y.5_seed=1.net", "Y.5.80_seed=1.trf", "Y5-080-1");
                convertXyz("Y.5_seed=2.net", "Y.5.80_seed=2.trf", "Y5-080-2");
                convertXyz("Y.5_seed=3.net", "Y.5.80_seed=3.trf", "Y5-080-3");
                convertXyz("Y.5_seed=4.net", "Y.5.80_seed=4.trf", "Y5-080-4");
                convertXyz("Y.5_seed=5.net", "Y.5.80_seed=5.trf", "Y5-080-5");
                convertXyz("Y.5_seed=1.net", "Y.5.100_seed=1.trf", "Y5-100-1");
                convertXyz("Y.5_seed=2.net", "Y.5.100_seed=2.trf", "Y5-100-2");
                convertXyz("Y.5_seed=3.net", "Y.5.100_seed=3.trf", "Y5-100-3");
                convertXyz("Y.5_seed=4.net", "Y.5.100_seed=4.trf", "Y5-100-4");
                convertXyz("Y.5_seed=5.net", "Y.5.100_seed=5.trf", "Y5-100-5");

                convertXyz("Z.4x25.net", "Z.4x25.20.trf", "Z4x25-020");
                convertXyz("Z.4x25.net", "Z.4x25.40.trf", "Z4x25-040");
                convertXyz("Z.4x25.net", "Z.4x25.60.trf", "Z4x25-060");
                convertXyz("Z.4x25.net", "Z.4x25.80.trf", "Z4x25-080");
                convertXyz("Z.4x25.net", "Z.4x25.100.trf", "Z4x25-100");
                convertXyz("Z.5x20.net", "Z.5x20.20.trf", "Z5x20-020");
                convertXyz("Z.5x20.net", "Z.5x20.40.trf", "Z5x20-040");
                convertXyz("Z.5x20.net", "Z.5x20.60.trf", "Z5x20-060");
                convertXyz("Z.5x20.net", "Z.5x20.80.trf", "Z5x20-080");
                convertXyz("Z.5x20.net", "Z.5x20.100.trf", "Z5x20-100");
                convertXyz("Z.6x17.net", "Z.6x17.20.trf", "Z6x17-020");
                convertXyz("Z.6x17.net", "Z.6x17.40.trf", "Z6x17-040");
                convertXyz("Z.6x17.net", "Z.6x17.60.trf", "Z6x17-060");
                convertXyz("Z.6x17.net", "Z.6x17.80.trf", "Z6x17-080");
                convertXyz("Z.6x17.net", "Z.6x17.100.trf", "Z6x17-100");
                convertXyz("Z.8x13.net", "Z.8x13.20.trf", "Z8x13-020");
                convertXyz("Z.8x13.net", "Z.8x13.40.trf", "Z8x13-040");
                convertXyz("Z.8x13.net", "Z.8x13.60.trf", "Z8x13-060");
                convertXyz("Z.8x13.net", "Z.8x13.80.trf", "Z8x13-080");
                convertXyz("Z.8x13.net", "Z.8x13.100.trf", "Z8x13-100");
                convertXyz("Z.10x10.net", "Z.10x10.20.trf", "Z10x10-020");
                convertXyz("Z.10x10.net", "Z.10x10.40.trf", "Z10x10-040");
                convertXyz("Z.10x10.net", "Z.10x10.60.trf", "Z10x10-060");
                convertXyz("Z.10x10.net", "Z.10x10.80.trf", "Z10x10-080");
                convertXyz("Z.10x10.net", "Z.10x10.100.trf", "Z10x10-100");
            }

            static void convertXyz(string netPath, string trafficPath, string instancePath) {
                string[] netLines = File.ReadAllLines(netPath);
                string[] netNums = netLines[0].Split(Checker.InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);

                string[] trafficLines = File.ReadAllLines(trafficPath);

                StringBuilder sb = new StringBuilder();
                sb.Append(netLines[0]).Append(' ').AppendLine(trafficLines[0]);
                for (int l = 1; l < netLines.Length; ++l) { sb.AppendLine(netLines[l]); }
                for (int l = 1; l < trafficLines.Length; ++l) { sb.AppendLine(trafficLines[l]); }

                File.WriteAllText(instancePath + ".n" + netNums[0] + "e" + netNums[1] + "t" + trafficLines[0] + ".txt", sb.ToString());
            }
        }

        public class VRPTW2d {
            public static void convertAll() {
                convertDimacs("solomon.c101");
                convertDimacs("solomon.c102");
                convertDimacs("solomon.c103");
                convertDimacs("solomon.c104");
                convertDimacs("solomon.c105");
                convertDimacs("solomon.c106");
                convertDimacs("solomon.c107");
                convertDimacs("solomon.c108");
                convertDimacs("solomon.c109");
                convertDimacs("solomon.c201");
                convertDimacs("solomon.c202");
                convertDimacs("solomon.c203");
                convertDimacs("solomon.c204");
                convertDimacs("solomon.c205");
                convertDimacs("solomon.c206");
                convertDimacs("solomon.c207");
                convertDimacs("solomon.c208");
                convertDimacs("solomon.r101");
                convertDimacs("solomon.r102");
                convertDimacs("solomon.r103");
                convertDimacs("solomon.r104");
                convertDimacs("solomon.r105");
                convertDimacs("solomon.r106");
                convertDimacs("solomon.r107");
                convertDimacs("solomon.r108");
                convertDimacs("solomon.r109");
                convertDimacs("solomon.r110");
                convertDimacs("solomon.r111");
                convertDimacs("solomon.r112");
                convertDimacs("solomon.r201");
                convertDimacs("solomon.r202");
                convertDimacs("solomon.r203");
                convertDimacs("solomon.r204");
                convertDimacs("solomon.r205");
                convertDimacs("solomon.r206");
                convertDimacs("solomon.r207");
                convertDimacs("solomon.r208");
                convertDimacs("solomon.r209");
                convertDimacs("solomon.r210");
                convertDimacs("solomon.r211");
                convertDimacs("solomon.rc101");
                convertDimacs("solomon.rc102");
                convertDimacs("solomon.rc103");
                convertDimacs("solomon.rc104");
                convertDimacs("solomon.rc105");
                convertDimacs("solomon.rc106");
                convertDimacs("solomon.rc107");
                convertDimacs("solomon.rc108");
                convertDimacs("solomon.rc201");
                convertDimacs("solomon.rc202");
                convertDimacs("solomon.rc203");
                convertDimacs("solomon.rc204");
                convertDimacs("solomon.rc205");
                convertDimacs("solomon.rc206");
                convertDimacs("solomon.rc207");
                convertDimacs("solomon.rc208");
                convertDimacs("homberger.c10201");
                convertDimacs("homberger.c10202");
                convertDimacs("homberger.c10203");
                convertDimacs("homberger.c10204");
                convertDimacs("homberger.c10205");
                convertDimacs("homberger.c10206");
                convertDimacs("homberger.c10207");
                convertDimacs("homberger.c10208");
                convertDimacs("homberger.c10209");
                convertDimacs("homberger.c10210");
                convertDimacs("homberger.c10401");
                convertDimacs("homberger.c10402");
                convertDimacs("homberger.c10403");
                convertDimacs("homberger.c10404");
                convertDimacs("homberger.c10405");
                convertDimacs("homberger.c10406");
                convertDimacs("homberger.c10407");
                convertDimacs("homberger.c10408");
                convertDimacs("homberger.c10409");
                convertDimacs("homberger.c10410");
                convertDimacs("homberger.c10601");
                convertDimacs("homberger.c10602");
                convertDimacs("homberger.c10603");
                convertDimacs("homberger.c10604");
                convertDimacs("homberger.c10605");
                convertDimacs("homberger.c10606");
                convertDimacs("homberger.c10607");
                convertDimacs("homberger.c10608");
                convertDimacs("homberger.c10609");
                convertDimacs("homberger.c10610");
                convertDimacs("homberger.c10801");
                convertDimacs("homberger.c10802");
                convertDimacs("homberger.c10803");
                convertDimacs("homberger.c10804");
                convertDimacs("homberger.c10805");
                convertDimacs("homberger.c10806");
                convertDimacs("homberger.c10807");
                convertDimacs("homberger.c10808");
                convertDimacs("homberger.c10809");
                convertDimacs("homberger.c10810");
                convertDimacs("homberger.c11001");
                convertDimacs("homberger.c11002");
                convertDimacs("homberger.c11003");
                convertDimacs("homberger.c11004");
                convertDimacs("homberger.c11005");
                convertDimacs("homberger.c11006");
                convertDimacs("homberger.c11007");
                convertDimacs("homberger.c11008");
                convertDimacs("homberger.c11009");
                convertDimacs("homberger.c11010");
                convertDimacs("homberger.c20201");
                convertDimacs("homberger.c20202");
                convertDimacs("homberger.c20203");
                convertDimacs("homberger.c20204");
                convertDimacs("homberger.c20205");
                convertDimacs("homberger.c20206");
                convertDimacs("homberger.c20207");
                convertDimacs("homberger.c20208");
                convertDimacs("homberger.c20209");
                convertDimacs("homberger.c20210");
                convertDimacs("homberger.c20401");
                convertDimacs("homberger.c20402");
                convertDimacs("homberger.c20403");
                convertDimacs("homberger.c20404");
                convertDimacs("homberger.c20405");
                convertDimacs("homberger.c20406");
                convertDimacs("homberger.c20407");
                convertDimacs("homberger.c20408");
                convertDimacs("homberger.c20409");
                convertDimacs("homberger.c20410");
                convertDimacs("homberger.c20601");
                convertDimacs("homberger.c20602");
                convertDimacs("homberger.c20603");
                convertDimacs("homberger.c20604");
                convertDimacs("homberger.c20605");
                convertDimacs("homberger.c20606");
                convertDimacs("homberger.c20607");
                convertDimacs("homberger.c20608");
                convertDimacs("homberger.c20609");
                convertDimacs("homberger.c20610");
                convertDimacs("homberger.c20801");
                convertDimacs("homberger.c20802");
                convertDimacs("homberger.c20803");
                convertDimacs("homberger.c20804");
                convertDimacs("homberger.c20805");
                convertDimacs("homberger.c20806");
                convertDimacs("homberger.c20807");
                convertDimacs("homberger.c20808");
                convertDimacs("homberger.c20809");
                convertDimacs("homberger.c20810");
                convertDimacs("homberger.c21001");
                convertDimacs("homberger.c21002");
                convertDimacs("homberger.c21003");
                convertDimacs("homberger.c21004");
                convertDimacs("homberger.c21005");
                convertDimacs("homberger.c21006");
                convertDimacs("homberger.c21007");
                convertDimacs("homberger.c21008");
                convertDimacs("homberger.c21009");
                convertDimacs("homberger.c21010");
                convertDimacs("homberger.r10201");
                convertDimacs("homberger.r10202");
                convertDimacs("homberger.r10203");
                convertDimacs("homberger.r10204");
                convertDimacs("homberger.r10205");
                convertDimacs("homberger.r10206");
                convertDimacs("homberger.r10207");
                convertDimacs("homberger.r10208");
                convertDimacs("homberger.r10209");
                convertDimacs("homberger.r10210");
                convertDimacs("homberger.r10401");
                convertDimacs("homberger.r10402");
                convertDimacs("homberger.r10403");
                convertDimacs("homberger.r10404");
                convertDimacs("homberger.r10405");
                convertDimacs("homberger.r10406");
                convertDimacs("homberger.r10407");
                convertDimacs("homberger.r10408");
                convertDimacs("homberger.r10409");
                convertDimacs("homberger.r10410");
                convertDimacs("homberger.r10601");
                convertDimacs("homberger.r10602");
                convertDimacs("homberger.r10603");
                convertDimacs("homberger.r10604");
                convertDimacs("homberger.r10605");
                convertDimacs("homberger.r10606");
                convertDimacs("homberger.r10607");
                convertDimacs("homberger.r10608");
                convertDimacs("homberger.r10609");
                convertDimacs("homberger.r10610");
                convertDimacs("homberger.r10801");
                convertDimacs("homberger.r10802");
                convertDimacs("homberger.r10803");
                convertDimacs("homberger.r10804");
                convertDimacs("homberger.r10805");
                convertDimacs("homberger.r10806");
                convertDimacs("homberger.r10807");
                convertDimacs("homberger.r10808");
                convertDimacs("homberger.r10809");
                convertDimacs("homberger.r10810");
                convertDimacs("homberger.r11001");
                convertDimacs("homberger.r11002");
                convertDimacs("homberger.r11003");
                convertDimacs("homberger.r11004");
                convertDimacs("homberger.r11005");
                convertDimacs("homberger.r11006");
                convertDimacs("homberger.r11007");
                convertDimacs("homberger.r11008");
                convertDimacs("homberger.r11009");
                convertDimacs("homberger.r11010");
                convertDimacs("homberger.r20201");
                convertDimacs("homberger.r20202");
                convertDimacs("homberger.r20203");
                convertDimacs("homberger.r20204");
                convertDimacs("homberger.r20205");
                convertDimacs("homberger.r20206");
                convertDimacs("homberger.r20207");
                convertDimacs("homberger.r20208");
                convertDimacs("homberger.r20209");
                convertDimacs("homberger.r20210");
                convertDimacs("homberger.r20401");
                convertDimacs("homberger.r20402");
                convertDimacs("homberger.r20403");
                convertDimacs("homberger.r20404");
                convertDimacs("homberger.r20405");
                convertDimacs("homberger.r20406");
                convertDimacs("homberger.r20407");
                convertDimacs("homberger.r20408");
                convertDimacs("homberger.r20409");
                convertDimacs("homberger.r20410");
                convertDimacs("homberger.r20601");
                convertDimacs("homberger.r20602");
                convertDimacs("homberger.r20603");
                convertDimacs("homberger.r20604");
                convertDimacs("homberger.r20605");
                convertDimacs("homberger.r20606");
                convertDimacs("homberger.r20607");
                convertDimacs("homberger.r20608");
                convertDimacs("homberger.r20609");
                convertDimacs("homberger.r20610");
                convertDimacs("homberger.r20801");
                convertDimacs("homberger.r20802");
                convertDimacs("homberger.r20803");
                convertDimacs("homberger.r20804");
                convertDimacs("homberger.r20805");
                convertDimacs("homberger.r20806");
                convertDimacs("homberger.r20807");
                convertDimacs("homberger.r20808");
                convertDimacs("homberger.r20809");
                convertDimacs("homberger.r20810");
                convertDimacs("homberger.r21001");
                convertDimacs("homberger.r21002");
                convertDimacs("homberger.r21003");
                convertDimacs("homberger.r21004");
                convertDimacs("homberger.r21005");
                convertDimacs("homberger.r21006");
                convertDimacs("homberger.r21007");
                convertDimacs("homberger.r21008");
                convertDimacs("homberger.r21009");
                convertDimacs("homberger.r21010");
                convertDimacs("homberger.rc10201");
                convertDimacs("homberger.rc10202");
                convertDimacs("homberger.rc10203");
                convertDimacs("homberger.rc10204");
                convertDimacs("homberger.rc10205");
                convertDimacs("homberger.rc10206");
                convertDimacs("homberger.rc10207");
                convertDimacs("homberger.rc10208");
                convertDimacs("homberger.rc10209");
                convertDimacs("homberger.rc10210");
                convertDimacs("homberger.rc10401");
                convertDimacs("homberger.rc10402");
                convertDimacs("homberger.rc10403");
                convertDimacs("homberger.rc10404");
                convertDimacs("homberger.rc10405");
                convertDimacs("homberger.rc10406");
                convertDimacs("homberger.rc10407");
                convertDimacs("homberger.rc10408");
                convertDimacs("homberger.rc10409");
                convertDimacs("homberger.rc10410");
                convertDimacs("homberger.rc10601");
                convertDimacs("homberger.rc10602");
                convertDimacs("homberger.rc10603");
                convertDimacs("homberger.rc10604");
                convertDimacs("homberger.rc10605");
                convertDimacs("homberger.rc10606");
                convertDimacs("homberger.rc10607");
                convertDimacs("homberger.rc10608");
                convertDimacs("homberger.rc10609");
                convertDimacs("homberger.rc10610");
                convertDimacs("homberger.rc10801");
                convertDimacs("homberger.rc10802");
                convertDimacs("homberger.rc10803");
                convertDimacs("homberger.rc10804");
                convertDimacs("homberger.rc10805");
                convertDimacs("homberger.rc10806");
                convertDimacs("homberger.rc10807");
                convertDimacs("homberger.rc10808");
                convertDimacs("homberger.rc10809");
                convertDimacs("homberger.rc10810");
                convertDimacs("homberger.rc11001");
                convertDimacs("homberger.rc11002");
                convertDimacs("homberger.rc11003");
                convertDimacs("homberger.rc11004");
                convertDimacs("homberger.rc11005");
                convertDimacs("homberger.rc11006");
                convertDimacs("homberger.rc11007");
                convertDimacs("homberger.rc11008");
                convertDimacs("homberger.rc11009");
                convertDimacs("homberger.rc11010");
                convertDimacs("homberger.rc20201");
                convertDimacs("homberger.rc20202");
                convertDimacs("homberger.rc20203");
                convertDimacs("homberger.rc20204");
                convertDimacs("homberger.rc20205");
                convertDimacs("homberger.rc20206");
                convertDimacs("homberger.rc20207");
                convertDimacs("homberger.rc20208");
                convertDimacs("homberger.rc20209");
                convertDimacs("homberger.rc20210");
                convertDimacs("homberger.rc20401");
                convertDimacs("homberger.rc20402");
                convertDimacs("homberger.rc20403");
                convertDimacs("homberger.rc20404");
                convertDimacs("homberger.rc20405");
                convertDimacs("homberger.rc20406");
                convertDimacs("homberger.rc20407");
                convertDimacs("homberger.rc20408");
                convertDimacs("homberger.rc20409");
                convertDimacs("homberger.rc20410");
                convertDimacs("homberger.rc20601");
                convertDimacs("homberger.rc20602");
                convertDimacs("homberger.rc20603");
                convertDimacs("homberger.rc20604");
                convertDimacs("homberger.rc20605");
                convertDimacs("homberger.rc20606");
                convertDimacs("homberger.rc20607");
                convertDimacs("homberger.rc20608");
                convertDimacs("homberger.rc20609");
                convertDimacs("homberger.rc20610");
                convertDimacs("homberger.rc20801");
                convertDimacs("homberger.rc20802");
                convertDimacs("homberger.rc20803");
                convertDimacs("homberger.rc20804");
                convertDimacs("homberger.rc20805");
                convertDimacs("homberger.rc20806");
                convertDimacs("homberger.rc20807");
                convertDimacs("homberger.rc20808");
                convertDimacs("homberger.rc20809");
                convertDimacs("homberger.rc20810");
                convertDimacs("homberger.rc21001");
                convertDimacs("homberger.rc21002");
                convertDimacs("homberger.rc21003");
                convertDimacs("homberger.rc21004");
                convertDimacs("homberger.rc21005");
                convertDimacs("homberger.rc21006");
                convertDimacs("homberger.rc21007");
                convertDimacs("homberger.rc21008");
                convertDimacs("homberger.rc21009");
                convertDimacs("homberger.rc21010");
            }

            static void convertDimacs(string oldPath) {
                string[] lines = File.ReadAllLines(oldPath + ".txt");

                string[] words = lines[4].Split(Checker.InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                string vehicleNum = words[0];
                string vehicleCapacity = words[1];

                int nodeNum = 0;
                StringBuilder sb = new StringBuilder();
                for (int l = 9; l < lines.Length; ++l) {
                    words = lines[l].Split(Checker.InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length < 7) { continue; }
                    ++nodeNum;
                    sb.Append(words[1]).Append(' ').Append(words[2]).Append(' ')
                        .Append(words[3]).Append(' ').Append(words[6]).Append(' ')
                        .Append(words[4]).Append(' ').AppendLine(words[5]);
                }

                StringBuilder header = new StringBuilder();
                header.Append(nodeNum).Append(' ').Append(vehicleNum).Append(' ').AppendLine(vehicleCapacity);
                StringBuilder fn = new StringBuilder();
                fn.Append(".n").Append(nodeNum).Append('v').Append(vehicleNum).Append('c').Append(vehicleCapacity).Append(".txt");
                File.WriteAllText(oldPath + fn.ToString(), header.ToString() + sb.ToString());
            }
        }

        public class OARSMT {
            public static void convertAll() {
                convertDimacs(@"Bonn_109_101");
                convertDimacs(@"Bonn_23292_54");
                convertDimacs(@"Bonn_35574_158");
                convertDimacs(@"Bonn_46269_127");
                convertDimacs(@"Bonn_108500_141");
                convertDimacs(@"Bonn_129399_210");
                convertDimacs(@"Bonn_639639_382");
                convertDimacs(@"Bonn_783352_175");
                convertDimacs(@"IND1");
                convertDimacs(@"IND2");
                convertDimacs(@"IND3");
                convertDimacs(@"IND4");
                convertDimacs(@"IND5");
                convertDimacs(@"RC01");
                convertDimacs(@"RC02");
                convertDimacs(@"RC03");
                convertDimacs(@"RC04");
                convertDimacs(@"RC05");
                convertDimacs(@"RC06");
                convertDimacs(@"RC07");
                convertDimacs(@"RC08");
                convertDimacs(@"RC09");
                convertDimacs(@"RC10");
                convertDimacs(@"RC11");
                convertDimacs(@"RC12");
                convertDimacs(@"RL01");
                convertDimacs(@"RL02");
                convertDimacs(@"RL03");
                convertDimacs(@"RL04");
                convertDimacs(@"RL05");
                convertDimacs(@"RT01");
                convertDimacs(@"RT02");
                convertDimacs(@"RT03");
                convertDimacs(@"RT04");
                convertDimacs(@"RT05");
            }

            static void convertDimacs(string oldPath) {
                string[] lines = File.ReadAllLines(oldPath + ".stp");
                string nodeNum = "";
                string obstacleNum = "";
                StringBuilder sb = new StringBuilder();
                for (int l = 0; l < lines.Length; ++l) {
                    string[] words = lines[l].Split(Checker.InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length < 2) { continue; }
                    if (words[0] == "DD") {
                        sb.Append(words[2]).Append(' ').AppendLine(words[3]);
                    } else if (words[0] == "RR") {
                        sb.Append(int.Parse(words[1]) + 1).Append(' ').Append(int.Parse(words[2]) + 1).Append(' ')
                            .Append(int.Parse(words[3]) - 1).Append(' ').Append(int.Parse(words[4]) - 1).Append('\n');
                    } else if (words[0] == "Nodes") {
                        nodeNum = words[1];
                    } else if (words[0] == "Obstacles") {
                        obstacleNum = words[1];
                    }
                }

                StringBuilder header = new StringBuilder();
                header.Append(nodeNum).Append(' ').AppendLine(obstacleNum);
                StringBuilder fn = new StringBuilder();
                fn.Append(".n").Append(nodeNum).Append('o').Append(obstacleNum).Append(".txt");
                File.WriteAllText(oldPath.subStr(0, '_').ToLower() + fn.ToString(), header.ToString() + sb.ToString());
            }
        }

        public class DFVSP {
            public static void convertAll() {
                convertPardalos(@"P50-100");
                convertPardalos(@"P50-150");
                convertPardalos(@"P50-200");
                convertPardalos(@"P50-250");
                convertPardalos(@"P50-300");
                convertPardalos(@"P50-500");
                convertPardalos(@"P50-600");
                convertPardalos(@"P50-700");
                convertPardalos(@"P50-800");
                convertPardalos(@"P50-900");
                convertPardalos(@"P100-200");
                convertPardalos(@"P100-300");
                convertPardalos(@"P100-400");
                convertPardalos(@"P100-500");
                convertPardalos(@"P100-600");
                convertPardalos(@"P100-1000");
                convertPardalos(@"P100-1100");
                convertPardalos(@"P100-1200");
                convertPardalos(@"P100-1300");
                convertPardalos(@"P100-1400");
                convertPardalos(@"P500-1000");
                convertPardalos(@"P500-1500");
                convertPardalos(@"P500-2000");
                convertPardalos(@"P500-2500");
                convertPardalos(@"P500-3000");
                convertPardalos(@"P500-5000");
                convertPardalos(@"P500-5500");
                convertPardalos(@"P500-6000");
                convertPardalos(@"P500-6500");
                convertPardalos(@"P500-7000");
                convertPardalos(@"P1000-3000");
                convertPardalos(@"P1000-3500");
                convertPardalos(@"P1000-4000");
                convertPardalos(@"P1000-4500");
                convertPardalos(@"P1000-5000");
                convertPardalos(@"P1000-10000");
                convertPardalos(@"P1000-15000");
                convertPardalos(@"P1000-20000");
                convertPardalos(@"P1000-25000");
                convertPardalos(@"P1000-30000");
                convertPace(@"h_001");
                convertPace(@"h_003");
                convertPace(@"h_005");
                convertPace(@"h_007");
                convertPace(@"h_009");
                convertPace(@"h_011");
                convertPace(@"h_013");
                convertPace(@"h_015");
                convertPace(@"h_017");
                convertPace(@"h_019");
                convertPace(@"h_021");
                convertPace(@"h_023");
                convertPace(@"h_025");
                convertPace(@"h_027");
                convertPace(@"h_029");
                convertPace(@"h_031");
                convertPace(@"h_033");
                convertPace(@"h_035");
                convertPace(@"h_037");
                convertPace(@"h_039");
                convertPace(@"h_041");
                convertPace(@"h_043");
                convertPace(@"h_045");
                convertPace(@"h_047");
                convertPace(@"h_049");
                convertPace(@"h_051");
                convertPace(@"h_053");
                convertPace(@"h_055");
                convertPace(@"h_057");
                convertPace(@"h_059");
                convertPace(@"h_061");
                convertPace(@"h_063");
                convertPace(@"h_065");
                convertPace(@"h_067");
                convertPace(@"h_069");
                convertPace(@"h_071");
                convertPace(@"h_073");
                convertPace(@"h_075");
                convertPace(@"h_077");
                convertPace(@"h_079");
                convertPace(@"h_081");
                convertPace(@"h_083");
                convertPace(@"h_085");
                convertPace(@"h_087");
                convertPace(@"h_089");
                convertPace(@"h_091");
                convertPace(@"h_093");
                convertPace(@"h_095");
                convertPace(@"h_097");
                convertPace(@"h_099");
                convertPace(@"h_101");
                convertPace(@"h_103");
                convertPace(@"h_105");
                convertPace(@"h_107");
                convertPace(@"h_109");
                convertPace(@"h_111");
                convertPace(@"h_113");
                convertPace(@"h_115");
                convertPace(@"h_117");
                convertPace(@"h_119");
                convertPace(@"h_121");
                convertPace(@"h_123");
                convertPace(@"h_125");
                convertPace(@"h_127");
                convertPace(@"h_129");
                convertPace(@"h_131");
                convertPace(@"h_133");
                convertPace(@"h_135");
                convertPace(@"h_137");
                convertPace(@"h_139");
                convertPace(@"h_141");
                convertPace(@"h_143");
                convertPace(@"h_145");
                convertPace(@"h_147");
                convertPace(@"h_149");
                convertPace(@"h_151");
                convertPace(@"h_153");
                convertPace(@"h_155");
                convertPace(@"h_157");
                convertPace(@"h_159");
                convertPace(@"h_161");
                convertPace(@"h_163");
                convertPace(@"h_165");
                convertPace(@"h_167");
                convertPace(@"h_169");
                convertPace(@"h_171");
                convertPace(@"h_173");
                convertPace(@"h_175");
                convertPace(@"h_177");
                convertPace(@"h_179");
                convertPace(@"h_181");
                convertPace(@"h_183");
                convertPace(@"h_185");
                convertPace(@"h_187");
                convertPace(@"h_189");
                convertPace(@"h_191");
                convertPace(@"h_193");
                convertPace(@"h_195");
                convertPace(@"h_197");
                convertPace(@"h_199");
                convertPace(@"e_001");
                convertPace(@"e_003");
                convertPace(@"e_005");
                convertPace(@"e_007");
                convertPace(@"e_009");
                convertPace(@"e_011");
                convertPace(@"e_013");
                convertPace(@"e_015");
                convertPace(@"e_017");
                convertPace(@"e_019");
                convertPace(@"e_021");
                convertPace(@"e_023");
                convertPace(@"e_025");
                convertPace(@"e_027");
                convertPace(@"e_029");
                convertPace(@"e_031");
                convertPace(@"e_033");
                convertPace(@"e_035");
                convertPace(@"e_037");
                convertPace(@"e_039");
                convertPace(@"e_041");
                convertPace(@"e_043");
                convertPace(@"e_045");
                convertPace(@"e_047");
                convertPace(@"e_049");
                convertPace(@"e_051");
                convertPace(@"e_053");
                convertPace(@"e_055");
                convertPace(@"e_057");
                convertPace(@"e_059");
                convertPace(@"e_061");
                convertPace(@"e_063");
                convertPace(@"e_065");
                convertPace(@"e_067");
                convertPace(@"e_069");
                convertPace(@"e_071");
                convertPace(@"e_073");
                convertPace(@"e_075");
                convertPace(@"e_077");
                convertPace(@"e_079");
                convertPace(@"e_081");
                convertPace(@"e_083");
                convertPace(@"e_085");
                convertPace(@"e_087");
                convertPace(@"e_089");
                convertPace(@"e_091");
                convertPace(@"e_093");
                convertPace(@"e_095");
                convertPace(@"e_097");
                convertPace(@"e_099");
                convertPace(@"e_101");
                convertPace(@"e_103");
                convertPace(@"e_105");
                convertPace(@"e_107");
                convertPace(@"e_109");
                convertPace(@"e_111");
                convertPace(@"e_113");
                convertPace(@"e_115");
                convertPace(@"e_117");
                convertPace(@"e_119");
                convertPace(@"e_121");
                convertPace(@"e_123");
                convertPace(@"e_125");
                convertPace(@"e_127");
                convertPace(@"e_129");
                convertPace(@"e_131");
                convertPace(@"e_133");
                convertPace(@"e_135");
                convertPace(@"e_137");
                convertPace(@"e_139");
                convertPace(@"e_141");
                convertPace(@"e_143");
                convertPace(@"e_145");
                convertPace(@"e_147");
                convertPace(@"e_149");
                convertPace(@"e_151");
                convertPace(@"e_153");
                convertPace(@"e_155");
                convertPace(@"e_157");
                convertPace(@"e_159");
                convertPace(@"e_161");
                convertPace(@"e_163");
                convertPace(@"e_165");
                convertPace(@"e_167");
                convertPace(@"e_169");
                convertPace(@"e_171");
                convertPace(@"e_173");
                convertPace(@"e_175");
                convertPace(@"e_177");
                convertPace(@"e_179");
                convertPace(@"e_181");
                convertPace(@"e_183");
                convertPace(@"e_185");
                convertPace(@"e_187");
                convertPace(@"e_189");
                convertPace(@"e_191");
                convertPace(@"e_193");
                convertPace(@"e_195");
                convertPace(@"e_197");
                convertPace(@"e_199");
            }

            static void convertPardalos(string oldPath) {
                string[] lines = File.ReadAllLines(oldPath + ".dat");
                string[] words = lines[0].Split(Checker.InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                string nodeNum = words[0];
                string arcNum = words[1];
                List<int>[] adjList = new List<int>[int.Parse(nodeNum)];
                for (int i = 0; i < adjList.Length; ++i) { adjList[i] = new List<int>(); }
                for (int l = 1; l < lines.Length; ++l) {
                    words = lines[l].Split(Checker.InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                    adjList[int.Parse(words[0]) - 1].Add(int.Parse(words[1]) - 1);
                }

                StringBuilder sb = new StringBuilder();
                sb.Append(nodeNum).Append(' ').AppendLine(arcNum);
                for (int i = 0; i < adjList.Length; ++i) {
                    if (adjList[i].Count > 0) {
                        foreach (var n in adjList[i]) {
                            sb.Append(n).Append(' ');
                        }
                        --sb.Length;
                    }
                    sb.Append('\n');
                }

                StringBuilder fn = new StringBuilder();
                fn.Append("pardalos.n").Append(nodeNum).Append('e').Append(arcNum).Append(".txt");
                File.WriteAllText(fn.ToString(), sb.ToString());
            }

            static void convertPace(string oldPath) {
                string[] lines = File.ReadAllLines(oldPath);
                string[] words = lines[0].Split(Checker.InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                string nodeNum = words[0];
                string arcNum = words[1];
                List<int>[] adjList = new List<int>[int.Parse(nodeNum)];
                for (int i = 0; i < adjList.Length; ++i) { adjList[i] = new List<int>(); }
                for (int l = 1; l < lines.Length; ++l) {
                    words = lines[l].Split(Checker.InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var w in words) { adjList[l - 1].Add(int.Parse(w) - 1); }
                }

                StringBuilder sb = new StringBuilder();
                sb.Append(nodeNum).Append(' ').AppendLine(arcNum);
                for (int i = 0; i < adjList.Length; ++i) {
                    if (adjList[i].Count > 0) {
                        foreach (var n in adjList[i]) {
                            sb.Append(n).Append(' ');
                        }
                        --sb.Length;
                    }
                    sb.Append('\n');
                }

                StringBuilder fn = new StringBuilder();
                fn.Append("pace.").Append(Path.GetFileName(oldPath).Replace("_", ""))
                    .Append(".n").Append(nodeNum).Append('e').Append(arcNum).Append(".txt");
                File.WriteAllText(fn.ToString(), sb.ToString());
            }
        }

        public class MCDSP {
            public static void convertAll() {
                convertEdgeList("n1000_ep0007.rg", "BOBL");
                convertEdgeList("n1000_ep0014.rg", "BOBL");
                convertEdgeList("n1000_ep0028.rg", "BOBL");
                convertEdgeList("n1000_ep0056.rg", "BOBL");
                convertEdgeList("n1000_ep0112.rg", "BOBL");
                convertEdgeList("n1000_ep0224.rg", "BOBL");
                convertEdgeList("n1000_r0048.rgg", "BOBL");
                convertEdgeList("n1000_r0070.rgg", "BOBL");
                convertEdgeList("n1000_r0100.rgg", "BOBL");
                convertEdgeList("n1000_r0140.rgg", "BOBL");
                convertEdgeList("n1000_r0207.rgg", "BOBL");
                convertEdgeList("n1000_r0308.rgg", "BOBL");
                convertEdgeList("n5000_ep0007.rg", "BOBL");
                convertEdgeList("n5000_ep0014.rg", "BOBL");
                convertEdgeList("n5000_ep0028.rg", "BOBL");
                convertEdgeList("n5000_ep0056.rg", "BOBL");
                convertEdgeList("n5000_ep0112.rg", "BOBL");
                convertEdgeList("n5000_ep0224.rg", "BOBL");
                convertEdgeList("n5000_r0048.rgg", "BOBL");
                convertEdgeList("n5000_r0070.rgg", "BOBL");
                convertEdgeList("n5000_r0100.rgg", "BOBL");
                convertEdgeList("n5000_r0140.rgg", "BOBL");
                convertEdgeList("n5000_r0207.rgg", "BOBL");
                convertEdgeList("n5000_r0308.rgg", "BOBL");

                convertEdgeList("ieee_14_bus.txt", "BPFTC");
                convertEdgeList("ieee_30_bus.txt", "BPFTC");
                convertEdgeList("ieee_57_bus.txt", "BPFTC");
                convertEdgeList("ieee_118_bus.txt", "BPFTC");
                convertEdgeList("ieee_300_bus.txt", "BPFTC");
                convertEdgeList("rts96.txt", "BPFTC");

                convertEdgeList("v30_d10.dat", "LMS");
                convertEdgeList("v30_d20.dat", "LMS");
                convertEdgeList("v30_d30.dat", "LMS");
                convertEdgeList("v30_d50.dat", "LMS");
                convertEdgeList("v30_d70.dat", "LMS");
                convertEdgeList("v50_d5.dat", "LMS");
                convertEdgeList("v50_d10.dat", "LMS");
                convertEdgeList("v50_d20.dat", "LMS");
                convertEdgeList("v50_d30.dat", "LMS");
                convertEdgeList("v50_d50.dat", "LMS");
                convertEdgeList("v50_d70.dat", "LMS");
                convertEdgeList("v70_d5.dat", "LMS");
                convertEdgeList("v70_d10.dat", "LMS");
                convertEdgeList("v70_d20.dat", "LMS");
                convertEdgeList("v70_d30.dat", "LMS");
                convertEdgeList("v70_d50.dat", "LMS");
                convertEdgeList("v70_d70.dat", "LMS");
                convertEdgeList("v100_d5.dat", "LMS");
                convertEdgeList("v100_d10.dat", "LMS");
                convertEdgeList("v100_d20.dat", "LMS");
                convertEdgeList("v100_d30.dat", "LMS");
                convertEdgeList("v100_d50.dat", "LMS");
                convertEdgeList("v100_d70.dat", "LMS");
                convertEdgeList("v120_d5.dat", "LMS");
                convertEdgeList("v120_d10.dat", "LMS");
                convertEdgeList("v120_d20.dat", "LMS");
                convertEdgeList("v120_d30.dat", "LMS");
                convertEdgeList("v120_d50.dat", "LMS");
                convertEdgeList("v120_d70.dat", "LMS");
                convertEdgeList("v150_d5.dat", "LMS");
                convertEdgeList("v150_d10.dat", "LMS");
                convertEdgeList("v150_d20.dat", "LMS");
                convertEdgeList("v150_d30.dat", "LMS");
                convertEdgeList("v150_d50.dat", "LMS");
                convertEdgeList("v150_d70.dat", "LMS");
                convertEdgeList("v200_d5.dat", "LMS");
                convertEdgeList("v200_d10.dat", "LMS");
                convertEdgeList("v200_d20.dat", "LMS");
                convertEdgeList("v200_d30.dat", "LMS");
                convertEdgeList("v200_d50.dat", "LMS");
                convertEdgeList("v200_d70.dat", "LMS");

                convertEdgeList("rand_40_200_1235844018_0_k=5_sat.gph", "LPNMR");
                convertEdgeList("rand_45_250_1235855661_0_k=5_sat.gph", "LPNMR");
                convertEdgeList("rand_50_250_1235336870_0_k=8_sat.gph", "LPNMR");
                convertEdgeList("rand_50_250_1235842610_0_k=7_sat.gph", "LPNMR");
                convertEdgeList("rand_55_250_1235855873_0_k=8_sat.gph", "LPNMR");
                convertEdgeList("rand_60_400_1235843083_0_k=7_sat.gph", "LPNMR");
                convertEdgeList("rand_70_250_1235844252_0_k=13_sat.gph", "LPNMR");
                convertEdgeList("rand_80_500_1235848939_0_k=9_sat.gph", "LPNMR");
                convertEdgeList("rand_90_600_1235859689_0_k=10_sat.gph", "LPNMR");

                convertEdgeList("n400_80_r60.txt", "RGG");
                convertEdgeList("n400_80_r70.txt", "RGG");
                convertEdgeList("n400_80_r80.txt", "RGG");
                convertEdgeList("n400_80_r90.txt", "RGG");
                convertEdgeList("n400_80_r100.txt", "RGG");
                convertEdgeList("n400_80_r110.txt", "RGG");
                convertEdgeList("n400_80_r120.txt", "RGG");
                convertEdgeList("n600_100_r80.txt", "RGG");
                convertEdgeList("n600_100_r90.txt", "RGG");
                convertEdgeList("n600_100_r100.txt", "RGG");
                convertEdgeList("n600_100_r110.txt", "RGG");
                convertEdgeList("n600_100_r120.txt", "RGG");
                convertEdgeList("n700_200_r70.txt", "RGG");
                convertEdgeList("n700_200_r80.txt", "RGG");
                convertEdgeList("n700_200_r90.txt", "RGG");
                convertEdgeList("n700_200_r100.txt", "RGG");
                convertEdgeList("n700_200_r110.txt", "RGG");
                convertEdgeList("n700_200_r120.txt", "RGG");
                convertEdgeList("n1000_200_r100.txt", "RGG");
                convertEdgeList("n1000_200_r110.txt", "RGG");
                convertEdgeList("n1000_200_r120.txt", "RGG");
                convertEdgeList("n1000_200_r130.txt", "RGG");
                convertEdgeList("n1000_200_r140.txt", "RGG");
                convertEdgeList("n1000_200_r150.txt", "RGG");
                convertEdgeList("n1000_200_r160.txt", "RGG");
                convertEdgeList("n1500_250_r130.txt", "RGG");
                convertEdgeList("n1500_250_r140.txt", "RGG");
                convertEdgeList("n1500_250_r150.txt", "RGG");
                convertEdgeList("n1500_250_r160.txt", "RGG");
                convertEdgeList("n2000_300_r200.txt", "RGG");
                convertEdgeList("n2000_300_r210.txt", "RGG");
                convertEdgeList("n2000_300_r220.txt", "RGG");
                convertEdgeList("n2000_300_r230.txt", "RGG");
                convertEdgeList("n2500_350_r200.txt", "RGG");
                convertEdgeList("n2500_350_r210.txt", "RGG");
                convertEdgeList("n2500_350_r220.txt", "RGG");
                convertEdgeList("n2500_350_r230.txt", "RGG");
                convertEdgeList("n3000_400_r210.txt", "RGG");
                convertEdgeList("n3000_400_r220.txt", "RGG");
                convertEdgeList("n3000_400_r230.txt", "RGG");
                convertEdgeList("n3000_400_r240.txt", "RGG");

                convertEdgeList("sparse-n1000-np300.txt", "Sparse");
                convertEdgeList("sparse-n1000-np500.txt", "Sparse");
                convertEdgeList("sparse-n1000-np700.txt", "Sparse");
                convertEdgeList("sparse-n1500-np500.txt", "Sparse");
                convertEdgeList("sparse-n1500-np700.txt", "Sparse");
                convertEdgeList("sparse-n1500-np900.txt", "Sparse");
                convertEdgeList("sparse-n2000-np600.txt", "Sparse");
                convertEdgeList("sparse-n2000-np800.txt", "Sparse");
                convertEdgeList("sparse-n2000-np1000.txt", "Sparse");
                convertEdgeList("sparse-n2500-np800.txt", "Sparse");
                convertEdgeList("sparse-n2500-np1000.txt", "Sparse");
                convertEdgeList("sparse-n2500-np1200.txt", "Sparse");
                convertEdgeList("sparse-n3000-np1000.txt", "Sparse");
                convertEdgeList("sparse-n3000-np1300.txt", "Sparse");
                convertEdgeList("sparse-n3000-np1600.txt", "Sparse");
            }

            static void convertEdgeList(string oldPath, string dataset) {
                string[] lines = File.ReadAllLines(oldPath);
                string[] words = lines[0].Split(Checker.InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                string nodeNum = words[0];
                string arcNum = words[1];
                StringBuilder sb = new StringBuilder();
                sb.Append(nodeNum).Append(' ').Append(arcNum).Append('\n');
                for (int l = 1; l < lines.Length; ++l) {
                    if (lines[l].Length <= 0) { continue; }
                    words = lines[l].Split(Checker.InlineDelimiters, StringSplitOptions.RemoveEmptyEntries);
                    sb.Append(int.Parse(words[0]) - 1).Append(' ').Append(int.Parse(words[1]) - 1).Append('\n');
                }

                StringBuilder fn = new StringBuilder();
                fn.Append(dataset).Append(".n").Append(nodeNum).Append('e').Append(arcNum).Append(".txt");
                File.WriteAllText(fn.ToString(), sb.ToString());
                Console.WriteLine(oldPath + ' ' + fn.ToString());
            }
        }

        public class PECCP {
            public static void convertAll() {
                for (int n = 50; n < 250; ++n) { File.WriteAllText($"n{n}.txt", $"{n} 1e-15"); }
                for (int n = 950; n < 1050; ++n) { File.WriteAllText($"n{n}.txt", $"{n} 1e-15"); }
                for (int n = 1950; n < 2050; ++n) { File.WriteAllText($"n{n}.txt", $"{n} 1e-15"); }
            }
        }
    }
}
