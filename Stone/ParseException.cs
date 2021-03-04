using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone
{
    public class ParseException : Exception
    {
        public ParseException(Token t) : this(string.Empty, t)
        {
        }
        public ParseException(string msg, Token t) : base("syntax error around " + location(t) + ". " + msg)
        {
        }
        private static String location(Token t)
        {
            if (t == Token.EOF)
                return "the last line";
            else
                return "\"" + t.Text + "\" at line " + t.LineNumber;
        }
        public ParseException() : base()
        {
        }
        public ParseException(string msg) : base(msg)
        {
        }


        public ParseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
