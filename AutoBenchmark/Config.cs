using System;
using System.Collections.Generic;
using System.Text;


namespace Analyzer {
    public class CommonCfg {
        public const string SolverSubDir = "Solver";
        public const string InstanceSubDir = "Instance";
        public const string SolutionSubDir = "Solution";
        public const string LogFilePrefix = "Log";
        public const string LogFileExt = ".tsv";

        public const string RankPath = "rank.json";

        public const string RankMarkdownPath = "ReadMe.md";
        public const string RankPagePath = "index.html";
        public const string RankCssPath = "base.css";

        public const int MaxResultsCountPerInstance = 10;

        public const int PollIntervalInMillisecond = 15 * 60 * 1000; // 15 minutes.

        public static readonly HashSet<string> ZipFileExts = new HashSet<string> { ".7z", ".gz", ".tar", ".rar", ".zip", ".bz2", ".iso", ".xz" };

        public static readonly Encoding DefaultEncoding = Util.getEncoding(936);
    }

    public class EmailCfg {
        public const int Pop3Port = 110;
        public const int ImapPort = 143;
        public const int SmtpPort = 25;
        public const int SmtpPort2 = 2525;

        public const int Pop3SslPort = 995;
        public const int ImapSslPort = 993;
        public const int SmtpSslPort = 465;
        public const int SmtpSslPort2 = 587;

        public class HustMail {
            public const string Pop3 = "mail.hust.edu.cn";
            public const string Imap = "mail.hust.edu.cn";
            public const string Smtp = "mail.hust.edu.cn";
        }
        public class QqMail {
            public const string Pop3 = "pop3.qq.com";
            public const string Imap = "imap.qq.com";
            public const string Smtp = "smtp.qq.com";
        }
        public class ExqqMail {
            public const string Pop3 = "pop3.exmail.qq.com";
            public const string Imap = "imap.exmail.qq.com";
            public const string Smtp = "smtp.exmail.qq.com";
        }

        public const string Pop3Addr = ExqqMail.Pop3;
        public const string ImapAddr = ExqqMail.Imap;
        public const string SmtpAddr = ExqqMail.Smtp;

        // https://docs.microsoft.com/zh-cn/dotnet/api/system.text.regularexpressions.regex
        public const string SubjectRegex = @"^Challenge20.*-.*$";
        public const string SubjectFilter = "Challenge20";
        public const int ProblemIndexBegin = 13; // "Challenge2020".Length;

        public const string MyAddress = "szx@duhe.tech";
        //public const string ToAddress = "";
        //public const string CcAddress = "";

        public const string Username = MyAddress;
        public const string Password = ""; // TODO[szx][0]: do not commit this.

        public const ulong MaxFileByteSize = 8 * 1024 * 1024;
    }

    public class ProblemName {
        public const string Coloring = "GCP";
        public const string PCenter = "PCP";
        public const string Jobshop = "FJSP";
        public const string RWA = "RWA";
        public const string RectPacking = "RPP";
        public const string VRP = "VRP";
    }

    public class BenchmarkCfg {
        public const int MillisecondCheckInterval = 1000;
        public const long ByteMemoryLimit = 1024 * 1024 * 1024;

        public static readonly int ParallelBenchmarkNum = Math.Min(8, Environment.ProcessorCount);

        public const int RandSeedInc = 2011; // TODO[szx][0]: do not commit this.
        public const int RandSeedMul = 2111; // TODO[szx][0]: do not commit this.

        public const string LogDelim = "\t";
        public const string LogBasicHeader = "Instance" + LogDelim + "Obj" + LogDelim + "Duration";
        public const string LogCommonHeader = "Solver" + LogDelim + "Seed" + LogDelim + LogBasicHeader;
        public static readonly Dictionary<string, string> LogHeaders = new Dictionary<string, string> {
            { ProblemName.Coloring, LogCommonHeader + LogDelim + "Conflict" },
            { ProblemName.PCenter, LogCommonHeader + LogDelim + "Center" + LogDelim + "Uncover" },
            { ProblemName.Jobshop, LogCommonHeader + LogDelim + "RestJob" },
            { ProblemName.RWA, LogCommonHeader + LogDelim + "BrokenPath" + LogDelim + "Conflict" },
            { ProblemName.RectPacking, LogCommonHeader + LogDelim + "Missing" + LogDelim + "Overlap" },
            { ProblemName.VRP, LogCommonHeader + LogDelim + "BrokenPath" + LogDelim + "Overload" + LogDelim + "NotOnTime" },
        };

        public static readonly Dictionary<string, Check> Checkers = new Dictionary<string, Check> {
            { ProblemName.Coloring, Checker.coloring },
            { ProblemName.PCenter, Checker.pCenter },
            { ProblemName.Jobshop, Checker.jobshop },
            { ProblemName.RWA, Checker.rwa },
            { ProblemName.RectPacking, Checker.rectPacking },
            { ProblemName.VRP, Checker.vrp },
        };

        public static Rank rank = Util.Json.load<Rank>(CommonCfg.RankPath);
    }
}
