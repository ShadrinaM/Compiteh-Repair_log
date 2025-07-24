namespace CompiTeh_Repair_log
{
    partial class Menu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Menu));
            btnViewData = new Button();
            btnPeriodReport = new Button();
            btnClientForma = new Button();
            picLogo = new PictureBox();
            panelHeader = new Panel();
            btnDebtorsReport = new Button();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            panelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // btnViewData
            // 
            btnViewData.BackColor = SystemColors.ControlLightLight;
            btnViewData.FlatAppearance.BorderColor = SystemColors.Highlight;
            btnViewData.FlatAppearance.BorderSize = 2;
            btnViewData.FlatAppearance.MouseDownBackColor = SystemColors.ActiveBorder;
            btnViewData.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
            btnViewData.FlatStyle = FlatStyle.Flat;
            btnViewData.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            btnViewData.ForeColor = SystemColors.Highlight;
            btnViewData.Location = new Point(12, 88);
            btnViewData.Margin = new Padding(3, 2, 3, 2);
            btnViewData.Name = "btnViewData";
            btnViewData.Size = new Size(564, 56);
            btnViewData.TabIndex = 1;
            btnViewData.Text = "Просмотр данных";
            btnViewData.UseVisualStyleBackColor = false;
            btnViewData.Click += btnViewData_Click;
            // 
            // btnPeriodReport
            // 
            btnPeriodReport.BackColor = SystemColors.ControlLightLight;
            btnPeriodReport.FlatAppearance.BorderColor = SystemColors.Highlight;
            btnPeriodReport.FlatAppearance.BorderSize = 2;
            btnPeriodReport.FlatAppearance.MouseDownBackColor = SystemColors.ActiveBorder;
            btnPeriodReport.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
            btnPeriodReport.FlatStyle = FlatStyle.Flat;
            btnPeriodReport.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            btnPeriodReport.ForeColor = SystemColors.Highlight;
            btnPeriodReport.Location = new Point(12, 149);
            btnPeriodReport.Margin = new Padding(3, 2, 3, 2);
            btnPeriodReport.Name = "btnPeriodReport";
            btnPeriodReport.Size = new Size(564, 56);
            btnPeriodReport.TabIndex = 3;
            btnPeriodReport.Text = "форма заказ";
            btnPeriodReport.UseVisualStyleBackColor = false;
            btnPeriodReport.Click += btnPeriodReport_Click;
            // 
            // btnClientForma
            // 
            btnClientForma.BackColor = SystemColors.ControlLightLight;
            btnClientForma.FlatAppearance.BorderColor = SystemColors.Highlight;
            btnClientForma.FlatAppearance.BorderSize = 2;
            btnClientForma.FlatAppearance.MouseDownBackColor = SystemColors.ActiveBorder;
            btnClientForma.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
            btnClientForma.FlatStyle = FlatStyle.Flat;
            btnClientForma.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            btnClientForma.ForeColor = SystemColors.Highlight;
            btnClientForma.Location = new Point(12, 271);
            btnClientForma.Margin = new Padding(3, 2, 3, 2);
            btnClientForma.Name = "btnClientForma";
            btnClientForma.Size = new Size(564, 56);
            btnClientForma.TabIndex = 5;
            btnClientForma.Text = "форма клиент";
            btnClientForma.UseVisualStyleBackColor = false;
            btnClientForma.Click += btnClientForma_Click;
            // 
            // picLogo
            // 
            picLogo.Image = (Image)resources.GetObject("picLogo.Image");
            picLogo.Location = new Point(12, 11);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(222, 48);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 0;
            picLogo.TabStop = false;
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(picLogo);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(9, 8, 9, 8);
            panelHeader.Size = new Size(590, 71);
            panelHeader.TabIndex = 6;
            // 
            // btnDebtorsReport
            // 
            btnDebtorsReport.BackColor = SystemColors.ControlLightLight;
            btnDebtorsReport.FlatAppearance.BorderColor = SystemColors.Highlight;
            btnDebtorsReport.FlatAppearance.BorderSize = 2;
            btnDebtorsReport.FlatAppearance.MouseDownBackColor = SystemColors.ActiveBorder;
            btnDebtorsReport.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
            btnDebtorsReport.FlatStyle = FlatStyle.Flat;
            btnDebtorsReport.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            btnDebtorsReport.ForeColor = SystemColors.Highlight;
            btnDebtorsReport.Location = new Point(12, 210);
            btnDebtorsReport.Margin = new Padding(3, 2, 3, 2);
            btnDebtorsReport.Name = "btnDebtorsReport";
            btnDebtorsReport.Size = new Size(564, 56);
            btnDebtorsReport.TabIndex = 4;
            btnDebtorsReport.Text = "форма дивайс";
            btnDebtorsReport.UseVisualStyleBackColor = false;
            btnDebtorsReport.Click += btnDevicesForm_Click;
            // 
            // Menu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientInactiveCaption;
            ClientSize = new Size(590, 337);
            Controls.Add(panelHeader);
            Controls.Add(btnClientForma);
            Controls.Add(btnDebtorsReport);
            Controls.Add(btnPeriodReport);
            Controls.Add(btnViewData);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Menu";
            Text = "Журнал ремонта";
            Load += Menu_Load;
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            panelHeader.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Button btnViewData;
        private Button btnPeriodReport;
        private Button btnClientForma;
        private PictureBox picLogo;
        private Panel panelHeader;
        private Button btnDebtorsReport;
    }
}