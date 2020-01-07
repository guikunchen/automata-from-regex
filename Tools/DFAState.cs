using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Tools
{
    public class DFAState
    {
        public int[] char_set = new int[PublicClass.CharSetCount];// the value is the transition
        public bool finished = false;
        public DFAState()
        {
            for(int i = 0; i < char_set.Length; i++)
            {
                char_set[i] = -1;
            }
        }
    }
}
