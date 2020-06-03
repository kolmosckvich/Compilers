using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Internal;
using Lab1;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class TestLab
    {
        [TestMethod]
        public void TestId()
        {
            string input = "15";
            GrammProcessor.Reset(input);
            bool res = GrammProcessor.CheckId(input);
            Assert.IsTrue(res);

            input = "196";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckId(input);
            Assert.IsTrue(res);

            input = "-196";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckId(input);
            Assert.IsFalse(res);

            input = "+196";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckId(input);
            Assert.IsFalse(res);

            input = "a23";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckId(input);
            Assert.IsFalse(res);

            input = "15a";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckId(input);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestLogVals()
        {
            string input = "true";
            GrammProcessor.Reset(input);
            bool res = GrammProcessor.CheckLogVal(input);
            Assert.IsTrue(res);

            input = "false";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogVal(input);
            Assert.IsTrue(res);

            input = "fal";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogVal(input);
            Assert.IsFalse(res);

            input = "T";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogVal(input);
            Assert.IsFalse(res);

            input = "true-5";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogVal(input);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestLogFir()
        {
            string input = "true";
            GrammProcessor.Reset(input);
            bool res = GrammProcessor.CheckLogFir(input);
            Assert.IsTrue(res);

            input = "196";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogFir(input);
            Assert.IsTrue(res);

            input = "fal";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogFir(input);
            Assert.IsFalse(res);

            input = "true-5";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogFir(input);
            Assert.IsTrue(res);

            input = "15a";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogFir(input);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestLogSec()
        {
            string input = "true";
            GrammProcessor.Reset(input);
            bool res = GrammProcessor.CheckLogSec(input);
            Assert.IsTrue(res);

            input = "~196";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogSec(input);
            Assert.IsTrue(res);

            input = "~fal";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogSec(input);
            Assert.IsFalse(res);

            input = "!true";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogSec(input);
            Assert.IsFalse(res);

            input = "&5";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogSec(input);
            Assert.IsFalse(res);

            input = "true-5";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogSec(input);
            Assert.IsTrue(res);

            input = "15a";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogSec(input);
            Assert.IsTrue(res);

            input = "w~5";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogSec(input);
            Assert.IsFalse(res);

        }

        [TestMethod]
        public void TestLogOne()
        {
            string input = "15 & true";
            GrammProcessor.Reset(input);
            bool res = GrammProcessor.CheckLogOne(input);
            res = res && GrammProcessor.CheckFinish();
            Assert.IsTrue(res);

            input = "~196 & ~false";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogOne(input);
            res = res && GrammProcessor.CheckFinish();
            Assert.IsTrue(res);

            input = "~196";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogOne(input);
            res = res && GrammProcessor.CheckFinish();
            Assert.IsTrue(res);

            input = "& false";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogOne(input);
            res = !res || !GrammProcessor.CheckFinish();
            Assert.IsTrue(res);

            input = "5 ! false";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogOne(input);
            res = !res || !GrammProcessor.CheckFinish();
            Assert.IsTrue(res);

            input = "15 & ~false";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogOne(input);
            res = res && GrammProcessor.CheckFinish();
            Assert.IsTrue(res);

        }

        [TestMethod]
        public void TestLogExpr()
        {
            string input = "15 & true";
            GrammProcessor.Reset(input);
            bool res = GrammProcessor.CheckLogExpr(input);
            res = res && GrammProcessor.CheckFinish();
            Assert.IsTrue(res);

            input = "~196 & ~false";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogExpr(input);
            res = res && GrammProcessor.CheckFinish();
            Assert.IsTrue(res);

            input = "~196 ! ~false";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogExpr(input);
            res = res && GrammProcessor.CheckFinish();
            Assert.IsTrue(res);

            input = "~196 & ~false ! true";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogExpr(input);
            res = res && GrammProcessor.CheckFinish();
            Assert.IsTrue(res);

            input = "! ~false";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogExpr(input);
            res = !res || !GrammProcessor.CheckFinish();
            Assert.IsTrue(res);

            input = "~196 & ~false ! 15 ! ~8 & false";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogExpr(input);
            res = res && GrammProcessor.CheckFinish();
            Assert.IsTrue(res);

            input = "true & !false";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckLogExpr(input);
            res = !res || !GrammProcessor.CheckFinish();
            Assert.IsTrue(res);

        }

        [TestMethod]
        public void TestOp()
        {
            string input = "15 = true";
            GrammProcessor.Reset(input);
            bool res = GrammProcessor.CheckOp(input);
            res = res && GrammProcessor.CheckFinish();
            Assert.IsTrue(res);

            input = "21 = ~false";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckOp(input);
            res = res && GrammProcessor.CheckFinish();
            Assert.IsTrue(res);

            input = "a21 = ~6";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckOp(input);
            Assert.IsFalse(res);

            input = "21 = 5";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckOp(input);
            res = res && GrammProcessor.CheckFinish();
            Assert.IsTrue(res);


            input = "178 = 3 & ~true";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckOp(input);
            res = res && GrammProcessor.CheckFinish();
            Assert.IsTrue(res);

            input = "true = 3";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckOp(input);
            Assert.IsFalse(res);
        }


        [TestMethod]
        public void TestOpList()
        {
            string input = "15 = true;" +
                           "89 = 7 & ~false";
            GrammProcessor.Reset(input);
            bool res = GrammProcessor.CheckOpList(input);
            res = res && GrammProcessor.CheckFinish();
            Assert.IsTrue(res);

            input = "89 = 15;" +
                    "true = 5";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckOpList(input);
            Assert.IsFalse(res);

            input = "89 = 15;" +
                    "5 = true;";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckOpList(input);
            Assert.IsFalse(res);
        }


        public void TestBlock()
        {
            string input = "begin" +
                           "15 = true;" +
                           "89 = 7 & ~false" +
                           "end";
            GrammProcessor.Reset(input);
            bool res = GrammProcessor.CheckBlock(input);
            res = res && GrammProcessor.CheckFinish();
            Assert.IsTrue(res);

            input = "begin" +
                    "15 = true" +
                    "end";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckBlock(input);
            res = res && GrammProcessor.CheckFinish();
            Assert.IsTrue(res);

            input = "begin" +
                    "15 = true";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckBlock(input);
            Assert.IsFalse(res);

            input = "15 = true" +
                    "end";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckBlock(input);
            Assert.IsFalse(res);

            input = "begin" +
                    "15 = true;" +
                    "end";
            GrammProcessor.Reset(input);
            res = GrammProcessor.CheckBlock(input);
            res = res && GrammProcessor.CheckFinish();
            Assert.IsFalse(res);
        }
    }
}
