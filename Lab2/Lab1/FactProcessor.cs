using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1
{
    public static class FactProcessor
    {
        public static Gramm Fact(Gramm gr)
        {
            var symbRules = GrammProcessor.GetAllSymbRules(gr);
            var newSymbRules = new Dictionary<string, List<List<string>>>(symbRules);
            foreach (var rule in symbRules)
            {
                var left = rule.Key;
                bool flag = true;

                while (flag)
                {
                    var rights = newSymbRules[left];
                    var fullRights = GetFullRights(rights);
                    var msub = GetMSub(fullRights);
                    if (msub.Length == 0)
                    {
                        flag = false;
                        break;
                    }
                    var subRights = GetSubRights(fullRights, msub);
                    var nonSubRights = fullRights.Except(subRights).ToList();
                    var cutRights = GetCutRights(subRights, msub);

                    List<List<string>> newRights = new List<List<string>>();
                    var newTerm = FindNewTerm(left, gr);
                    newRights.Add(GetRight(msub + newTerm));
                    newRights.AddRange(GetRights(nonSubRights));
                    newSymbRules[left] = newRights;
                    newSymbRules.Add(newTerm, GetRights(cutRights));
                    gr.NonTerms.Add(newTerm);
                }

            }

            gr.Rules = new List<Rule>();
            foreach (var rule in newSymbRules)
            {
                foreach (var right in rule.Value)
                {
                    gr.Rules.Add(new Rule(rule.Key, right));
                }
            }

            return gr;
        }

        private static string GetMSub(List<string> fullRights)
        {
            var orderedRights = fullRights.OrderByDescending(x => x.Length).ToList();
            var mostSubstr = "";
            for (int i = 0; i < orderedRights.Count; i++)
            {
                var currRight = orderedRights[i];
                int len = currRight.Length;
                if (len <= mostSubstr.Length)
                {
                    break;
                }
                var substr = currRight;
                string tmostSub = "";

                //Поиск самого крупного неодинарного вхождения подстроки
                while (len > 0)
                {
                    int count = orderedRights.Where(x => x.StartsWith(substr)).Count();
                    if (count > 1)
                    {
                        tmostSub = substr;
                        break;
                    }
                    else
                    {
                        substr = substr.Substring(0, substr.Length - 1);
                        len = substr.Length;
                    }
                }

                if (len != 0 && mostSubstr.Length < tmostSub.Length)
                {
                    mostSubstr = tmostSub;
                }

            }
            return mostSubstr;
        }


        private static List<string> GetSubRights(List<string> fullRights, string sub)
        {
            return fullRights.Where(x => x.StartsWith(sub)).ToList();
        }

        private static List<string> GetCutRights(List<string> subRights, string sub)
        {
            int len = sub.Length;
            return subRights.Select(x => x.Substring(len, x.Length - len).Length != 0 ? x.Substring(len, x.Length - len) : "e").ToList();
        }

        private static string FindNewTerm(string left, Gramm gr)
        {
            bool cont = false;
            string newTerm = left;
            do
            {
                newTerm = newTerm + "'";
                cont = gr.NonTerms.Contains(newTerm);
            } while (cont);

            return newTerm;
        }

        private static List<string> GetFullRights(List<List<string>> rights)
        {
            List<string> fullRights = new List<string>();
            foreach (var right in rights)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var s in right)
                {
                    sb.Append(s);
                }
                fullRights.Add(sb.ToString());
            }
            return fullRights;
        }

        private static List<List<string>> GetRights(List<string> fullRights)
        {
            return fullRights.Select(x => GetRight(x)).ToList();
        }

        private static List<string> GetRight(string fullRight)
        {
            int curr = -1;
            List<string> result = new List<string>();
            for (int i = 0; i < fullRight.Length; i++)
            {
                if (fullRight[i] == '\'')
                {
                    result[curr] = result[curr] + '\'';
                }
                else
                {
                    result.Add(fullRight[i].ToString());
                    curr++;
                }
            }
            return result;
        }
    }
}
