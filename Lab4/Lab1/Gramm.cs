using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    [JsonObject]
    public class Gramm
    {
        [JsonProperty]
        public List<string> Terms = new List<string>();

        [JsonProperty]
        public List<string> NonTerms = new List<string>();

        [JsonProperty]
        public List<Rule> Rules = new List<Rule>();

        [JsonProperty]
        public string St;
    }
}
