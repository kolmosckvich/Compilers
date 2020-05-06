using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    public static class NKAProcessor
    {

        public static Node GetNKA(List<char> polska)
        {
            Stack<Node> nodes = new Stack<Node>();
            Node init = new Node();
            //Node first = null;
            nodes.Push(init);
            bool isFirst = true;
            foreach(var ch in polska)
            {
                if(Utils.alphabet.Contains(ch))
                {
                    Node charN = new GraphAutomat(ch);
                    if(isFirst)
                    {
                        isFirst = false;
                        GraphAutomat.Bind(init, charN);
                        //first = charN;
                    }
                    nodes.Push(charN);
                }
                else if (ch == '&')
                {
                    Node a = nodes.Pop();
                    Node b = nodes.Pop();
                    Node res = GraphAutomat.GetConcatAuto(b, a);
                    nodes.Push(res);
                }
                else if (ch == '|')
                {
                    Node a = nodes.Pop();
                    Node b = nodes.Pop();
                    Node res = GraphAutomat.GetChooseAuto(b, a);
                    nodes.Push(res);
                }
                else if (ch == '*')
                {
                    Node a = nodes.Pop();
                    Node res = GraphAutomat.GetStar(a);
                    nodes.Push(res);
                }
                else if (ch == '+')
                {
                    Node a = nodes.Pop();
                    Node res = GraphAutomat.GetPlus(a);
                    nodes.Push(res);
                }
            }
            //return first;
            return init;
        }
    }
}
