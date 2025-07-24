namespace CompiTeh_Repair_log.Forms
{
    partial class ClientForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelHeader;
        private Label labelHeader;
        private Label labelClientType;
        private ComboBox comboBoxClientType;
        private Label labelFullName;
        private TextBox textBoxFullName;
        private Label labelContactPhone;
        private TextBox textBoxContactPhone;
        private Label labelOrganizationName;
        private TextBox textBoxOrganizationName;
        private Label labelEmail;
        private TextBox textBoxEmail;
        private Label labelClientNotes;
        private TextBox textBoxClientNotes;
        private Button btnOK;

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
            panelHeader = new Panel();
            labelHeader = new Label();
            labelClientType = new Label();
            comboBoxClientType = new ComboBox();
            labelFullName = new Label();
            textBoxFullName = new TextBox();
            labelContactPhone = new Label();
            textBoxContactPhone = new TextBox();
            labelOrganizationName = new Label();
            textBoxOrganizationName = new TextBox();
            labelEmail = new Label();
            textBoxEmail = new TextBox();
            labelClientNotes = new Label();
            textBoxClientNotes = new TextBox();
            btnOK = new Button();
            btnCancel = new Button();
            panelHeader.SuspendLayout();
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
            panelHeader.Size = new Size(464, 48);
            panelHeader.TabIndex = 0;
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            labelHeader.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            labelHeader.ForeColor = SystemColors.Highlight;
            labelHeader.Location = new Point(10, 7);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new Size(296, 37);
            labelHeader.TabIndex = 0;
            labelHeader.Text = "Добавление клиента";
            // 
            // labelClientType
            // 
            labelClientType.AutoSize = true;
            labelClientType.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            labelClientType.ForeColor = SystemColors.Highlight;
            labelClientType.Location = new Point(10, 58);
            labelClientType.Name = "labelClientType";
            labelClientType.Size = new Size(120, 25);
            labelClientType.TabIndex = 1;
            labelClientType.Text = "Тип клиента";
            // 
            // comboBoxClientType
            // 
            comboBoxClientType.BackColor = SystemColors.ActiveCaption;
            comboBoxClientType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxClientType.FormattingEnabled = true;
            comboBoxClientType.Items.AddRange(new object[] { "Физический", "Юридический" });
            comboBoxClientType.Location = new Point(231, 62);
            comboBoxClientType.Name = "comboBoxClientType";
            comboBoxClientType.Size = new Size(219, 23);
            comboBoxClientType.TabIndex = 2;
            comboBoxClientType.SelectedIndexChanged += comboBoxClientType_SelectedIndexChanged;
            // 
            // labelFullName
            // 
            labelFullName.AutoSize = true;
            labelFullName.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            labelFullName.ForeColor = SystemColors.Highlight;
            labelFullName.Location = new Point(10, 88);
            labelFullName.Name = "labelFullName";
            labelFullName.Size = new Size(55, 25);
            labelFullName.TabIndex = 3;
            labelFullName.Text = "ФИО";
            // 
            // textBoxFullName
            // 
            textBoxFullName.Location = new Point(231, 93);
            textBoxFullName.Name = "textBoxFullName";
            textBoxFullName.Size = new Size(219, 23);
            textBoxFullName.TabIndex = 4;
            // 
            // labelContactPhone
            // 
            labelContactPhone.AutoSize = true;
            labelContactPhone.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            labelContactPhone.ForeColor = SystemColors.Highlight;
            labelContactPhone.Location = new Point(10, 118);
            labelContactPhone.Name = "labelContactPhone";
            labelContactPhone.Size = new Size(199, 25);
            labelContactPhone.TabIndex = 5;
            labelContactPhone.Text = "Контактный телефон";
            // 
            // textBoxContactPhone
            // 
            textBoxContactPhone.Location = new Point(231, 123);
            textBoxContactPhone.Name = "textBoxContactPhone";
            textBoxContactPhone.Size = new Size(219, 23);
            textBoxContactPhone.TabIndex = 6;
            // 
            // labelOrganizationName
            // 
            labelOrganizationName.AutoSize = true;
            labelOrganizationName.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            labelOrganizationName.ForeColor = SystemColors.Highlight;
            labelOrganizationName.Location = new Point(10, 179);
            labelOrganizationName.Name = "labelOrganizationName";
            labelOrganizationName.Size = new Size(216, 25);
            labelOrganizationName.TabIndex = 7;
            labelOrganizationName.Text = "Название организации";
            // 
            // textBoxOrganizationName
            // 
            textBoxOrganizationName.Location = new Point(231, 184);
            textBoxOrganizationName.Name = "textBoxOrganizationName";
            textBoxOrganizationName.Size = new Size(219, 23);
            textBoxOrganizationName.TabIndex = 8;
            // 
            // labelEmail
            // 
            labelEmail.AutoSize = true;
            labelEmail.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            labelEmail.ForeColor = SystemColors.Highlight;
            labelEmail.Location = new Point(10, 147);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(58, 25);
            labelEmail.TabIndex = 9;
            labelEmail.Text = "Email";
            // 
            // textBoxEmail
            // 
            textBoxEmail.Location = new Point(231, 152);
            textBoxEmail.Name = "textBoxEmail";
            textBoxEmail.Size = new Size(219, 23);
            textBoxEmail.TabIndex = 10;
            // 
            // labelClientNotes
            // 
            labelClientNotes.AutoSize = true;
            labelClientNotes.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            labelClientNotes.ForeColor = SystemColors.Highlight;
            labelClientNotes.Location = new Point(10, 208);
            labelClientNotes.Name = "labelClientNotes";
            labelClientNotes.Size = new Size(125, 25);
            labelClientNotes.TabIndex = 11;
            labelClientNotes.Text = "Примечания";
            // 
            // textBoxClientNotes
            // 
            textBoxClientNotes.Location = new Point(231, 213);
            textBoxClientNotes.Multiline = true;
            textBoxClientNotes.Name = "textBoxClientNotes";
            textBoxClientNotes.Size = new Size(219, 60);
            textBoxClientNotes.TabIndex = 12;
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
            btnOK.Location = new Point(293, 287);
            btnOK.Margin = new Padding(3, 2, 3, 2);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(157, 33);
            btnOK.TabIndex = 13;
            btnOK.Text = "ОК";
            btnOK.UseVisualStyleBackColor = false;
            btnOK.Click += btnOK_Click;
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
            btnCancel.Location = new Point(12, 287);
            btnCancel.Margin = new Padding(3, 2, 3, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(157, 33);
            btnCancel.TabIndex = 14;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // ClientForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientInactiveCaption;
            ClientSize = new Size(464, 333);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(textBoxClientNotes);
            Controls.Add(labelClientNotes);
            Controls.Add(textBoxEmail);
            Controls.Add(labelEmail);
            Controls.Add(textBoxOrganizationName);
            Controls.Add(labelOrganizationName);
            Controls.Add(textBoxContactPhone);
            Controls.Add(labelContactPhone);
            Controls.Add(textBoxFullName);
            Controls.Add(labelFullName);
            Controls.Add(comboBoxClientType);
            Controls.Add(labelClientType);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ClientForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Клиент";
            Load += ClientForm_Load;
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }  
        private Button btnCancel;
    }
}