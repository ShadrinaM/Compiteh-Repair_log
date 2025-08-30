namespace CompiTeh_Repair_log.Forms
{
    partial class SparepartForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelHeader;
        private Label labelHeader;
        private Label labelName;
        private TextBox textBoxName;
        private Label labelPrice;
        private NumericUpDown numericPrice;
        private Label labelQuantity;
        private NumericUpDown numericQuantity;
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
            labelName = new Label();
            textBoxName = new TextBox();
            labelPrice = new Label();
            numericPrice = new NumericUpDown();
            labelQuantity = new Label();
            numericQuantity = new NumericUpDown();
            btnOK = new Button();
            btnCancel = new Button();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericQuantity).BeginInit();
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
            labelHeader.Size = new Size(312, 37);
            labelHeader.TabIndex = 0;
            labelHeader.Text = "Добавление запчасти";
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            labelName.ForeColor = SystemColors.Highlight;
            labelName.Location = new Point(10, 58);
            labelName.Name = "labelName";
            labelName.Size = new Size(182, 25);
            labelName.TabIndex = 1;
            labelName.Text = "Название запчасти";
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(231, 61);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(219, 23);
            textBoxName.TabIndex = 2;
            // 
            // labelPrice
            // 
            labelPrice.AutoSize = true;
            labelPrice.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            labelPrice.ForeColor = SystemColors.Highlight;
            labelPrice.Location = new Point(10, 88);
            labelPrice.Name = "labelPrice";
            labelPrice.Size = new Size(58, 25);
            labelPrice.TabIndex = 3;
            labelPrice.Text = "Цена";
            // 
            // numericPrice
            // 
            numericPrice.DecimalPlaces = 2;
            numericPrice.Location = new Point(231, 93);
            numericPrice.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numericPrice.Name = "numericPrice";
            numericPrice.Size = new Size(219, 23);
            numericPrice.TabIndex = 4;
            // 
            // labelQuantity
            // 
            labelQuantity.AutoSize = true;
            labelQuantity.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            labelQuantity.ForeColor = SystemColors.Highlight;
            labelQuantity.Location = new Point(10, 118);
            labelQuantity.Name = "labelQuantity";
            labelQuantity.Size = new Size(115, 25);
            labelQuantity.TabIndex = 5;
            labelQuantity.Text = "Количество";
            // 
            // numericQuantity
            // 
            numericQuantity.Location = new Point(231, 123);
            numericQuantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericQuantity.Name = "numericQuantity";
            numericQuantity.Size = new Size(219, 23);
            numericQuantity.TabIndex = 6;
            numericQuantity.Value = new decimal(new int[] { 1, 0, 0, 0 });
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
            btnOK.Location = new Point(293, 160);
            btnOK.Margin = new Padding(3, 2, 3, 2);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(157, 33);
            btnOK.TabIndex = 7;
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
            btnCancel.Location = new Point(12, 160);
            btnCancel.Margin = new Padding(3, 2, 3, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(157, 33);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // SparepartForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientInactiveCaption;
            ClientSize = new Size(464, 202);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(numericQuantity);
            Controls.Add(labelQuantity);
            Controls.Add(numericPrice);
            Controls.Add(labelPrice);
            Controls.Add(textBoxName);
            Controls.Add(labelName);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SparepartForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Запчасть";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericQuantity).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}