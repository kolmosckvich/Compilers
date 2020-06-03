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
            //string input = "15 & true & ~ 15 & true ! 19 & 10 & ~5 ! true";
            string input = "begin" +
                           "15 = true;"+
                           "21 = true & 15;"+
                           "8 = 21 ! true & ~false;"+
                           "15 = ~8 & 15 ! false"+
                           "end";
            bool res = GrammProcessor.CheckInput(input);
            if(!res)
            {
                Console.WriteLine("Incorrect input");
            }
            else
            {
                Console.WriteLine("Correct input");
            }
        }

        private static Gramm CreateFinGramm()
        {
            Gramm testGramm = new Gramm();

            //4
            testGramm.NonTerms.Add("Expr");
            testGramm.NonTerms.Add("LogExpr");
            testGramm.NonTerms.Add("LogOne");
            testGramm.NonTerms.Add("LogSec");
            testGramm.NonTerms.Add("LogFir");
            testGramm.NonTerms.Add("LogVal");
            testGramm.NonTerms.Add("LogTerm");
            testGramm.NonTerms.Add("Id");

            testGramm.NonTerms.Add("Prog");
            testGramm.NonTerms.Add("Block");
            testGramm.NonTerms.Add("OpList");
            testGramm.NonTerms.Add("Op");

            testGramm.NonTerms.Add("LogExpr_");
            testGramm.NonTerms.Add("LogOne_");
            testGramm.NonTerms.Add("OpList_");

            testGramm.St = "Expr";

            testGramm.Rules.Add(new Rule("Expr", new List<string>() { "LogExpr" }));

            testGramm.Rules.Add(new Rule("LogExpr", new List<string>() { "LogOne", "LogExpr_" }));

            testGramm.Rules.Add(new Rule("LogExpr_", new List<string>() {"!", "LogOne", "LogExpr_" }));
            testGramm.Rules.Add(new Rule("LogExpr_", new List<string>() { "Eps" }));

            testGramm.Rules.Add(new Rule("LogOne", new List<string>() { "LogSec", "LogOne_" }));

            testGramm.Rules.Add(new Rule("LogOne_", new List<string>() { "&", "LogSec", "LogOne_" }));
            testGramm.Rules.Add(new Rule("LogOne_", new List<string>() { "Eps" }));

            testGramm.Rules.Add(new Rule("LogSec", new List<string>() { "LogFir" }));
            testGramm.Rules.Add(new Rule("LogSec", new List<string>() { "~", "LogFir" }));

            testGramm.Rules.Add(new Rule("LogFir", new List<string>() { "LogVal" }));
            testGramm.Rules.Add(new Rule("LogFir", new List<string>() { "Id" }));

            testGramm.Rules.Add(new Rule("LogVal", new List<string>() { "true" }));
            testGramm.Rules.Add(new Rule("LogVal", new List<string>() { "false" }));

            testGramm.Rules.Add(new Rule("LogTerm", new List<string>() { "~" }));
            testGramm.Rules.Add(new Rule("LogTerm", new List<string>() { "&" }));
            testGramm.Rules.Add(new Rule("LogTerm", new List<string>() { "!" }));


            testGramm.Rules.Add(new Rule("Prog", new List<string>() { "Block" }));
            testGramm.Rules.Add(new Rule("Block", new List<string>() { "begin", "OpList", "end" }));
            testGramm.Rules.Add(new Rule("OpList", new List<string>() { "Op", "OpList_" }));
            testGramm.Rules.Add(new Rule("OpList_", new List<string>() { ";", "Op", "OpList_" }));
            testGramm.Rules.Add(new Rule("OpList_", new List<string>() { "Eps" }));
            testGramm.Rules.Add(new Rule("Op", new List<string>() { "Id", "=", "Expr" }));



            return testGramm;
        }

        private static Gramm CreateTestGramm()
        {
            Gramm testGramm = new Gramm();
           
            //4
            testGramm.NonTerms.Add("Expr");
            testGramm.NonTerms.Add("LogExpr");
            testGramm.NonTerms.Add("LogOne");
            testGramm.NonTerms.Add("LogSec");
            testGramm.NonTerms.Add("LogFir");
            testGramm.NonTerms.Add("LogVal");
            testGramm.NonTerms.Add("LogTerm");
            testGramm.NonTerms.Add("Id");

            testGramm.NonTerms.Add("Prog");
            testGramm.NonTerms.Add("Block");
            testGramm.NonTerms.Add("OpList");
            testGramm.NonTerms.Add("Op");

            testGramm.St = "Expr";

            testGramm.Rules.Add(new Rule("Expr", new List<string>() { "LogExpr" }));

            testGramm.Rules.Add(new Rule("LogExpr", new List<string>() { "LogOne" }));
            testGramm.Rules.Add(new Rule("LogExpr", new List<string>() { "LogExpr", "!", "LogOne" }));

            testGramm.Rules.Add(new Rule("LogOne", new List<string>() { "LogSec" }));
            testGramm.Rules.Add(new Rule("LogOne", new List<string>() { "LogOne", "&", "LogSec" }));

            testGramm.Rules.Add(new Rule("LogSec", new List<string>() { "LogFir" }));
            testGramm.Rules.Add(new Rule("LogSec", new List<string>() {"~", "LogFir" }));

            testGramm.Rules.Add(new Rule("LogFir", new List<string>() { "LogVal"}));
            testGramm.Rules.Add(new Rule("LogFir", new List<string>() { "Id" }));

            testGramm.Rules.Add(new Rule("LogVal", new List<string>() { "true" }));
            testGramm.Rules.Add(new Rule("LogVal", new List<string>() { "false" }));

            testGramm.Rules.Add(new Rule("LogTerm", new List<string>() { "~" }));
            testGramm.Rules.Add(new Rule("LogTerm", new List<string>() { "&" }));
            testGramm.Rules.Add(new Rule("LogTerm", new List<string>() { "!" }));


            testGramm.Rules.Add(new Rule("Prog", new List<string>() { "Block" }));
            testGramm.Rules.Add(new Rule("Block", new List<string>() { "begin", "OpList", "end" }));
            testGramm.Rules.Add(new Rule("OpList", new List<string>() { "Op"}));
            testGramm.Rules.Add(new Rule("OpList", new List<string>() { "OpList", ";", "Op" }));
            testGramm.Rules.Add(new Rule("Op", new List<string>() { "Id", "=", "Expr" }));



            return testGramm;
        }
    }
}
