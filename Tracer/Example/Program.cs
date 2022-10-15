using Core;
using System.Reflection;
//using Xml;
using Serialization.Abstractions;

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
            threads.Add(ThirdThread);

            foreach (Thread t in threads)
            {
                t.Start();
            }

            Boolean threadEnd = false;

            while (!threadEnd)
            {
                foreach (Thread t in threads)
                {
                    if (t.IsAlive)
                        threadEnd = false;
                    else
                        threadEnd = true;
                }
            }
            TraceResult threadtracerres = tracer.GetResult();

            Assembly assembly = Assembly.LoadFrom("D:/C#/Tracer/SerializeLibrary/SerializeLibrary/bin/Debug/net6.0/SerializeLibrary.dll"); 
            
            Type[] serTypes = assembly.GetTypes();
            foreach (Type serType in serTypes)
            {
                //Console.WriteLine(serType);
                if (serType.GetInterface("ITraceResultSerializer") != null)
                {
                    Console.WriteLine(serType.FullName);

                    ITraceResultSerializer serializer = (ITraceResultSerializer)assembly.CreateInstance(serType.FullName);
                    StreamWriter sw = new StreamWriter(serType.Name + serializer.Format);
                    StreamWriter sw2 = new StreamWriter(serType.Name + ".txt");
                    serializer.Serialize(threadtracerres, sw);
                    serializer.Serialize(threadtracerres, sw2);
                }
            }
            
            Console.WriteLine("Done! " + "Threads count: " + threads.Count);
        }
    }
}