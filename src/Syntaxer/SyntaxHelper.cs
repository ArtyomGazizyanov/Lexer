using Compiler.LexicalAnalyzer;
using System;

namespace Compiler.SyntaxAnalyzer
{
	public class SyntaxHelper
	{
		public SyntaxHelper(Lexer lexer)
		{
			_lexer = lexer;

			Parse();
		}

		private readonly Lexer _lexer;

		private void Parse()
		{
			while ( true )
			{
				Token token = _lexer.ReadStream();
			    if (token != null)
			    {
			        Console.WriteLine(token.ToString());
                }

				if ( token != null && token.Type == TokenType.Eof )
				{
					break;
				}
			}
		}

	}
}
