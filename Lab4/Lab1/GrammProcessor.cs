using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static Lab1.Utils;

namespace Lab1
{
    public class GrammProcessor
    {
        private List<Oper> operations;
        private List<Element> terms;
        private List<Element> brackets;
        private Element brake;
        private List<Element> elements;
        private Table table;
        private Element start;

        public GrammProcessor(List<Oper> opers, List<Element> ter, List<Element> brack, Element br, Table tab)
        {
            operations = opers;
            terms = ter;
            brackets = brack;
            brake = br;
            table = tab;
            start = new Element("@");
        }

        public string Process(string input)
        {
            string result = "";
            List<Element> parsInp = ToElems(input);
            result = Process(parsInp);
            return result;
        }

        private List<Element> ToElems(string input)
        {
            var merge = terms.Append(brake).Concat(brackets).Concat(operations).ToList();
            List<Element> elems = new List<Element>();
            foreach(var s in input.Replace(" ", String.Empty))
            {
                elems.Add(merge.Where(x => x.Name == s.ToString()).First());
            }
            return elems;
        }

        private string Process(List<Element> elems)
        {
            string postfix = "";
            Stack<Element> stack = new Stack<Element>();
            stack.Push(brake);

            int curr = 0;

            while (elems[curr] != brake || !IsStackFin(stack))
            {
                /*
                if (isTerm(elems[curr]))
                    vars.Push(elems[curr].Name);*/

                string firstSElem = FirstNSElem(stack).Name;
                string currElem = elems[curr].Name;

                Relat relation;

                relation = table.GetRelat(firstSElem, currElem);

                if (relation == Relat.Lesser || relation == Relat.Equal)
                {
                    stack.Push(table.elements.Find(s => s.Name == elems[curr].Name));
                    curr++;
                }
                else if (relation == Relat.More)
                {
                    bool flag = true;
                    for (int i = 1; i < stack.Count && flag; i++)
                    {
                        List<Element> stackCut = stack.Take(i).ToList();
                        if (stackCut.Count == 1)
                        {
                            if (isTerm(stackCut[0]))
                            {
                                var t = stack.Pop();
                                stack.Push(new Element(start.Name, t.Name));

                                flag = false;
                            }
                        }
                        else if(stackCut.Count == 2)
                        {
                            if(isUnar(stackCut[1]) && stackCut[0].Name == start.Name)
                            {
                                string postfixAdd = stackCut[1].Name;
                                if(stackCut[0].varVal != "")
                                    postfixAdd = stackCut[0].varVal + postfixAdd;

                                Console.Write(postfixAdd);
                                postfix += postfixAdd;

                                stack.Pop(); stack.Pop();
                                stack.Push(start);

                                flag = false;
                            }
                        }
                        else if (stackCut.Count == 3)
                        {
                            if (stackCut[0].Name == ")" && stackCut[1].Name == start.Name && stackCut[2].Name == "(")
                            {
                                stack.Pop(); stack.Pop(); stack.Pop();
                                stack.Push(new Element(stackCut[1].Name, stackCut[1].varVal));

                                flag = false;
                            }
                            else if(stackCut[0].Name == "]" && stackCut[1].Name == start.Name && stackCut[2].Name == "[")
                            {
                                stack.Pop(); stack.Pop(); stack.Pop();
                                stack.Push(new Element(stackCut[1].Name, stackCut[1].varVal));

                                flag = false;
                            }
                            else if (stackCut[0].Name == start.Name && isOperation(stackCut[1]) && stackCut[2].Name == start.Name)
                            {
                                Oper curOp = (Oper)stackCut[1];
                                string postfixAdd = curOp.Name;
                                var fir = stack.Pop();
                                var sec = stack.Pop();
                                var thi = stack.Pop();
                                if (stackCut[0].varVal != "")
                                    postfixAdd = stackCut[0].varVal + postfixAdd;
                                if (stackCut[2].varVal != "")
                                    postfixAdd = stackCut[2].varVal + postfixAdd;

                                Console.Write(postfixAdd);
                                postfix += postfixAdd;

                                stack.Push(start);

                                flag = false;
                            }
                        }
                    }

                    if (flag)
                    {
                        Console.WriteLine($"Wrong struct at {curr + 1} pos!");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"Error at {curr + 1} pos!");
                    return null;
                }
            }
            Console.WriteLine();
            return postfix;
        }

        private bool IsStackFin(Stack<Element> stack)
        {
            if (stack.Count == 2)
                if (stack.ElementAt(0).Name == start.Name && stack.ElementAt(1) == brake)
                    return true;

            return false;
        }


        private bool isOperation(Element e)
        {
            return operations.Contains(e);
        }

        private bool isUnar(Element e)
        {
            if(isOperation(e))
                if(((Oper)e).Unar)
                    return true;
            return false;
        }

        private bool isBracket(Element e)
        {
            return brackets.Contains(e);
        }

        private bool isLeftBracket(Element e)
        {
            return e.Name == "(" || e.Name == "[";
        }

        private bool isRightBracket(Element e)
        {
            return e.Name == ")" || e.Name == "]";
        }

        private bool isTerm(Element e)
        {
            return terms.Contains(e);
        }

        private Element FirstNSElem (Stack<Element> stack)
        {
            for (int i = 0; i < stack.Count; i++)
                if (stack.ElementAt(i).Name != start.Name)
                    return stack.ElementAt(i);

            return null;
        }
    }
}
