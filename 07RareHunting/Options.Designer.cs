namespace _07RareHunting
{
    partial class Options
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
            this.label1 = new System.Windows.Forms.Label();
            this.alwaysNameText = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.flashRadio = new System.Windows.Forms.RadioButton();
            this.frontRadio = new System.Windows.Forms.RadioButton();
            this.ontopCheck = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Always use this name:";
            // 
            // alwaysNameText
            // 
            this.alwaysNameText.Location = new System.Drawing.Point(126, 10);
            this.alwaysNameText.Name = "alwaysNameText";
            this.alwaysNameText.Size = new System.Drawing.Size(100, 20);
            this.alwaysNameText.TabIndex = 1;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveButton.Location = new System.Drawing.Point(12, 131);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(151, 131);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(9, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Periodic Check:";
            // 
            // flashRadio
            // 
            this.flashRadio.AutoSize = true;
            this.flashRadio.Enabled = false;
            this.flashRadio.Location = new System.Drawing.Point(126, 44);
            this.flashRadio.Name = "flashRadio";
            this.flashRadio.Size = new System.Drawing.Size(89, 17);
            this.flashRadio.TabIndex = 5;
            this.flashRadio.TabStop = true;
            this.flashRadio.Text = "Taskbar flash";
            this.flashRadio.UseVisualStyleBackColor = true;
            // 
            // frontRadio
            // 
            this.frontRadio.AutoSize = true;
            this.frontRadio.Enabled = false;
            this.frontRadio.Location = new System.Drawing.Point(126, 68);
            this.frontRadio.Name = "frontRadio";
            this.frontRadio.Size = new System.Drawing.Size(85, 17);
            this.frontRadio.TabIndex = 6;
            this.frontRadio.TabStop = true;
            this.frontRadio.Text = "Bring to front";
            this.frontRadio.UseVisualStyleBackColor = true;
            // 
            // ontopCheck
            // 
            this.ontopCheck.AutoSize = true;
            this.ontopCheck.Location = new System.Drawing.Point(126, 91);
            this.ontopCheck.Name = "ontopCheck";
            this.ontopCheck.Size = new System.Drawing.Size(15, 14);
            this.ontopCheck.TabIndex = 8;
            this.ontopCheck.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Always on top:";
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 166);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ontopCheck);
            this.Controls.Add(this.frontRadio);
            this.Controls.Add(this.flashRadio);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.alwaysNameText);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Options";
            this.ShowIcon = false;
            this.Text = "Options";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox alwaysNameText;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton flashRadio;
        private System.Windows.Forms.RadioButton frontRadio;
        private System.Windows.Forms.CheckBox ontopCheck;
        private System.Windows.Forms.Label label3;
    }
}