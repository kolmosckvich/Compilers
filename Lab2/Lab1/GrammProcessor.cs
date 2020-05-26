using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Lab1
{
    public static class GrammProcessor
    {
        public static Gramm RemoveLR(Gramm gr)
        {
            var symbRules = GetAllSymbRules(gr);
            var lefts = symbRules.Keys.ToList();

            for(int i = 0; i < lefts.Count; i++)
            {
                string left = lefts[i];
                var rights = new List<List<string>>(symbRules[left]);
                
                //Подстановка старых, "чистых правил" в новые
                for(int j = 0; j < i; j++)
                {
                    string prevLeft = lefts[j];
                    for (int k = 0; k < rights.Count; k++)
                    {
                        var right = rights[k];

                        if(right[0] == prevLeft)
                        {
                            var prevRights = new List<List<string>>(symbRules[prevLeft]);
                            rights.RemoveAt(k);
                            right.RemoveAt(0);

                            foreach(var prevRight in prevRights)
                            {
                                var newRight = new List<string>(prevRight);
                                newRight.AddRange(right);
                                rights.Add(newRight);
                            }
                        }
                    }
                }

                symbRules.Remove(left);

                //Устранение непосредственной рекурсии
                if (rights.Any(x => x.First() == left))
                {
                    string newTerm = left + "'";
                    gr.NonTerms.Add(newTerm);
                    var sRights = new List<List<string>>();
                    var prRights = new List<List<string>>();

                    foreach(var right in rights)
                    {
                        if(right.First() == left)
                        {
                            right.RemoveAt(0);

                            var newRight = new List<string>();
                            newRight.AddRange(right);
                            newRight.Add(newTerm);
                            sRights.Add(newRight);
                        }
                        else
                        {
                            var newRight = new List<string>();
                            newRight.AddRange(right);
                            newRight.Add(newTerm);
                            prRights.Add(newRight);
                        }
                    }

                    symbRules.Add(left, prRights);
                    symbRules.Add(newTerm, sRights);
                }
                else
                {
                    symbRules.Add(left, rights);
                }

            }

            gr.Rules = new List<Rule>();
            foreach(var rule in symbRules)
            {
                foreach(var right in rule.Value)
                {
                    gr.Rules.Add(new Rule(rule.Key, right));
                }
            }

            return gr;
        }

        public static Dictionary<string, List<List<string>>> GetAllSymbRules(Gramm gr)
        {
            var lefts = new HashSet<string>();
            var symbRules = new Dictionary<string, List<List<string>>>();
            foreach (var rule in gr.Rules)
            {
                if(!lefts.Contains(rule.Left))
                {
                    lefts.Add(rule.Left);
                    symbRules.Add(rule.Left, GetAllRightParts(rule.Left, gr));
                }
            }
            return symbRules;
        }

        private static List<List<string>> GetAllRightParts(string left, Gramm gr)
        {
            var rightParts = new List<List<string>>();
            foreach(var rule in gr.Rules)
            {
                if (rule.Left == left)
                {
                    rightParts.Add(rule.Rights);
                }
            }
            return rightParts;
        }
    }
}
