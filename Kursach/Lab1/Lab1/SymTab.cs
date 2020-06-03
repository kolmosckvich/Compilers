using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    [Flags]
    public enum SymType
    {
        Nil = 0,
        Int = 1,
        Float = 2,
        Num = 3,
        Bool = 4,
        Fun = 8,
        Str = 16,
        Char = 32,
        Sym = 64,
        Vec = 128,
        Key = 256,
        Any = 511
    }

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
            AddSymbol("+", "add_fun", SymType.Fun, 2, new List<SymType> { SymType.Num, SymType.Num });
            AddSymbol("-", "red_fun", SymType.Fun, 2, new List<SymType> { SymType.Num, SymType.Num });
            AddSymbol("/", "div_fun", SymType.Fun, 2, new List<SymType> { SymType.Num, SymType.Num });
            AddSymbol("*", "mul_fun", SymType.Fun, 2, new List<SymType> { SymType.Num, SymType.Num });

            AddSymbol("def", "def_sym", SymType.Fun, 2, new List<SymType> { SymType.Sym, SymType.Any });
            AddSymbol("if", "cond", SymType.Fun, 3, new List<SymType> { SymType.Any, SymType.Any, SymType.Any });
            AddSymbol("fn", "def_fun", SymType.Fun, 3, new List<SymType> { SymType.Sym, SymType.Vec, SymType.Any });
            AddSymbol("first", "get_first", SymType.Fun);
            AddSymbol("str", "to_str", SymType.Fun);
        }
    }
}
