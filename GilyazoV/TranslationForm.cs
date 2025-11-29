using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GilyazoV
{
    public partial class TranslationForm : Form
    {
        private TextBox tbSource;
        private TextBox tbIntermediate;
        private Label lblSource;
        private Label lblIntermediate;
        private Button btnClose;

        public TranslationForm()
        {
            InitializeComponent();
        }

        public void SetSourceCode(string sourceCode)
        {
            tbSource.Text = sourceCode;
        }

        public void SetIntermediateCode(string intermediateCode)
        {
            tbIntermediate.Text = intermediateCode;
        }

        private void InitializeComponent()
        {
            lblSource = new Label();
            lblIntermediate = new Label();
            tbSource = new TextBox();
            tbIntermediate = new TextBox();
            btnClose = new Button();
            SuspendLayout();
            // 
            // lblSource
            // 
            lblSource.Location = new Point(12, 12);
            lblSource.Name = "lblSource";
            lblSource.Size = new Size(500, 20);
            lblSource.TabIndex = 4;
            lblSource.Text = "Исходный код:";
            // 
            // lblIntermediate
            // 
            lblIntermediate.Location = new Point(530, 12);
            lblIntermediate.Name = "lblIntermediate";
            lblIntermediate.Size = new Size(500, 20);
            lblIntermediate.TabIndex = 3;
            lblIntermediate.Text = "Промежуточный код:";
            // 
            // tbSource
            // 
            tbSource.Location = new Point(12, 35);
            tbSource.Multiline = true;
            tbSource.Name = "tbSource";
            tbSource.ReadOnly = true;
            tbSource.ScrollBars = ScrollBars.Both;
            tbSource.Size = new Size(500, 250);
            tbSource.TabIndex = 0;
            // 
            // tbIntermediate
            // 
            tbIntermediate.Location = new Point(530, 35);
            tbIntermediate.Multiline = true;
            tbIntermediate.Name = "tbIntermediate";
            tbIntermediate.ReadOnly = true;
            tbIntermediate.ScrollBars = ScrollBars.Both;
            tbIntermediate.Size = new Size(500, 250);
            tbIntermediate.TabIndex = 1;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(452, 306);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(150, 35);
            btnClose.TabIndex = 2;
            btnClose.Text = "Закрыть";
            btnClose.Click += BtnClose_Click;
            // 
            // TranslationForm
            // 
            ClientSize = new Size(1050, 361);
            Controls.Add(btnClose);
            Controls.Add(lblIntermediate);
            Controls.Add(lblSource);
            Controls.Add(tbIntermediate);
            Controls.Add(tbSource);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TranslationForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Исходный и промежуточный код";
            ResumeLayout(false);
            PerformLayout();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

