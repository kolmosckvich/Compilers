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
        private static List<Oper> operations;
        private static List<Element> brackets;
        private static List<Element> terms;
        private static Element brake;

        static void Main(string[] args)
        {
            operations = StandOpers();
            brackets = StandBrackets();
            terms = StandTerms();
            brake = new Element("$");
            Table tab = new Table(operations, terms, brackets, brake);
            tab.PrintTable();
            //string input = "(1) & 0 & ~1 ! a & 1 $";
            //string input = "(1) & 0 & ~1 ! a & 1 & 0 ! 1 & (1 ! ~0)$";
            string input = "~1 & ~0$";
            GrammProcessor GP = new GrammProcessor(operations, terms, brackets, brake, tab);

            string res = GP.Process(input);
            if(res == "")
                Console.WriteLine("Error during processing!");
            else
                Console.WriteLine($"Result: {res}");

        }

        private static List<Oper> StandOpers()
        {
            List<Oper> opers = new List<Oper>();

            opers.Add(new Oper("~", true, true, 3));
            opers.Add(new Oper("!", true, false, 1));
            opers.Add(new Oper("|", true, false, 1));
            opers.Add(new Oper("&", true, false, 2));

            return opers;
        }

        private static List<Element> StandBrackets()
        {
            List<Element> bracks = new List<Element>();

            bracks.Add(new Element("("));
            bracks.Add(new Element(")"));

            return bracks;
        }

        private static List<Element> StandTerms()
        {
            List<Element> ter = new List<Element>();

            ter.Add(new Element("a"));
            ter.Add(new Element("0"));
            ter.Add(new Element("1"));

            return ter;
        }


        private static Gramm CreateTestGramm()
        {
            Gramm testGramm = new Gramm();
            testGramm.Terms.Add("a");
            testGramm.Terms.Add("b");
            testGramm.Terms.Add("c");
            testGramm.Terms.Add("d");
            testGramm.Terms.Add("e");

            testGramm.NonTerms.Add("E");
            testGramm.NonTerms.Add("F");
            testGramm.NonTerms.Add("T");
            testGramm.NonTerms.Add("H");
            testGramm.NonTerms.Add("Z");
            testGramm.St = "E";


            testGramm.Rules.Add(new Rule("E", new List<string>() { "E", "a" }));
            testGramm.Rules.Add(new Rule("F", new List<string>() { "T", "F" }));
            testGramm.Rules.Add(new Rule("T", new List<string>() { "a", "E" }));
            testGramm.Rules.Add(new Rule("E", new List<string>() { "a", "F", "a" }));
            testGramm.Rules.Add(new Rule("H", new List<string>() { "a", "F", "a" }));
            testGramm.Rules.Add(new Rule("H", new List<string>() { "E", "E", "a" }));
            testGramm.Rules.Add(new Rule("Z", new List<string>() { "Z", "Z" }));

            return testGramm;
        }


    }
}
