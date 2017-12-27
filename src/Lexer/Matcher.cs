using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Compiller.Lexer
{
    public static class Matcher
    {
        public static bool IsSeporator(char symbol)
        {
            return LexerRules.Separators.Contains(symbol);
        }

        public static bool IsSeparator(string str)
        {
            if (!String.IsNullOrEmpty(str) && str.Length == 1)
            {
                return LexerRules.Separators.Contains(str.First());
            }

            return false;
        }

        public static bool IsValidIdentifierSymbol(char symbol)
        {
            return LexerRules.ValidIdentifierSymbols.Contains(symbol);
        }

        public static bool IsIdentifier(string part)
        {
            if (!CheckLength(part))
            {
                return false;
            }

            if (!Char.IsLetter(part.First()) && IsSeporator(part.First()) || Char.IsNumber(part.First()))
            {
                return false;
            }

            foreach (char symbol in part)
            {
                if (!Char.IsLetter(symbol) && !Char.IsNumber(symbol) && !IsValidIdentifierSymbol(symbol))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsKeyword(string part)
        {
            part = part.ToLower();
            if (!CheckLength(part))
            {
                return false;
            }

            return LexerRules.Keywords.Contains(part);
        }

        public static bool IsOperator(string part)
        {
            if (!CheckLength(part))
            {
                return false;
            }

            return LexerRules.Operators.Contains(part.First());
        }

        public static bool IsNumber(string part)
        {
            if (!CheckLength(part))
            {
                return false;
            }

            if (!Char.IsNumber(part.First()) && IsSeporator(part.First()) || (part.Length > 1 && part.First() == '0'))
            {
                return false;
            }
            int floatDelimiterCounter = 0;
            foreach (char symbol in part)
            {
                if (symbol == LexerRules.NumberDelimiter)
                {
                    ++floatDelimiterCounter;
                }
                if (!Char.IsNumber(symbol) && !IsSeporator(part.First()) && floatDelimiterCounter > 1
                    || Char.IsLetter(symbol))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsFloatNumber(string part)
        {
            if (!CheckLength(part))
            {
                return false;
            }

            if (!Char.IsNumber(part.First()) && IsSeporator(part.First()))
            {
                return false;
            }
            int floatDelimiterCounter = 0;
            foreach (char symbol in part)
            {
                if (symbol == LexerRules.NumberDelimiter)
                {
                    ++floatDelimiterCounter;
                }
                if (!Char.IsNumber(symbol) && !IsSeporator(part.First()) && floatDelimiterCounter > 1
                    || Char.IsLetter(symbol))
                {
                    return false;
                }
            }

            return floatDelimiterCounter == 1;
        }

        public static bool IsReservedSymbol(char symbol)
        {
            return LexerRules.ReservedSymbols.Contains(symbol) || Char.IsSeparator(symbol);
        }

        public static bool IsValidIdentifierNameLength(int identifierLength)
        {
            return identifierLength < LexerRules.MaxIdentifirenNameLength;
        }

        public static bool IsEqualityOperator(string str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                return str.First() == LexerRules.EqualityOperator;
            }

            return false;
        }

        public static bool CheckCommentBegining(string symbol)
        {
            if (symbol.Length > 1)
            {
                return symbol.Substring(0, 2) == "/*";
            }

            return false;
        }

        public static bool IsStringLiteral(string tokenString)
        {
            return Regex.IsMatch(tokenString, "\\\"(\\\\.|[^\\\"])*\\\"");
        }

        public static bool IsCharLiteral(string tokenString)
        {
            return Regex.IsMatch(tokenString, "\'[\\\\]?.\'");
        }

        public static bool HasNumberSystem(string tokenString)
        {
            return Regex.IsMatch(tokenString, "^(0|[1-9]([0-9]*)?)x([0-1][0-9]|[1-9])");
        }

        public static bool IsExponentialNumber(string tokenString)
        {
            return Regex.IsMatch(tokenString, "^[0-9]*[.[0-9]*]?(e\\+|e\\-)]?[0-9]{0,3}");
        }

        public static string GetExponentialNumber(string tokenString)
        {
            return Regex.Match(tokenString, "^[0-9]*[.[0-9]*]?(e\\+|e\\-)]?[0-9]{0,3}").ToString();
        }

        public static Tuple<int, string> CutStringLiteralLength(string text)
        {
            string match = Regex.Match(text, "\\\"(\\\\.|[^\\\"])*\\\"").ToString();

            return new Tuple<int, string>(match.Length, match);
        }

        public static Tuple<int, string> CutCharLiteral(string text)
        {
            string match = Regex.Match(text, "\'[\\\\]?.\'").ToString();

            return new Tuple<int, string>(match.Length, match);
        }

        public static bool IsNumberDelimiter(char symbol)
        {
            return symbol == LexerRules.NumberDelimiter;
        }

        public static bool IsOpenRoundBracket(string str)
        {
            return String.IsNullOrEmpty(str) && LexerRules.OpenRoundBracket == str.First();
        }

        public static bool IsCloseRoundBracket(string str)
        {
            return String.IsNullOrEmpty(str) && LexerRules.CloseRoundBracket == str.First();
        }

        public static bool IsOpenCurlyBracket(string str)
        {
            return String.IsNullOrEmpty(str) && LexerRules.OpenCurlyBracket == str.First();
        }

        public static bool IsCloseCurlyBracket(string str)
        {
            return String.IsNullOrEmpty(str) && LexerRules.CloseCurlyBracket == str.First();
        }

        public static bool IsOpenSquareBrackets(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return false;
            }

            return LexerRules.OpenSquareBrackets == str.First();
        }

        public static bool IsCloseSquareBrackets(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return false;
            }

            return LexerRules.CloseSquareBrackets == str.First();
        }

        private static bool CheckLength(string part)
        {
            return !String.IsNullOrEmpty(part);
        }
    }
}
