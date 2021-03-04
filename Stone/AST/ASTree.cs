using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.AST
{
    public abstract class ASTree : IEnumerable<ASTree>
    {
        public abstract ASTree Child(int i);

        public abstract int NumChildren();

        public abstract IEnumerable<ASTree> Children();

        public abstract string Location();

        public IEnumerator<ASTree> GetEnumerator()
        {
            return Children().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
