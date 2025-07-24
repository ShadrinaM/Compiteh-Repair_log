namespace CompiTeh_Repair_log
{
    partial class ReceiptsForms
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
            panelHeader = new Panel();
            labelHeader = new Label();
            btnCancel = new Button();
            btnOK = new Button();
            comboBoxClientType = new ComboBox();
            labelClientType = new Label();
            lbСhoiceСlient = new Label();
            dataGridViewClient = new DataGridView();
            btnAddClient = new Button();
            lbСhoiceDevices = new Label();
            btnAddDevices = new Button();
            dataGridViewDevices = new DataGridView();
            textBoxRepairNotes = new TextBox();
            labelRepairNotes = new Label();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewClient).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDevices).BeginInit();
            SuspendLayout();
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
            panelHeader.TabIndex = 1;
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            labelHeader.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            labelHeader.ForeColor = SystemColors.Highlight;
            labelHeader.Location = new Point(10, 7);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new Size(303, 37);
            labelHeader.TabIndex = 0;
            labelHeader.Text = "Добавление ремонта";
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
            btnCancel.Location = new Point(10, 736);
            btnCancel.Margin = new Padding(3, 2, 3, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(157, 33);
            btnCancel.TabIndex = 18;
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
            btnOK.Location = new Point(631, 736);
            btnOK.Margin = new Padding(3, 2, 3, 2);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(157, 33);
            btnOK.TabIndex = 17;
            btnOK.Text = "ОК";
            btnOK.UseVisualStyleBackColor = false;
            // 
            // comboBoxClientType
            // 
            comboBoxClientType.BackColor = SystemColors.ActiveCaption;
            comboBoxClientType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxClientType.FormattingEnabled = true;
            comboBoxClientType.Items.AddRange(new object[] { "Физический", "Юридический" });
            comboBoxClientType.Location = new Point(157, 94);
            comboBoxClientType.Margin = new Padding(5);
            comboBoxClientType.Name = "comboBoxClientType";
            comboBoxClientType.Size = new Size(219, 23);
            comboBoxClientType.TabIndex = 16;
            comboBoxClientType.SelectedIndexChanged += comboBoxClientType_SelectedIndexChanged;
            // 
            // labelClientType
            // 
            labelClientType.AutoSize = true;
            labelClientType.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            labelClientType.ForeColor = SystemColors.Highlight;
            labelClientType.Location = new Point(31, 92);
            labelClientType.Margin = new Padding(5);
            labelClientType.Name = "labelClientType";
            labelClientType.Size = new Size(120, 25);
            labelClientType.TabIndex = 15;
            labelClientType.Text = "Тип клиента";
            // 
            // lbСhoiceСlient
            // 
            lbСhoiceСlient.AutoSize = true;
            lbСhoiceСlient.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lbСhoiceСlient.ForeColor = SystemColors.Highlight;
            lbСhoiceСlient.Location = new Point(10, 50);
            lbСhoiceСlient.Name = "lbСhoiceСlient";
            lbСhoiceСlient.Size = new Size(220, 37);
            lbСhoiceСlient.TabIndex = 19;
            lbСhoiceСlient.Text = "Выбор клиента";
            // 
            // dataGridViewClient
            // 
            dataGridViewClient.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewClient.Location = new Point(12, 124);
            dataGridViewClient.Name = "dataGridViewClient";
            dataGridViewClient.Size = new Size(778, 193);
            dataGridViewClient.TabIndex = 20;
            // 
            // btnAddClient
            // 
            btnAddClient.BackColor = SystemColors.ControlLightLight;
            btnAddClient.FlatAppearance.BorderColor = SystemColors.Highlight;
            btnAddClient.FlatAppearance.BorderSize = 2;
            btnAddClient.FlatAppearance.MouseDownBackColor = SystemColors.ActiveBorder;
            btnAddClient.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
            btnAddClient.FlatStyle = FlatStyle.Flat;
            btnAddClient.Font = new Font("Segoe UI Black", 14F, FontStyle.Bold);
            btnAddClient.ForeColor = SystemColors.Highlight;
            btnAddClient.Location = new Point(236, 50);
            btnAddClient.Margin = new Padding(3, 2, 3, 2);
            btnAddClient.Name = "btnAddClient";
            btnAddClient.Size = new Size(37, 37);
            btnAddClient.TabIndex = 21;
            btnAddClient.Text = "+";
            btnAddClient.UseVisualStyleBackColor = false;
            btnAddClient.Click += btnAddClient_Click;
            // 
            // lbСhoiceDevices
            // 
            lbСhoiceDevices.AutoSize = true;
            lbСhoiceDevices.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lbСhoiceDevices.ForeColor = SystemColors.Highlight;
            lbСhoiceDevices.Location = new Point(12, 322);
            lbСhoiceDevices.Name = "lbСhoiceDevices";
            lbСhoiceDevices.Size = new Size(391, 37);
            lbСhoiceDevices.TabIndex = 22;
            lbСhoiceDevices.Text = "Ремонтируемые устройства";
            // 
            // btnAddDevices
            // 
            btnAddDevices.BackColor = SystemColors.ControlLightLight;
            btnAddDevices.FlatAppearance.BorderColor = SystemColors.Highlight;
            btnAddDevices.FlatAppearance.BorderSize = 2;
            btnAddDevices.FlatAppearance.MouseDownBackColor = SystemColors.ActiveBorder;
            btnAddDevices.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
            btnAddDevices.FlatStyle = FlatStyle.Flat;
            btnAddDevices.Font = new Font("Segoe UI Black", 14F, FontStyle.Bold);
            btnAddDevices.ForeColor = SystemColors.Highlight;
            btnAddDevices.Location = new Point(409, 322);
            btnAddDevices.Margin = new Padding(3, 2, 3, 2);
            btnAddDevices.Name = "btnAddDevices";
            btnAddDevices.Size = new Size(37, 37);
            btnAddDevices.TabIndex = 23;
            btnAddDevices.Text = "+";
            btnAddDevices.UseVisualStyleBackColor = false;
            btnAddDevices.Click += btnAddDevices_Click;
            // 
            // dataGridViewDevices
            // 
            dataGridViewDevices.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewDevices.Location = new Point(10, 364);
            dataGridViewDevices.Name = "dataGridViewDevices";
            dataGridViewDevices.Size = new Size(778, 197);
            dataGridViewDevices.TabIndex = 24;
            // 
            // textBoxRepairNotes
            // 
            textBoxRepairNotes.Location = new Point(10, 604);
            textBoxRepairNotes.Multiline = true;
            textBoxRepairNotes.Name = "textBoxRepairNotes";
            textBoxRepairNotes.Size = new Size(778, 127);
            textBoxRepairNotes.TabIndex = 30;
            // 
            // labelRepairNotes
            // 
            labelRepairNotes.AutoSize = true;
            labelRepairNotes.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            labelRepairNotes.ForeColor = SystemColors.Highlight;
            labelRepairNotes.Location = new Point(10, 564);
            labelRepairNotes.Name = "labelRepairNotes";
            labelRepairNotes.Size = new Size(303, 37);
            labelRepairNotes.TabIndex = 31;
            labelRepairNotes.Text = "Примечания к заказу";
            // 
            // ReceiptsForms
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientInactiveCaption;
            ClientSize = new Size(800, 780);
            Controls.Add(labelRepairNotes);
            Controls.Add(textBoxRepairNotes);
            Controls.Add(dataGridViewDevices);
            Controls.Add(btnAddDevices);
            Controls.Add(lbСhoiceDevices);
            Controls.Add(btnAddClient);
            Controls.Add(dataGridViewClient);
            Controls.Add(lbСhoiceСlient);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(panelHeader);
            Controls.Add(comboBoxClientType);
            Controls.Add(labelClientType);
            Name = "ReceiptsForms";
            Text = "ViewData";
            Load += ViewData_Load;
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewClient).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDevices).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panelHeader;
        private Label labelHeader;
        private Button btnCancel;
        private Button btnOK;
        private ComboBox comboBoxClientType;
        private Label labelClientType;
        private Label lbСhoiceСlient;
        private DataGridView dataGridViewClient;
        private Button btnAddClient;
        private Label lbСhoiceDevices;
        private Button btnAddDevices;
        private DataGridView dataGridViewDevices;
        private TextBox textBoxRepairNotes;
        private Label labelRepairNotes;
    }
}