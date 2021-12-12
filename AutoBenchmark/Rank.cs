using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace AutoBenchmark {
    [DataContract]
    public class Rank {
        [DataMember] public Dictionary<string, Problem> problems = new Dictionary<string, Problem>();
    }

    [DataContract]
    public class Problem {
        public const double MaxObjValue = 1E9;

        public double normalizeObj(double obj) { return minimize ? obj : -obj; }
        public double restoreObj(double obj) { return minimize ? obj : -obj; }

        [DataMember] public bool minimize = true; // for the maximizing problems, the objective value is turned to its inverse number (negative).
        [DataMember] public List<Dataset> datasets = new List<Dataset>(); // ordered by complexity. the benchmark may stop if the solver fails on easy datasets.
    }

    [DataContract]
    public class Dataset {
        [DataMember] public double minFeasibleRate = 0; // the benchmark may stop if the feasible rate on this dataset is below `minFeasibleRate`.
        [DataMember] public double minOptRate = 0; // the benchmark may stop if the optimality rate on this dataset is below `minOptRate`.
        [DataMember] public double maxTimeoutRate = 1; // the benchmark may stop if the timeout rate on this dataset is above `maxTimeoutRate`.
        [DataMember] public Dictionary<string, Instance> instances = new Dictionary<string, Instance>();
    }

    [DataContract]
    public class Instance {
        public bool matchRecord(double obj) { return (Math.Abs(obj) < Problem.MaxObjValue) && ((results.Count <= 0) || (obj <= results.Min.obj)); }
        public bool isNewRecord(double obj) { return (Math.Abs(obj) < Problem.MaxObjValue) && ((results.Count <= 0) || (obj < results.Min.obj)); }

        [DataMember] public int repeat = 10;
        [DataMember] public long secTimeout = 999;
        [DataMember] public SortedSet<Result> results = new SortedSet<Result>();
        public string[] data; // load from file when necessary.
        //public char[] data1; // single-line data.
    }

    [DataContract]
    public class Result : IComparable<Result> {
        public int CompareTo(Result other) {
            int objDiff = obj.CompareTo(other.obj);
            if (objDiff != 0) { return objDiff; }
            double durationDiff = duration - other.duration;
            if (Math.Abs(durationDiff) > 1) { return (int)durationDiff; }
            return date.CompareTo(other.date);
        }

        [DataMember(Order = 1)] public double obj;
        [DataMember(Order = 2)] public double duration;

        [DataMember(Order = 11)] public string author;
        [DataMember(Order = 12)] public string date;
    }

    public class Submission {
        public string problem;
        public string author;
        public string date;
        public string email;
        public string exePath;
    }

    public class Statistic {
        public double obj;
        public double duration;
        public int seed;
        public string info;
    }
}
