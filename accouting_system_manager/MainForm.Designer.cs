namespace accouting_system_manager
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
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.lblCa = new System.Windows.Forms.Label();
            this.btnConfigureDB = new accouting_system_manager.Components.MenuItem();
            this.btnRemoveInvoices = new accouting_system_manager.Components.MenuItem();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.pnlButtons.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlButtons.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pnlButtons.Controls.Add(this.lblCa);
            this.pnlButtons.Controls.Add(this.btnConfigureDB);
            this.pnlButtons.Controls.Add(this.btnRemoveInvoices);
            this.pnlButtons.Location = new System.Drawing.Point(0, 1);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(173, 455);
            this.pnlButtons.TabIndex = 7;
            // 
            // lblCa
            // 
            this.lblCa.AutoSize = true;
            this.lblCa.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCa.Location = new System.Drawing.Point(9, 8);
            this.lblCa.Name = "lblCa";
            this.lblCa.Size = new System.Drawing.Size(38, 17);
            this.lblCa.TabIndex = 2;
            this.lblCa.Text = "CA :";
            // 
            // btnConfigureDB
            // 
            this.btnConfigureDB.ActiveImage = global::accouting_system_manager.Properties.Resources.db_config_hover;
            this.btnConfigureDB.BackColor = System.Drawing.Color.Transparent;
            this.btnConfigureDB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfigureDB.Image = global::accouting_system_manager.Properties.Resources.db_config;
            this.btnConfigureDB.ItemActiveColor = System.Drawing.SystemColors.Highlight;
            this.btnConfigureDB.ItemColor = System.Drawing.Color.Empty;
            this.btnConfigureDB.ItemText = "Database";
            this.btnConfigureDB.Location = new System.Drawing.Point(4, 122);
            this.btnConfigureDB.Name = "btnConfigureDB";
            this.btnConfigureDB.SelectedBackgroundColor = System.Drawing.SystemColors.Control;
            this.btnConfigureDB.Size = new System.Drawing.Size(170, 66);
            this.btnConfigureDB.TabIndex = 1;
            this.btnConfigureDB.onItemClicked += new accouting_system_manager.Components.MenuItem.OnItemClicked(this.btnConfigureDB_onItemClicked);
            // 
            // btnRemoveInvoices
            // 
            this.btnRemoveInvoices.ActiveImage = global::accouting_system_manager.Properties.Resources.remove_invoice_hover;
            this.btnRemoveInvoices.BackColor = System.Drawing.Color.Transparent;
            this.btnRemoveInvoices.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRemoveInvoices.Image = global::accouting_system_manager.Properties.Resources.remove_invoice;
            this.btnRemoveInvoices.ItemActiveColor = System.Drawing.SystemColors.Highlight;
            this.btnRemoveInvoices.ItemColor = System.Drawing.Color.Empty;
            this.btnRemoveInvoices.ItemText = "Remove Invoices";
            this.btnRemoveInvoices.Location = new System.Drawing.Point(3, 50);
            this.btnRemoveInvoices.Name = "btnRemoveInvoices";
            this.btnRemoveInvoices.SelectedBackgroundColor = System.Drawing.SystemColors.Control;
            this.btnRemoveInvoices.Size = new System.Drawing.Size(170, 66);
            this.btnRemoveInvoices.TabIndex = 0;
            this.btnRemoveInvoices.onItemClicked += new accouting_system_manager.Components.MenuItem.OnItemClicked(this.btnRemoveInvoices_onItemClicked);
            // 
            // pnlContent
            // 
            this.pnlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlContent.Controls.Add(this.splitter3);
            this.pnlContent.Controls.Add(this.splitter1);
            this.pnlContent.Location = new System.Drawing.Point(170, 1);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(461, 455);
            this.pnlContent.TabIndex = 8;
            // 
            // splitter3
            // 
            this.splitter3.Enabled = false;
            this.splitter3.Location = new System.Drawing.Point(3, 0);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(10, 455);
            this.splitter3.TabIndex = 1;
            this.splitter3.TabStop = false;
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 455);
            this.splitter1.TabIndex = 0;
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(0, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 453);
            this.splitter2.TabIndex = 10;
            this.splitter2.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 453);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlButtons);
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Account System Manager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Panel pnlContent;
        private Components.MenuItem btnRemoveInvoices;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Splitter splitter3;
        private Components.MenuItem btnConfigureDB;
        private System.Windows.Forms.Label lblCa;
    }
}



