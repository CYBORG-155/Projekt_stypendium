using Microsoft.VisualStudio.TestTools.UnitTesting;
using SredniaSemestralna;
using System;

namespace TestStypendium
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var ob = new Form1();
            var output = ob.countSubjectsTest();
            Assert.AreEqual(2, output);
        }
    }
}
