using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Internal;
using Lab1;
using System;

namespace Tests
{
    [TestClass]
    public class RegexpTests
    {
        [TestMethod]
        public void TestRegexp()
        {
            string expr = "0";
            string preproc = RegexpProcessor.PreprocessRegexp(expr);
            Assert.AreEqual("0", preproc);
        }

        [TestMethod]
        public void TestRegexp2()
        {
            string expr = "0*";
            string preproc = RegexpProcessor.PreprocessRegexp(expr);
            Assert.AreEqual("0*", preproc);
        }

        [TestMethod]
        public void TestRegexp3()
        {
            string expr = "0|(1|0)10*0";
            string preproc = RegexpProcessor.PreprocessRegexp(expr);
            Assert.AreEqual("0|(1|0)&1&0*&0", preproc);
        }

        [TestMethod]
        public void TestRegexp4()
        {
            string expr = "0";
            string preproc = RegexpProcessor.PreprocessRegexp(expr);
            string polska = new String(RegexpProcessor.SortingStation(preproc).ToArray());
            Assert.AreEqual("0", polska);
        }

        [TestMethod]
        public void TestRegexp5()
        {
            string expr = "0*";
            string preproc = RegexpProcessor.PreprocessRegexp(expr);
            string polska = new String(RegexpProcessor.SortingStation(preproc).ToArray());
            Assert.AreEqual("0*", polska);
        }

        [TestMethod]
        public void TestRegexp6()
        {
            string expr = "0|(1|0)10*0";
            string preproc = RegexpProcessor.PreprocessRegexp(expr);
            string polska = new String(RegexpProcessor.SortingStation(preproc).ToArray());
            Assert.AreEqual("010||1&0*&0&", polska);
        }
    }
}
