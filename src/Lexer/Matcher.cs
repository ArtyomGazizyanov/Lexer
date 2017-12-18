using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniJavaCompiller.Lexer
{
    class Matcher
	{
	    public static List<char> OperatorsList { get; } = new List<char> { '&', '<', '>', '=', '!', '=', '+', '-', '*', '/', '{', '}', '(', ')', '[', ']', ';', '.' };
	    public static List<char> SeporatorsList { get; } = OperatorsList;
	    public static List<char> ValidIdentifierSymbols { get; } = new List<char> { '_', '-' };

	    public static bool IsSeporator( char symbol )
		{
			return SeporatorsList.Contains( symbol );
		}
		public static bool IsValidIdentifierSymbol(char symbol)
		{
			return ValidIdentifierSymbols.Contains(symbol);
		}

		public static bool IsIdentifier( string part )
		{
		    if ( !CheckLength( part ) )
			{
				return false;
			}

			if ( !Char.IsLetter( part.First() ) && IsSeporator( part.First() ) || Char.IsNumber(part.First()))
			{
				return false;
			}

			foreach ( char symbol in part)
			{
				if ( !Char.IsLetter( symbol ) && !Char.IsNumber( symbol ) && !IsValidIdentifierSymbol(symbol))
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsKeyword( string part )
		{
			if ( !CheckLength( part ) )
			{
				return false;
			}
			return part == "static" || part == "main" || part == "extends"
			         || part == "return" || part == "new" || part == "this"
			         || part == "public" || part == "void" || part == "class"
			         || part == "String" || part == "int" || part == "boolean"
			         || part == "if" || part == "else" || part == "while"
			         || part == "true" || part == "false" || part == "println";
		}

		public static bool IsOperator(string part)
		{
			if (!CheckLength(part))
			{
				return false;
			}
			return OperatorsList.Contains( part.First() );
		}

		public static bool IsNumber( string part )
		{
		    if (!CheckLength(part))
			{
				return false;
			}

			if (!Char.IsNumber(part.First()) && IsSeporator(part.First()) || part.First() == '0')
			{
				return false;
			}
			
			foreach (char symbol in part)
			{
				if (!Char.IsNumber(symbol) && !IsSeporator(part.First()))
				{
					return false;
				}
			}
			
			return true;
		}
		
		public static bool IsComment(string part)
		{
		    if (part.Length > 1 && part.First() != '/' && part.First() + 1 != '/')
			{
				return false;
			}

			return true;
		}

		public static bool CheckComment(char symbol)
		{
			return symbol == '/';
		}

		private static bool CheckLength(string part)
		{
			return part.Length > 0;
		}
	}
}
