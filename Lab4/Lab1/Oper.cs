using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    public class Oper : Element
    {
        public bool LAssoc;
        public bool Unar;
        public int Prior;

        public Oper(string name, bool lAssoc, bool unar, int prior) : base(name)
        {
            LAssoc = lAssoc;
            Prior = prior;
            Unar = unar;
        }
    }
}
