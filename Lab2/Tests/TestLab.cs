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
        public void TestUnreach1()
        {
            Gramm gr = new Gramm();
            gr.Terms.Add("a");
            gr.Terms.Add("+");

            gr.NonTerms.Add("A");
            gr.NonTerms.Add("B");
            gr.NonTerms.Add("C");
            gr.NonTerms.Add("D");

            gr.St = "B";

            gr.Rules.Add(new Rule("A", new List<string>() { "a", "A" }));
            gr.Rules.Add(new Rule("A", new List<string>() { "+", "D" }));
            gr.Rules.Add(new Rule("B", new List<string>() { "B", "A" }));
            gr.Rules.Add(new Rule("B", new List<string>() { "A", "B", "+" }));
            gr.Rules.Add(new Rule("C", new List<string>() { "a", "A", "a" }));
            gr.Rules.Add(new Rule("C", new List<string>() { "D", "+", "A" }));
            gr.Rules.Add(new Rule("D", new List<string>() { "B", "a" }));
            gr.Rules.Add(new Rule("D", new List<string>() { "a" }));

            Gramm newGr = UnreachableProcessor.RemoveUnreachable(gr);

            Assert.IsTrue(newGr.NonTerms.Contains("A"));
            Assert.IsTrue(newGr.NonTerms.Contains("B"));
            Assert.IsFalse(newGr.NonTerms.Contains("C"));
            Assert.IsTrue(newGr.NonTerms.Contains("D"));

            Assert.AreEqual(6, newGr.Rules.Count);
            Assert.IsNull(newGr.Rules.Find(x => x.Left == "C"));

        }

        [TestMethod]
        public void TestUnreach2()
        {
            Gramm gr = new Gramm();
            gr.Terms.Add("a");
            gr.Terms.Add("+");

            gr.NonTerms.Add("A");
            gr.NonTerms.Add("B");
            gr.NonTerms.Add("C");
            gr.NonTerms.Add("D");

            gr.St = "B";

            gr.Rules.Add(new Rule("A", new List<string>() { "a", "A" }));
            gr.Rules.Add(new Rule("A", new List<string>() { "+", "D" }));
            gr.Rules.Add(new Rule("B", new List<string>() { "B", "A" }));
            gr.Rules.Add(new Rule("B", new List<string>() { "A", "B", "+" }));
            gr.Rules.Add(new Rule("C", new List<string>() { "a", "A", "a" }));
            gr.Rules.Add(new Rule("C", new List<string>() { "D", "+", "A" }));
            gr.Rules.Add(new Rule("D", new List<string>() { "B", "C" }));
            gr.Rules.Add(new Rule("D", new List<string>() { "a" }));

            Gramm newGr = UnreachableProcessor.RemoveUnreachable(gr);

            Assert.IsTrue(newGr.NonTerms.Contains("A"));
            Assert.IsTrue(newGr.NonTerms.Contains("B"));
            Assert.IsTrue(newGr.NonTerms.Contains("C"));
            Assert.IsTrue(newGr.NonTerms.Contains("D"));

            Assert.AreEqual(8, newGr.Rules.Count);
            Assert.IsNotNull(newGr.Rules.Find(x => x.Left == "C"));

        }

        [TestMethod]
        public void TestLR1()
        {
            Gramm gr = new Gramm();
            gr.Terms.Add("a");
            gr.Terms.Add("+");

            gr.NonTerms.Add("A");
            gr.NonTerms.Add("B");
            gr.NonTerms.Add("C");
            gr.NonTerms.Add("D");

            gr.St = "A";

            gr.Rules.Add(new Rule("A", new List<string>() { "a", "A" }));
            gr.Rules.Add(new Rule("A", new List<string>() { "+", "D" }));
            gr.Rules.Add(new Rule("A", new List<string>() { "D", "+" }));
            gr.Rules.Add(new Rule("B", new List<string>() { "B", "A" }));
            gr.Rules.Add(new Rule("B", new List<string>() { "C" }));
            gr.Rules.Add(new Rule("C", new List<string>() { "a", "A", "a" }));
            gr.Rules.Add(new Rule("C", new List<string>() { "B", "A", "+" }));
            gr.Rules.Add(new Rule("C", new List<string>() { "A", "a", "+" }));
            gr.Rules.Add(new Rule("D", new List<string>() { "B", "C" }));
            gr.Rules.Add(new Rule("D", new List<string>() { "a" }));
            gr.Rules.Add(new Rule("D", new List<string>() { "C" }));

            Gramm newGr = GrammProcessor.RemoveLR(gr);

            //GramFileProcessor.WriteGramm(gr, "lilon.json");

            Assert.IsTrue(gr.NonTerms.Contains("A"));
            Assert.IsTrue(gr.NonTerms.Contains("B"));
            Assert.IsTrue(gr.NonTerms.Contains("C"));
            Assert.IsTrue(gr.NonTerms.Contains("D"));
            Assert.IsTrue(gr.NonTerms.Contains("B'"));
            Assert.AreEqual(16, gr.Rules.Count);

        }

        [TestMethod]
        public void TestLR2()
        {
            Gramm gr = new Gramm();
            gr.Terms.Add("a");
            gr.Terms.Add("c");
            gr.Terms.Add("d");

            gr.NonTerms.Add("S");
            gr.NonTerms.Add("A");

            gr.St = "S";

            gr.Rules.Add(new Rule("S", new List<string>() { "A", "a" }));
            gr.Rules.Add(new Rule("S", new List<string>() { "b" }));
            gr.Rules.Add(new Rule("A", new List<string>() { "A", "c" }));
            gr.Rules.Add(new Rule("A", new List<string>() { "S", "d" }));

            Gramm newGr = GrammProcessor.RemoveLR(gr);

            Assert.IsTrue(gr.NonTerms.Contains("A"));
            Assert.IsTrue(gr.NonTerms.Contains("S"));
            Assert.IsTrue(gr.NonTerms.Contains("A'"));
            Assert.AreEqual(5, gr.Rules.Count);
            Assert.IsNotNull(gr.Rules.Find(x => x.Left == "A'"));
            Assert.AreEqual("bdA'", gr.Rules.Find(x => x.Left == "A").Right);

        }

        [TestMethod]
        public void TestLF1()
        {
            Gramm gr = new Gramm();
            gr.Terms.Add("i");
            gr.Terms.Add("t");
            gr.Terms.Add("l");
            gr.Terms.Add("a");
            gr.Terms.Add("b");

            gr.NonTerms.Add("E");
            gr.NonTerms.Add("S");

            gr.St = "S";

            gr.Rules.Add(new Rule("S", new List<string>() { "i", "E", "t", "S" }));
            gr.Rules.Add(new Rule("S", new List<string>() { "i", "E", "t", "S", "l", "S" }));
            gr.Rules.Add(new Rule("S", new List<string>() { "a"}));
            gr.Rules.Add(new Rule("E", new List<string>() { "b"}));

            Gramm newGr = GrammProcessor.RemoveLR(gr);
            newGr = FactProcessor.Fact(newGr);

            Assert.IsTrue(gr.NonTerms.Contains("S"));
            Assert.IsTrue(gr.NonTerms.Contains("E"));
            Assert.IsTrue(gr.NonTerms.Contains("S'"));
            Assert.AreEqual(5, gr.Rules.Count);
            Assert.AreEqual("iEtSS'", gr.Rules.Find(x => x.Left == "S" && x.Right != "a").Right);

        }

        [TestMethod]
        public void TestLF2()
        {
            Gramm gr = new Gramm();
            gr.Terms.Add("a");
            gr.Terms.Add("+");

            gr.NonTerms.Add("A");
            gr.NonTerms.Add("B");
            gr.NonTerms.Add("C");
            gr.NonTerms.Add("D");

            gr.St = "A";

            gr.Rules.Add(new Rule("A", new List<string>() { "a", "A" }));
            gr.Rules.Add(new Rule("A", new List<string>() { "+", "D" }));
            gr.Rules.Add(new Rule("A", new List<string>() { "D", "+" }));
            gr.Rules.Add(new Rule("B", new List<string>() { "B", "A" }));
            gr.Rules.Add(new Rule("B", new List<string>() { "C" }));
            gr.Rules.Add(new Rule("C", new List<string>() { "a", "A", "a" }));
            gr.Rules.Add(new Rule("C", new List<string>() { "B", "A", "+" }));
            gr.Rules.Add(new Rule("C", new List<string>() { "A", "a", "+" }));
            gr.Rules.Add(new Rule("D", new List<string>() { "B", "C" }));
            gr.Rules.Add(new Rule("D", new List<string>() { "a" }));
            gr.Rules.Add(new Rule("D", new List<string>() { "C" }));

            Gramm newGr = GrammProcessor.RemoveLR(gr);
            newGr = FactProcessor.Fact(newGr);

            Assert.IsTrue(gr.NonTerms.Contains("A"));
            Assert.IsTrue(gr.NonTerms.Contains("B"));
            Assert.IsTrue(gr.NonTerms.Contains("C"));
            Assert.IsTrue(gr.NonTerms.Contains("D"));
            Assert.IsTrue(gr.NonTerms.Contains("B'"));
            Assert.IsTrue(gr.NonTerms.Contains("D'''"));
            Assert.AreEqual(19, gr.Rules.Count);

        }

    }
}
