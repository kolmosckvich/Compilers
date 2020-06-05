using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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

        public ParseTreeProperty<BaseNode> TypedNodes = new ParseTreeProperty<BaseNode>();
        public FileNode typedRoot;

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
            //


        }

        public override void ExitList([NotNull] ClojureObrParser.ListContext context)
        {
            //
            //Console.WriteLine("Вышел из списка");
            //
            var exprs = ((ExpressionsNode)TypedNodes.Get(context.children[1])).expressions;
            ListNode node = new ListNode(exprs);
            TypedNodes.Put(context, node);
        }

        public override void EnterDefine([NotNull] ClojureObrParser.DefineContext context)
        {
            //
            //Console.WriteLine("Вошел в define");
            //Console.WriteLine("Вышел из define");
            var sym = context.children[2];
            var expr = context.children[3];
            currentSymbolTable.AddSymbol(sym.GetText(), expr.GetText(), SymType.Fun);
            //Console.WriteLine($"Определяемый символ - {sym.GetText()}");
            //
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
                //Console.WriteLine($"Значение выражения - {symbol.Value}");
            }
            else
            {
                currentSymbolTable.AddSymbol(sym.GetText(), expr.GetText(), SymType.Fun);
                //Console.WriteLine($"Значение выражения - {expr.GetText()}");
            }
            //Console.WriteLine("Вышел из define");
            //

            var symNode = (SymNode)TypedNodes.Get(sym);
            var exprNode = (ExprNode)TypedNodes.Get(expr);
            var node = new DefNode(symNode, exprNode);
            TypedNodes.Put(context, node);
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
            //
        }

        public override void ExitSymbol([NotNull] ClojureObrParser.SymbolContext context)
        {
            //
            //Console.WriteLine("Вышел из символа");
            string symbText = context.GetText();
            Symbol symb = null;
            if(ExprSymTypes.Get(context.Parent) != SymType.Key 
                && ExprSymTypes.Get(context.Parent) != SymType.Fun)
            {
                symb = currentSymbolTable.Symbols[symbText];
                ExprSymTypes.Put(context, SymType.Sym);
                ExprSyms.Put(context, symb);
            }
            //
            
            SymNode node = new SymNode(symbText, symb?.Value);
            TypedNodes.Put(context, node);
            
        }

        public override void EnterLiteral([NotNull] ClojureObrParser.LiteralContext context)
        {
            //
        }

        public override void ExitLiteral([NotNull] ClojureObrParser.LiteralContext context)
        {
            //
            Symbol symb = null;
            if (context.children.Count == 1 && ExprSymTypes.Get(context.children[0]) == SymType.Sym)
            {
                ExprSymTypes.Put(context, SymType.Sym);
                symb = ExprSyms.Get(context.children[0]);
                ExprSyms.Put(context, symb);
            }
            //
            var text = context.GetText();
            var node = TypedNodes.Get(context.children[0]);

            TypedNodes.Put(context, node);
        }

        public override void EnterKeyword([NotNull] ClojureObrParser.KeywordContext context)
        {
            //
            ExprSymTypes.Put(context, SymType.Key);
            //
        }

        public override void ExitKeyword([NotNull] ClojureObrParser.KeywordContext context)
        {
            //
            KeyWordNode node = new KeyWordNode(context.GetText());
            TypedNodes.Put(context, node);
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
            //
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
            //
            ExprNode node = new ExprNode();

            foreach (var child in context.children)
            {
                var childNode = TypedNodes.Get(child);
                if(childNode != null)
                {
                    switch(childNode.type)
                    {
                        case NodeType.Def:
                            node = (ExprNode)childNode;
                            break;
                        case NodeType.Cond:
                            node = (ExprNode)childNode;
                            break;
                        case NodeType.Fun:
                            node = (ExprNode)childNode;
                            break;
                        case NodeType.List:
                            node = (ExprNode)childNode;
                            break;
                        case NodeType.Vector:
                            node = (ExprNode)childNode;
                            break;
                        case NodeType.Map:
                            node = (ExprNode)childNode;
                            break;
                        case NodeType.Val:
                            node = (ExprNode)childNode;
                            break;
                        case NodeType.Sym:
                            node = (ExprNode)childNode;
                            break;
                        case NodeType.KeyWord:
                            node = (ExprNode)childNode;
                            break;
                        default:
                            break;
                    }
                }
            }

            TypedNodes.Put(context, node);
        }

        public override void EnterFun([NotNull] ClojureObrParser.FunContext context)
        {
            //
            ExprSymTypes.Put(context, SymType.Fun);
        }

        public override void ExitFun([NotNull] ClojureObrParser.FunContext context)
        {
            //
            int childsCount = context.children.Count;
            List<SymNode> paramNodes = new List<SymNode>();

            for(int i = 3; i < childsCount - 3; i++)
            {
                paramNodes.Add((SymNode)TypedNodes.Get(context.children[i]));
            }
            var exprNodes = (ExpressionsNode)TypedNodes.Get(context.children[childsCount - 2]);
            int exprCount = exprNodes.expressions.Count;
            var lastExpr = exprNodes.expressions[exprCount - 1];
            var exprMid = exprNodes.expressions.SkipLast(1).ToList();

            FunNode node = new FunNode(paramNodes, lastExpr, exprMid);
            TypedNodes.Put(context, node);
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
            //
        }

        public override void ExitEveryRule([NotNull] ParserRuleContext context)
        {
            //
            if(context.Parent != null)
            {

                var ParentNode = Nodes.Get(context.Parent);
                var currNode = Nodes.Get(context);
                var NodesList = context.children.Where(x => Nodes.Get(x) != null).ToList();

                if (NodesList.Count == 1)
                {
                    if(currNode.TypeNode == "List" || currNode.TypeNode == "Vector" || currNode.TypeNode == "Keyword")
                    {
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
                        foreach(var child in currNode.Childs[0].Childs)
                        {
                            child.Parent = currNode;
                            currNode.Childs.Add(child);
                        }
                        currNode.Childs[0].Parent = null;
                        currNode.Childs.RemoveAt(0);
                        ParentNode.Childs.Add(currNode);
                    }
                    else
                    {
                        var oldCurrNode = currNode;
                        currNode = Nodes.Get(NodesList[0]);

                        Nodes.Put(context, currNode);

                        currNode.Parent = ParentNode;
                        ParentNode.Childs.Add(currNode);
                        oldCurrNode.Childs.Remove(currNode);
                    }
                    
                }
                else
                {
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
            //
        }

        public override void EnterFile([NotNull] ClojureObrParser.FileContext context)
        {
            //
            root = new TreeNode();
            Nodes.Put(context, root);
            //
        }

        public override void ExitFile([NotNull] ClojureObrParser.FileContext context)
        {
            //
            string fullName = context.GetType().ToString();
            int from = fullName.LastIndexOf('+');
            string name = fullName.Substring(from + 1, fullName.Length - from - 1).Replace("Context", "");

            root.TypeNode = name;
            root.TextNode = "File";
            //

            var list = new List<ExprNode>();
            foreach (var child in context.children)
            {
                var node = TypedNodes.Get(child);
                if (node != null)
                {
                    var exprNode = (ExprNode)node;
                    list.Add(exprNode);
                }
            }
            typedRoot = new FileNode(list);
        }

        public override void EnterCondition([NotNull] ClojureObrParser.ConditionContext context)
        {
            //
        }

        public override void ExitCondition([NotNull] ClojureObrParser.ConditionContext context)
        {
            //
            ExprNode condExpr = (ExprNode)TypedNodes.Get(context.children[2]);
            ExprNode thenExpr = (ExprNode)TypedNodes.Get(context.children[3]);
            ExprNode elseExpr = null;
            if (context.children.Count > 5)
                elseExpr = (ExprNode)TypedNodes.Get(context.children[4]);
            CondNode node = new CondNode(condExpr, thenExpr, elseExpr);
            TypedNodes.Put(context, node);
        }

        public override void EnterExpressions([NotNull] ClojureObrParser.ExpressionsContext context)
        {
            //
        }

        public override void ExitExpressions([NotNull] ClojureObrParser.ExpressionsContext context)
        {
            //
            List<ExprNode> nodes = new List<ExprNode>();
            foreach(var child in context.children)
            {
                nodes.Add((ExprNode)TypedNodes.Get(child));
            }
            ExpressionsNode node = new ExpressionsNode(nodes);
            TypedNodes.Put(context, node);
        }

        public override void EnterVector([NotNull] ClojureObrParser.VectorContext context)
        {
            //
        }

        public override void ExitVector([NotNull] ClojureObrParser.VectorContext context)
        {
            //
            var exprs = ((ExpressionsNode)TypedNodes.Get(context.children[1])).expressions;
            VectorNode node = new VectorNode(exprs);
            TypedNodes.Put(context, node);
        }

        public override void EnterMap([NotNull] ClojureObrParser.MapContext context)
        {
            //
        }

        public override void ExitMap([NotNull] ClojureObrParser.MapContext context)
        {
            //
            int count = context.children.Count;
            var pairs = new List<KeyValuePairNode>();
            for (int i = 1; i < count -1; i+=2)
            {
                KeyWordNode key = (KeyWordNode)TypedNodes.Get(context.children[i]);
                ExprNode val = (ExprNode)TypedNodes.Get(context.children[i+1]);
                pairs.Add(new KeyValuePairNode(key, val));
            }
            MapNode node = new MapNode(pairs);
            TypedNodes.Put(context, node);
        }

        public override void EnterBool([NotNull] ClojureObrParser.BoolContext context)
        {
            //
        }

        public override void ExitBool([NotNull] ClojureObrParser.BoolContext context)
        {
            //
            LitNode node = new LitNode(SymType.Bool, context.GetText());
            TypedNodes.Put(context, node);
        }

        public override void EnterCharacter([NotNull] ClojureObrParser.CharacterContext context)
        {
            //
        }

        public override void ExitCharacter([NotNull] ClojureObrParser.CharacterContext context)
        {
            //
            LitNode node = new LitNode(SymType.Char, context.GetText());
            TypedNodes.Put(context, node);
        }

        public override void EnterFloat([NotNull] ClojureObrParser.FloatContext context)
        {
            //
        }

        public override void ExitFloat([NotNull] ClojureObrParser.FloatContext context)
        {
            //
            LitNode node = new LitNode(SymType.Float, context.GetText());
            TypedNodes.Put(context, node);
        }

        public override void EnterInt([NotNull] ClojureObrParser.IntContext context)
        {
            //
        }

        public override void ExitInt([NotNull] ClojureObrParser.IntContext context)
        {
            //
            LitNode node = new LitNode(SymType.Int, context.GetText());
            TypedNodes.Put(context, node);
        }

        public override void EnterNil([NotNull] ClojureObrParser.NilContext context)
        {
            //
        }

        public override void ExitNil([NotNull] ClojureObrParser.NilContext context)
        {
            //
            LitNode node = new LitNode(SymType.Nil, context.GetText());
            TypedNodes.Put(context, node);
        }

        public override void EnterNumber([NotNull] ClojureObrParser.NumberContext context)
        {
            //
        }

        public override void ExitNumber([NotNull] ClojureObrParser.NumberContext context)
        {
            //
            var node = TypedNodes.Get(context.children[0]);
            TypedNodes.Put(context, node);
        }

        public override void EnterString([NotNull] ClojureObrParser.StringContext context)
        {
            //
        }

        public override void ExitString([NotNull] ClojureObrParser.StringContext context)
        {
            //
            LitNode node = new LitNode(SymType.Str, context.GetText());
            TypedNodes.Put(context, node);
        }
    }


}
