namespace TextEditor.Tools
{
    partial class FrmFind
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnFindNext = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpboxDirection = new System.Windows.Forms.GroupBox();
            this.radbtnDown = new System.Windows.Forms.RadioButton();
            this.radbtnUp = new System.Windows.Forms.RadioButton();
            this.lblFindWhat = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.grpboxDirection.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFindNext
            // 
            this.btnFindNext.Location = new System.Drawing.Point(391, 14);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(117, 35);
            this.btnFindNext.TabIndex = 0;
            this.btnFindNext.Text = "Find Next";
            this.btnFindNext.UseVisualStyleBackColor = true;
            this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(391, 63);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(117, 35);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grpboxDirection
            // 
            this.grpboxDirection.Controls.Add(this.radbtnDown);
            this.grpboxDirection.Controls.Add(this.radbtnUp);
            this.grpboxDirection.Location = new System.Drawing.Point(199, 75);
            this.grpboxDirection.Name = "grpboxDirection";
            this.grpboxDirection.Size = new System.Drawing.Size(174, 61);
            this.grpboxDirection.TabIndex = 2;
            this.grpboxDirection.TabStop = false;
            this.grpboxDirection.Text = "Direction";
            // 
            // radbtnDown
            // 
            this.radbtnDown.AutoSize = true;
            this.radbtnDown.Location = new System.Drawing.Point(98, 24);
            this.radbtnDown.Name = "radbtnDown";
            this.radbtnDown.Size = new System.Drawing.Size(60, 19);
            this.radbtnDown.TabIndex = 1;
            this.radbtnDown.Text = "Down";
            this.radbtnDown.UseVisualStyleBackColor = true;
            // 
            // radbtnUp
            // 
            this.radbtnUp.AutoSize = true;
            this.radbtnUp.Checked = true;
            this.radbtnUp.Location = new System.Drawing.Point(25, 24);
            this.radbtnUp.Name = "radbtnUp";
            this.radbtnUp.Size = new System.Drawing.Size(44, 19);
            this.radbtnUp.TabIndex = 0;
            this.radbtnUp.TabStop = true;
            this.radbtnUp.Text = "Up";
            this.radbtnUp.UseVisualStyleBackColor = true;
            // 
            // lblFindWhat
            // 
            this.lblFindWhat.AutoSize = true;
            this.lblFindWhat.Location = new System.Drawing.Point(21, 24);
            this.lblFindWhat.Name = "lblFindWhat";
            this.lblFindWhat.Size = new System.Drawing.Size(87, 15);
            this.lblFindWhat.TabIndex = 3;
            this.lblFindWhat.Text = "Find what:";
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(130, 21);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(243, 25);
            this.txtInput.TabIndex = 4;
            this.txtInput.TextChanged += new System.EventHandler(this.txtInput_TextChanged);
            // 
            // FrmFind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(535, 171);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.lblFindWhat);
            this.Controls.Add(this.grpboxDirection);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnFindNext);
            this.Name = "FrmFind";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find";
            this.grpboxDirection.ResumeLayout(false);
            this.grpboxDirection.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFindNext;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpboxDirection;
        private System.Windows.Forms.RadioButton radbtnDown;
        private System.Windows.Forms.RadioButton radbtnUp;
        private System.Windows.Forms.Label lblFindWhat;
        private System.Windows.Forms.TextBox txtInput;
    }
}