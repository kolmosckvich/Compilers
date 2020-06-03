using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Internal;
using Lab1;
using System.Collections.Generic;
using static Lab1.Utils;

namespace Tests
{
    [TestClass]
    public class TestLab
    {
        [TestMethod]
        public void TestTab()
        {
            List<Oper> opers = new List<Oper>();

            opers.Add(new Oper("~", true, true, 3));
            opers.Add(new Oper("!", true, false, 1));
            opers.Add(new Oper("|", true, false, 1));
            opers.Add(new Oper("&", true, false, 2));

            List<Element> ter = new List<Element>();

            ter.Add(new Element("a"));
            ter.Add(new Element("0"));
            ter.Add(new Element("1"));

            List<Element> bracks = new List<Element>();

            bracks.Add(new Element("("));
            bracks.Add(new Element(")"));


            Element brake = new Element("$");

            Table tab = new Table(opers, ter, bracks, brake);

            bool res = tab.GetRelat(opers[0], opers[0]) == Relat.Lesser;
            Assert.IsTrue(res);

            res = tab.GetRelat(opers[0], opers[2]) == Relat.More;
            Assert.IsTrue(res);

            res = tab.GetRelat(brake, brake) == Relat.None;
            Assert.IsTrue(res);

            res = tab.GetRelat(ter[0], ter[2]) == Relat.None;
            Assert.IsTrue(res);

            res = tab.GetRelat(opers[0], opers[1]) == Relat.More;
            Assert.IsTrue(res);

            res = tab.GetRelat(opers[1], opers[3]) == Relat.Lesser;
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestProc()
        {
            List<Oper> opers = new List<Oper>();

            opers.Add(new Oper("~", true, true, 3));
            opers.Add(new Oper("!", true, false, 1));
            opers.Add(new Oper("|", true, false, 1));
            opers.Add(new Oper("&", true, false, 2));

            List<Element> ter = new List<Element>();

            ter.Add(new Element("a"));
            ter.Add(new Element("0"));
            ter.Add(new Element("1"));

            List<Element> bracks = new List<Element>();

            bracks.Add(new Element("("));
            bracks.Add(new Element(")"));


            Element brake = new Element("$");

            Table tab = new Table(opers, ter, bracks, brake);

            GrammProcessor GP = new GrammProcessor(opers, ter, bracks, brake, tab);

            string input = "(1) & 0 & ~1! a & 1 $";
            string output = "10&1~&a1&!";
            Assert.AreEqual(output, GP.Process(input));

            input = "~1 & ~0$";
            output = "1~0~&";
            Assert.AreEqual(output, GP.Process(input));

            input = "(1) & 0 & ~1 ! a & 1 & 0 ! 1 & (1 ! ~0)$";
            output = "10&1~&a1&0&!0~1!1&!";
            Assert.AreEqual(output, GP.Process(input));

            input = "(1 | 0) & (0 & ~1) | 1$";
            output = "10|1~0&&1|";
            Assert.AreEqual(output, GP.Process(input));
        }

    }
}
