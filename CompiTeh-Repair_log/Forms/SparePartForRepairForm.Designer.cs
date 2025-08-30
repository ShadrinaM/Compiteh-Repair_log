namespace CompiTeh_Repair_log.Forms
{
    partial class SparePartForRepairForm
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
            btnAddSparepart = new Button();
            dataGridViewSparepart = new DataGridView();
            lbСhoiceСlient = new Label();
            panelHeader = new Panel();
            labelHeader = new Label();
            btnCancel = new Button();
            btnOK = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewSparepart).BeginInit();
            panelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // btnAddSparepart
            // 
            btnAddSparepart.BackColor = SystemColors.ControlLightLight;
            btnAddSparepart.FlatAppearance.BorderColor = SystemColors.Highlight;
            btnAddSparepart.FlatAppearance.BorderSize = 2;
            btnAddSparepart.FlatAppearance.MouseDownBackColor = SystemColors.ActiveBorder;
            btnAddSparepart.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
            btnAddSparepart.FlatStyle = FlatStyle.Flat;
            btnAddSparepart.Font = new Font("Segoe UI Black", 14F, FontStyle.Bold);
            btnAddSparepart.ForeColor = SystemColors.Highlight;
            btnAddSparepart.Location = new Point(153, 59);
            btnAddSparepart.Margin = new Padding(3, 2, 3, 2);
            btnAddSparepart.Name = "btnAddSparepart";
            btnAddSparepart.Size = new Size(37, 37);
            btnAddSparepart.TabIndex = 25;
            btnAddSparepart.Text = "+";
            btnAddSparepart.UseVisualStyleBackColor = false;
            // 
            // dataGridViewSparepart
            // 
            dataGridViewSparepart.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewSparepart.Location = new Point(10, 110);
            dataGridViewSparepart.Name = "dataGridViewSparepart";
            dataGridViewSparepart.Size = new Size(778, 281);
            dataGridViewSparepart.TabIndex = 24;
            // 
            // lbСhoiceСlient
            // 
            lbСhoiceСlient.AutoSize = true;
            lbСhoiceСlient.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lbСhoiceСlient.ForeColor = SystemColors.Highlight;
            lbСhoiceСlient.Location = new Point(10, 59);
            lbСhoiceСlient.Name = "lbСhoiceСlient";
            lbСhoiceСlient.Size = new Size(137, 37);
            lbСhoiceСlient.TabIndex = 23;
            lbСhoiceСlient.Text = "Запчасти";
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(labelHeader);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.ForeColor = SystemColors.Window;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Margin = new Padding(3, 2, 3, 2);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(800, 48);
            panelHeader.TabIndex = 22;
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            labelHeader.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            labelHeader.ForeColor = SystemColors.Highlight;
            labelHeader.Location = new Point(10, 7);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new Size(465, 37);
            labelHeader.TabIndex = 0;
            labelHeader.Text = "Добавление запчастей к ремонту";
            // 
            // btnCancel
            // 
            btnCancel.BackColor = SystemColors.ControlLightLight;
            btnCancel.FlatAppearance.BorderColor = SystemColors.Highlight;
            btnCancel.FlatAppearance.BorderSize = 2;
            btnCancel.FlatAppearance.MouseDownBackColor = SystemColors.ActiveBorder;
            btnCancel.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnCancel.ForeColor = SystemColors.Highlight;
            btnCancel.Location = new Point(12, 406);
            btnCancel.Margin = new Padding(3, 2, 3, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(157, 33);
            btnCancel.TabIndex = 27;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnOK
            // 
            btnOK.BackColor = SystemColors.ControlLightLight;
            btnOK.FlatAppearance.BorderColor = SystemColors.Highlight;
            btnOK.FlatAppearance.BorderSize = 2;
            btnOK.FlatAppearance.MouseDownBackColor = SystemColors.ActiveBorder;
            btnOK.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnOK.ForeColor = SystemColors.Highlight;
            btnOK.Location = new Point(633, 406);
            btnOK.Margin = new Padding(3, 2, 3, 2);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(157, 33);
            btnOK.TabIndex = 26;
            btnOK.Text = "ОК";
            btnOK.UseVisualStyleBackColor = false;
            // 
            // SparePartForRepairForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientInactiveCaption;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(btnAddSparepart);
            Controls.Add(dataGridViewSparepart);
            Controls.Add(lbСhoiceСlient);
            Controls.Add(panelHeader);
            Name = "SparePartForRepairForm";
            Text = "SparePartForRepairForm";
            ((System.ComponentModel.ISupportInitialize)dataGridViewSparepart).EndInit();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnAddSparepart;
        private DataGridView dataGridViewSparepart;
        private Label lbСhoiceСlient;
        private Panel panelHeader;
        private Label labelHeader;
        private Button btnCancel;
        private Button btnOK;
    }
}