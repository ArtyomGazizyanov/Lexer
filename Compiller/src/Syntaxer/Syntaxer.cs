using Compiler.LexicalAnalyzer;
using Compiler.SyntaxAnalyzer.AST;
using System;
using System.Collections.Generic;

namespace Compiler.SyntaxAnalyzer
{
	public class Syntaxer
	{
		public Syntaxer(Lexer lexer)
		{
			_lexer = lexer;

			Parse();
		}

		public Stack<Symbol> Stack => _stack;

		private Lexer _lexer;
		private Stack<Symbol> _stack = new Stack<Symbol>();

		private void Parse()
		{
			while ( true )
			{
				Token token = _lexer.ReadStream();
				Console.WriteLine( token.ToString() );

				if ( token.Type == TokenType.Eof )
				{
					break;
				}
			}
		}

	}
}
