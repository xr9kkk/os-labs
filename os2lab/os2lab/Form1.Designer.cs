namespace os_lab2
{
    partial class Form1 : Form
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox txtDir1;
        private System.Windows.Forms.TextBox txtDir2;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblResult1;
        private System.Windows.Forms.Label lblResult2;
        private System.Windows.Forms.Label lblCompare;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtDir1 = new System.Windows.Forms.TextBox();
            this.txtDir2 = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblResult1 = new System.Windows.Forms.Label();
            this.lblResult2 = new System.Windows.Forms.Label();
            this.lblCompare = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // txtDir1
            this.txtDir1.Location = new System.Drawing.Point(120, 20);
            this.txtDir1.Name = "txtDir1";
            this.txtDir1.Size = new System.Drawing.Size(300, 20);
            this.txtDir1.TabIndex = 0;
            this.txtDir1.Text = @"C:\Test1";

            // txtDir2
            this.txtDir2.Location = new System.Drawing.Point(120, 50);
            this.txtDir2.Name = "txtDir2";
            this.txtDir2.Size = new System.Drawing.Size(300, 20);
            this.txtDir2.TabIndex = 1;
            this.txtDir2.Text = @"C:\Test2";

            // btnStart
            this.btnStart.Location = new System.Drawing.Point(20, 90);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 30);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Сравнить";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);

            // lblResult1
            this.lblResult1.AutoSize = true;
            this.lblResult1.Location = new System.Drawing.Point(20, 140);
            this.lblResult1.Name = "lblResult1";
            this.lblResult1.Size = new System.Drawing.Size(85, 13);
            this.lblResult1.TabIndex = 3;
            this.lblResult1.Text = "Каталог 1: ...";

            // lblResult2
            this.lblResult2.AutoSize = true;
            this.lblResult2.Location = new System.Drawing.Point(20, 170);
            this.lblResult2.Name = "lblResult2";
            this.lblResult2.Size = new System.Drawing.Size(85, 13);
            this.lblResult2.TabIndex = 4;
            this.lblResult2.Text = "Каталог 2: ...";

            // lblCompare
            this.lblCompare.AutoSize = true;
            this.lblCompare.Location = new System.Drawing.Point(20, 210);
            this.lblCompare.Name = "lblCompare";
            this.lblCompare.Size = new System.Drawing.Size(35, 13);
            this.lblCompare.TabIndex = 5;
            this.lblCompare.Text = "Итог:";

            // label1
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Каталог 1 путь:";

            // label2
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Каталог 2 путь:";

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 280);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblCompare);
            this.Controls.Add(this.lblResult2);
            this.Controls.Add(this.lblResult1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtDir2);
            this.Controls.Add(this.txtDir1);
            this.Name = "Form1";
            this.Text = "Сравнение объема файлов в каталогах";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}