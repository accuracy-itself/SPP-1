﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Core
{
    [Serializable]
    public class MethodTraceInfo
    {
        public string Name { get; set; }
        public string ClassName { get; set; }
        public long Time { get; set; }
        public List<MethodTraceInfo> ChildrenMethods { get; set; }
        //public int ThreadId { get; private set; }   
        public Stopwatch StopWatch { get; set; }

        public MethodTraceInfo()
        {

        }

        public MethodTraceInfo(string name, string className)
        {
            Name = name;
            ClassName = className;
            //Time = time;
            ChildrenMethods = new List<MethodTraceInfo>();
            //ThreadId = threadId;
            StopWatch = new Stopwatch();
        }


    }
}
