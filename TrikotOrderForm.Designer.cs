namespace SVU_Bestellungen
{
    partial class TrikotOrderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrikotOrderForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownQuantity = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.btnAddOrder = new System.Windows.Forms.Button();
            this.dataGridViewOrders = new System.Windows.Forms.DataGridView();
            this.btnSaveOrders = new System.Windows.Forms.Button();
            this.btnSaveSummary = new System.Windows.Forms.Button();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.txtNachname = new System.Windows.Forms.TextBox();
            this.txtVorname = new System.Windows.Forms.TextBox();
            this.txtInitialen = new System.Windows.Forms.TextBox();
            this.comboBoxSize = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrders)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Nachname:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(241, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Vorname:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(474, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Initialen";
            // 
            // numericUpDownQuantity
            // 
            this.numericUpDownQuantity.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.numericUpDownQuantity.Location = new System.Drawing.Point(1077, 56);
            this.numericUpDownQuantity.Name = "numericUpDownQuantity";
            this.numericUpDownQuantity.Size = new System.Drawing.Size(120, 26);
            this.numericUpDownQuantity.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1139, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Menge";
            // 
            // btnAddOrder
            // 
            this.btnAddOrder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddOrder.Location = new System.Drawing.Point(611, 121);
            this.btnAddOrder.Name = "btnAddOrder";
            this.btnAddOrder.Size = new System.Drawing.Size(587, 55);
            this.btnAddOrder.TabIndex = 8;
            this.btnAddOrder.Text = "Bestellung hinzufügen";
            this.btnAddOrder.UseVisualStyleBackColor = true;
            this.btnAddOrder.Click += new System.EventHandler(this.BtnAddOrder_Click);
            // 
            // dataGridViewOrders
            // 
            this.dataGridViewOrders.AllowUserToOrderColumns = true;
            this.dataGridViewOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOrders.Location = new System.Drawing.Point(12, 253);
            this.dataGridViewOrders.Name = "dataGridViewOrders";
            this.dataGridViewOrders.RowHeadersWidth = 62;
            this.dataGridViewOrders.RowTemplate.Height = 28;
            this.dataGridViewOrders.Size = new System.Drawing.Size(1186, 900);
            this.dataGridViewOrders.TabIndex = 9;
            // 
            // btnSaveOrders
            // 
            this.btnSaveOrders.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveOrders.Location = new System.Drawing.Point(249, 121);
            this.btnSaveOrders.Name = "btnSaveOrders";
            this.btnSaveOrders.Size = new System.Drawing.Size(223, 55);
            this.btnSaveOrders.TabIndex = 10;
            this.btnSaveOrders.Text = "Speichern";
            this.btnSaveOrders.UseVisualStyleBackColor = true;
            this.btnSaveOrders.Click += new System.EventHandler(this.BtnSaveOrders_Click);
            // 
            // btnSaveSummary
            // 
            this.btnSaveSummary.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveSummary.Location = new System.Drawing.Point(12, 121);
            this.btnSaveSummary.Name = "btnSaveSummary";
            this.btnSaveSummary.Size = new System.Drawing.Size(223, 55);
            this.btnSaveSummary.TabIndex = 11;
            this.btnSaveSummary.Text = "Auswerten";
            this.btnSaveSummary.UseVisualStyleBackColor = true;
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenFolder.Location = new System.Drawing.Point(12, 182);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(223, 55);
            this.btnOpenFolder.TabIndex = 12;
            this.btnOpenFolder.Text = "Zur Datei";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // txtNachname
            // 
            this.txtNachname.Location = new System.Drawing.Point(12, 56);
            this.txtNachname.Name = "txtNachname";
            this.txtNachname.Size = new System.Drawing.Size(223, 26);
            this.txtNachname.TabIndex = 13;
            // 
            // txtVorname
            // 
            this.txtVorname.Location = new System.Drawing.Point(245, 56);
            this.txtVorname.Name = "txtVorname";
            this.txtVorname.Size = new System.Drawing.Size(223, 26);
            this.txtVorname.TabIndex = 14;
            // 
            // txtInitialen
            // 
            this.txtInitialen.Location = new System.Drawing.Point(478, 56);
            this.txtInitialen.Name = "txtInitialen";
            this.txtInitialen.Size = new System.Drawing.Size(227, 26);
            this.txtInitialen.TabIndex = 15;
            // 
            // comboBoxSize
            // 
            this.comboBoxSize.FormattingEnabled = true;
            this.comboBoxSize.Location = new System.Drawing.Point(711, 56);
            this.comboBoxSize.Name = "comboBoxSize";
            this.comboBoxSize.Size = new System.Drawing.Size(227, 28);
            this.comboBoxSize.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(707, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 20);
            this.label5.TabIndex = 17;
            this.label5.Text = "Größe";
            // 
            // TrikotOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1209, 1168);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxSize);
            this.Controls.Add(this.txtInitialen);
            this.Controls.Add(this.txtVorname);
            this.Controls.Add(this.txtNachname);
            this.Controls.Add(this.btnOpenFolder);
            this.Controls.Add(this.btnSaveSummary);
            this.Controls.Add(this.btnSaveOrders);
            this.Controls.Add(this.dataGridViewOrders);
            this.Controls.Add(this.btnAddOrder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDownQuantity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TrikotOrderForm";
            this.ShowIcon = false;
            this.Text = "Bestellungs-Oberfläche";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrders)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownQuantity;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnAddOrder;
        private System.Windows.Forms.DataGridView dataGridViewOrders;
        private System.Windows.Forms.Button btnSaveOrders;
        private System.Windows.Forms.Button btnSaveSummary;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.TextBox txtNachname;
        private System.Windows.Forms.TextBox txtVorname;
        private System.Windows.Forms.TextBox txtInitialen;
        private System.Windows.Forms.ComboBox comboBoxSize;
        private System.Windows.Forms.Label label5;
    }
}

