namespace OS_Lab3
{
    partial class SnapshotForm
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
            buttonStart = new Button();
            textBoxTopCount = new TextBox();
            richTextBoxProcesses = new RichTextBox();
            label1 = new Label();
            labelTitle = new Label();
            SuspendLayout();
            // 
            // buttonStart
            // 
            buttonStart.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            buttonStart.Location = new Point(299, 26);
            buttonStart.Margin = new Padding(3, 4, 3, 4);
            buttonStart.Name = "buttonStart";
            buttonStart.Size = new Size(262, 80);
            buttonStart.TabIndex = 0;
            buttonStart.Text = "Найти процессы";
            buttonStart.UseVisualStyleBackColor = true;
            buttonStart.Click += buttonStart_Click;
            // 
            // textBoxTopCount
            // 
            textBoxTopCount.Location = new Point(14, 85);
            textBoxTopCount.Margin = new Padding(3, 4, 3, 4);
            textBoxTopCount.Name = "textBoxTopCount";
            textBoxTopCount.Size = new Size(157, 27);
            textBoxTopCount.TabIndex = 1;
            textBoxTopCount.Text = "10";
            // 
            // richTextBoxProcesses
            // 
            richTextBoxProcesses.Font = new Font("Consolas", 9F);
            richTextBoxProcesses.Location = new Point(14, 160);
            richTextBoxProcesses.Margin = new Padding(3, 4, 3, 4);
            richTextBoxProcesses.Name = "richTextBoxProcesses";
            richTextBoxProcesses.ReadOnly = true;
            richTextBoxProcesses.Size = new Size(453, 439);
            richTextBoxProcesses.TabIndex = 2;
            richTextBoxProcesses.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(14, 53);
            label1.Name = "label1";
            label1.Size = new Size(228, 28);
            label1.TabIndex = 3;
            label1.Text = "Количество процессов:";
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            labelTitle.Location = new Point(14, 12);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(190, 32);
            labelTitle.TabIndex = 4;
            labelTitle.Text = "Топ процессов";
            // 
            // SnapshotForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(620, 616);
            Controls.Add(labelTitle);
            Controls.Add(label1);
            Controls.Add(richTextBoxProcesses);
            Controls.Add(textBoxTopCount);
            Controls.Add(buttonStart);
            Margin = new Padding(3, 4, 3, 4);
            Name = "SnapshotForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Процессы с наибольшим размером модулей";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonStart;
        private TextBox textBoxTopCount;
        private RichTextBox richTextBoxProcesses;
        private Label label1;
        private Label labelTitle;
    }
}
