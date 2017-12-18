using System;
using System.IO;
using MiniJavaCompiller.Lexer;

namespace Compiler.LexicalAnalyzer
{
	public enum TokenType
	{
		Identifier,
		Keyword,
		Number,
		Operator,
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
			if ( Matcher.IsNumber( _substringPart ) )
			{
				return ActualToken( TokenType.Number, _substringPart );
			}
			if ( Matcher.IsOperator( _substringPart ) )
			{
				return ActualToken( TokenType.Operator, _substringPart );
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

        private bool IsEof => Reader.EndOfStream;

		private bool IsEoln => _caretPos == _bufferText.Length;
		
		private void SkipWhitespaces()
		{
			while (!IsEof && !IsEoln)
			{
				if (Char.IsWhiteSpace(_bufferText[_caretPos]) 
                    || Char.IsSeparator(_bufferText[_caretPos])
                    || Matcher.CheckComment(_bufferText[_caretPos]))
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

		private string GetSubstringFromText
		{
			get
			{
				int jumpTospaceOrEnd = 0;
				foreach ( char symbol in _bufferText.Substring(_caretPos, _bufferText.Length - _caretPos))
				{
					if ( !Char.IsSeparator( symbol ) && !Matcher.SeporatorsList.Contains(symbol))
					{
						++jumpTospaceOrEnd;
					}
					else
					{
						return jumpTospaceOrEnd == 0 ? _bufferText.Substring(_caretPos, 1) : _bufferText.Substring(_caretPos, jumpTospaceOrEnd);
					}
					
				}

				return jumpTospaceOrEnd == 0 ? _bufferText.Substring( _caretPos, 1 ) : _bufferText.Substring( _caretPos, jumpTospaceOrEnd );
			}
		}
	}
}
