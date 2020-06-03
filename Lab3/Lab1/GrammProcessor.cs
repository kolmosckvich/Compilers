using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lab1
{
    public static class GrammProcessor
    {
        private static int cur;
        private static string curString;
        private static string errMsg = "";

        public static void Reset(string input)
        {
            cur = 0;
            input = input.Replace(" ", String.Empty);
            input = input.Replace("\t", String.Empty);
            input = input.Replace("\n", String.Empty);
            input = input.Replace("\r", String.Empty);
            curString = input;
        }

        public static bool CheckFinish()
        {
            return curString == "";
        }

        public static bool CheckInput(string input)
        {
            cur = 0;
            input = input.Replace(" ", String.Empty);
            input = input.Replace("\t", String.Empty);
            input = input.Replace("\n", String.Empty);
            input = input.Replace("\r", String.Empty);
            curString = input;

            bool result = true;
            if (!CheckProg(input))
            {
                result = false;
            }
            if (cur != input.Length)
            {
                result = false;
                //Console.WriteLine("Something else after statement!");
            }
            return result;
        }

        public static bool CheckProg(string input)
        {
            bool result = true;
            if (!CheckBlock(input))
            {
                result = false;
            }
            return result;
        }

        public static bool CheckBlock(string input)
        {
            bool result = true;
            if (curString.StartsWith("begin"))
            {
                cur += 5;
                curString = curString.Substring(5, curString.Length - 5);
                if (CheckOpList(input))
                {
                    if (curString.StartsWith("end"))
                    {
                        cur += 3;
                        curString = curString.Substring(3, curString.Length - 3);
                    }
                    else
                    {
                        errMsg = $"Expected end at position {cur}";
                        //Console.WriteLine(errMsg);
                        result = false;
                    }
                }
                else
                {

                }
            }
            else
            {
                errMsg = $"Expected start at position {cur}";
                //Console.WriteLine(errMsg);
                result = false;
            }
            return result;
        }

        public static bool CheckOpList(string input)
        {
            bool result = true;
            int savedCur = cur;
            string savedInput = input;

            if (CheckOp(input))
            {
                if (!CheckOpList_(input))
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }

            if (!result)
            {
                errMsg = $"Expected Op at position {savedCur}";
                //Console.WriteLine(errMsg);
            }

            return result;
        }

        public static bool CheckOpList_(string input)
        {
            bool result = true;
            if (curString.StartsWith(";"))
            {
                cur++;
                curString = curString.Substring(1, curString.Length - 1);
                if (CheckOp(input))
                {
                    if (!CheckOpList_(input))
                    {
                        result = false;
                    }
                }
                else
                {
                    errMsg = $"Expected Op at position {cur}";
                    //Console.WriteLine(errMsg);
                    result = false;
                }
            }
            else
            {
                //errMsg = $"Expected ; at position {cur}";
                //Console.WriteLine(errMsg);
            }


            return result;
        }

        public static bool CheckOp(string input)
        {
            bool result = true;
            int savedCur = cur;
            string savedInput = curString;

            if (CheckId(input))
            {
                if (curString.StartsWith("="))
                {
                    cur++;
                    curString = curString.Substring(1, curString.Length - 1);
                    if (!CheckExpr(input))
                    {
                        result = false;
                    }
                }
            }
            else
            {
                result = false;
            }


            return result;
        }

        public static bool CheckExpr(string input)
        {
            bool result = true;
            int savedCur = cur;

            if (!CheckLogExpr(input))
            {
                result = false;

                errMsg = $"Expected LE at position {savedCur}";
                //Console.WriteLine(errMsg);
            }
            return result;
        }

        public static bool CheckLogExpr(string input)
        {
            bool result = true;
            int savedCur = cur;
            string savedInput = input;

            if(CheckLogOne(input))
            {
                if (!CheckLogExpr_(input))
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }

            

            if (!result)
            {
                errMsg = $"Expected LO at position {savedCur}";
                //Console.WriteLine(errMsg);
            }

            return result;
        }

        public static bool CheckLogExpr_(string input)
        {
            bool result = true;
            if (curString.StartsWith("!"))
            {
                cur++;
                curString = curString.Substring(1, curString.Length - 1);
                if (CheckLogOne(input))
                {
                    if(!CheckLogExpr_(input))
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }

            if (!result)
            {
                errMsg = $"Expected ! LO at position {cur}";
                //Console.WriteLine(errMsg);
            }

            return result;
        }

        public static bool CheckLogOne(string input)
        {
            bool result = true;

            if (CheckLogSec(input))
            {
                if (!CheckLogOne_(input))
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }

            if (!result)
            {
                errMsg = $"Expected LS at position {cur}";
                //Console.WriteLine(errMsg);
            }

            return result;
        }

        public static bool CheckLogOne_(string input)
        {
            bool result = true;
            if (curString.StartsWith("&"))
            {
                cur++;
                curString = curString.Substring(1, curString.Length - 1);
                if (CheckLogSec(input))
                {
                    if (!CheckLogOne_(input))
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }

            if (!result)
            {
                errMsg = $"Expected ! LS at position {cur}";
                //Console.WriteLine(errMsg);
            }

            return result;
        }

        public static bool CheckLogSec(string input)
        {
            bool result = true;
            int savedCur = cur;
            string savedInput = curString;
            bool first = true;
            bool second = true;

            if (!CheckLogFir(input))
            {
                first = false;
                cur = savedCur;
                curString = savedInput;
            }

            if (curString.StartsWith("~"))
            {
                cur++;
                curString = curString.Substring(1, curString.Length - 1);
                if (!CheckLogFir(input))
                {
                    second = false;
                }
            }
            else
            {
                second = false;
            }

            result = first || second;

            if (!result)
            {
                errMsg = $"Expected LF or ~LF at position {savedCur}";
                //Console.WriteLine(errMsg);
            }

            return result;
        }

        public static bool CheckLogFir(string input)
        {
            bool result = true;
            int savedCur = cur;
            string savedInput = curString;
            bool first = true;
            bool second = true;

            if (!CheckLogVal(input))
            {
                first = false;
                cur = savedCur;
                curString = savedInput;
            }

            if (!CheckId(input))
            {
                second = false;
            }

            result = first || second;
            if (!result)
            {
                errMsg = $"Expected boolean const or numeric id at position {savedCur}";
                //Console.WriteLine(errMsg);
            }
                
            return result;
        }

        public static bool CheckLogVal(string input)
        {
            bool result = true;

            if(curString.StartsWith("true"))
            {
                cur += 4;
                curString = curString.Substring(4, curString.Length - 4);
            }
            else if(curString.StartsWith("false"))
            {
                cur += 5;
                curString = curString.Substring(5, curString.Length - 5);
            }
            else
            {
                errMsg = $"Expected boolean const at position {cur}";
                //Console.WriteLine(errMsg);
                result = false;
            }

            return result;
        }

        public static bool CheckLogTerm(string input)
        {
            bool result = true;

            return result;
        }

        public static bool CheckId(string input)
        {
            bool result = true;

            if(Regex.IsMatch(curString, "\\A[0-9]+"))
            {
                var match = Regex.Match(curString, "\\A[0-9]+");
                int skip = match.Length;
                cur += skip;
                curString = curString.Substring(skip, curString.Length - skip);
            }
            else
            {
                errMsg = $"Expected numeric id at position {cur}";
                //Console.WriteLine(errMsg);
                result = false;
            }

            return result;
        }
    }
}
