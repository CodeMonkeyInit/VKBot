namespace VKWordCounter.UI
{
    partial class MainForm
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
            this.wordToCountTextBox = new System.Windows.Forms.TextBox();
            this.dialogsList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SendToDialogCheckBox = new System.Windows.Forms.CheckBox();
            this.CountButton = new System.Windows.Forms.Button();
            this.CheckPeriodiclyButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.IntervalBetweenChecksNumeric = new System.Windows.Forms.NumericUpDown();
            this.ResultsRichTextBox = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.IntervalBetweenChecksNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // wordToCountTextBox
            // 
            this.wordToCountTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.wordToCountTextBox.Location = new System.Drawing.Point(7, 68);
            this.wordToCountTextBox.Name = "wordToCountTextBox";
            this.wordToCountTextBox.Size = new System.Drawing.Size(191, 26);
            this.wordToCountTextBox.TabIndex = 0;
            // 
            // dialogsList
            // 
            this.dialogsList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dialogsList.FormattingEnabled = true;
            this.dialogsList.Location = new System.Drawing.Point(8, 140);
            this.dialogsList.Name = "dialogsList";
            this.dialogsList.Size = new System.Drawing.Size(191, 28);
            this.dialogsList.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(7, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Слово для поиска";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(8, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Список Диалогов";
            // 
            // SendToDialogCheckBox
            // 
            this.SendToDialogCheckBox.AutoSize = true;
            this.SendToDialogCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SendToDialogCheckBox.Location = new System.Drawing.Point(12, 177);
            this.SendToDialogCheckBox.Name = "SendToDialogCheckBox";
            this.SendToDialogCheckBox.Size = new System.Drawing.Size(267, 24);
            this.SendToDialogCheckBox.TabIndex = 5;
            this.SendToDialogCheckBox.Text = "Отправить результат в диалог";
            this.SendToDialogCheckBox.UseVisualStyleBackColor = true;
            // 
            // CountButton
            // 
            this.CountButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CountButton.Location = new System.Drawing.Point(12, 211);
            this.CountButton.Name = "CountButton";
            this.CountButton.Size = new System.Drawing.Size(186, 48);
            this.CountButton.TabIndex = 6;
            this.CountButton.Text = "Считать";
            this.CountButton.UseVisualStyleBackColor = true;
            this.CountButton.Click += new System.EventHandler(this.CountButtonClick);
            // 
            // CheckPeriodiclyButton
            // 
            this.CheckPeriodiclyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CheckPeriodiclyButton.Location = new System.Drawing.Point(279, 211);
            this.CheckPeriodiclyButton.Name = "CheckPeriodiclyButton";
            this.CheckPeriodiclyButton.Size = new System.Drawing.Size(309, 48);
            this.CheckPeriodiclyButton.TabIndex = 7;
            this.CheckPeriodiclyButton.Text = "Переодически проверять";
            this.CheckPeriodiclyButton.UseVisualStyleBackColor = true;
            this.CheckPeriodiclyButton.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(275, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(256, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Промежутки проверки в минутах";
            this.label3.Visible = false;
            // 
            // IntervalBetweenChecksNumeric
            // 
            this.IntervalBetweenChecksNumeric.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.IntervalBetweenChecksNumeric.Location = new System.Drawing.Point(279, 168);
            this.IntervalBetweenChecksNumeric.Name = "IntervalBetweenChecksNumeric";
            this.IntervalBetweenChecksNumeric.Size = new System.Drawing.Size(309, 26);
            this.IntervalBetweenChecksNumeric.TabIndex = 9;
            this.IntervalBetweenChecksNumeric.Visible = false;
            // 
            // ResultsRichTextBox
            // 
            this.ResultsRichTextBox.BackColor = System.Drawing.Color.White;
            this.ResultsRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ResultsRichTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ResultsRichTextBox.Location = new System.Drawing.Point(279, 68);
            this.ResultsRichTextBox.Name = "ResultsRichTextBox";
            this.ResultsRichTextBox.ReadOnly = true;
            this.ResultsRichTextBox.Size = new System.Drawing.Size(309, 191);
            this.ResultsRichTextBox.TabIndex = 10;
            this.ResultsRichTextBox.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(279, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "Результат";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 290);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ResultsRichTextBox);
            this.Controls.Add(this.IntervalBetweenChecksNumeric);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CheckPeriodiclyButton);
            this.Controls.Add(this.CountButton);
            this.Controls.Add(this.SendToDialogCheckBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dialogsList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.wordToCountTextBox);
            this.Name = "MainForm";
            this.Text = "Main";
            ((System.ComponentModel.ISupportInitialize)(this.IntervalBetweenChecksNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox wordToCountTextBox;
        private System.Windows.Forms.ComboBox dialogsList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox SendToDialogCheckBox;
        private System.Windows.Forms.Button CountButton;
        private System.Windows.Forms.Button CheckPeriodiclyButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown IntervalBetweenChecksNumeric;
        private System.Windows.Forms.RichTextBox ResultsRichTextBox;
        private System.Windows.Forms.Label label4;
    }
}