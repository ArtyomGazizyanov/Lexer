using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Compiler.LexicalAnalyzer
{
    public static class Matcher
	{
	    public static List<char> OperatorsList { get; } = new List<char> { '&', '<', '>', '=', '!', '=', '+', '-', '*', '/', '{', '}', '(', ')', '[', ']', ';', '.'};
	    public static List<char> SeporatorsList { get; } = OperatorsList;
	    public static List<char> ValidIdentifierSymbols { get; } = new List<char> { '_', '-' };
		public static List<char> ExponentialAttribute { get; } = new List<char> { 'E', 'e' };

		public static bool IsSeporator( char symbol )
		{
			return LexerRules.Seporators.Contains( symbol );
		}
		public static bool IsSeporator(string str)
		{
			if ( !String.IsNullOrEmpty( str ) && str.Length == 1 )
			{
				return LexerRules.Seporators.Contains(str.First());
			}
			return false;
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
			part = part.ToLower();
			if ( !CheckLength( part ) )
			{
				return false;
			}
			return LexerRules.Keywords.Contains( part );
		}

		public static bool IsOperator(string part)
		{
			if (!CheckLength(part))
			{
				return false;
			}
			return LexerRules.Operators.Contains( part.First() );
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
			int floatDelimiterCounter = 0;
			foreach (char symbol in part)
			{
				if (symbol == LexerRules.NumberDelimiter)
				{
					++floatDelimiterCounter;
				}
				if (!Char.IsNumber(symbol) && !IsSeporator(part.First()) && floatDelimiterCounter > 1 
					|| Char.IsLetter(symbol) )					
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

		public static bool IsReservedSymbol( char symbol )
		{
			return LexerRules.ReservedSymbols.Contains( symbol ) || Char.IsSeparator( symbol );
		}

		private static bool IsExponentialNumber(int index, string attribute)
		{
			if(attribute.Length < 2)
			{
				return false;
			}

			if(LexerRules.ExponentialAttribute.Contains(attribute[index]) && LexerRules.Operators.Contains(attribute[index + 1]))
			{
				return IsNumber(attribute.Substring(1, attribute.Length));
			}

			return false;
		}

		public static bool IsValidIdentifierNameLength(int identifierLength)
		{
			return identifierLength < LexerRules.MaxIdentifirenNameLength;
		}

		public static bool IsEqualityOperator( string str )
		{
			if ( !String.IsNullOrEmpty( str ) )
			{
				return str.First() == LexerRules.EqualityOperator;
			}
			return false;
		}

		public static bool IsLastSymbolInString( char symbol, string subString)
		{
			return subString.IndexOf( symbol ) != subString.Length - 1;
		}

		public static bool IsComment(string part)
		{
		    if (part.Length > 1 && part.First() != '/' && part.First() + 1 != '/')
			{
				return false;
			}

			return true;
		}

		public static bool CheckCommentBegining(string symbol)
		{
			if ( symbol.Length > 1 )
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
			return Regex.IsMatch(tokenString, "^[1-9]([0-9]*)?x([0-1][0-9]|[1-9])");
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
		public static Tuple<int, string> CutStringLiteralLengthNew(string text)
		{
			int jumpTo = 0;
			if ( String.IsNullOrEmpty( text ) )
			{
				if ( text.First() == LexerRules.QuotionMark )
				{
					bool EndQuotionMarkFound = false;
					bool SuspicionOfQuotation = false;
					while ( jumpTo < text.Length - 1 && !EndQuotionMarkFound)
					{
						jumpTo++;
						if (text[jumpTo] == LexerRules.ReverseSlash)
						{
							SuspicionOfQuotation = true;
						}

						if ( text[ jumpTo ] == LexerRules.QuotionMark )
						{
							EndQuotionMarkFound = true;
						}
					}
				}
			}
			string match = Regex.Match(text, "@^\"(?:[^\"]|\"\")*\"|\"(?:\\.|[^\\\"])*\"").ToString();

			return new Tuple<int, string>(match.Length, match);
		}
		public static bool IsNumberDelimiter(char symbol)
		{
			return symbol == LexerRules.NumberDelimiter;
		}

		public static bool IsOpenRoundBracket(string str)
		{
			if ( String.IsNullOrEmpty( str ) )
			{
				return false;
			}
			return LexerRules.OpenRoundBracket == str.First();
		}
		public static bool IsCloseRoundBracket(string str)
		{
			if (String.IsNullOrEmpty(str))
			{
				return false;
			}
			return LexerRules.CloseRoundBracket == str.First();
		}
		public static bool IsOpenCurlyBracket(string str)
		{
			if (String.IsNullOrEmpty(str))
			{
				return false;
			}
			return LexerRules.OpenCurlyBracket == str.First();
		}
		public static bool IsCloseCurlyBracket(string str)
		{
			if (String.IsNullOrEmpty(str))
			{
				return false;
			}
			return LexerRules.CloseCurlyBracket == str.First(); ;
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
			return !String.IsNullOrEmpty( part );
		}
	}
}
