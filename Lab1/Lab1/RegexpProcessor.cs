using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1
{
    public static class RegexpProcessor
    {
        public static string PreprocessRegexp(string regexp)
        {
            StringBuilder sb = new StringBuilder();
            char prev = char.MaxValue;
            char curr;
            foreach (var ch in regexp)
            {
                curr = ch;
                if (!Utils.alphabet.Concat(Utils.operations).Contains(curr))
                {
                    throw new Exception($"Unknown symbol - {curr}");
                }
                if ((prev == '*' || prev == '+') && (curr == '+' || curr == '*'))
                {
                    throw new Exception("Preprocessing ops error");
                }
                if ((prev == '|' || prev == '&') && (curr == '|' || curr == '&'))
                {
                    throw new Exception("Preprocessing ops error");
                }
                if (Utils.alphabet.Contains(prev) || prev == ')' || prev == '+' || prev == '*')
                {
                    if (Utils.alphabet.Contains(curr) || curr == '(')
                    {
                        sb.Append('&');
                    }
                }
                prev = curr;
                sb.Append(curr);
            }
            return sb.ToString();
        }

        public static List<char> SortingStation(string regexp)
        {
            List<char> result = new List<char>();
            Stack<char> stack = new Stack<char>();
            char op;
            foreach (var ch in regexp)
            {
                if (!Utils.operations.Contains(ch))
                {
                    result.Add(ch);
                }
                else if (ch != '(' && ch != ')')
                {
                    char hOp;
                    if (stack.TryPeek(out hOp))
                    {
                        int curInd = Utils.operations.IndexOf(ch);
                        int hOpInd = Utils.operations.IndexOf(hOp);
                        while (Utils.opPriors[hOpInd] >= Utils.opPriors[curInd])
                        {
                            op = stack.Pop();
                            result.Add(op);
                            if (!stack.TryPeek(out hOp))
                            {
                                break;
                            }
                            hOpInd = Utils.operations.IndexOf(hOp);
                        }
                    }
                    stack.Push(ch);
                }
                else if (ch == '(')
                {
                    stack.Push(ch);
                }
                else if (ch == ')')
                {
                    if (!stack.TryPop(out op))
                    {
                        throw new Exception("Unopened bracket");
                    }
                    while (op != '(')
                    {
                        result.Add(op);
                        if (!stack.TryPop(out op))
                        {
                            throw new Exception("Unopened bracket");
                        }
                    }
                }
                else
                {
                    throw new Exception("Wut");
                }
            }
            while (stack.TryPop(out op))
            {
                if (op == '(')
                {
                    throw new Exception("Unclosed bracket");
                }
                else
                {
                    result.Add(op);
                }
            }

            return result;
        }
    }
}
