using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static Lab1.Utils;

namespace Lab1
{
    public class Table
    {
        public List<Element> elements;

        private Relat[][] relations;

        private List<Oper> operations;
        private List<Element> terms;
        private List<Element> brackets;
        private Element brake;

        public Table(List<Oper> opers, List<Element> ter, List<Element> brack, Element br)
        {
            operations = opers;
            terms = ter;
            brackets = brack;
            brake = br;

            var merge = terms.Append(brake).Concat(brackets).Concat(opers);
            elements = merge.ToList();

            int size = elements.Count;
            relations = new Relat[size][];
            for(int i = 0; i < size; i++)
            {
                relations[i] = new Relat[size];
                var firEl = elements[i];
                for (int j = 0; j < size; j++)
                {
                    var secEl = elements[j];
                    if (isOperation(firEl))
                    {
                        var firOp = (Oper)firEl;
                        if (isOperation(secEl))
                        {
                            var secOp = (Oper)secEl;
                            if (secOp.Unar)
                                relations[i][j] = Relat.Lesser;
                            else if (firOp.Prior > secOp.Prior)
                                relations[i][j] = Relat.More;
                            else if (firOp.Prior < secOp.Prior)
                                relations[i][j] = Relat.Lesser;
                            else if (firOp.LAssoc)
                                relations[i][j] = Relat.More;
                            else if (!firOp.LAssoc)
                                relations[i][j] = Relat.Lesser;
                            else
                                relations[i][j] = Relat.None;
                        }
                        else if (isTerm(secEl) || isLeftBracket(secEl))
                            relations[i][j] = Relat.Lesser;
                        else if (isRightBracket(secEl) || secEl == brake)
                            relations[i][j] = Relat.More;
                        else
                            relations[i][j] = Relat.None;
                    }
                    else if (isTerm(firEl))
                        if (isOperation(secEl) || isRightBracket(secEl) || secEl == brake)
                            relations[i][j] = Relat.More;
                        else
                            relations[i][j] = Relat.None;
                    else if (firEl.Name == "(")
                        if (isOperation(secEl) || secEl.Name == "(" || isTerm(secEl) || secEl.Name == "[")
                            relations[i][j] = Relat.Lesser;
                        else if (secEl.Name == ")")
                            relations[i][j] = Relat.Equal;
                        else
                            relations[i][j] = Relat.None;
                    else if (firEl.Name == "[")
                        if (isOperation(secEl) || secEl.Name == "(" || isTerm(secEl) || secEl.Name == "[")
                            relations[i][j] = Relat.Lesser;
                        else if (secEl.Name == "]")
                            relations[i][j] = Relat.Equal;
                        else
                            relations[i][j] = Relat.None;
                    else if (isRightBracket(firEl))
                        if (isOperation(secEl) || secEl.Name == ")" || secEl == brake || secEl.Name == "]")
                            relations[i][j] = Relat.More;
                        else
                            relations[i][j] = Relat.None;
                    else if (firEl == brake)
                        if (isOperation(secEl) || isLeftBracket(secEl) || isTerm(secEl))
                            relations[i][j] = Relat.Lesser;
                        else
                            relations[i][j] = Relat.None;
                    else
                        relations[i][j] = Relat.None;

                }
            }
        }

        public void PrintTable()
        {
            int size = elements.Count;
            for(int i = 0; i <= (size+1)*4; i++)
                Console.Write("-");
            Console.WriteLine();
            Console.Write("|   |");
            for (int i = 0; i < size; i++)
            {
                PrintTabSymbol(elements[i].Name);
            } 
            Console.WriteLine();
            for (int i = 0; i <= (size + 1) * 4; i++)
                Console.Write("-");
            Console.WriteLine();

            for (int i = 0; i < size; i++)
            {
                Console.Write("|");
                PrintTabSymbol(elements[i].Name);
                for(int j = 0; j < size; j++)
                {
                    PrintTabSymbol(RTS(relations[i][j]));
                }
                Console.WriteLine();
                for (int j = 0; j <= (size + 1) * 4; j++)
                    Console.Write("-");
                Console.WriteLine();
            }
        }

        private void PrintTabSymbol(string sym)
        {
            Console.Write(" ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{sym}");
            Console.ResetColor();
            Console.Write(" |");
        }

        public Relat GetRelat(Element fir, Element sec)
        {
            int firInd = elements.IndexOf(fir);
            int secInd = elements.IndexOf(sec);
            return relations[firInd][secInd];
        }

        public Relat GetRelat(string fir, string sec)
        {
            int firInd = elements.FindIndex(x => x.Name == fir);
            int secInd = elements.FindIndex(x => x.Name == sec);
            return relations[firInd][secInd];
        }

        public string RTS(Relat r)
        {
            string res = " ";
            switch(r)
            {
                case Relat.More:
                    res = ">";
                    break;
                case Relat.Lesser:
                    res = "<";
                    break;
                case Relat.Equal:
                    res = "=";
                    break;
                case Relat.None:
                    res = " ";
                    break;
                default:
                    res = " ";
                    break;
            }
            return res;
        }

        private bool isOperation(Element e)
        {
            return operations.Contains(e);
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
    }
}
