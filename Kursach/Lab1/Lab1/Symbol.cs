using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    public class Symbol
    {
        public string Value;
        public SymType Type;

        public bool InfParams;
        public int ReqParams;

        public List<SymType> ParamTypes;

        public Symbol(string val, SymType type, int reqParams = 0, List<SymType> paramTypes = null)
        {
            Value = val;
            Type = type;
            if(reqParams == 0)
            {
                InfParams = true;
            }
            else
            {
                ReqParams = reqParams;
            }
            if(paramTypes == null)
            {
                ParamTypes = new List<SymType>();
            }
            else
            {
                ParamTypes = paramTypes;
            }
        }
    }
}
