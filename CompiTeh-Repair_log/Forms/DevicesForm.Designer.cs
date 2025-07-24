namespace CompiTeh_Repair_log.Forms
{
    partial class DevicesForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelHeader;
        private Label labelHeader;
        private Label labelDeviceType;
        private Label labelManufacturer;
        private TextBox textBoxManufacturer;
        private Label labelModelNumber;
        private TextBox textBoxModelNumber;
        private Label labelSerialNumber;
        private TextBox textBoxSerialNumber;
        private Label labelCompleteness;
        private TextBox textBoxCompleteness;
        private Label labelDeviceNotes;
        private TextBox textBoxDeviceNotes;
        private Button btnOK;
        private Button btnCancel;

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
            labelDeviceType = new Label();
            labelManufacturer = new Label();
            textBoxManufacturer = new TextBox();
            labelModelNumber = new Label();
            textBoxModelNumber = new TextBox();
            labelSerialNumber = new Label();
            textBoxSerialNumber = new TextBox();
            labelCompleteness = new Label();
            textBoxCompleteness = new TextBox();
            labelDeviceNotes = new Label();
            textBoxDeviceNotes = new TextBox();
            btnOK = new Button();
            btnCancel = new Button();
            textBoxDeviceType = new TextBox();
            textBoxFaultDescription = new TextBox();
            labelFaultDescription = new Label();
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
            labelHeader.Size = new Size(336, 37);
            labelHeader.TabIndex = 0;
            labelHeader.Text = "Добавление устройства";
            // 
            // labelDeviceType
            // 
            labelDeviceType.AutoSize = true;
            labelDeviceType.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            labelDeviceType.ForeColor = SystemColors.Highlight;
            labelDeviceType.Location = new Point(10, 58);
            labelDeviceType.Name = "labelDeviceType";
            labelDeviceType.Size = new Size(146, 25);
            labelDeviceType.TabIndex = 1;
            labelDeviceType.Text = "Тип устройства";
            // 
            // labelManufacturer
            // 
            labelManufacturer.AutoSize = true;
            labelManufacturer.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            labelManufacturer.ForeColor = SystemColors.Highlight;
            labelManufacturer.Location = new Point(10, 88);
            labelManufacturer.Name = "labelManufacturer";
            labelManufacturer.Size = new Size(150, 25);
            labelManufacturer.TabIndex = 3;
            labelManufacturer.Text = "Производитель";
            // 
            // textBoxManufacturer
            // 
            textBoxManufacturer.Location = new Point(231, 93);
            textBoxManufacturer.Name = "textBoxManufacturer";
            textBoxManufacturer.Size = new Size(219, 23);
            textBoxManufacturer.TabIndex = 4;
            // 
            // labelModelNumber
            // 
            labelModelNumber.AutoSize = true;
            labelModelNumber.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            labelModelNumber.ForeColor = SystemColors.Highlight;
            labelModelNumber.Location = new Point(10, 118);
            labelModelNumber.Name = "labelModelNumber";
            labelModelNumber.Size = new Size(82, 25);
            labelModelNumber.TabIndex = 5;
            labelModelNumber.Text = "Модель";
            // 
            // textBoxModelNumber
            // 
            textBoxModelNumber.Location = new Point(231, 123);
            textBoxModelNumber.Name = "textBoxModelNumber";
            textBoxModelNumber.Size = new Size(219, 23);
            textBoxModelNumber.TabIndex = 6;
            // 
            // labelSerialNumber
            // 
            labelSerialNumber.AutoSize = true;
            labelSerialNumber.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            labelSerialNumber.ForeColor = SystemColors.Highlight;
            labelSerialNumber.Location = new Point(10, 148);
            labelSerialNumber.Name = "labelSerialNumber";
            labelSerialNumber.Size = new Size(165, 25);
            labelSerialNumber.TabIndex = 7;
            labelSerialNumber.Text = "Серийный номер";
            // 
            // textBoxSerialNumber
            // 
            textBoxSerialNumber.Location = new Point(231, 153);
            textBoxSerialNumber.Name = "textBoxSerialNumber";
            textBoxSerialNumber.Size = new Size(219, 23);
            textBoxSerialNumber.TabIndex = 8;
            // 
            // labelCompleteness
            // 
            labelCompleteness.AutoSize = true;
            labelCompleteness.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            labelCompleteness.ForeColor = SystemColors.Highlight;
            labelCompleteness.Location = new Point(10, 178);
            labelCompleteness.Name = "labelCompleteness";
            labelCompleteness.Size = new Size(142, 25);
            labelCompleteness.TabIndex = 9;
            labelCompleteness.Text = "Комплектация";
            // 
            // textBoxCompleteness
            // 
            textBoxCompleteness.Location = new Point(231, 183);
            textBoxCompleteness.Multiline = true;
            textBoxCompleteness.Name = "textBoxCompleteness";
            textBoxCompleteness.Size = new Size(219, 60);
            textBoxCompleteness.TabIndex = 10;
            // 
            // labelDeviceNotes
            // 
            labelDeviceNotes.AutoSize = true;
            labelDeviceNotes.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            labelDeviceNotes.ForeColor = SystemColors.Highlight;
            labelDeviceNotes.Location = new Point(10, 310);
            labelDeviceNotes.Name = "labelDeviceNotes";
            labelDeviceNotes.Size = new Size(125, 25);
            labelDeviceNotes.TabIndex = 11;
            labelDeviceNotes.Text = "Примечания";
            // 
            // textBoxDeviceNotes
            // 
            textBoxDeviceNotes.Location = new Point(231, 315);
            textBoxDeviceNotes.Multiline = true;
            textBoxDeviceNotes.Name = "textBoxDeviceNotes";
            textBoxDeviceNotes.Size = new Size(219, 60);
            textBoxDeviceNotes.TabIndex = 12;
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
            btnOK.Location = new Point(293, 391);
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
            btnCancel.Location = new Point(12, 391);
            btnCancel.Margin = new Padding(3, 2, 3, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(157, 33);
            btnCancel.TabIndex = 14;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // textBoxDeviceType
            // 
            textBoxDeviceType.Location = new Point(231, 61);
            textBoxDeviceType.Name = "textBoxDeviceType";
            textBoxDeviceType.Size = new Size(219, 23);
            textBoxDeviceType.TabIndex = 15;
            // 
            // textBoxFaultDescription
            // 
            textBoxFaultDescription.Location = new Point(231, 249);
            textBoxFaultDescription.Multiline = true;
            textBoxFaultDescription.Name = "textBoxFaultDescription";
            textBoxFaultDescription.Size = new Size(219, 60);
            textBoxFaultDescription.TabIndex = 17;
            // 
            // labelFaultDescription
            // 
            labelFaultDescription.AutoSize = true;
            labelFaultDescription.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            labelFaultDescription.ForeColor = SystemColors.Highlight;
            labelFaultDescription.Location = new Point(10, 244);
            labelFaultDescription.Name = "labelFaultDescription";
            labelFaultDescription.Size = new Size(145, 50);
            labelFaultDescription.TabIndex = 16;
            labelFaultDescription.Text = "Характер \r\nнеисправности";
            // 
            // DevicesForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientInactiveCaption;
            ClientSize = new Size(464, 433);
            Controls.Add(textBoxFaultDescription);
            Controls.Add(labelFaultDescription);
            Controls.Add(textBoxDeviceType);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(textBoxDeviceNotes);
            Controls.Add(labelDeviceNotes);
            Controls.Add(textBoxCompleteness);
            Controls.Add(labelCompleteness);
            Controls.Add(textBoxSerialNumber);
            Controls.Add(labelSerialNumber);
            Controls.Add(textBoxModelNumber);
            Controls.Add(labelModelNumber);
            Controls.Add(textBoxManufacturer);
            Controls.Add(labelManufacturer);
            Controls.Add(labelDeviceType);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DevicesForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Устройство";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private TextBox textBoxDeviceType;
        private TextBox textBoxFaultDescription;
        private Label labelFaultDescription;
    }
}