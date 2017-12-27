using System;
using System.IO;
using System.Linq;

namespace Compiler.LexicalAnalyzer
{
	public enum TokenType
	{
		Identifier,
		Keyword,
		Number,
		FloatNumber,
		ExponentionalNumber,
		Operator,
		Separator,
		OpenRoundBracket,
		CloseRoundBracket,
		OpenCurlyBracket,
		CloseCurlyBracket,
		OpenSquareBrackets,
		CloseSquareBrackets,
		EqualityOperator,
		NumberWithNumberSystem,
		StringLiteral,
		CharLiteral,
		Eof,
		Eoln,
		Undefined,
	}

	public class Lexer
	{
		public StreamReader Reader { get; }

		public Lexer(StreamReader reader)
		{
			Reader = reader;
			UpdateBufferText();
			_peek = null;
		}

		public Token Read()
		{

			if ( _peek != null )
			{
				return GetAndResetPeak();
			}

			if ( !IsEof || !IsEoln)
			{
				SkipWhitespaces();
				SkipComments(_bufferText.Substring(_caretPos));
			}

			if (IsEof)
			{
				return new Token(TokenType.Eof, _caretPos + 1, _caeretRow);
			}
			if (IsEoln)
			{
				return new Token(TokenType.Eoln, _caretPos + 1, _caeretRow);
			}

			return GetToken(GetSubstringFromText);
		}

		private Token GetAndResetPeak()
		{
			Token result = _peek;
			_peek = null;

			return result;
		}

		private bool IsStringMoreOneSymbol(string str)
		{
			if ( String.IsNullOrEmpty( str ) )
			{
				return false;
			}
			return str.Length > 1;
		}

		private Token GetSymbolToken( string subString )
		{
			if (Matcher.IsOperator(subString))
			{
				return GetToken(TokenType.Operator, subString);
			}
			if (Matcher.IsSeporator(subString))
			{
				return GetToken(TokenType.Separator, subString);
			}
			if (Matcher.IsCloseCurlyBracket(subString))
			{
				return GetToken(TokenType.CloseCurlyBracket, subString);
			}
			if (Matcher.IsOpenCurlyBracket(subString))
			{
				return GetToken(TokenType.OpenCurlyBracket, subString);
			}
			if (Matcher.IsOpenRoundBracket(subString))
			{
				return GetToken(TokenType.OpenRoundBracket, subString);
			}
			if (Matcher.IsCloseRoundBracket(subString))
			{
				return GetToken(TokenType.CloseRoundBracket, subString);
			}
			if (Matcher.IsOpenSquareBrackets(subString))
			{
				return GetToken(TokenType.OpenSquareBrackets, subString);
			}
			if (Matcher.IsCloseSquareBrackets(subString))
			{
				return GetToken(TokenType.CloseSquareBrackets, subString);
			}
			if (Matcher.IsEqualityOperator(subString))
			{
				return GetToken(TokenType.EqualityOperator, subString);
			}

			return GetToken(TokenType.Undefined, subString);
		}

		private bool HasWordAttribute( string str )
		{
			if ( string.IsNullOrEmpty( str ) )
			{
				return false;
			}
			return Char.IsLetter( str.FirstOrDefault() );
		}
		private bool HasNumberAttribute( string str )
		{
			if ( string.IsNullOrEmpty( str ) )
			{
				return false;
			}
			return Char.IsNumber( str.FirstOrDefault() ) || str.FirstOrDefault() == '+' || str.FirstOrDefault() == '-';
		}
		private Token GetWordToken(string subString)
		{
			if (Matcher.IsKeyword(subString))
			{
				return GetToken(TokenType.Keyword, subString);
			}
			if (Matcher.IsIdentifier(subString))
			{
				return GetToken(TokenType.Identifier, subString);
			}

			return GetToken(TokenType.Undefined, subString);
		}

		private Token GetNumberToken(string subString)
		{
			if (Matcher.IsFloatNumber(subString))
			{
				return GetToken(TokenType.FloatNumber, subString);
			}
			if (Matcher.IsNumber(subString))
			{
				return GetToken(TokenType.Number, subString);
			}
			if (Matcher.IsExponentialNumber(subString))
			{
				return GetToken(TokenType.ExponentionalNumber, subString);
			}
			if (Matcher.HasNumberSystem(subString))
			{
				return GetToken(TokenType.NumberWithNumberSystem, subString);
			}

			return GetToken(TokenType.Undefined, subString);
		}

		private Token GetLiteral(string subString)
		{
			if (Matcher.IsStringLiteral(subString))
			{
				return GetToken(TokenType.StringLiteral, subString);
			}
			if (Matcher.IsCharLiteral(subString))
			{
				return GetToken(TokenType.CharLiteral, subString);
			}
			return GetToken(TokenType.Undefined, subString);
		}

		private bool HasQuotionMark(string str)
		{
			return str.First() == LexerRules.QuotionMark || str.First() == LexerRules.CharMark;
		}
		private Token GetToken(string subString)
		{
			if ( String.IsNullOrEmpty(subString) )
			{
				return GetToken(TokenType.Undefined, subString);
			}
			if (HasWordAttribute(subString))
			{
				return GetWordToken(subString);
			}
			if ( !IsStringMoreOneSymbol( subString ) )
			{
				return  GetSymbolToken( subString );
			}
			if ( HasQuotionMark( subString ) )
			{
				return GetLiteral( subString );
			}

			if ( HasNumberAttribute( subString ) )
			{
				return GetNumberToken(subString);
			}

			return GetToken(TokenType.Undefined, subString);
		}

