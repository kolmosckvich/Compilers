using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Lab1
{
    public static class GramFileProcessor
    {
        public static void WriteGramm(Gramm gr, string filename)
        {
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            string output = JsonConvert.SerializeObject(gr, options);
            File.WriteAllText(filename, output);
        }

        public static Gramm ReadGramm(string filename)
        {
            string input = File.ReadAllText(filename);
            Gramm inputGramm = JsonConvert.DeserializeObject<Gramm>(input);
            return inputGramm;
        }
    }
}
