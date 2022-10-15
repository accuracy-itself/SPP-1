using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Example;

namespace Core.Tests
{
    [TestClass]
    public class TracerTests
    {
        [TestMethod]
        public void TestThreads()
        {
            TracerRealizer tracer = new TracerRealizer();

            Thread[] Threads = new Thread[6];
            for (int i = 0; i < 6; i++)
            {
                Threads[i] = new Thread(new Bar(tracer).InnerMethod);
                Threads[i].Start();
            }

            for (int i = 0; i < 6; i++)
                Threads[i].Join();

            Assert.AreEqual(tracer.GetResult().ThreadTracersResult.Count, 6);
        }

        [TestMethod]
        public void TestThreadTime()
        {
            TracerRealizer tracer = new TracerRealizer();

            Thread FirstThread = new Thread(new Foo(tracer).MyMethod),
                SecondThread = new Thread(new Bar(tracer).InnerMethod),
                ThirdThread = new Thread(new Bar(tracer).InnerMethod);
            FirstThread.Start();
            FirstThread.Join();
            SecondThread.Start();
            SecondThread.Join();
            ThirdThread.Start();
            ThirdThread.Join();

            TraceResult result = tracer.GetResult();
            int[] time = { 150, 50, 50};

            if(result.ThreadTracersResult.Count == 3)
            {
                for (int i = 0; i < 3; i++)
                    Assert.AreEqual(time[i], result.ThreadTracersResult[i].Time, 10);
            }
        }


        [TestMethod]
        public void TestNames()
        {
            TracerRealizer tracer = new TracerRealizer();

            Thread FirstThread = new Thread(new Foo(tracer).MyMethod),
                SecondThread = new Thread(new Bar(tracer).InnerMethod),
                ThirdThread = new Thread(new Bar(tracer).InnerMethod);
            FirstThread.Start();
            FirstThread.Join();
            SecondThread.Start();
            SecondThread.Join();
            ThirdThread.Start();
            ThirdThread.Join();

            TraceResult result = tracer.GetResult();
            string[] names = { "MyMethod", "InnerMethod", "InnerMethod"};

            if (result.ThreadTracersResult.Count == 3)
            {
                for (int i = 0; i < 3; i++)
                    Assert.AreEqual(names[i], result.ThreadTracersResult[i].MethodTree[0].Name);
            }
        }
    }
}