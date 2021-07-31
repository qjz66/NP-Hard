using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization.Json;
using System.Text;


namespace Analyzer {
    public static class Util {
        public class OppositeComparer<T> : IComparer<T> {
            public static OppositeComparer<T> Default { get { return oppositeComparer; } }

            public int Compare(T x, T y) { return -Comparer<T>.Default.Compare(x, y); }

            protected static OppositeComparer<T> oppositeComparer = new OppositeComparer<T>();
        }

        public static string toString(this MemoryStream ms) {
            ms.Position = 0;
            using (StreamReader sr = new StreamReader(ms)) {
                return sr.ReadToEnd();
            }
        }

        public static class Json {
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
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T));
                js.WriteObject(stream, obj);
            }

            public static T deserialize<T>(Stream stream) {
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T));
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

        public const string IntRegex = @"[+-]?\d+";
        public const string DoubleRegex = @"[+-]?(\d+\.?\d*)|(\d*\.?\d+)";

        public static readonly char[] LineEndings = { '\r', '\n' };


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


        public static string removeSuffix(this string str, string suffix) {
            return (str.EndsWith(suffix) ? str.Substring(0, str.Length - suffix.Length) : str);
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


        public static void moveFile(string sourceFileName, string destFileName) {
            if (!File.Exists(sourceFileName)) { return; }
            if (File.Exists(destFileName)) { File.Delete(destFileName); }
            File.Move(sourceFileName, destFileName);
        }


        public static string quote(this string str) { return "\"" + str + "\""; }

        // https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
        public static string compactDateTime(DateTime dt) { return dt.ToString("yyyyMMddHHmmss"); }
        public static string compactDateTime() { return compactDateTime(DateTime.Now); }
        public static string friendlyDateTime(DateTime dt) { return dt.ToString("yyyy-MM-dd HH:mm:ss.fff"); }
        public static string friendlyDateTime() { return friendlyDateTime(DateTime.Now); }


        public static void log(string msg) { // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated
            Console.WriteLine($"{friendlyDateTime()} {msg}");
        }


        public static void pop<T>(this List<T> l) { l.RemoveAt(l.Count - 1); }

        public static void tryInc<TKey>(IDictionary<TKey, int> dictionary, TKey key) {
            dictionary.TryAdd(key, 0);
            ++dictionary[key];
        }
        public static void tryDec<TKey>(IDictionary<TKey, int> dictionary, TKey key) {
            if (--dictionary[key] <= 0) { dictionary.Remove(key); }
        }

        public static string subStr(this string str, int beginIndex, int endIndex) {
            return str.Substring(beginIndex, endIndex - beginIndex);
        }
    }
}
