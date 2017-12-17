using System.Data;

namespace Compiler.LexicalAnalyzer
{
	public sealed class Token
	{
		public Token(TokenType type, int start, int row, string text = "")
		{
			Type = type;
			Start = start;
			Row = row;
			Text = text;
		}

		public TokenType Type { get; private set; }
		public string Text { get; private set; }
		public int Start { get; private set; }
		public int End { get { return Start + (Text.Length > 0 ? Text.Length - 1 : 0); } }
		public int Row { get; private set; }

		public override string ToString()
		{
			return string.Format(PATTERN, Text, Type, Start, End, Row);
		}

		private readonly string PATTERN = "Token:  Text=\"{0}\" Type=\"{1}\" First=\"{2}\" Last=\"{3}\" Row = \"{4}\"";
	}
}
