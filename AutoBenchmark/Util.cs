using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;


namespace AutoBenchmark {
    public static class Util {
        public static void log(string msg) { // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated
            Console.WriteLine($"{friendlyDateTime()} {msg}");
        }

        #region String
        public static string toString(this MemoryStream ms) {
            ms.Position = 0;
            using (StreamReader sr = new StreamReader(ms)) { return sr.ReadToEnd(); }
        }

        public static string removeSuffix(this string str, string suffix) {
            return (str.EndsWith(suffix) ? str.Substring(0, str.Length - suffix.Length) : str);
        }

        public static string quote(this string str) { return "\"" + str + "\""; }

        public static string subStr(this string str, int beginIndex, int endIndex) {
            return str.Substring(beginIndex, endIndex - beginIndex);
        }


        public static void save(this Attachment attachment, string filePath) {
            using (FileStream fs = File.Create(filePath)) {
                attachment.ContentStream.CopyTo(fs);
            }
        }
        public static string toString(this Attachment attachment) {
            using (MemoryStream ms = new MemoryStream()) {
                attachment.ContentStream.CopyTo(ms);
                return ms.toString();
            }
        }
        #endregion String

        #region Serialization
        public static Encoding getEncoding(int codePage = 936) {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding(codePage);
        }

        public static void appendText(string path, string contents) {
            File.AppendAllText(path, contents, CommonCfg.DefaultEncoding);
        }
        public static void appendLine(string path, string contents) {
            File.AppendAllText(path, contents + Environment.NewLine, CommonCfg.DefaultEncoding);
        }
        public static void appendLines(string path, IEnumerable<string> contents) {
            File.AppendAllLines(path, contents, CommonCfg.DefaultEncoding);
        }
        public static void writeText(string path, string contents) {
            File.WriteAllText(path, contents, CommonCfg.DefaultEncoding);
        }
        public static void writeLine(string path, string contents) {
            File.WriteAllText(path, contents + Environment.NewLine, CommonCfg.DefaultEncoding);
        }
        public static void writeLines(string path, IEnumerable<string> contents) {
            File.WriteAllLines(path, contents, CommonCfg.DefaultEncoding);
        }

        public static class Json {
            static readonly DataContractJsonSerializerSettings JsonSettings = new DataContractJsonSerializerSettings {
                UseSimpleDictionaryFormat = true
            };

            public static void save<T>(string path, T obj) {
                using (FileStream fs = File.Open(path,
                    FileMode.Create, FileAccess.Write, FileShare.Read)) {
                    serialize<T>(fs, obj);
                }
            }

            public static T load<T>(string path) where T : new() {
                if (!File.Exists(path)) { return new T(); }
                using (FileStream fs = File.Open(path,
                    FileMode.Open, FileAccess.Read, FileShare.Read)) {
                    return deserialize<T>(fs);
                }
            }

            public static void serialize<T>(Stream stream, T obj) {
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T), JsonSettings);
                js.WriteObject(stream, obj);
            }

