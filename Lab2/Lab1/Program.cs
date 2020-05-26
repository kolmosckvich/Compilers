using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
using System.Drawing;
using System.Drawing.Imaging;

namespace Lab1
{
    public class Program
    {
        static void Main(string[] args)
        {
            GramFileProcessor.WriteGramm(CreateTestGramm(), "TestGramm.json");

            Gramm input = GramFileProcessor.ReadGramm("TestGramm.json");
            var reachGr = UnreachableProcessor.RemoveUnreachable(input);
            var newGr = GrammProcessor.RemoveLR(reachGr);
            var factGr = FactProcessor.Fact(newGr);
            GramFileProcessor.WriteGramm(factGr, "ResultGramm.json");
        }

        private static Gramm CreateTestGramm()
        {
            Gramm testGramm = new Gramm();
            //testGramm.Terms.Add("+");
            //testGramm.Terms.Add("*");
            //testGramm.Terms.Add("(");
            //testGramm.Terms.Add(")");
            testGramm.Terms.Add("a");
            testGramm.Terms.Add("b");
            testGramm.Terms.Add("c");
            testGramm.Terms.Add("d");
            testGramm.Terms.Add("e");

            testGramm.NonTerms.Add("E");//Escape
            testGramm.NonTerms.Add("F");//From
            testGramm.NonTerms.Add("T");//Tarkov
            testGramm.NonTerms.Add("H");
            //testGramm.NonTerms.Add("S");
            //testGramm.NonTerms.Add("A");
            //testGramm.NonTerms.Add("B");
            //testGramm.NonTerms.Add("C");
            
            testGramm.St = "E";
            //testGramm.St = "S";


            testGramm.Rules.Add(new Rule("E", new List<string>() { "E", "a"}));
            testGramm.Rules.Add(new Rule("F", new List<string>() { "T", "F"}));
            testGramm.Rules.Add(new Rule("T", new List<string>() { "a", "E"}));
            testGramm.Rules.Add(new Rule("E", new List<string>() { "a", "F", "a"}));
            testGramm.Rules.Add(new Rule("H", new List<string>() { "a", "F", "a"}));
            testGramm.Rules.Add(new Rule("H", new List<string>() { "E", "E", "a"}));

            //testGramm.Rules.Add(new Rule("S", new List<string>() { "A", "a"}));
            //testGramm.Rules.Add(new Rule("S", "b"));
            //testGramm.Rules.Add(new Rule("A", new List<string>() { "A", "c"}));
            //testGramm.Rules.Add(new Rule("A", new List<string>() {"S", "d"}));
            //testGramm.Rules.Add(new Rule("A", new List<string>() { "a", "b" }));
            //testGramm.Rules.Add(new Rule("A", new List<string>() { "a", "b", "c" }));
            //testGramm.Rules.Add(new Rule("A", new List<string>() { "a", "b", "c", "S" }));
            //testGramm.Rules.Add(new Rule("A", new List<string>() { "b", "b", "b", "b", "S" }));
            //testGramm.Rules.Add(new Rule("A", new List<string>() { "a", "b", "c", "A" }));
            //testGramm.Rules.Add(new Rule("A", new List<string>() { "a", "b", "S" }));

            //testGramm.Rules.Add(new Rule("A", new List<string>() { "c", "d", "B" }));

            //testGramm.Rules.Add(new Rule("B", new List<string>() { "a", "b", "S" }));
            //testGramm.Rules.Add(new Rule("B", new List<string>() { "a", "b", "A" }));

            //testGramm.Rules.Add(new Rule("C", new List<string>() { "b", "S" }));

            return testGramm;
        }
    }
}
