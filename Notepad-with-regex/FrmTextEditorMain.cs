using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextEditor.Tools;
using System.IO;

namespace TextEditor
{
    public partial class FrmTextEditorMain : Form
    {
        private string fileName = "";
        private bool changed = false;
        private Font defaultFont = new Font("SimSun", 15.75f);

        public FrmTextEditorMain()
        {
            InitializeComponent();
        }

        private void ResetMember()
        {
            fileName = "";
            changed = false;
        }

        private void ReloadForm()
        {
            rtxtboxMain.Text = "";
            ResetMember();
            FrmTextEditorMain_Load(null, null);
        }

        private void SetFileMember(string fileName)
        {
            this.fileName = fileName;
            this.Text = System.IO.Path.GetFileNameWithoutExtension(fileName) + " - Notepad";// reset form text
        }

        private DialogResult CheckFileSaved()
        {
            DialogResult result = new DialogResult();
            result = DialogResult.No;
            if (changed == true)
            {
                string tmp = "Do you want to save changes to " + System.IO.Path.GetFileNameWithoutExtension(fileName);
                if (fileName == "")
                {
                    tmp += "Untitled";
                }
                result = MessageBox.Show(tmp,
                    "Notepad",
                    MessageBoxButtons.YesNoCancel);
            }
            return result;
        }

        private void SaveToFile(string fileName)
        {
            StreamWriter sw = new StreamWriter(fileName, false, Encoding.Default);
            sw.Write(rtxtboxMain.Text);
            sw.Close();
        }

        private void FrmTextEditorMain_Load(object sender, EventArgs e)
        {
            rtxtboxMain.LanguageOption = RichTextBoxLanguageOptions.UIFonts;// Chinese and English input using the same font
            FrmTextEditorMain_SizeChanged(null, null);// reset winform size
            this.Text = "Untitled - Notepad";
            rtxtboxMain.Focus();
            #region test DFA
            RegexToMinDFA tmp = new RegexToMinDFA("ab*(c|a)o");// input regex
            tmp.Show_NFA();// output
            tmp.Show_DFA();
            tmp.Show_Min_DFA();
            //tmp.Change_Regex("abb*bbaaccddoo");// change regex
            //tmp.Show_NFA();// output
            //tmp.Show_DFA();
            //tmp.Show_Min_DFA();
            //RegexToMinDFA tmp = new RegexToMinDFA("abb*(a|b)(ab|ba)*aba");// input regex
            //if (tmp.Simulate("abbbbbbbbbbbbbabbbbbb"))// test string
            //{
            //    Console.WriteLine("abbbbbbbbbbbbbabbbbbb");
            //}
            //if (tmp.Simulate("abbbbbbbbbbbbbabbaabbaaba"))
            //{
            //    Console.WriteLine("abbbbbbbbbbbbbabbaabbaaba");
            //}
            //tmp.Change_Regex("abb*bbaaccddoo");// change regex
            //if (tmp.Simulate("ertytry"))
            //{
            //    Console.WriteLine("ertytry");
            //}
            //if (tmp.Simulate("abbbbbbbbbbbbbbbbbbaaccddoo"))
            //{
            //    Console.WriteLine("abbbbbbbbbbbbbbbbbbaaccddoo");
            //}
            #endregion
        }

        private void FrmTextEditorMain_SizeChanged(object sender, EventArgs e)
        {
            rtxtboxMain.Width = this.Width - 16;
            rtxtboxMain.Height = this.Height - 63;
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Dispose();
        }

