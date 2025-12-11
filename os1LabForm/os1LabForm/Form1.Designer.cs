namespace os1LabForm
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button pauseAllButton;
        private System.Windows.Forms.Button spawnNowButton;
        private System.Windows.Forms.Button clearLogButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown writerItemsNumeric;
        private System.Windows.Forms.NumericUpDown readerItemsNumeric;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown spawnIntervalNumeric;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer spawnTimer;
        private System.Windows.Forms.Timer updateTimer;

        // Новые элементы
        private System.Windows.Forms.ListBox workersListBox;
        private System.Windows.Forms.ListBox buffersListBox;
        private System.Windows.Forms.GroupBox groupBox5;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            logTextBox = new TextBox();
            statusLabel = new Label();
            startButton = new Button();
            stopButton = new Button();
            pauseAllButton = new Button();
            spawnNowButton = new Button();
            clearLogButton = new Button();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            groupBox3 = new GroupBox();
            buttonAddWriter = new Button();
            label3 = new Label();
            spawnIntervalNumeric = new NumericUpDown();
            label2 = new Label();
            label1 = new Label();
            readerItemsNumeric = new NumericUpDown();
            writerItemsNumeric = new NumericUpDown();
            spawnTimer = new System.Windows.Forms.Timer(components);
            updateTimer = new System.Windows.Forms.Timer(components);
            workersListBox = new ListBox();
            buffersListBox = new ListBox();
            groupBox5 = new GroupBox();
            buttonAddReader = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)spawnIntervalNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)readerItemsNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)writerItemsNumeric).BeginInit();
            groupBox5.SuspendLayout();
            SuspendLayout();
            // 
            // logTextBox
            // 
            logTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            logTextBox.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            logTextBox.Location = new Point(5, 20);
            logTextBox.Multiline = true;
            logTextBox.Name = "logTextBox";
            logTextBox.ReadOnly = true;
            logTextBox.ScrollBars = ScrollBars.Vertical;
            logTextBox.Size = new Size(672, 235);
            logTextBox.TabIndex = 0;
            // 
            // statusLabel
            // 
            statusLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            statusLabel.BorderStyle = BorderStyle.FixedSingle;
            statusLabel.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 204);
            statusLabel.Location = new Point(5, 20);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(672, 38);
            statusLabel.TabIndex = 1;
            statusLabel.Text = "Состояние: Ожидание запуска...";
            statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // startButton
            // 
            startButton.BackColor = Color.Transparent;
            startButton.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            startButton.Location = new Point(5, 20);
            startButton.Name = "startButton";
            startButton.Size = new Size(88, 33);
            startButton.TabIndex = 2;
            startButton.Text = "Старт";
            startButton.UseVisualStyleBackColor = false;
            startButton.Click += StartButton_Click;
            // 
            // stopButton
            // 
            stopButton.BackColor = Color.White;
            stopButton.Enabled = false;
            stopButton.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            stopButton.Location = new Point(98, 20);
            stopButton.Name = "stopButton";
            stopButton.Size = new Size(88, 33);
            stopButton.TabIndex = 3;
            stopButton.Text = "Стоп";
            stopButton.UseVisualStyleBackColor = false;
            stopButton.Click += StopButton_Click;
            // 
            // pauseAllButton
            // 
            pauseAllButton.BackColor = Color.White;
            pauseAllButton.Enabled = false;
            pauseAllButton.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            pauseAllButton.Location = new Point(191, 20);
            pauseAllButton.Name = "pauseAllButton";
            pauseAllButton.Size = new Size(131, 33);
            pauseAllButton.TabIndex = 4;
            pauseAllButton.Text = "Пауза всех читателей";
            pauseAllButton.UseVisualStyleBackColor = false;
            pauseAllButton.Click += PauseAllButton_Click;
            // 
            // spawnNowButton
            // 
            spawnNowButton.BackColor = Color.White;
            spawnNowButton.Enabled = false;
            spawnNowButton.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            spawnNowButton.Location = new Point(327, 20);
            spawnNowButton.Name = "spawnNowButton";
            spawnNowButton.Size = new Size(131, 33);
            spawnNowButton.TabIndex = 5;
            spawnNowButton.Text = "Создать поток сейчас";
            spawnNowButton.UseVisualStyleBackColor = false;
            spawnNowButton.Click += SpawnNowButton_Click;
            // 
            // clearLogButton
            // 
            clearLogButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            clearLogButton.BackColor = SystemColors.Control;
            clearLogButton.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 204);
            clearLogButton.Location = new Point(590, 260);
            clearLogButton.Name = "clearLogButton";
            clearLogButton.Size = new Size(88, 23);
            clearLogButton.TabIndex = 7;
            clearLogButton.Text = "Очистить лог";
            clearLogButton.UseVisualStyleBackColor = false;
            clearLogButton.Click += ClearLogButton_Click;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(logTextBox);
            groupBox1.Controls.Add(clearLogButton);
            groupBox1.Location = new Point(10, 11);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(682, 291);
            groupBox1.TabIndex = 14;
            groupBox1.TabStop = false;
            groupBox1.Text = "Лог операций";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(statusLabel);
            groupBox2.Location = new Point(10, 308);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(682, 66);
            groupBox2.TabIndex = 15;
            groupBox2.TabStop = false;
            groupBox2.Text = "Общий статус системы";
            // 
            // groupBox3
            // 
            groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox3.Controls.Add(buttonAddReader);
            groupBox3.Controls.Add(buttonAddWriter);
            groupBox3.Controls.Add(startButton);
            groupBox3.Controls.Add(stopButton);
            groupBox3.Controls.Add(pauseAllButton);
            groupBox3.Controls.Add(spawnNowButton);
            groupBox3.Controls.Add(label3);
            groupBox3.Controls.Add(spawnIntervalNumeric);
            groupBox3.Controls.Add(label2);
            groupBox3.Controls.Add(label1);
            groupBox3.Controls.Add(readerItemsNumeric);
            groupBox3.Controls.Add(writerItemsNumeric);
            groupBox3.Location = new Point(10, 379);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(682, 112);
            groupBox3.TabIndex = 16;
            groupBox3.TabStop = false;
            groupBox3.Text = "Управление системой";
            // 
            // buttonAddWriter
            // 
            buttonAddWriter.Location = new Point(328, 54);
            buttonAddWriter.Name = "buttonAddWriter";
            buttonAddWriter.Size = new Size(130, 43);
            buttonAddWriter.TabIndex = 14;
            buttonAddWriter.Text = "Добавить писателя";
            buttonAddWriter.UseVisualStyleBackColor = true;
            buttonAddWriter.Click += buttonAddWriter_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(464, 82);
            label3.Name = "label3";
            label3.Size = new Size(63, 15);
            label3.TabIndex = 13;
            label3.Text = "Интервал:";
            // 
            // spawnIntervalNumeric
            // 
            spawnIntervalNumeric.Increment = new decimal(new int[] { 500, 0, 0, 0 });
            spawnIntervalNumeric.Location = new Point(525, 80);
            spawnIntervalNumeric.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            spawnIntervalNumeric.Minimum = new decimal(new int[] { 1000, 0, 0, 0 });
            spawnIntervalNumeric.Name = "spawnIntervalNumeric";
            spawnIntervalNumeric.Size = new Size(70, 23);
            spawnIntervalNumeric.TabIndex = 12;
            spawnIntervalNumeric.Value = new decimal(new int[] { 3000, 0, 0, 0 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(464, 54);
            label2.Name = "label2";
            label2.Size = new Size(60, 15);
            label2.TabIndex = 11;
            label2.Text = "Читатель:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(464, 26);
            label1.Name = "label1";
            label1.Size = new Size(62, 15);
            label1.TabIndex = 10;
            label1.Text = "Писатель:";
            // 
            // readerItemsNumeric
            // 
            readerItemsNumeric.Location = new Point(525, 52);
            readerItemsNumeric.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            readerItemsNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            readerItemsNumeric.Name = "readerItemsNumeric";
            readerItemsNumeric.Size = new Size(70, 23);
            readerItemsNumeric.TabIndex = 9;
            readerItemsNumeric.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // writerItemsNumeric
            // 
            writerItemsNumeric.Location = new Point(525, 23);
            writerItemsNumeric.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            writerItemsNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            writerItemsNumeric.Name = "writerItemsNumeric";
            writerItemsNumeric.Size = new Size(70, 23);
            writerItemsNumeric.TabIndex = 8;
            writerItemsNumeric.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // spawnTimer
            // 
            spawnTimer.Interval = 3000;
            spawnTimer.Tick += SpawnRandomThread;
            // 
            // updateTimer
            // 
            updateTimer.Interval = 500;
            updateTimer.Tick += UpdateTimer_Tick;
            // 
            // workersListBox
            // 
            workersListBox.Dock = DockStyle.Fill;
            workersListBox.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            workersListBox.FormattingEnabled = true;
            workersListBox.ItemHeight = 18;
            workersListBox.Location = new Point(3, 23);
            workersListBox.Name = "workersListBox";
            workersListBox.Size = new Size(774, 130);
            workersListBox.TabIndex = 0;
            // 
            // buffersListBox
            // 
            buffersListBox.Dock = DockStyle.Fill;
            buffersListBox.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            buffersListBox.FormattingEnabled = true;
            buffersListBox.ItemHeight = 14;
            buffersListBox.Location = new Point(3, 19);
            buffersListBox.Margin = new Padding(3, 2, 3, 2);
            buffersListBox.Name = "buffersListBox";
            buffersListBox.Size = new Size(676, 95);
            buffersListBox.TabIndex = 0;
            // 
            // groupBox5
            // 
            groupBox5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox5.Controls.Add(buffersListBox);
            groupBox5.Location = new Point(15, 497);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(682, 117);
            groupBox5.TabIndex = 18;
            groupBox5.TabStop = false;
            groupBox5.Text = "Активные буферы";
            // 
            // buttonAddReader
            // 
            buttonAddReader.Location = new Point(191, 56);
            buttonAddReader.Name = "buttonAddReader";
            buttonAddReader.Size = new Size(131, 41);
            buttonAddReader.TabIndex = 15;
            buttonAddReader.Text = "Добавить читателя";
            buttonAddReader.UseVisualStyleBackColor = true;
            buttonAddReader.Click += buttonAddReader_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(704, 747);
            Controls.Add(groupBox5);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            MinimumSize = new Size(720, 670);
            Name = "Form1";
            Text = "Producer-Consumer: Независимые потоки с общими буферами";
            FormClosing += MainForm_FormClosing;
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)spawnIntervalNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)readerItemsNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)writerItemsNumeric).EndInit();
            groupBox5.ResumeLayout(false);
            ResumeLayout(false);
        }
        private Button buttonAddWriter;
        private Button buttonAddReader;
    }
}