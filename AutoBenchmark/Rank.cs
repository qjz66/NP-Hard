using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Analyzer {
    [DataContract]
    public class Rank {
        [DataMember] public Dictionary<string, Problem> problems = new Dictionary<string, Problem>();
    }

    [DataContract]
    public class Problem {
        public const double MaxObjValue = 1E9;
        public const double MinObjValue = -1E9;

        public double worstObjValue() { return minimize ? MaxObjValue : MinObjValue; }

        [DataMember] public bool minimize = true;
        [DataMember] public List<Dataset> datasets = new List<Dataset>(); // ordered by complexity. the benchmark may stop if the solver fails on easy datasets.
    }

    [DataContract]
    public class Dataset {
        [DataMember] public double minOptRate = 0; // the benchmark may stop if the optimality rate on this dataset is below `minOptRate`.
        [DataMember] public Dictionary<string, Instance> instances = new Dictionary<string, Instance>();
    }

    [DataContract]
    public class Instance {
        [DataMember] public int repeat = 10;
        [DataMember] public long secTimeout = 999;
        [DataMember] public SortedSet<Result> results = new SortedSet<Result>();
        public string[] data; // load from file when necessary.
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