            public static T deserialize<T>(Stream stream) {
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T), JsonSettings);
                return (T)js.ReadObject(stream);
            }

            public static string toJsonString<T>(T obj) {
                using (MemoryStream ms = new MemoryStream()) {
                    serialize(ms, obj);
                    return ms.toString();
                }
            }

            public static T fromJsonString<T>(string json) {
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json))) {
                    return deserialize<T>(ms);
                }
            }
        }
        #endregion Serialization

        #region FileSystem
        public delegate void ManipulateFile(FileInfo file);
        public delegate bool ManipulateDir(DirectoryInfo dir); // return false if there is no need to look into it.


        public static void traverseDirectory(string dirPath, ManipulateFile fileOp, ManipulateDir dirOp) {
            DirectoryInfo root = new DirectoryInfo(dirPath);
            traverseDirectory(root, fileOp, dirOp);
        }

        public static void traverseDirectory(DirectoryInfo root, ManipulateFile fileOp, ManipulateDir dirOp) {
            foreach (var dir in root.GetDirectories()) {
                if (dirOp(dir)) { traverseDirectory(dir.FullName, fileOp, dirOp); }
            }
            foreach (var file in root.GetFiles()) { fileOp(file); }
        }

        public static List<string> listFiles(string dir, bool recursive = false) {
            List<string> fileList = new List<string>();
            traverseDirectory(dir, (f) => {
                fileList.Add(f.Name);
            }, (d) => { return recursive; });
            return fileList;
        }

        public static void moveFile(string sourceFileName, string destFileName) {
            if (!File.Exists(sourceFileName)) { return; }
            if (File.Exists(destFileName)) { File.Delete(destFileName); }
            File.Move(sourceFileName, destFileName);
        }
        #endregion FileSystem

        #region Process
        // [Blocking][NoWindow][InterceptOutput]
        public static string runRead(string fileName, string arguments = "") {
            ProcessStartInfo psi = new ProcessStartInfo(fileName, arguments);
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            Process p = Process.Start(psi);
            p.WaitForExit();
            return p.StandardOutput.ReadToEnd();
        }
        // [NonBlocking][NoWindow]
        public static Process runAsync(string fileName, string arguments = "") {
            ProcessStartInfo psi = new ProcessStartInfo(fileName, arguments);
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            return Process.Start(psi);
        }
        // [Blocking][NoWindow][GetExitCode]
        public static int run(string fileName, string arguments = "") {
            Process p = runAsync(fileName, arguments);
            p.WaitForExit();
            return p.ExitCode;
        }
        // [Blocking][ShowWindow]
        public static Process runUI(string fileName, string arguments = "") {
            Process p = Process.Start(fileName, arguments);
            p.WaitForExit();
            return p;
        }
        // [NonBlocking][ShowWindow]
        public static Process runUIAsync(string fileName, string arguments = "") {
            return Process.Start(fileName, arguments);
        }
        #endregion Process

        #region Thread
        public delegate bool IsTaskTaken();
        public delegate void UserTask(IsTaskTaken isTaskTaken);

        /// <summary> workers scramble for tasks. </summary>
        /// <remarks>call `isTaskTaken` in `userTask` to avoid repeating.</remarks>
        /// <example>
        /// HashSet<int> s = new HashSet<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        /// Util.TaskTaker.run(4, (isTaskTaken) => {
        ///     foreach (var i in s) {
        ///         if (isTaskTaken()) { continue; }
        ///         Console.WriteLine(i);
        ///         Thread.Sleep(2000);
        ///     }
        /// });
        /// </example>
        public static void scrambleForTasks(int workerNum, UserTask userTask) {
            Thread[] workers = new Thread[workerNum];
            int gt = 0; // global task index.
            for (int w = 0; w < workerNum; ++w) {
                workers[w] = new Thread(() => {
                    int t = 0; // local task index.
                    userTask(() => {
                        if (t < gt) { ++t; return true; }
                        int ogt = gt; // old global task index.
                        return ogt != Interlocked.CompareExchange(ref gt, ogt + 1, t++);
                    });
                });
                workers[w].Start();
            }
            for (int w = 0; w < workerNum; ++w) { workers[w].Join(); }
        }
        #endregion Thread

        #region DateTime
        // https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
        public static string compactDateTime(DateTime dt) { return dt.ToString("yyyyMMddHHmmss"); }
        public static string compactDateTime() { return compactDateTime(DateTime.Now); }
        public static string friendlyDateTime(DateTime dt) { return dt.ToString("yyyy-MM-dd HH:mm:ss.fff"); }
        public static string friendlyDateTime() { return friendlyDateTime(DateTime.Now); }
        #endregion DateTime

        #region Container
        public static void pop<T>(this List<T> l) { l.RemoveAt(l.Count - 1); }

        public static void tryInc<TKey>(IDictionary<TKey, int> dictionary, TKey key) {
            dictionary.TryAdd(key, 0);
            ++dictionary[key];
        }
        public static void tryDec<TKey>(IDictionary<TKey, int> dictionary, TKey key) {
            if (--dictionary[key] <= 0) { dictionary.Remove(key); }
        }
        #endregion Container

        public class OppositeComparer<T> : IComparer<T> {
            public static OppositeComparer<T> Default { get { return oppositeComparer; } }

            public int Compare(T x, T y) { return -Comparer<T>.Default.Compare(x, y); }

            protected static OppositeComparer<T> oppositeComparer = new OppositeComparer<T>();
        }
    }
}
