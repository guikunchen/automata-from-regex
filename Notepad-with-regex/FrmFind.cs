using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor.Tools
{
    public partial class FrmFind : Form
    {
        RegexToMinDFA automata;
        private RichTextBox rtxtbox = null;
        private int findPosition = 0;
        private int findLength = 0;
        private Dictionary<int, HashSet<int>> dicResult = new Dictionary<int, HashSet<int>>();
        public FrmFind(ref RichTextBox rtxtbox)
        {
            InitializeComponent();
            this.rtxtbox = rtxtbox;
        }

        private string GetStringLastN(string s, int N)
        {
            if (N > s.Length)
            {
                return "";
            }
            string tmp = "";
            tmp = s.Substring(s.Length - N);
            return tmp;
        }

        private bool CheckSubsetString(string s, int findPosition, int length)
        {
            bool flag = false;
            if (automata.Simulate(s))
            {
                if (dicResult.ContainsKey(findPosition))
                {
                    if (dicResult[findPosition].Contains(length))
                    {
                        ;
                    }
                    else
                    {
                        flag = true;
                        Console.WriteLine(findPosition);
                        dicResult[findPosition].Add(length);
                        this.findLength = length;
                        this.findPosition = findPosition;
                        rtxtbox.Select(this.findPosition, this.findLength);
                    }
                }
                else
                {
                    HashSet<int> hashsetTmp = new HashSet<int>();
                    hashsetTmp.Add(length);
                    dicResult.Add(findPosition, hashsetTmp);
                    flag = true;
                    Console.WriteLine(findPosition);
                    this.findLength = length;
                    this.findPosition = findPosition;
                    rtxtbox.Select(this.findPosition, this.findLength);
                }
            }
            return flag;
        }

        private void FindResult()
        {
            if (txtInput.Text.Trim() == "")
            {
                MessageBox.Show("Please input regex",
                    "Notepad",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            bool flag = false;
            automata = new RegexToMinDFA(txtInput.Text.Trim());
            if (radbtnUp.Checked)
            {
                string tmp1 = rtxtbox.Text.Substring(0, rtxtbox.SelectionStart);
                for (int length = 1; length <= tmp1.Length; length++)
                {
                    string tmp2 = tmp1;
                    while (tmp2.Length >= length)
                    {
                        if (CheckSubsetString(GetStringLastN(tmp2, length), tmp2.Length - length, length))
                        {
                            flag = true;
                            return;
                        }
                        else
                        {
                            flag = false;
                        }
                        tmp2 = tmp2.Remove(tmp2.Length - 1, 1);
                    }
                }
            }
            else
            {
                string tmp1 = rtxtbox.Text.Substring(rtxtbox.SelectionStart);
                for (int length = 1; length <= tmp1.Length; length++)
                {
                    string tmp2 = tmp1;
                    while (tmp2.Length >= length)
                    {
                        if (CheckSubsetString(tmp2.Substring(0, length), rtxtbox.Text.Length - tmp2.Length, length))
                        {
                            flag = true;
                            return;
                        }
                        else
                        {
                            flag = false;
                        }
                        tmp2 = tmp2.Remove(0, 1);
                    }
                }
            }
            if (!flag)
            {
                MessageBox.Show("Cannot find \"" + txtInput.Text.Trim() + "\"",
                    "Notepad",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            FindResult();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            dicResult.Clear();
        }
    }
}
