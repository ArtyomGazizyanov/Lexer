using System.Collections.Generic;

namespace Compiler.LexicalAnalyzer
{
    public static class LexerRules
    {
        public static readonly int MaxIdentifirenNameLength = 1000;
        public static readonly HashSet<char> Operators = new HashSet<char> {'&', '<', '>', '!', '+', '-', '*', '/', '^'};
        public static readonly char EqualityOperator = '=';
        public static readonly char PlusOperator = '+';
        public static readonly char MinusOperator = '-';
        public static readonly char NumberDelimiter = '.';
        public static readonly char OpenRoundBracket = '(';
        public static readonly char CloseRoundBracket = ')';
        public static readonly char OpenCurlyBracket = '{';
        public static readonly char CloseCurlyBracket = '}';
        public static readonly char OpenSquareBrackets = '[';
        public static readonly char CloseSquareBrackets = ']';
        public static readonly char QuotionMark = '"';
        public static readonly char CharMark = '\'';
        public static readonly char ReverseSlash = '\\';
        public static HashSet<char> Seporators = new HashSet<char> {';', '.', ','};

        public static HashSet<char> ExponentialAttribute = new HashSet<char> {'E', 'e'};

        public static readonly string ComentStart = "/*";
        public static readonly string ComentEnd = "*/";

        public static HashSet<char> ReservedSymbols
        {
            get
            {
                HashSet<char> validSymbols = new HashSet<char>();
                validSymbols.UnionWith(Operators);
                validSymbols.UnionWith(Seporators);
                validSymbols.Add(EqualityOperator);
                validSymbols.Add(NumberDelimiter);
                validSymbols.Add(OpenCurlyBracket);
                validSymbols.Add(CloseCurlyBracket);
                validSymbols.Add(OpenRoundBracket);
                validSymbols.Add(CloseRoundBracket);
                validSymbols.Add(OpenSquareBrackets);
                validSymbols.Add(CloseSquareBrackets);
                return validSymbols;
            }
        }

        public static HashSet<string> Keywords = new HashSet<string>
        {
            "static",
            "main",
            "extends",
            "return",
            "new",
            "this",
            "public",
            "void",
            "class",
            "string",
            "char",
            "int",
            "boolean",
            "if",
            "do",
            "else",
            "while",
            "true",
            "false",
            "println"
        };
    }
}
