﻿using System;
using System.IO;
using Compiler.LexicalAnalyzer;
using Compiler.SyntaxAnalyzer;

namespace Compiler
{
	class Program
	{
		public static uint MIN_ARGS_COUNT = 1;

		public static int Main(string[] args)
		{
			try
			{
				if (args.Length < MIN_ARGS_COUNT)
				{
					throw new ArgumentException("Invalid arguments count.");
				}

				StreamReader reader = new StreamReader(args[0]);
				Lexer lexer = new Lexer(reader);

				Syntaxer syntaxer = new Syntaxer(lexer);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);

				return 1;
			}

			return 0;
		}
	}
}