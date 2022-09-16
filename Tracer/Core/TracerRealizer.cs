using System.Diagnostics;

namespace Core
{
    [Serializable]
    public class TracerRealizer:ITracer
    {
        //private readonly 
        //private Stack<MethodTraceInfo> CurrentMethods = new Stack<MethodTraceInfo>();
        internal List<ThreadTracer> ThreadTracers; 
        private readonly object Locker = new object();
        //public IReadOnlyList<ThreadTracer> ThreadTracersResult;
        //public readonly List<ThreadTracer> ThreadTracersResult;
        public TracerRealizer()
        {
            ThreadTracers = new List<ThreadTracer>();
        }

        public void StartTrace()
        {
            ThreadTracer threadTracer = GetThread();
            if (threadTracer == null)
            {
                threadTracer = new ThreadTracer(Thread.CurrentThread.ManagedThreadId);
                lock(Locker)
                {
                    ThreadTracers.Add(threadTracer);
                }
            }
            
            threadTracer.StartTrace();
        }

        // вызывается в конце замеряемого метода
        public void StopTrace()
        {
            ThreadTracer threadTracer = GetThread();
            if (threadTracer == null)
            {
                threadTracer = new ThreadTracer(Thread.CurrentThread.ManagedThreadId);
                lock (Locker)
                {
                    ThreadTracers.Add(threadTracer);
                }
            }

            threadTracer.StopTrace();
        }

        internal  ThreadTracer GetThread()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            foreach(ThreadTracer threadTraceInfo in ThreadTracers)
            {
                if(threadTraceInfo.Id == threadId)
                    return threadTraceInfo;
            }
            return null;
        }

        public TraceResult GetResult()
        {
            long time;
            foreach (ThreadTracer threadTracer in ThreadTracers)
            {
                time = 0;
                foreach (MethodTraceInfo method in threadTracer.MethodTree)
                {
                    time += method.Time; 
                }
                threadTracer.Time = time;
            }
            //ThreadTracersResult = new List<ThreadTracer>(ThreadTracers);
            return new TraceResult(ThreadTracers);
        }
    }
}