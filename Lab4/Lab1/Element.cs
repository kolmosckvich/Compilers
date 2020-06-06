using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    public class Element
    {
        public string Name;
        public string varVal;

        public Element(string name, string val = "")
        {
            Name = name;
            varVal = val;
        }
    }
}
