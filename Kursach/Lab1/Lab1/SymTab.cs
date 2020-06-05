using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{

    public class SymTab
    {
        public Dictionary<string, Symbol> Symbols = new Dictionary<string, Symbol>();

        public SymTab()
        {
            AddDefaultSymbols();
        }

        public void AddSymbol(string sym, string val, SymType type, int reqParams = 0, List<SymType> paramTypes = null)
        {
            Symbol smb = new Symbol(val, type, reqParams);
            if(Symbols.ContainsKey(sym))
            {
                Symbols[sym] = smb;
            }
            else
            {
                Symbols.Add(sym, smb);
            }
        }

        private void AddDefaultSymbols()
        {
            AddSymbol("+", "add_fun", SymType.Fun);
            AddSymbol("-", "red_fun", SymType.Fun);
            AddSymbol("/", "div_fun", SymType.Fun);
            AddSymbol("*", "mul_fun", SymType.Fun);

            AddSymbol("def", "def_sym", SymType.Fun);
            AddSymbol("if", "cond", SymType.Fun);
            AddSymbol("fn", "def_fun", SymType.Fun);
            AddSymbol("first", "get_first", SymType.Fun);
            AddSymbol("str", "to_str", SymType.Fun);
        }
    }
}
