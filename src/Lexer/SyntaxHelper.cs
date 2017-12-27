using System;

namespace Compiller.Lexer
{
    public class SyntaxHelper
    {
        public SyntaxHelper(Lexer lexer)
        {
            _lexer = lexer;

            StartParse();
        }

        private readonly Lexer _lexer;

        private void StartParse()
        {
            while (true)
            {
                Token token = _lexer.ReadStream();
                if (token.Type != TokenType.Eoln)
                {
                    Console.WriteLine(token.ToString());
                }

                if (token.Type == TokenType.Eof)
                {
                    break;
                }
            }
        }
    }
}
