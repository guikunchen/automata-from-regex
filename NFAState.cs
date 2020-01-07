using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Tools
{
    public class NFAState
    {
        public List<List<int>> char_set = new List<List<int>>(PublicClass.CharSetCount);// charset
        public List<int> eps = new List<int>();// eps
        public bool finished = false;// final state

        public NFAState()// initial
        {
            for(int i = 0; i < char_set.Capacity; i++)// initial
            {
                char_set.Add(new List<int>());
            }
        }
    }
}
