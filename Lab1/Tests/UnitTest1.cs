using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Internal;
using Lab1;

namespace Tests
{
    [TestClass]
    public class AutomatTests
    {
        [TestMethod]
        public void TestAuto()
        {
            string regExp = "0";
            string preProcRegExp = RegexpProcessor.PreprocessRegexp(regExp);
            var polska = RegexpProcessor.SortingStation(preProcRegExp);
            var NKA = NKAProcessor.GetNKA(polska);
            var DKA = DKAProcessor.GetDKA(NKA);
            Program.GetNodesTable(DKA);
            var MKA = MKAProcessor.GetMKA(DKA);
            Utils.ClearNodesID(MKA);
            Model model = new Model(MKA);
            Assert.AreEqual(true, model.Process("0"));
            Assert.AreEqual(false, model.Process("1"));
            Assert.AreEqual(false, model.Process("01"));
            Assert.AreEqual(false, model.Process("00"));
        }

        [TestMethod]
        public void TestAuto2()
        {
            string regExp = "0*";
            string preProcRegExp = RegexpProcessor.PreprocessRegexp(regExp);
            var polska = RegexpProcessor.SortingStation(preProcRegExp);
            var NKA = NKAProcessor.GetNKA(polska);
            var DKA = DKAProcessor.GetDKA(NKA);
            Program.GetNodesTable(DKA);
            var MKA = MKAProcessor.GetMKA(DKA);
            Utils.ClearNodesID(MKA);
            Model model = new Model(MKA);
            Assert.AreEqual(true, model.Process("0"));
            Assert.AreEqual(false, model.Process("1"));
            Assert.AreEqual(false, model.Process("10"));
            Assert.AreEqual(false, model.Process("01"));
            Assert.AreEqual(true, model.Process("00"));
            Assert.AreEqual(true, model.Process("000"));
            Assert.AreEqual(true, model.Process(""));
        }

        [TestMethod]
        public void TestAuto3()
        {
            string regExp = "0|1";
            string preProcRegExp = RegexpProcessor.PreprocessRegexp(regExp);
            var polska = RegexpProcessor.SortingStation(preProcRegExp);
            var NKA = NKAProcessor.GetNKA(polska);
            var DKA = DKAProcessor.GetDKA(NKA);
            Program.GetNodesTable(DKA);
            var MKA = MKAProcessor.GetMKA(DKA);
            Utils.ClearNodesID(MKA);
            Model model = new Model(MKA);
            Assert.AreEqual(true, model.Process("0"));
            Assert.AreEqual(true, model.Process("1"));
            Assert.AreEqual(false, model.Process("10"));
            Assert.AreEqual(false, model.Process("01"));
            Assert.AreEqual(false, model.Process("00"));
            Assert.AreEqual(false, model.Process("000"));
            Assert.AreEqual(false, model.Process(""));
        }

        [TestMethod]
        public void TestAuto4()
        {
            string regExp = "(0+|1+)|101001";
            string preProcRegExp = RegexpProcessor.PreprocessRegexp(regExp);
            var polska = RegexpProcessor.SortingStation(preProcRegExp);
            var NKA = NKAProcessor.GetNKA(polska);
            var DKA = DKAProcessor.GetDKA(NKA);
            Program.GetNodesTable(DKA);
            var MKA = MKAProcessor.GetMKA(DKA);
            Utils.ClearNodesID(MKA);
            Model model = new Model(MKA);
            Assert.AreEqual(true, model.Process("0"));
            Assert.AreEqual(true, model.Process("1"));
            Assert.AreEqual(false, model.Process("10"));
            Assert.AreEqual(false, model.Process("01"));
            Assert.AreEqual(true, model.Process("00"));
            Assert.AreEqual(true, model.Process("11"));
            Assert.AreEqual(true, model.Process("000"));
            Assert.AreEqual(false, model.Process(""));
            Assert.AreEqual(true, model.Process("101001"));
            Assert.AreEqual(false, model.Process("101000"));
            Assert.AreEqual(false, model.Process("1010010"));
        }

        [TestMethod]
        public void TestAuto5()
        {
            string regExp = "(0|1)0*10+(00)";
            string preProcRegExp = RegexpProcessor.PreprocessRegexp(regExp);
            var polska = RegexpProcessor.SortingStation(preProcRegExp);
            var NKA = NKAProcessor.GetNKA(polska);
            var DKA = DKAProcessor.GetDKA(NKA);
            Program.GetNodesTable(DKA);
            var MKA = MKAProcessor.GetMKA(DKA);
            Utils.ClearNodesID(MKA);
            Model model = new Model(MKA);
            Assert.AreEqual(false, model.Process("011"));
            Assert.AreEqual(false, model.Process("100011"));
            Assert.AreEqual(false, model.Process("1101001"));
            Assert.AreEqual(false, model.Process("1"));
            Assert.AreEqual(false, model.Process("0"));
            Assert.AreEqual(false, model.Process(""));
            Assert.AreEqual(true, model.Process("11000"));
            Assert.AreEqual(false, model.Process("1100010"));
            Assert.AreEqual(true, model.Process("000001000000"));
            Assert.AreEqual(false, model.Process("110001"));
            Assert.AreEqual(true, model.Process("110000"));
        }
    }
}
