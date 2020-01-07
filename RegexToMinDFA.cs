using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace TextEditor.Tools
{
    public class RegexToMinDFA
    {
        private int[] char_set_active = new int[PublicClass.CharSetCount];
        private List<int> active_charset_index = new List<int>();
        private string origin_regex;

        private List<NFAState> NFA_state_list = new List<NFAState>();
        private Stack<int> st = new Stack<int>();
        private int NFA_size = 0;

        private List<DFAState> DFA_state_list = new List<DFAState>();
        private int DFA_size = 0;

        private List<DFAState> Min_DFA_state_list = new List<DFAState>();

        public RegexToMinDFA(string regex)
        {
            Change_Regex(regex);
        }

        public void Change_Regex(string regex)
        {
            for (int i = 0; i < char_set_active.Length; i++)
            {
                char_set_active[i] = 0;// initial value
            }
            this.origin_regex = regex;
            active_charset_index.Clear();
            foreach (char c in this.origin_regex)
            {
                if (c >= 'a' && c <= 'z')// charset declare
                {
                    char_set_active[c - 'a'] = 1;// set active
                    active_charset_index.Add(c - 'a');
                }
            }
            NFA_state_list.Clear();// clear origin state
            st.Clear();
            NFA_size = 0;
            DFA_state_list.Clear();
            DFA_size = 0;
            Min_DFA_state_list.Clear();// clear origin state
            int start_state = Regex_To_NFA();// reload
            NFA_To_DFA(start_state);
            Minimize_DFA();// reload
        }

        #region Regex To NFA
        private int Regex_To_NFA()
        {
            string postfix = RegexA_To_Postfix(Insert_Connection(origin_regex));// get postfix
            //Console.WriteLine(postfix);
            Postfix_To_NFA(postfix);
            NFA_state_list[st.Peek()].finished = true;
            st.Pop();
            return st.Peek();
        }

        public void Show_NFA()
        {
            Console.Write("State\t|");
            for(int i = 0; i < PublicClass.CharSetCount; i++)
            {
                if (char_set_active[i] == 1)
                {
                    char tmp = (char)('a' + i);
                    Console.Write("\t" + tmp + "\t|");
                }
            }
            Console.Write("\teps\t|Accept|\n");
            for(int i = 0; i < NFA_state_list.Count; i++)
            {
                Console.Write(i + "\t|\t");
                for (int j = 0; j < PublicClass.CharSetCount; j++)
                {
                    if (char_set_active[j] == 1)
                    {
                        foreach (int state in NFA_state_list[i].char_set[j]) Console.Write(state + " ");
                        Console.Write("\t|\t");
                    }
                }
                foreach (int state in NFA_state_list[i].eps) Console.Write(state + " ");
                Console.Write("\t|\t");
                if (NFA_state_list[i].finished) Console.Write("Yes");
                else Console.Write("NO");
                Console.Write("\t|\n");
            }
            // the code show below was able to shown on graphviz
            Console.WriteLine("digraph automata_0 {");
            Console.WriteLine("");
            Console.WriteLine("rankdir = LR;");
            Console.WriteLine("fontname = \"Microsoft YaHei\";");
            Console.WriteLine("fontsize = 10;");
            Console.WriteLine("");
            Console.WriteLine("node [shape = circle, fontname = \"Microsoft YaHei\", ");
            Console.WriteLine("fontsize = 10];");
            Console.WriteLine("edge [fontname = \"Microsoft YaHei\", fontsize = 10];");
            Console.WriteLine("");

            Console.WriteLine(st.Peek() + " [ style = filled, color=lightgrey ];");// start state
            int finalState = 0;
            for(int i = 0; i < NFA_state_list.Count; i++)
            {
                if (NFA_state_list[i].finished == true)
                {
                    finalState = i;
                    break;
                }
            }
            Console.WriteLine(finalState + " [ shape = doublecircle ];");// final state

            for (int i = 0; i < NFA_state_list.Count; i++)
            {
                for(int j = 0; j < NFA_state_list[i].char_set.Count; j++)
                {
                    foreach(int state in NFA_state_list[i].char_set[j])
                    {
                        Console.WriteLine(i + " -> "+ state + " [ label = \"" + Chr('a'+j) + "\" ];");
                    }
                }
                if (NFA_state_list[i].eps.Count != 0)
                {
                    foreach (int state in NFA_state_list[i].eps)
                    {
                        Console.WriteLine(i + " -> " + state + " [ label = \"eps\" ];");
                    }
                }
            }
            Console.WriteLine("\"Machine: " + origin_regex + "\" [ shape = plaintext ];");
            Console.WriteLine("}");
            // u can just copy it to file "xxx.dot" and in command run "dot -Tpng xxx.dot -o xxx.png"
        }
        private string Chr(int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)asciiCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            else
            {
                throw new Exception("ASCII Code is not valid.");
            }
        }

        private void Basic_Rules(int char_index_in_charset)
        {
            NFA_state_list.Add(new NFAState());
            NFA_state_list.Add(new NFAState());// add two states
            NFA_state_list[NFA_size].char_set[char_index_in_charset].Add(NFA_size + 1);
            st.Push(NFA_size);// second one of the stack is the start state
            NFA_size++;
            st.Push(NFA_size);// top of the stack is the final state
            NFA_size++;
        }

        private void Union()
        {
            NFA_state_list.Add(new NFAState());
            NFA_state_list.Add(new NFAState());// add two states
            int d = st.Peek(); st.Pop();// final state
            int c = st.Peek(); st.Pop();// start state
            int b = st.Peek(); st.Pop();// final state
            int a = st.Peek(); st.Pop();// start state
            NFA_state_list[NFA_size].eps.Add(a);
            NFA_state_list[NFA_size].eps.Add(c);
            NFA_state_list[b].eps.Add(NFA_size + 1);
            NFA_state_list[d].eps.Add(NFA_size + 1);
            st.Push(NFA_size);// second one of the stack is the start state
            NFA_size++;
            st.Push(NFA_size);// top of the stack is the final state
            NFA_size++;
        }

        private void Connection()
        {
            int d = st.Peek(); st.Pop();// final state
            int c = st.Peek(); st.Pop();// start state
            int b = st.Peek(); st.Pop();// final state
            int a = st.Peek(); st.Pop();// start state
            NFA_state_list[b].eps.Add(c);
            st.Push(a);// start state
            st.Push(d);// final state
        }

        private void Kleene_Closure()
        {
            NFA_state_list.Add(new NFAState());
            NFA_state_list.Add(new NFAState());// add two states
            int b = st.Peek(); st.Pop();// final state
            int a = st.Peek(); st.Pop();// start state
            NFA_state_list[NFA_size].eps.Add(a);
            NFA_state_list[NFA_size].eps.Add(NFA_size + 1);// start state
            NFA_state_list[b].eps.Add(a);
            NFA_state_list[b].eps.Add(NFA_size + 1);
            st.Push(NFA_size);// second one of the stack is the start state
            NFA_size++;
            st.Push(NFA_size);// top of the stack is the final state
            NFA_size++;
        }

        private string Insert_Connection(string regex)
        {
            string regexA = "";
            char c, c2;
            for (int i = 0; i < regex.Length; i++)
            {
                c = regex[i];
                if (i + 1 < regex.Length)
                {
                    c2 = regex[i + 1];
                    regexA += c;
                    if (c != '(' && c != '|' && c2 != ')' && c2 != '|' && c2 != '*')
                    {
                        regexA += '.';// insert
                    }
                }
            }
            regexA += regex[regex.Length - 1];
            return regexA;
        }

        private int Operator_Priority(char op)
        {
            switch (op)
            {
                case '*': return 3;
                case '.': return 2;
                case '|': return 1;
                default: return 0;
            }
        }

        private string RegexA_To_Postfix(string regexA)
        {
            string postfix = "";
            Stack<char> op = new Stack<char>();
            char c;
            for (int i = 0; i < regexA.Length; i++)
            {
                if (regexA[i] >= 'a' && regexA[i] <= 'z')// charset declare
                {
                    postfix += regexA[i];
                    continue;
                }
                switch (regexA[i])
                {
                    case '(':
                        op.Push(regexA[i]); break;
                    case ')':
                        while (op.Peek() != '(')
                        {
                            postfix += op.Peek();
                            op.Pop();
                        }
                        op.Pop();// pop ')'
                        break;
                    default:
                        while (op.Count != 0)// no empty
                        {
                            c = op.Peek();
                            if (Operator_Priority(c) >= Operator_Priority(regexA[i]))
                            {
                                postfix += op.Peek();
                                op.Pop();
                            }
                            else break;
                        }
                        op.Push(regexA[i]);
                        break;
                }
            }
            while (op.Count != 0)// no empty
            {
                postfix += op.Peek();
                op.Pop();
            }
            return postfix;
        }


        private void Postfix_To_NFA(string postfix)
        {
            for (int i = 0; i < postfix.Length; i++)
            {
                if(postfix[i] >= 'a' && postfix[i] <= 'z')// charset declare
                {
                    Basic_Rules(postfix[i] - 'a');
                    continue;
                }
                switch (postfix[i])
                {
                    case '*': Kleene_Closure(); break;
                    case '.': Connection(); break;
                    case '|': Union(); break;
                }
            }
        }
        #endregion

        #region NFA To DFA

        public void Show_DFA()
        {
            Console.Write("State\t|");
            for (int i = 0; i < PublicClass.CharSetCount; i++)
            {
                if (char_set_active[i] == 1)
                {
                    char tmp = (char)('a' + i);
                    Console.Write("\t" + tmp + "\t|");
                }
            }
            Console.Write("Accept|\n");
            for (int i = 0; i < DFA_state_list.Count; i++)
            {
                Console.Write(i + "\t|\t");
                for (int j = 0; j < PublicClass.CharSetCount; j++)
                {
                    if (char_set_active[j] == 1)
                    {
                        Console.Write(DFA_state_list[i].char_set[j] + " ");
                        Console.Write("\t|\t");
                    }
                }
                if (DFA_state_list[i].finished) Console.Write("Yes");
                else Console.Write("NO");
                Console.Write("\t|\n");
            }
            // the code show below was able to shown on graphviz
            Console.WriteLine("digraph automata_0 {");
            Console.WriteLine("");
            Console.WriteLine("rankdir = LR;");
            Console.WriteLine("fontname = \"Microsoft YaHei\";");
            Console.WriteLine("fontsize = 10;");
            Console.WriteLine("");
            Console.WriteLine("node [shape = circle, fontname = \"Microsoft YaHei\", ");
            Console.WriteLine("fontsize = 10];");
            Console.WriteLine("edge [fontname = \"Microsoft YaHei\", fontsize = 10];");
            Console.WriteLine("");

            Console.WriteLine("0" + " [ style = filled, color=lightgrey ];");// start state
            for (int i = 0; i < DFA_state_list.Count; i++)
            {
                if (DFA_state_list[i].finished == true)
                {
                    Console.WriteLine(i + " [ shape = doublecircle ];");// final state
                    break;
                }
            }

            for (int i = 0; i < DFA_state_list.Count; i++)
            {
                for (int j = 0;j < DFA_state_list[i].char_set.Length; j++)
                {
                    if (char_set_active[j] == 0 || DFA_state_list[i].char_set[j] == -1)
                    {
                        continue;
                    }
                    Console.WriteLine(i + " -> " + DFA_state_list[i].char_set[j] + " [ label = \"" + Chr('a' + j) + "\" ];");
                }
            }
            Console.WriteLine("\"Machine: " + origin_regex + "\" [ shape = plaintext ];");
            Console.WriteLine("}");
            // u can just copy it to file "xxx.dot" and in command run "dot -Tpng xxx.dot -o xxx.png"
        }

        // I used List<T> as the key of a Dictionary, and in order to distinct the state sets i adopt Hashset.
        // But it still don't work.
        // For example, the HashCode of a HashSet which contains (0, 1, 2), will be different with another HashSet which contains the same element.
        // I have to write two methods below since the different HashCode of state sets which contain the same element.
        // TODO: Override the getHashCode() method of HashSet to replace the current stupid design.
        private bool My_Contains_Key(Dictionary<HashSet<int>, int> keyValuePairs, HashSet<int> hash_set)
        {
            
            foreach (HashSet<int> tmp in keyValuePairs.Keys)
            {
                if (tmp.Count != hash_set.Count)
                {
                    continue;
                }
                else
                {
                    bool flag = true;
                    foreach (int i in tmp)
                    {
                        if (!hash_set.Contains(i))
                        {
                            flag = false;
                        }
                    }
                    if (flag == true)
                    {
                        return flag;
                    }
                }
            }
            return false;
        }
        private HashSet<int> My_Gets_Key(Dictionary<HashSet<int>, int> keyValuePairs, HashSet<int> hash_set)
        {
            foreach (HashSet<int> tmp in keyValuePairs.Keys)
            {
                if (tmp.Count != hash_set.Count)
                {
                    continue;
                }
                else
                {
                    bool flag = true;
                    foreach (int i in tmp)
                    {
                        if (!hash_set.Contains(i))
                        {
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        return tmp;
                    }
                }
            }
            return hash_set;
        }

        private void NFA_To_DFA(int start_state)
        {
            Dictionary<HashSet<int>, int> keyValuePairs = new Dictionary<HashSet<int>, int>();
            Queue<HashSet<int>> queue = new Queue<HashSet<int>>();
            HashSet<int> subset0 = new HashSet<int>();
            Epsilon_Closure(start_state, ref subset0);
            queue.Enqueue(Epsilon_Closure(subset0));
            while (queue.Count != 0)
            {
                HashSet<int> T = queue.Dequeue();
                DFA_state_list.Add(new DFAState());
                bool finished = false;
                foreach (int i in T)
                {
                    if (NFA_state_list[i].finished == true)
                    {
                        finished = true;
                    }
                }
                if (!My_Contains_Key(keyValuePairs, T))
                {
                    keyValuePairs.Add(My_Gets_Key(keyValuePairs, T), DFA_size++);
                }
                DFA_state_list[keyValuePairs[My_Gets_Key(keyValuePairs, T)]].finished = finished;
                for (int active = 0; active < PublicClass.CharSetCount; active++)
                {
                    if (char_set_active[active] == 0)
                    {
                        continue;
                    }
                    HashSet<int> U = Epsilon_Closure(Move(T, active));
                    if (U.Count != 0 && !My_Contains_Key(keyValuePairs, U))
                    {
                        queue.Enqueue(U);
                        keyValuePairs.Add(My_Gets_Key(keyValuePairs, U), DFA_size++);
                    }
                    if (U.Count != 0)
                    {
                        DFA_state_list[keyValuePairs[My_Gets_Key(keyValuePairs, T)]].char_set[active] = keyValuePairs[My_Gets_Key(keyValuePairs, U)];
                    }
                }
            }
            if (!NeedDeadState())// Each state has a transition under each input symbol
            {
                return;
            }
            // add dead state
            for (int DFA_index = 0; DFA_index < DFA_size; DFA_index++)
            {
                for (int i = 0; i < DFA_state_list[DFA_index].char_set.Length; i++)
                {
                    if (DFA_state_list[DFA_index].char_set[i] == -1)
                    {
                        DFA_state_list[DFA_index].char_set[i] = DFA_size;
                    }
                }
            }
            DFA_state_list.Add(new DFAState());
            DFA_state_list[DFA_size].finished = false;
            for (int active = 0; active < PublicClass.CharSetCount; active++)
            {
                if (char_set_active[active] == 0)
                {
                    continue;
                }
                DFA_state_list[DFA_size].char_set[active] = DFA_size;
            }
            DFA_size++;
        }

        private HashSet<int> Epsilon_Closure(HashSet<int> subset)
        {
            HashSet<int> tmp = new HashSet<int>();
            foreach(int state in subset)
            {
                Epsilon_Closure(state, ref tmp);
            }
            return tmp;
        }

        private void Epsilon_Closure(int state, ref HashSet<int> subset)
        {
            if (!subset.Contains(state))// don't contains
            {
                subset.Add(state);// insert
            }
            foreach (int i in NFA_state_list[state].eps)
            {
                if (!subset.Contains(i))// don't contains
                {
                    subset.Add(i);// insert
                    Epsilon_Closure(i, ref subset);
                }
            }
        }

        private HashSet<int> Move(HashSet<int> subset, int char_index)
        {
            HashSet<int> tmp = new HashSet<int>();
            foreach(int state in subset)
            {
                foreach (int i in NFA_state_list[state].char_set[char_index])
                {
                    tmp.Add(i);
                }
            }
            return tmp;
        }

        private bool NeedDeadState()
        {
            for (int DFA_index = 0; DFA_index < DFA_size; DFA_index++)
            {
                foreach (int i in active_charset_index)
                {
                    if (DFA_state_list[DFA_index].char_set[i] == -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region DFA To Minimum DFA

        private void Minimize_DFA()
        {
            // initializing the partitions
            List<List<int>> part = Initialize_Partition();
            // structure the partitions
            Structure_Partition(ref part);
            // build minimum DFA
            CleanEmptyPart(ref part);
            for(int i = 0; i < part.Count; i++)
            {
                DFAState tmp = Build_New_DFA(part, i);
                Min_DFA_state_list.Add(tmp);
            }
        }

        public void Show_Min_DFA()
        {
            Console.Write("State\t|");
            for (int i = 0; i < PublicClass.CharSetCount; i++)
            {
                if (char_set_active[i] == 1)
                {
                    char tmp = (char)('a' + i);
                    Console.Write("\t" + tmp + "\t|");
                }
            }
            Console.Write("Accept|\n");
            for (int i = 0; i < Min_DFA_state_list.Count; i++)
            {
                Console.Write(i + "\t|\t");
                for (int j = 0; j < PublicClass.CharSetCount; j++)
                {
                    if (char_set_active[j] == 1)
                    {
                        Console.Write(Min_DFA_state_list[i].char_set[j] + " ");
                        Console.Write("\t|\t");
                    }
                }
                if (Min_DFA_state_list[i].finished) Console.Write("Yes");
                else Console.Write("NO");
                Console.Write("\t|\n");
            }
            // the code show below was able to shown on graphviz
            Console.WriteLine("digraph automata_0 {");
            Console.WriteLine("");
            Console.WriteLine("rankdir = LR;");
            Console.WriteLine("fontname = \"Microsoft YaHei\";");
            Console.WriteLine("fontsize = 10;");
            Console.WriteLine("");
            Console.WriteLine("node [shape = circle, fontname = \"Microsoft YaHei\", ");
            Console.WriteLine("fontsize = 10];");
            Console.WriteLine("edge [fontname = \"Microsoft YaHei\", fontsize = 10];");
            Console.WriteLine("");

            Console.WriteLine("0" + " [ style = filled, color=lightgrey ];");// start state
            for (int i = 0; i < Min_DFA_state_list.Count; i++)
            {
                if (Min_DFA_state_list[i].finished == true)
                {
                    Console.WriteLine(i + " [ shape = doublecircle ];");// final state
                    break;
                }
            }

            for (int i = 0; i < Min_DFA_state_list.Count; i++)
            {
                for (int j = 0; j < Min_DFA_state_list[i].char_set.Length; j++)
                {
                    if (char_set_active[j] == 0 || Min_DFA_state_list[i].char_set[j] == -1)
                    {
                        continue;
                    }
                    Console.WriteLine(i + " -> " + Min_DFA_state_list[i].char_set[j] + " [ label = \"" + Chr('a' + j) + "\" ];");
                }
            }
            Console.WriteLine("\"Machine: " + origin_regex + "\" [ shape = plaintext ];");
            Console.WriteLine("}");
            // u can just copy it to file "xxx.dot" and in command run "dot -Tpng xxx.dot -o xxx.png"
        }

        private List<List<int>> Initialize_Partition()
        {
            List<List<int>> tmp = new List<List<int>>();
            tmp.Add(new List<int>());
            tmp.Add(new List<int>());
            for (int i = 0; i < DFA_state_list.Count; i++)
            {
                if (DFA_state_list[i].finished == false)
                {
                    tmp[0].Add(i);// begin state group (S-F)
                }
                else
                {
                    tmp[1].Add(i);// final state group (F)
                }
            }
            return tmp;
        }

        private void CleanEmptyPart(ref List<List<int>> part)
        {
            List<List<int>> tmp = new List<List<int>>();
            for(int i = 0; i < part.Count; i++)
            {
                if (part[i].Count != 0)
                {
                    tmp.Add(part[i]);
                }
            }
            part = tmp;
        }

        private void Structure_Partition(ref List<List<int>> part)
        {
            // loop until no new group is created
            bool check = true;  // check if any new group is created
            while (check)
            {
                check = false;
                // iterate over groups and alphabets
                for (int i = 0; i < part.Count; i++)
                {
                    if (check == true)// if any new group is created
                    {
                        break;
                    }
                    foreach (int j in active_charset_index)
                    {
                        if (check == true)// if any new group is created
                        {
                            break;
                        }
                        if (part[i].Count == 0)
                        {
                            break;
                        }
                        int tmp0 = DFA_state_list[part[i][0]].char_set[j];// get the transition of the first state
                        int group_index = Get_Group_Index(part, tmp0);
                        for (int other = 1; other < part[i].Count; other++)
                        {
                            if (part[group_index].Contains(DFA_state_list[part[i][other]].char_set[j]))
                            {
                                continue;
                            }
                            else
                            {
                                // create new group
                                check = true;
                                Create_New_Group(ref part, i, Get_Group_Index(part, DFA_state_list[part[i][other]].char_set[j]), j);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private int Get_Group_Index(List<List<int>> part, int state)
        {
            int index = 0;
            for(int i = 0; i < part.Count; i++)
            {
                if (part[i].Contains(state))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private void Create_New_Group(ref List<List<int>> part, int group_index, int transition_group_index, int charset_index)
        {
            part.Add(new List<int>());// add new group
            for(int i = 1; i < part[group_index].Count; i++)
            {
                int transition_state_index = DFA_state_list[part[group_index][i]].char_set[charset_index];
                if (part[transition_group_index].Contains(transition_state_index))
                {
                    part[part.Count - 1].Add(part[group_index][i]);
                }
            }
            foreach(int i in part[part.Count - 1])// delete items of origin group
            {
                part[group_index].Remove(i);
            }
        }

        private DFAState Build_New_DFA(List<List<int>> part, int state)
        {
            DFAState tmp = new DFAState();
            tmp.finished = false;
            if (DFA_state_list[part[state][0]].finished)
            {
                tmp.finished = true;
            }
            foreach (int index in active_charset_index)
            {
                tmp.char_set[index] = Get_Group_Index(part, DFA_state_list[part[state][0]].char_set[index]);
            }
            return tmp;
        }

        #endregion

        #region Simulation

        // using minimum DFA for simulation
        public bool Simulate(string s)
        {
            foreach(char c in s)// contains inactive characters
            {
                if (c < 'a' || c > 'z')
                {
                    return false;
                }
                if (char_set_active[c - 'a'] == 0)
                {
                    return false;
                }
            }
            int state = 0;// start state
            char[] str = s.ToArray();
            for(int i = 0; i < str.Length; i++)
            {
                state = Min_DFA_state_list[state].char_set[str[i] - 'a'];
            }
            if (Min_DFA_state_list[state].finished)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
