using Stone;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace chap3
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var str = @"while i<10 {
                        sum = sum+i
                        i = i +1
                        }
                        sum";
            Lexer l = new Lexer(str);
            for (Token t; (t = await l.Read()) != Token.EOF;)
                Console.WriteLine("=> " + t.Text);
        }
    }
}
