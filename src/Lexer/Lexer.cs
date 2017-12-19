using System;
using System.IO; 

namespace Compiler.LexicalAnalyzer
{
	public enum TokenType
	{
		Identifier,
		Keyword,
		Number,
		Operator,
		StringLiteral,
		Eof,
		Undefined,
	}

	public class Lexer
	{
		public StreamReader Reader { get; }

		public Lexer(string text)
		{
			_bufferText = text.ToLower();
			_peek = null;

			Peek();
		}

		public Lexer(StreamReader reader)
		{
			Reader = reader;
			_bufferText = Reader.ReadLine()?.ToLower();
			_peek = null;

			Peek();
		}

		public Token Read()
		{
			if ( _peek != null )
			{
				Token result = _peek;
				_peek = null;

				return result;
			}

			if ( IsEof )
			{
				return new Token( TokenType.Eof, _caretPos + 1, _caeretRow );
			}
			if ( IsEoln )
			{
			    return null;
			}

			SkipWhitespaces();

			_substringPart = GetSubstringFromText;

			if ( Matcher.IsIdentifier( _substringPart ) )
			{
				if ( Matcher.IsKeyword( _substringPart ) )
				{
					return ActualToken( TokenType.Keyword, _substringPart );
				}

				return ActualToken( TokenType.Identifier, _substringPart );
			}
			if(Matcher.IsStringLiteral(_substringPart))
			{
				return ActualToken(TokenType.StringLiteral, _substringPart);
		    }
		    if (Matcher.IsOperator(_substringPart))
		    {
		        return ActualToken(TokenType.Operator, _substringPart);
		    }
            if ( Matcher.IsNumber( _substringPart ) )
			{
				return ActualToken( TokenType.Number, _substringPart );
			}

			return ActualToken( TokenType.Undefined, _bufferText[ _caretPos - 1 ].ToString() );
		}

		public Token ReadStream()
		{
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
	    private string _substringPart;

        private bool IsEof => Reader.EndOfStream && _bufferText.Length > 0 && IsEoln;

		private bool IsEoln => _caretPos == _bufferText.Length;

	    private string GetSubstringFromText
	    {
	        get
	        {
	            int jumpFor = 0;
				string subString = _bufferText.Substring(_caretPos, _bufferText.Length - _caretPos);
				if (subString.Length == 0)
				{
					return _bufferText.Substring(_caretPos, 1);
				}

				if (IsQuotationMark(subString[0]))
				{
					var comment = CalculateStringLiteralLength();

					return comment.Item2;
				}

	            if (subString[0] == ',')
	            {
	                return _bufferText.Substring(_caretPos, 1);
                }


                foreach (char symbol in subString)
				{
	                if (!Char.IsSeparator(symbol) && !Matcher.SeporatorsList.Contains(symbol))
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
	    }

		private Tuple<int, string> CalculateStringLiteralLength()
		{
		    return Matcher.CutStringLiteralLength(_bufferText);
		}

		private bool IsQuotationMark(char symbol)
		{
			return symbol == '\"';
		}

		private void SkipWhitespaces()
		{
			while (!IsEof && !IsEoln)
			{
				if ((Char.IsWhiteSpace(_bufferText[_caretPos]) 
                    || Char.IsSeparator(_bufferText[_caretPos])
                    || Matcher.CheckComment(_bufferText[_caretPos])) 
					&& !IsQuotationMark(_bufferText[_caretPos]))
				{
					++_caretPos;
					continue;
				}

				break;
			}
		}

		private Token ActualToken(TokenType type, string value)
		{
			UpdateCarretPos( value.Length );
			return new Token(type, _caretPos - value.Length, _caeretRow, value);
		}

		private void UpdateCarretPos( int jump )
		{
			_caretPos += jump;
		}
	}
}
