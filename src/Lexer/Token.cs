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

        public TokenType Type { get; }
        public string Text { get; }
        public int Start { get; }
        public int End => Start + (Text.Length > 0 ? Text.Length - 1 : 0);
        public int Row { get; }

        public override string ToString()
        {
            return string.Format(PATTERN, Text, Type, Start, End, Row);
        }

        private readonly string PATTERN = "Token:  Text=\"{0}\" Type=\"{1}\" First=\"{2}\" Last=\"{3}\" Row = \"{4}\"";
    }
}
