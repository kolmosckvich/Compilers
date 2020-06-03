using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    public class ObrListener : ClojureObrBaseListener
    {

        //public ParseTreeProperty<SymTab> SymbolTables = new ParseTreeProperty<SymTab>();
        public ParseTreeProperty<SymType> ExprSymTypes = new ParseTreeProperty<SymType>();
        public ParseTreeProperty<Symbol> ExprSyms = new ParseTreeProperty<Symbol>();
        public SymTab currentSymbolTable = new SymTab();
        public ParseTreeProperty<TreeNode> Nodes = new ParseTreeProperty<TreeNode>();
        public TreeNode root;

        public override void EnterList([NotNull] ClojureObrParser.ListContext context)
        {
            //
            /*
            Console.WriteLine("Вошел в список");
            if(context.children.Count > 2)
            {
                var first = context.children[1];
                Console.WriteLine($"Первый элемент - {first.GetText()}");
            }
            */  
        }

        public override void ExitList([NotNull] ClojureObrParser.ListContext context)
        {
            //
            //Console.WriteLine("Вышел из списка");
        }

        public override void EnterDefine([NotNull] ClojureObrParser.DefineContext context)
        {
            //
            //Console.WriteLine("Вошел в define");
            Console.WriteLine("Вышел из define");
            var sym = context.children[2];
            var expr = context.children[3];
            currentSymbolTable.AddSymbol(sym.GetText(), expr.GetText(), SymType.Fun);
            Console.WriteLine($"Определяемый символ - {sym.GetText()}");
        }

        public override void ExitDefine([NotNull] ClojureObrParser.DefineContext context)
        {
            //
            var sym = context.children[2];
            var expr = context.children[3];
            /*
            if (ExprSymTypes.Get(sym) != SymType.Sym)
            {
                throw new Exception($"Define requires symbol as second parameter");
            }
            */
            if (ExprSymTypes.Get(context) == SymType.Sym)
            {
                var symbol = ExprSyms.Get(expr);
                currentSymbolTable.AddSymbol(sym.GetText(), symbol.Value, SymType.Fun);
                Console.WriteLine($"Значение выражения - {symbol.Value}");
            }
            else
            {
                currentSymbolTable.AddSymbol(sym.GetText(), expr.GetText(), SymType.Fun);
                Console.WriteLine($"Значение выражения - {expr.GetText()}");
            }
            Console.WriteLine("Вышел из define");
        }

        public override void EnterSymbol([NotNull] ClojureObrParser.SymbolContext context)
        {
            //
            //Console.WriteLine("Вошел в символ");
            //Console.WriteLine($"Символ - {context.GetText()}");

            //Add keyword check

            string symbText = context.GetText();
            if (!currentSymbolTable.Symbols.ContainsKey(symbText) 
                && ExprSymTypes.Get(context.Parent) != SymType.Key 
                && ExprSymTypes.Get(context.Parent) != SymType.Fun)
            {
                throw new Exception($"Symbol does not defined, {context.Start.Line}: {context.Start.Column}");
            }
        }

        public override void ExitSymbol([NotNull] ClojureObrParser.SymbolContext context)
        {
            //
            //Console.WriteLine("Вышел из символа");
            string symbText = context.GetText();
            if(ExprSymTypes.Get(context.Parent) != SymType.Key 
                && ExprSymTypes.Get(context.Parent) != SymType.Fun)
            {
                Symbol symb = currentSymbolTable.Symbols[symbText];
                ExprSymTypes.Put(context, SymType.Sym);
                ExprSyms.Put(context, symb);
            }
        }

        public override void EnterLiteral([NotNull] ClojureObrParser.LiteralContext context)
        {
            //
        }

        public override void ExitLiteral([NotNull] ClojureObrParser.LiteralContext context)
        {
            //
            if (context.children.Count == 1 && ExprSymTypes.Get(context.children[0]) == SymType.Sym)
            {
                ExprSymTypes.Put(context, SymType.Sym);
                Symbol symb = ExprSyms.Get(context.children[0]);
                ExprSyms.Put(context, symb);
            }
        }

        public override void EnterKeyword([NotNull] ClojureObrParser.KeywordContext context)
        {
            //
            ExprSymTypes.Put(context, SymType.Key);
        }

        public override void ExitKeyword([NotNull] ClojureObrParser.KeywordContext context)
        {
            //
        }

        public override void EnterExpr([NotNull] ClojureObrParser.ExprContext context)
        {
            //
            /*
            Console.WriteLine("Вошел в выражение");
            Console.WriteLine($"Выражение - {context.GetText()}");
            Console.WriteLine($"--|Текущяя таблица символов :");
            foreach(var sym in currentSymbolTable.Symbols)
            {
                string symName = sym.Key;
                string symVal = sym.Value.Value;
                Console.WriteLine($"----|{symName} -- {symVal}");
            }
            */
        }

        public override void ExitExpr([NotNull] ClojureObrParser.ExprContext context)
        {
            //
            //Console.WriteLine("Вышел из выражения");
            ExprSymTypes.Put(context, SymType.Fun);
            if (context.children.Count == 1 && ExprSymTypes.Get(context.children[0]) == SymType.Sym)
            {
                ExprSymTypes.Put(context, SymType.Sym);
                Symbol symb = ExprSyms.Get(context.children[0]);
                ExprSyms.Put(context, symb);
            }
        }

        public override void EnterFun([NotNull] ClojureObrParser.FunContext context)
        {
            //
            ExprSymTypes.Put(context, SymType.Fun);
        }

        public override void ExitFun([NotNull] ClojureObrParser.FunContext context)
        {
            //
        }


        public override void EnterEveryRule([NotNull] ParserRuleContext context)
        {
            //
            string fullName = context.GetType().ToString();
            int from = fullName.LastIndexOf('+');
            string name = fullName.Substring(from + 1, fullName.Length - from - 1).Replace("Context", "");
            //Console.WriteLine(name);

            var node = new TreeNode();
            node.TypeNode = name;

            Nodes.Put(context, node);
        }

        public override void ExitEveryRule([NotNull] ParserRuleContext context)
        {
            //
            if(context.Parent != null)
            {

                var ParentNode = Nodes.Get(context.Parent);
                var currNode = Nodes.Get(context);

                currNode.TextNode = context.GetText();
                if (ExprSymTypes.Get(context) != SymType.Nil)
                {
                    currNode.MacroType = ExprSymTypes.Get(context).ToString();
                }

                if (ExprSyms.Get(context) != null)
                {
                    currNode.NodeValue = ExprSyms.Get(context).Value;
                }

                currNode.Parent = ParentNode;
                ParentNode.Childs.Add(currNode);
            }
        }

        public override void EnterFile([NotNull] ClojureObrParser.FileContext context)
        {
            //
            root = new TreeNode();
            Nodes.Put(context, root);
        }

        public override void ExitFile([NotNull] ClojureObrParser.FileContext context)
        {
            //
            string fullName = context.GetType().ToString();
            int from = fullName.LastIndexOf('+');
            string name = fullName.Substring(from + 1, fullName.Length - from - 1).Replace("Context", "");

            root.TypeNode = name;
            root.TextNode = "File";
        }

        public override void EnterCondition([NotNull] ClojureObrParser.ConditionContext context)
        {
            //
        }

        public override void ExitCondition([NotNull] ClojureObrParser.ConditionContext context)
        {
            //
        }

        public override void EnterExpressions([NotNull] ClojureObrParser.ExpressionsContext context)
        {
            //
        }

        public override void ExitExpressions([NotNull] ClojureObrParser.ExpressionsContext context)
        {
            //
        }

        public override void EnterVector([NotNull] ClojureObrParser.VectorContext context)
        {
            //
        }

        public override void ExitVector([NotNull] ClojureObrParser.VectorContext context)
        {
            //
        }

        public override void EnterMap([NotNull] ClojureObrParser.MapContext context)
        {
            //
        }

        public override void ExitMap([NotNull] ClojureObrParser.MapContext context)
        {
            //
        }

        public override void EnterBool([NotNull] ClojureObrParser.BoolContext context)
        {
            //
        }

        public override void ExitBool([NotNull] ClojureObrParser.BoolContext context)
        {
            //
        }

        public override void EnterChar([NotNull] ClojureObrParser.CharContext context)
        {
            //
        }

        public override void ExitChar([NotNull] ClojureObrParser.CharContext context)
        {
            //
        }

        public override void EnterNamed_char([NotNull] ClojureObrParser.Named_charContext context)
        {
            //
        }

        public override void ExitNamed_char([NotNull] ClojureObrParser.Named_charContext context)
        {
            //
        }

        public override void EnterCharacter([NotNull] ClojureObrParser.CharacterContext context)
        {
            //
        }

        public override void ExitCharacter([NotNull] ClojureObrParser.CharacterContext context)
        {
            //
        }

        public override void EnterFloat([NotNull] ClojureObrParser.FloatContext context)
        {
            //
        }

        public override void ExitFloat([NotNull] ClojureObrParser.FloatContext context)
        {
            //
        }

        public override void EnterInt([NotNull] ClojureObrParser.IntContext context)
        {
            //
        }

        public override void ExitInt([NotNull] ClojureObrParser.IntContext context)
        {
            //
        }

        public override void EnterNil([NotNull] ClojureObrParser.NilContext context)
        {
            //
        }

        public override void ExitNil([NotNull] ClojureObrParser.NilContext context)
        {
            //
        }

        public override void EnterNumber([NotNull] ClojureObrParser.NumberContext context)
        {
            //
        }

        public override void ExitNumber([NotNull] ClojureObrParser.NumberContext context)
        {
            //
        }

        public override void EnterString([NotNull] ClojureObrParser.StringContext context)
        {
            //
        }

        public override void ExitString([NotNull] ClojureObrParser.StringContext context)
        {
            //
        }
    }


}
