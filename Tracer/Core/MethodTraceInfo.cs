using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Core
{
    public class MethodTraceInfo
    {
        public string Name { get; private set; }
        public string ClassName { get; private set; }
        public long Time { get; set; }
        public List<MethodTraceInfo> ChildrenMethods { get; private set; }
        //public int ThreadId { get; private set; }   
        public Stopwatch StopWatch { get; private set; }

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
