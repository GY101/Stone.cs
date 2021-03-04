using Stone.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone
{
    public class StoneException : Exception
    {
        public StoneException() : base()
        {
        }

        public StoneException(string message) : base(message)
        {
        }

        public StoneException(string message, ASTree tree) : this($"{message} {tree.Location()}") { }

        public StoneException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
