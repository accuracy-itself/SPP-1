using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.TestClasses;
using System.Collections.Generic;
using Faker.Core;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCycle()
        {
            FakerRealizer faker = new FakerRealizer();
            try { A amember = faker.Create<A>(); }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "!!cycle exception!!");
                return;
            }
            Assert.AreEqual(0, 1);
        }

        [TestMethod]
        public void TestDifference()
        {
            FakerRealizer faker = new FakerRealizer();
            int a = faker.Create<int>(), b = faker.Create<int>();
            Assert.IsTrue(a != b);
            string s1 = faker.Create<string>(), s2 = faker.Create<string>();
            Assert.IsTrue(!string.Equals(s1, s2));
        }

        [TestMethod]
        public void TestList()
        {
            FakerRealizer faker = new FakerRealizer();
            List<CodeIdentity> Alist = faker.Create<List<CodeIdentity>>();
            Assert.IsTrue((Alist.Count != 0) && (Alist[0] != null));

        }

        [TestMethod]
        public void TestDate()
        {
            FakerRealizer faker = new FakerRealizer();
            //System.DateTime dateTime = new System.DateTime();
            System.DateTime dateTime = faker.Create<System.DateTime>();
            Assert.IsTrue((dateTime != null));
        }

        [TestMethod]
        public void TestConstructors()
        {
            FakerRealizer faker = new FakerRealizer();
            try
            {
                D d = faker.Create<D>();
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "!!no normal constructors available!!");
                return;
            }
            Assert.AreEqual(0, 1);
        }

    }
}