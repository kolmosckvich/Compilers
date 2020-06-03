using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Lab1
{
    public class Program
    {
        private static bool R = false;

        static void Main(string[] args)
        {
            string filename = "code.txt";

            using (StreamReader file = new StreamReader(filename))
            {
                AntlrInputStream inputStream = new AntlrInputStream(file.ReadToEnd());

                ClojureObrLexer lexer = new ClojureObrLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
                ClojureObrParser parser = new ClojureObrParser(commonTokenStream);

                ObrErrorListener errorListener = new ObrErrorListener();
                parser.AddErrorListener(errorListener);

                ClojureObrParser.FileContext tree = parser.file();
                if (parser.NumberOfSyntaxErrors != 0)
                {
                    Console.WriteLine($"Syntax is bad :( ");
                    foreach(var error in errorListener.Errors)
                    {
                        Console.WriteLine(error);
                    }
                    Console.ReadKey();
                    return;
                }
                if(R)
                {
                    ParseTreeWalker walker = new ParseTreeWalker();
                    ObrListener semantic = new ObrListener();
                    try
                    {
                        walker.Walk(semantic, tree);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($" :( Error:  {e.ToString()}");

                        Console.ReadKey();
                        return;
                    }
                    GraphProcessor.BuildGraph(semantic.root, "tree");
                }
                
            }

            Console.WriteLine("Syntax not so bad :| ");
            Console.ReadKey();
        }

    }
}
