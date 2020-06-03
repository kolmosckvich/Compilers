using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1
{
    [JsonObject]
    public class Rule
    {
        [JsonProperty]
        public string Left;

        [JsonProperty]
        public List<string> Rights = new List<string>();

        //Для отладки
        [JsonIgnore]
        public string Right 
        { 
            get 
            {
                StringBuilder sb = new StringBuilder();
                foreach(var s in Rights)
                {
                    sb.Append(s);
                }
                return sb.ToString();
            } 
        }

        public Rule(string left, IEnumerable<string> rights)
        {
            Left = left;
            Rights = rights.ToList();
        }

        public Rule(string left, string right)
        {
            Left = left;
            Rights.Add(right);
        }

        public Rule()
        {

        }
    }
}
