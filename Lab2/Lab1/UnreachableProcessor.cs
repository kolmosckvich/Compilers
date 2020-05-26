using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Lab1
{
    public static class UnreachableProcessor
    {
        public static Gramm RemoveUnreachable(Gramm gr)
        {
            var symbRules = GrammProcessor.GetAllSymbRules(gr);
            bool changed = true;
            List<string> UsedNT = new List<string>() { gr.St };
            while(changed)
            {
                int len = UsedNT.Count;
                foreach(var rule in symbRules)
                {
                    if(UsedNT.Contains(rule.Key))
                    {
                        UsedNT = UsedNT.Union(FindNTInRules(rule.Value, gr)).ToList();
                    }
                }
                if(UsedNT.Count == len)
                {
                    changed = false;
                }
            }

            List<string> unreachable = gr.NonTerms.Except(UsedNT).ToList();
            gr.NonTerms = UsedNT;
            foreach(var un in unreachable)
            {
                symbRules.Remove(un);
            }

            gr.Rules = new List<Rule>();
            foreach (var rule in symbRules)
            {
                foreach (var right in rule.Value)
                {
                    gr.Rules.Add(new Rule(rule.Key, right));
                }
            }

            return gr;
        }

        private static List<string> FindNTInRules(List<List<string>> rights, Gramm gr)
        {
            List<string> NT = new List<string>();
            foreach(var right in rights)
            {
                NT.AddRange(FindNTInRule(right, gr));
            }
            return NT;
        }

        private static List<string> FindNTInRule(List<string> right, Gramm gr)
        {
            List<string> NT = new List<string>();
            foreach(var t in right)
            {
                if(gr.NonTerms.Contains(t))
                {
                    NT.Add(t);
                }
            }
            return NT;
        }
    }
}
