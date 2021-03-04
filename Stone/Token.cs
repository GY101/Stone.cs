using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone
{
    public class Token
    {
        public static readonly Token EOF = new Token(-1);
        public const string EOL = "\\n";
        protected Token(int line)
        {
            LineNumber = line;
        }
        public int LineNumber { get; private set; }

        public virtual bool IsIdentifier => false;
        public virtual bool IsString => false;
        public virtual bool IsNumber => false;


        public virtual int Number { get { throw new StoneException("not number token"); } }
        public virtual string Text => string.Empty;
    }
}
