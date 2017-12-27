using System;
using System.IO;
using Compiller.Lexer;

namespace Compiller
{
    class Program
    {
        public static uint MinArgsCount = 1;

        public static int Main(string[] args)
        {
            try
            {
                if (args.Length < MinArgsCount)
                {
                    throw new ArgumentException("Invalid arguments count.");
                }

                StreamReader reader = new StreamReader(args[0]);
                Lexer.Lexer lexer = new Lexer.Lexer(reader);

                SyntaxHelper syntaxer = new SyntaxHelper(lexer);
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
