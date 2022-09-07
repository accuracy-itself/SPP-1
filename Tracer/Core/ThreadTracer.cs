using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Core
{
    public class ThreadTracer
    {
        static object balanceLock = new object();
        public List<MethodTraceInfo> MethodTree;
        private Stack<MethodTraceInfo> CurrentMethods = new Stack<MethodTraceInfo>();
        public int Id;
        public string ClassName, MethodName;
        public long Time;
        

        public ThreadTracer(int id)
        {
            MethodTree = new List<MethodTraceInfo>();
            Id = id;
        }

        public void StartTrace()
        {
            StackTrace sT = new StackTrace(true);
            {
                //string stackIndent = "";
                //lock (balanceLock)
                //{
                //    for (int i = 2; i < sT.FrameCount - 1; i++)
                //    {
                //        StackFrame sf = sT.GetFrame(i);
                //        Console.WriteLine();
                //        Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

                //        MethodName = sf.GetMethod().Name;
                //        Console.WriteLine(stackIndent + MethodName);

                //        ClassName = sf.GetFileName();
                //        ClassName = ClassName.Substring(ClassName.LastIndexOf('\\') + 1);
                //        ClassName = ClassName.Substring(0, ClassName.LastIndexOf('.'));
                //        Console.WriteLine(stackIndent + ClassName);

                //        Console.WriteLine(stackIndent + "Line Number: {0}",
                //            sf.GetFileLineNumber());
                //        stackIndent += "  ";
                //    }
                //}
            }//long comment
            for (int i = 2; i < sT.FrameCount - 1; i++)
            {
                StackFrame sf = sT.GetFrame(i);
                
                MethodName = sf.GetMethod().Name;

                ClassName = sf.GetFileName();
                ClassName = ClassName.Substring(ClassName.LastIndexOf('\\') + 1);
                ClassName = ClassName.Substring(0, ClassName.LastIndexOf('.'));

                CurrentMethods.Push(new MethodTraceInfo(MethodName, ClassName));
            }

            //lock(balanceLock)
            //{
            //    Console.WriteLine(CurrentMethods.Count);
            //}
            GetMethod().StopWatch.Start();
        }

        public void StopTrace()
        {
            StackTrace sT = new StackTrace(true);
            {
                //string stackIndent = "";
                //lock (balanceLock)
                //{
                //    for (int i = 2; i < sT.FrameCount - 1; i++)
                //    {
                //        StackFrame sf = sT.GetFrame(i);
                //        Console.WriteLine();
                //        Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

                //        MethodName = sf.GetMethod().Name;
                //        Console.WriteLine(stackIndent + MethodName);

                //        ClassName = sf.GetFileName();
                //        ClassName = ClassName.Substring(ClassName.LastIndexOf('\\') + 1);
                //        ClassName = ClassName.Substring(0, ClassName.LastIndexOf('.'));
                //        Console.WriteLine(stackIndent + ClassName);

                //        Console.WriteLine(stackIndent + "Line Number: {0}",
                //            sf.GetFileLineNumber());
                //        stackIndent += "  ";
                //    }
                //}
            }//long comment
            for (int i = 2; i < sT.FrameCount - 1; i++)
            {
                StackFrame sf = sT.GetFrame(i);

                MethodName = sf.GetMethod().Name;

                ClassName = sf.GetFileName();
                ClassName = ClassName.Substring(ClassName.LastIndexOf('\\') + 1);
                ClassName = ClassName.Substring(0, ClassName.LastIndexOf('.'));

                CurrentMethods.Push(new MethodTraceInfo(MethodName, ClassName));
            }

            MethodTraceInfo methodTraceInfo = GetMethod();
            methodTraceInfo.StopWatch.Stop();
            methodTraceInfo.Time = methodTraceInfo.StopWatch.ElapsedMilliseconds;
        }

        public MethodTraceInfo GetMethod()
        {
            MethodTraceInfo info = null, method = null;
            List<MethodTraceInfo> methodlist = MethodTree;
            while(CurrentMethods.Count > 0)
            {
                info = CurrentMethods.Pop();
                method = null;

                foreach (MethodTraceInfo methodInfo in methodlist)
                {
                    if(methodInfo.Name == info.Name && methodInfo.ClassName == info.ClassName)
                    {
                        methodlist = methodInfo.ChildrenMethods;
                        method = methodInfo;
                        break;
                    }
                }
            }

            if(method == null)
            {
                methodlist.Add(info);
            }
            else
            {
                return method;
            }

            return info;
        }
    }
}
