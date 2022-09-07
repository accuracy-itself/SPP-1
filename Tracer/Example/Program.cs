using Core;

namespace Example
{
    public class Program
    {
        public static void Main()
        {
            System.Console.WriteLine("Hey");
            TracerRealizer tracer = new TracerRealizer();
            Thread FirstThread = new Thread(new Foo(tracer).MyMethod), SecondThread = new Thread(new Bar(tracer).InnerMethod);
            Thread ThirdThread = new Thread(new Foo(tracer).M0);
            List <Thread> threads = new List<Thread>();
            threads.Add(FirstThread);
            threads.Add(SecondThread);
            threads.Add (ThirdThread);
            //FirstThread.Start();
            //SecondThread.Start();

            foreach (Thread t in threads)
            {
                t.Start();
            }

            Boolean threadEnd = false;
            
            while(!threadEnd)
            {
                foreach (Thread t in threads)
                {
                    if (t.IsAlive)
                        threadEnd = false;
                    else
                        threadEnd = true;
                }
            }
            List<ThreadTracer> threadtracerres = (List<ThreadTracer>)tracer.GetResult();
            Console.WriteLine("Done! " + "Threads count: " + threads.Count);
        }
    }
}