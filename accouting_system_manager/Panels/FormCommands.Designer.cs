namespace accouting_system_manager
{
    partial class FormCommands
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNextMonth = new System.Windows.Forms.Button();
            this.btnPreviousMonth = new System.Windows.Forms.Button();
            this.dtPickerTo = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtPickerFrom = new System.Windows.Forms.DateTimePicker();
            this.prgBarre = new System.Windows.Forms.ProgressBar();
            this.lblInfo = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pnlRemoveByMinMax = new System.Windows.Forms.Panel();
            this.btnRemoveByMinMax = new System.Windows.Forms.Button();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.rdbtnMinMax = new System.Windows.Forms.RadioButton();
            this.pnlRemoveByPercent = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.numUpDownPercent = new System.Windows.Forms.NumericUpDown();
            this.btnRemoveByPercent = new System.Windows.Forms.Button();
            this.rdbtnPercent = new System.Windows.Forms.RadioButton();
            this.btnRecover = new System.Windows.Forms.Button();
            this.lstViewOrders = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnlRemoveByMinMax.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.pnlRemoveByPercent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownPercent)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnNextMonth);
            this.panel1.Controls.Add(this.btnPreviousMonth);
            this.panel1.Controls.Add(this.dtPickerTo);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.dtPickerFrom);
            this.panel1.Controls.Add(this.prgBarre);
            this.panel1.Controls.Add(this.lblInfo);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.btnRecover);
            this.panel1.Controls.Add(this.lstViewOrders);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(468, 434);
            this.panel1.TabIndex = 0;
            // 
            // btnNextMonth
            // 
            this.btnNextMonth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextMonth.BackgroundImage = global::accouting_system_manager.Properties.Resources.next;
            this.btnNextMonth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNextMonth.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNextMonth.Location = new System.Drawing.Point(411, 3);
            this.btnNextMonth.Name = "btnNextMonth";
            this.btnNextMonth.Size = new System.Drawing.Size(50, 50);
            this.btnNextMonth.TabIndex = 18;
            this.btnNextMonth.UseVisualStyleBackColor = true;
            this.btnNextMonth.Click += new System.EventHandler(this.btnNextMonth_Click);
            // 
            // btnPreviousMonth
            // 
            this.btnPreviousMonth.BackgroundImage = global::accouting_system_manager.Properties.Resources.back;
            this.btnPreviousMonth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPreviousMonth.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPreviousMonth.Location = new System.Drawing.Point(3, 4);
            this.btnPreviousMonth.Name = "btnPreviousMonth";
            this.btnPreviousMonth.Size = new System.Drawing.Size(50, 50);
            this.btnPreviousMonth.TabIndex = 17;
            this.btnPreviousMonth.UseVisualStyleBackColor = true;
            this.btnPreviousMonth.Click += new System.EventHandler(this.btnPreviousMonth_Click);
            // 
            // dtPickerTo
            // 
            this.dtPickerTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtPickerTo.Location = new System.Drawing.Point(140, 30);
            this.dtPickerTo.Name = "dtPickerTo";
            this.dtPickerTo.Size = new System.Drawing.Size(200, 20);
            this.dtPickerTo.TabIndex = 16;
            this.dtPickerTo.ValueChanged += new System.EventHandler(this.dtPickerTo_ValueChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(103, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 16);
            this.label4.TabIndex = 15;
            this.label4.Text = "To :";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(89, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "From :";
            // 
            // dtPickerFrom
            // 
            this.dtPickerFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtPickerFrom.Location = new System.Drawing.Point(140, 4);
            this.dtPickerFrom.Name = "dtPickerFrom";
            this.dtPickerFrom.Size = new System.Drawing.Size(200, 20);
            this.dtPickerFrom.TabIndex = 13;
            this.dtPickerFrom.ValueChanged += new System.EventHandler(this.dtPickerFrom_ValueChanged);
            // 
            // prgBarre
            // 
            this.prgBarre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgBarre.Location = new System.Drawing.Point(19, 144);
            this.prgBarre.Name = "prgBarre";
            this.prgBarre.Size = new System.Drawing.Size(373, 23);
            this.prgBarre.TabIndex = 12;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.Location = new System.Drawing.Point(17, 128);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(45, 16);
            this.lblInfo.TabIndex = 10;
            this.lblInfo.Text = "label3";
            this.lblInfo.Visible = false;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.pnlRemoveByMinMax);
            this.panel3.Controls.Add(this.rdbtnMinMax);
            this.panel3.Controls.Add(this.pnlRemoveByPercent);
            this.panel3.Controls.Add(this.rdbtnPercent);
            this.panel3.Location = new System.Drawing.Point(2, 56);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(462, 71);
            this.panel3.TabIndex = 9;
            // 
            // pnlRemoveByMinMax
            // 
            this.pnlRemoveByMinMax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlRemoveByMinMax.Controls.Add(this.btnRemoveByMinMax);
            this.pnlRemoveByMinMax.Controls.Add(this.numericUpDown2);
            this.pnlRemoveByMinMax.Controls.Add(this.label2);
            this.pnlRemoveByMinMax.Controls.Add(this.numericUpDown1);
            this.pnlRemoveByMinMax.Location = new System.Drawing.Point(100, 34);
            this.pnlRemoveByMinMax.Name = "pnlRemoveByMinMax";
            this.pnlRemoveByMinMax.Size = new System.Drawing.Size(359, 34);
            this.pnlRemoveByMinMax.TabIndex = 10;
            // 
            // btnRemoveByMinMax
            // 
            this.btnRemoveByMinMax.Location = new System.Drawing.Point(273, 3);
            this.btnRemoveByMinMax.Name = "btnRemoveByMinMax";
            this.btnRemoveByMinMax.Size = new System.Drawing.Size(83, 23);
            this.btnRemoveByMinMax.TabIndex = 7;
            this.btnRemoveByMinMax.Text = "Remove";
            this.btnRemoveByMinMax.UseVisualStyleBackColor = true;
            this.btnRemoveByMinMax.Click += new System.EventHandler(this.btnRemoveByMinMax_Click);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.BackColor = System.Drawing.SystemColors.Window;
            this.numericUpDown2.Location = new System.Drawing.Point(147, 5);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown2.TabIndex = 6;
            this.numericUpDown2.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(129, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "/";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(3, 5);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            19999,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 5;
            this.numericUpDown1.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            // 
            // rdbtnMinMax
            // 
            this.rdbtnMinMax.AutoSize = true;
            this.rdbtnMinMax.Location = new System.Drawing.Point(16, 39);
            this.rdbtnMinMax.Name = "rdbtnMinMax";
            this.rdbtnMinMax.Size = new System.Drawing.Size(80, 17);
            this.rdbtnMinMax.TabIndex = 4;
            this.rdbtnMinMax.Text = "By min/max";
            this.rdbtnMinMax.UseVisualStyleBackColor = true;
            this.rdbtnMinMax.CheckedChanged += new System.EventHandler(this.rdbtnMinMax_CheckedChanged);
            // 
            // pnlRemoveByPercent
            // 
            this.pnlRemoveByPercent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlRemoveByPercent.Controls.Add(this.label1);
            this.pnlRemoveByPercent.Controls.Add(this.numUpDownPercent);
            this.pnlRemoveByPercent.Controls.Add(this.btnRemoveByPercent);
            this.pnlRemoveByPercent.Location = new System.Drawing.Point(100, 3);
            this.pnlRemoveByPercent.Name = "pnlRemoveByPercent";
            this.pnlRemoveByPercent.Size = new System.Drawing.Size(359, 30);
            this.pnlRemoveByPercent.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(129, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "%";
            // 
            // numUpDownPercent
            // 
            this.numUpDownPercent.Location = new System.Drawing.Point(3, 5);
            this.numUpDownPercent.Name = "numUpDownPercent";
            this.numUpDownPercent.Size = new System.Drawing.Size(120, 20);
            this.numUpDownPercent.TabIndex = 2;
            this.numUpDownPercent.Value = new decimal(new int[] {
            85,
            0,
            0,
            0});
            this.numUpDownPercent.ValueChanged += new System.EventHandler(this.numUpDownPercent_ValueChanged);
            // 
            // btnRemoveByPercent
            // 
            this.btnRemoveByPercent.Location = new System.Drawing.Point(155, 3);
            this.btnRemoveByPercent.Name = "btnRemoveByPercent";
            this.btnRemoveByPercent.Size = new System.Drawing.Size(83, 23);
            this.btnRemoveByPercent.TabIndex = 3;
            this.btnRemoveByPercent.Text = "Remove";
            this.btnRemoveByPercent.UseVisualStyleBackColor = true;
            this.btnRemoveByPercent.Click += new System.EventHandler(this.btnRemoveByPercent_Click);
            // 
            // rdbtnPercent
            // 
            this.rdbtnPercent.AutoSize = true;
            this.rdbtnPercent.Checked = true;
            this.rdbtnPercent.Location = new System.Drawing.Point(16, 10);
            this.rdbtnPercent.Name = "rdbtnPercent";
            this.rdbtnPercent.Size = new System.Drawing.Size(77, 17);
            this.rdbtnPercent.TabIndex = 1;
            this.rdbtnPercent.TabStop = true;
            this.rdbtnPercent.Text = "By Percent";
            this.rdbtnPercent.UseVisualStyleBackColor = true;
            this.rdbtnPercent.CheckedChanged += new System.EventHandler(this.rdbtnPercent_CheckedChanged);
            // 
            // btnRecover
            // 
            this.btnRecover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRecover.BackgroundImage = global::accouting_system_manager.Properties.Resources.icons8_Reset_96;
            this.btnRecover.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRecover.Enabled = false;
            this.btnRecover.Location = new System.Drawing.Point(398, 171);
            this.btnRecover.Name = "btnRecover";
            this.btnRecover.Size = new System.Drawing.Size(49, 48);
            this.btnRecover.TabIndex = 8;
            this.btnRecover.UseVisualStyleBackColor = true;
            this.btnRecover.Click += new System.EventHandler(this.btnRecover_Click);
            // 
            // lstViewOrders
            // 
            this.lstViewOrders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstViewOrders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader4});
            this.lstViewOrders.FullRowSelect = true;
            this.lstViewOrders.GridLines = true;
            this.lstViewOrders.Location = new System.Drawing.Point(20, 173);
            this.lstViewOrders.Name = "lstViewOrders";
            this.lstViewOrders.Size = new System.Drawing.Size(373, 251);
            this.lstViewOrders.TabIndex = 0;
            this.lstViewOrders.UseCompatibleStateImageBehavior = false;
            this.lstViewOrders.View = System.Windows.Forms.View.Details;
            this.lstViewOrders.SelectedIndexChanged += new System.EventHandler(this.lstViewOrders_SelectedIndexChanged);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Invoice number";
            this.columnHeader5.Width = 155;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Date";
            this.columnHeader4.Width = 212;
            // 
            // bgWorker
            // 
            this.bgWorker.WorkerReportsProgress = true;
            // 
            // FormCommands
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 434);
            this.Controls.Add(this.panel1);
            this.Name = "FormCommands";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.pnlRemoveByMinMax.ResumeLayout(false);
            this.pnlRemoveByMinMax.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.pnlRemoveByPercent.ResumeLayout(false);
            this.pnlRemoveByPercent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownPercent)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView lstViewOrders;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button btnRecover;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numUpDownPercent;
        private System.Windows.Forms.Button btnRemoveByPercent;
        private System.Windows.Forms.Panel pnlRemoveByPercent;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel pnlRemoveByMinMax;
        private System.Windows.Forms.RadioButton rdbtnMinMax;
        private System.Windows.Forms.RadioButton rdbtnPercent;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button btnRemoveByMinMax;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.DateTimePicker dtPickerTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtPickerFrom;
        private System.Windows.Forms.ProgressBar prgBarre;
        private System.ComponentModel.BackgroundWorker bgWorker;
        private System.Windows.Forms.Button btnNextMonth;
        private System.Windows.Forms.Button btnPreviousMonth;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}