		public Token ReadStream()
		{
			if (IsEof)
			{
				return new Token(TokenType.Eof, _caretPos, _caeretRow, String.Empty);
			}
			SkipWhitespaces();
			if ( IsEoln )
			{
				++_caeretRow;
				if ( !Reader.EndOfStream )
				{
					_bufferText = Reader.ReadLine();
					_caretPos = 0;
					_peek = null;

					Peek();
				}
			}
			
			return Read();
		}
		
		public Token Peek()
		{
			return _peek ?? ( _peek = Read() );
		}

		private int _caretPos;
		private int _caeretRow = 1;
		private string _bufferText;
		private Token _peek;
		private int _commentCounterInBuffer = 0;

        private bool IsEof => Reader.EndOfStream && (string.IsNullOrEmpty( _bufferText )|| (_bufferText!= null && IsEoln));

		private bool IsEoln => _caretPos == _bufferText.Length || _caretPos > _bufferText.Length ;

	    private string GetSubstringFromText
	    {
	        get
	        {
				string subString = _bufferText.Substring(_caretPos, _bufferText.Length - _caretPos);
				if (subString.Length == 0)
				{
					return String.Empty; //_bufferText.Substring(_caretPos, 1);
				}

		        if (IsCharMark(subString[0]))
		        {
			        var stringLiteral = CalculateCharLiteral();
			        return stringLiteral.Item2;
		        }

				if (IsStringMark(subString[0]))
				{
					var stringLiteral = CalculateStringLiteralLength();
					if ( stringLiteral.Item1 == 0 )
					{
						if ( _caretPos + 1 <= _bufferText.Length )
						{
							string unfinishedStringLiteral = _bufferText.Substring(_caretPos);
							++_caretPos;
							return unfinishedStringLiteral;
						}
					}
					return stringLiteral.Item2;
				}

				if (Matcher.IsReservedSymbol(subString[0]))
	            {
	                return _bufferText.Substring(_caretPos, 1);
                }

		        if ( Matcher.IsExponentialNumber( subString ) )
		        {
			        return Matcher.GetExponentialNumber( subString );
		        }

		        return GetSubString( subString );

	        }
	    }

		private string GetSubString(string subString)
		{
			int jumpFor = 0;
			bool suspicionOfNumber = Char.IsNumber(subString.First());
			foreach (char symbol in subString)
			{

				if ((!Matcher.IsReservedSymbol(symbol) || (suspicionOfNumber && Matcher.IsNumberDelimiter(symbol)))
				    //&& Matcher.IsLastSymbolInString(symbol, subString)
				    && Matcher.IsValidIdentifierNameLength(jumpFor))
				{
					++jumpFor;
				}
				else
				{
					return jumpFor == 0 ? _bufferText.Substring(_caretPos, 1) : _bufferText.Substring(_caretPos, jumpFor);
				}
			}
			return jumpFor == 0 ? _bufferText.Substring(_caretPos, 1) : _bufferText.Substring(_caretPos, jumpFor);
		}

		private string GetStringLiteral(char symbol)
		{
			var stringLiteral = CalculateStringLiteralLength();
			return stringLiteral.Item2;			
		}

		private Tuple<int, string> CalculateStringLiteralLength()
		{
		    return Matcher.CutStringLiteralLength(_bufferText.Substring(_caretPos));
		}
		private Tuple<int, string> CalculateCharLiteral()
		{
			return Matcher.CutCharLiteral(_bufferText);
		}

		private bool IsStringMark(char symbol)
		{
			return symbol == LexerRules.QuotionMark;
		}

		private bool IsCharMark(char symbol)
		{
			return symbol == LexerRules.CharMark;
		}

		private void SkipWhitespaces()
		{
			while (!IsEof && !IsEoln)
			{
				if ((Char.IsWhiteSpace(_bufferText[_caretPos]) 
                    || Char.IsSeparator(_bufferText[_caretPos])) 
					&& !IsStringMark(_bufferText[_caretPos]))
				{
					++_caretPos;
					continue;
				}

				break;
			}
		}

		private bool IsOneStringCommnet( string str )
		{
			if ( String.IsNullOrEmpty( str ) )
			{
				return false;
			}
			if ( str.Length < 2 )
			{
				return false;
			}

			return str[ 0 ] == '/' && str[ 1 ] == '/';
		}

		private void SkipComments(string stringToCheck)
		{
			if ( IsOneStringCommnet( stringToCheck ) )
			{
				_caretPos = stringToCheck.Length + _caretPos;
			}
			if ( Matcher.CheckCommentBegining(stringToCheck) )
			{
				int jumpTo;
				while (true)
				{
					jumpTo = _bufferText.IndexOf(LexerRules.ComentEnd, _caretPos, StringComparison.Ordinal);
					if ( IsEof || jumpTo != -1 || Reader.EndOfStream)
					{
						break;
					}
					UpdateBufferText();
					
				}
				if ( jumpTo == -1 )
				{
					_commentCounterInBuffer++;
					_caretPos = _bufferText.Length;
				}
				else
				{
					_caretPos = jumpTo + LexerRules.ComentEnd.Length;
				}
				SkipWhitespaces();
			}
		}

		private Token GetToken(TokenType type, string value)
		{
			UpdateCarretPos( value.Length );
			return new Token(type, _caretPos - value.Length, _caeretRow, value);
		}

		private void UpdateCarretPos( int jump )
		{
			_caretPos += jump;
		}

		private void UpdateBufferText()
		{
			if ( !Reader.EndOfStream )
			{
				_bufferText = Reader.ReadLine();
				_caretPos = 0;
			}
		}
	}
}
