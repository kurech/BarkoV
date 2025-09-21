namespace GilyazoV
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tbFSource = new TextBox();
            btnFStart = new Button();
            tbFMessage = new TextBox();
            btnFRecord = new Button();
            btnDelete = new Button();
            findBtn = new Button();
            ChngBtn = new Button();
            listBox1 = new ListBox();
            listBox2 = new ListBox();
            listBox3 = new ListBox();
            syntTree = new TreeView();
            structTree = new TreeView();
            SuspendLayout();
            // 
            // tbFSource
            // 
            tbFSource.AcceptsReturn = true;
            tbFSource.AcceptsTab = true;
            tbFSource.Cursor = Cursors.IBeam;
            tbFSource.Location = new Point(32, 31);
            tbFSource.Multiline = true;
            tbFSource.Name = "tbFSource";
            tbFSource.Size = new Size(471, 131);
            tbFSource.TabIndex = 0;
            // 
            // btnFStart
            // 
            btnFStart.Location = new Point(56, 168);
            btnFStart.Name = "btnFStart";
            btnFStart.Size = new Size(83, 43);
            btnFStart.TabIndex = 1;
            btnFStart.Text = "Проверить";
            btnFStart.UseVisualStyleBackColor = true;
            btnFStart.Click += btnFStart_Click;
            // 
            // tbFMessage
            // 
            tbFMessage.AcceptsReturn = true;
            tbFMessage.AcceptsTab = true;
            tbFMessage.Cursor = Cursors.IBeam;
            tbFMessage.Location = new Point(32, 217);
            tbFMessage.Multiline = true;
            tbFMessage.Name = "tbFMessage";
            tbFMessage.Size = new Size(471, 58);
            tbFMessage.TabIndex = 2;
            // 
            // btnFRecord
            // 
            btnFRecord.Location = new Point(145, 168);
            btnFRecord.Name = "btnFRecord";
            btnFRecord.Size = new Size(83, 43);
            btnFRecord.TabIndex = 3;
            btnFRecord.Text = "Записать";
            btnFRecord.UseVisualStyleBackColor = true;
            btnFRecord.Click += btnFRecord_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(234, 168);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(83, 43);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "Удалить";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // findBtn
            // 
            findBtn.Location = new Point(323, 168);
            findBtn.Name = "findBtn";
            findBtn.Size = new Size(83, 43);
            findBtn.TabIndex = 5;
            findBtn.Text = "Найти";
            findBtn.UseVisualStyleBackColor = true;
            findBtn.Click += findBtn_Click;
            // 
            // ChngBtn
            // 
            ChngBtn.Location = new Point(412, 168);
            ChngBtn.Name = "ChngBtn";
            ChngBtn.Size = new Size(83, 43);
            ChngBtn.TabIndex = 6;
            ChngBtn.Text = "Изменить";
            ChngBtn.UseVisualStyleBackColor = true;
            ChngBtn.Click += ChngBtn_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(516, 31);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(193, 244);
            listBox1.TabIndex = 7;
            // 
            // listBox2
            // 
            listBox2.FormattingEnabled = true;
            listBox2.ItemHeight = 15;
            listBox2.Location = new Point(715, 31);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(193, 244);
            listBox2.TabIndex = 8;
            // 
            // listBox3
            // 
            listBox3.FormattingEnabled = true;
            listBox3.ItemHeight = 15;
            listBox3.Location = new Point(914, 31);
            listBox3.Name = "listBox3";
            listBox3.Size = new Size(193, 244);
            listBox3.TabIndex = 9;
            // 
            // syntTree
            // 
            syntTree.Location = new Point(32, 281);
            syntTree.Name = "syntTree";
            syntTree.Size = new Size(471, 277);
            syntTree.TabIndex = 10;
            // 
            // structTree
            // 
            structTree.Location = new Point(516, 281);
            structTree.Name = "structTree";
            structTree.Size = new Size(591, 277);
            structTree.TabIndex = 11;
            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1121, 570);
            Controls.Add(structTree);
            Controls.Add(syntTree);
            Controls.Add(listBox3);
            Controls.Add(listBox2);
            Controls.Add(listBox1);
            Controls.Add(ChngBtn);
            Controls.Add(findBtn);
            Controls.Add(btnDelete);
            Controls.Add(btnFRecord);
            Controls.Add(tbFMessage);
            Controls.Add(btnFStart);
            Controls.Add(tbFSource);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbFSource;
        private Button btnFStart;
        private TextBox tbFMessage;
        private Button btnFRecord;
        private Button btnDelete;
        private Button findBtn;
        private Button ChngBtn;
        private ListBox listBox1;
        private ListBox listBox2;
        private ListBox listBox3;
        private TreeView syntTree;
        private TreeView structTree;
    }
}
