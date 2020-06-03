using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    public class ObrErrorListener : BaseErrorListener
    {
        public List<string> Errors { get; }

        public ObrErrorListener()
        {
            Errors = new List<string>();
        }

        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            Errors.Add(msg + " in  line: " + line + ", position: " + charPositionInLine);
        }

    }
}
