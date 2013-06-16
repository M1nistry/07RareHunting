namespace _07RareHunting
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.stripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.stripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.spawnDGV = new System.Windows.Forms.DataGridView();
            this.SpawnNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Occupied = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Names = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.spawnCombo = new System.Windows.Forms.ComboBox();
            this.activeCheck = new System.Windows.Forms.CheckBox();
            this.updateButton = new System.Windows.Forms.Button();
            this.tT = new System.Windows.Forms.ToolTip(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.optionsButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.timerLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.activeTimer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spawnDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stripLabel,
            this.toolStripStatusLabel1,
            this.stripProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 518);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(272, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // stripLabel
            // 
            this.stripLabel.Name = "stripLabel";
            this.stripLabel.Size = new System.Drawing.Size(55, 17);
            this.stripLabel.Text = "In-Active";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Enabled = false;
            this.toolStripStatusLabel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel1.Text = "|";
            this.toolStripStatusLabel1.ToolTipText = "|";
            // 
            // stripProgressBar
            // 
            this.stripProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.stripProgressBar.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.stripProgressBar.Name = "stripProgressBar";
            this.stripProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // spawnDGV
            // 
            this.spawnDGV.AllowUserToAddRows = false;
            this.spawnDGV.AllowUserToDeleteRows = false;
            this.spawnDGV.AllowUserToResizeColumns = false;
            this.spawnDGV.AllowUserToResizeRows = false;
            this.spawnDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spawnDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.spawnDGV.BackgroundColor = System.Drawing.SystemColors.Control;
            this.spawnDGV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.spawnDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.spawnDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SpawnNumber,
            this.Occupied,
            this.Names});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.spawnDGV.DefaultCellStyle = dataGridViewCellStyle2;
            this.spawnDGV.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.spawnDGV.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.spawnDGV.Location = new System.Drawing.Point(0, 0);
            this.spawnDGV.MultiSelect = false;
            this.spawnDGV.Name = "spawnDGV";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.spawnDGV.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.spawnDGV.RowHeadersVisible = false;
            this.spawnDGV.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.spawnDGV.Size = new System.Drawing.Size(272, 418);
            this.spawnDGV.StandardTab = true;
            this.spawnDGV.TabIndex = 1;
            this.spawnDGV.TabStop = false;
            // 
            // SpawnNumber
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.SpawnNumber.DefaultCellStyle = dataGridViewCellStyle1;
            this.SpawnNumber.FillWeight = 60.9137F;
            this.SpawnNumber.HeaderText = "Spawn";
            this.SpawnNumber.Name = "SpawnNumber";
            this.SpawnNumber.ReadOnly = true;
            this.SpawnNumber.ToolTipText = "Spawn number from list";
            // 
            // Occupied
            // 
            this.Occupied.FillWeight = 64.34736F;
            this.Occupied.HeaderText = "Occupied";
            this.Occupied.Name = "Occupied";
            this.Occupied.ReadOnly = true;
            // 
            // Names
            // 
            this.Names.FillWeight = 174.7389F;
            this.Names.HeaderText = "Names";
            this.Names.Name = "Names";
            this.Names.ReadOnly = true;
            this.Names.ToolTipText = "Names of characters currently at this location";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 430);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // nameBox
            // 
            this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nameBox.Location = new System.Drawing.Point(56, 427);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(100, 20);
            this.nameBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 457);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Spawn:";
            // 
            // spawnCombo
            // 
            this.spawnCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.spawnCombo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.spawnCombo.FormattingEnabled = true;
            this.spawnCombo.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59",
            "60",
            "61",
            "62",
            "63",
            "64",
            "65",
            "66",
            "67",
            "68",
            "69",
            "70",
            "71",
            "72",
            "73",
            "74",
            "75",
            "76",
            "77",
            "78",
            "79",
            "80",
            "81",
            "82",
            "83",
            "84",
            "85",
            "86",
            "87",
            "88",
            "89",
            "90",
            "91",
            "92",
            "93",
            "94",
            "95",
            "96",
            "97",
            "98",
            "99",
            "100",
            "101",
            "102",
            "103",
            "104",
            "105",
            "106",
            "107",
            "108",
            "109",
            "110",
            "111",
            "112",
            "113",
            "114",
            "115",
            "116",
            "117",
            "118",
            "119"});
            this.spawnCombo.Location = new System.Drawing.Point(56, 454);
            this.spawnCombo.MaxDropDownItems = 15;
            this.spawnCombo.Name = "spawnCombo";
            this.spawnCombo.Size = new System.Drawing.Size(100, 21);
            this.spawnCombo.TabIndex = 5;
            this.spawnCombo.Text = "1";
            // 
            // activeCheck
            // 
            this.activeCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.activeCheck.AutoSize = true;
            this.activeCheck.Location = new System.Drawing.Point(172, 429);
            this.activeCheck.Name = "activeCheck";
            this.activeCheck.Size = new System.Drawing.Size(56, 17);
            this.activeCheck.TabIndex = 6;
            this.activeCheck.Text = "Active";
            this.tT.SetToolTip(this.activeCheck, "Check if you\'re actively camping");
            this.activeCheck.UseVisualStyleBackColor = true;
            // 
            // updateButton
            // 
            this.updateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.updateButton.Location = new System.Drawing.Point(172, 452);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(75, 23);
            this.updateButton.TabIndex = 7;
            this.updateButton.Text = "Update";
            this.tT.SetToolTip(this.updateButton, "Click to update your name and spawn");
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipTitle = "OSRS Rare spawn tool";
            this.notifyIcon1.Text = "Runescape Rare Spawn Tool";
            this.notifyIcon1.Visible = true;
            // 
            // optionsButton
            // 
            this.optionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsButton.Location = new System.Drawing.Point(172, 481);
            this.optionsButton.Name = "optionsButton";
            this.optionsButton.Size = new System.Drawing.Size(75, 23);
            this.optionsButton.TabIndex = 8;
            this.optionsButton.Text = "Options";
            this.optionsButton.UseVisualStyleBackColor = true;
            this.optionsButton.Click += new System.EventHandler(this.optionsButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(197, 395);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timerLabel
            // 
            this.timerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.timerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timerLabel.Location = new System.Drawing.Point(101, 482);
            this.timerLabel.Name = "timerLabel";
            this.timerLabel.Size = new System.Drawing.Size(55, 18);
            this.timerLabel.TabIndex = 170;
            this.timerLabel.Text = "0:00";
            this.timerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tT.SetToolTip(this.timerLabel, "Time until active check");
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 486);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 171;
            this.label3.Text = "Active for:";
            // 
            // activeTimer
            // 
            this.activeTimer.Interval = 1000;
            this.activeTimer.Tick += new System.EventHandler(this.activeTimer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(272, 540);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.timerLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.optionsButton);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.activeCheck);
            this.Controls.Add(this.spawnCombo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.spawnDGV);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Runescape Rare Spawn Tool";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spawnDGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar stripProgressBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolTip tT;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        public System.Windows.Forms.CheckBox activeCheck;
        public System.Windows.Forms.ToolStripStatusLabel stripLabel;
        public System.Windows.Forms.TextBox nameBox;
        public System.Windows.Forms.ComboBox spawnCombo;
        private System.Windows.Forms.DataGridViewTextBoxColumn SpawnNumber;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Occupied;
        private System.Windows.Forms.DataGridViewTextBoxColumn Names;
        public System.Windows.Forms.DataGridView spawnDGV;
        private System.Windows.Forms.Button optionsButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label timerLabel;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Timer activeTimer;

    }
}