        private void rtxtboxMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 116)// F5, append current time
            {
                e.Handled = true;
                tsmiTimeDate_Click(null, null);// append current time
            }
            else if (e.Control && e.KeyValue == 78)// ctrl+n new
            {
                e.Handled = true;
                tsmiNew_Click(null, null);
            }
            else if (e.Control && e.KeyValue == 79)// ctrl+o open
            {
                e.Handled = true;
                tsmiOpen_Click(null, null);
            }
            else if (e.Control && e.KeyValue == 83)// ctrl+s save
            {
                e.Handled = true;
                tsmiSave_Click(null, null);
            }
            else if (e.Control &&  e.Shift && e.KeyValue == 83)// ctrl+shift+s save as
            {
                e.Handled = true;
                tsmiSaveAs_Click(null, null);
            }
            else if (e.Control && e.KeyValue == 70)// ctrl+f find
            {
                e.Handled = true;
                tsmiFind_Click(null, null);
            }
            else if (e.Control && e.KeyValue == 72)// ctrl+h replace
            {
                e.Handled = true;
                tsmiReplace_Click(null, null);
            }
            else if (e.Control && e.KeyValue == 107)// ctrl+plus zoom in
            {
                e.Handled = true;
                tsmiZoomIn_Click(null, null);
            }
            else if (e.Control && e.KeyValue == 109)// ctrl+minus zoom out
            {
                e.Handled = true;
                tsmiZoomOut_Click(null, null);
            }
            else if (e.Control && e.KeyValue == 96)// restore default font
            {
                e.Handled = true;
                tsmiRestoreDefaultZoom_Click(null, null);
            }
        }

        private void tsmiFont_Click(object sender, EventArgs e)
        {
            DialogResult result = changeFontDialogMenu.ShowDialog();
            if (result == DialogResult.OK)
            {
                rtxtboxMain.Font = changeFontDialogMenu.Font;
            }
        }

        private void tsmiOpen_Click(object sender, EventArgs e)
        {
            // open file dialog
            OpenFileDialog openFileDialogMain = new OpenFileDialog();
            openFileDialogMain.Filter = "Text(*.txt)|*.txt|All(*.*)|*.*";// set filter
            openFileDialogMain.FilterIndex = 1;// set filter index
            openFileDialogMain.InitialDirectory = "D:\\";// set initial directory
            if (openFileDialogMain.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // if dialog had been confirmed
                SetFileMember(openFileDialogMain.FileName);
                rtxtboxMain.LoadFile(fileName, RichTextBoxStreamType.PlainText);// plain text, without image
            }
        }

        private void tsmiWordWrap_CheckedChanged(object sender, EventArgs e)
        {
            rtxtboxMain.WordWrap = tsmiWordWrap.Checked;
        }

        private void tsmiTimeDate_Click(object sender, EventArgs e)
        {
            rtxtboxMain.SelectedText += System.DateTime.Now;// append current time
        }

        private void tsmiNew_Click(object sender, EventArgs e)
        {
            DialogResult result = CheckFileSaved();// saved?
            switch (result)
            {
                case DialogResult.Cancel:
                    break;
                case DialogResult.Yes:
                    tsmiSave_Click(null, null);
                    ReloadForm();
                    break;
                case DialogResult.No:
                    ReloadForm();
                    break;
                default:
                    break;
            }
        }

        private void tsmiNewWindow_Click(object sender, EventArgs e)
        {
            
        }

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            if (this.fileName != "")// had opened a file
            {
                SaveToFile(fileName);
            }
            else
            {
                tsmiSaveAs_Click(null, null);
            }
        }

        private void tsmiSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text(*.txt)|*.txt|All(*.*)|*.*";// set filter
            saveFileDialog.FileName = "*.txt";// default file name
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SetFileMember(saveFileDialog.FileName);
                SaveToFile(fileName);
            }
        }

        private void FrmTextEditorMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = CheckFileSaved();
            switch (result)
            {
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
                case DialogResult.Yes:
                    tsmiSave_Click(null, null);
                    break;
                case DialogResult.No:
                    break;
                default:
                    break;
            }
        }

        private void rtxtboxMain_TextChanged(object sender, EventArgs e)
        {
            if (changed == false)
            {
                changed = true;
                this.Text = "*" + this.Text;
            }
        }

        private void tsmiSelectAll_Click(object sender, EventArgs e)
        {
            rtxtboxMain.SelectAll();
        }

        private void tsmiUndo_Click(object sender, EventArgs e)
        {
            rtxtboxMain.Undo();
        }

        private void tsmiCut_Click(object sender, EventArgs e)
        {
            rtxtboxMain.Cut();
        }

        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            rtxtboxMain.Copy();
        }

        private void tsmiPaste_Click(object sender, EventArgs e)
        {
            rtxtboxMain.Paste();
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {

        }

        private void tsmiFind_Click(object sender, EventArgs e)
        {
            FrmFind tmp = new FrmFind(ref rtxtboxMain);
            tmp.Show();
        }

        private void tsmiReplace_Click(object sender, EventArgs e)
        {
            FrmReplace tmp = new FrmReplace(ref rtxtboxMain);
            tmp.Show();
        }

        private void tsmiZoomIn_Click(object sender, EventArgs e)
        {
            rtxtboxMain.Font = new Font(rtxtboxMain.Font.FontFamily, rtxtboxMain.Font.Size + 1);
        }

        private void tsmiZoomOut_Click(object sender, EventArgs e)
        {
            rtxtboxMain.Font = new Font(rtxtboxMain.Font.FontFamily, rtxtboxMain.Font.Size - 1);
        }

        private void tsmiRestoreDefaultZoom_Click(object sender, EventArgs e)
        {
            rtxtboxMain.Font = defaultFont;
        }
    }
}
