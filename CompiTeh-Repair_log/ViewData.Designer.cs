﻿namespace CompiTeh_Repair_log
{
    partial class ViewData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewData));
            panelHeader = new Panel();
            picLogo = new PictureBox();
            labelHeader = new Label();
            dataGridViewRepairs = new DataGridView();
            dataGridViewRepairsInfo = new DataGridView();
            btnAddRepairs = new Button();
            btnEditRepairs = new Button();
            btnDelRepairs = new Button();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewRepairs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewRepairsInfo).BeginInit();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(picLogo);
            panelHeader.Controls.Add(labelHeader);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.ForeColor = SystemColors.Window;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Margin = new Padding(3, 2, 3, 2);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1008, 72);
            panelHeader.TabIndex = 2;
            // 
            // picLogo
            // 
            picLogo.Image = (Image)resources.GetObject("picLogo.Image");
            picLogo.Location = new Point(12, 12);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(222, 48);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 1;
            picLogo.TabStop = false;
            picLogo.Click += btnBack_Click;
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            labelHeader.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            labelHeader.ForeColor = SystemColors.Highlight;
            labelHeader.Location = new Point(240, 19);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new Size(422, 37);
            labelHeader.TabIndex = 0;
            labelHeader.Text = "Просмотр данных о ремонтах";
            // 
            // dataGridViewRepairs
            // 
            dataGridViewRepairs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewRepairs.Location = new Point(12, 140);
            dataGridViewRepairs.Name = "dataGridViewRepairs";
            dataGridViewRepairs.Size = new Size(984, 322);
            dataGridViewRepairs.TabIndex = 3;
            // 
            // dataGridViewRepairsInfo
            // 
            dataGridViewRepairsInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewRepairsInfo.Location = new Point(12, 483);
            dataGridViewRepairsInfo.Name = "dataGridViewRepairsInfo";
            dataGridViewRepairsInfo.Size = new Size(984, 234);
            dataGridViewRepairsInfo.TabIndex = 4;
            // 
            // btnAddRepairs
            // 
            btnAddRepairs.BackColor = SystemColors.ControlLightLight;
            btnAddRepairs.FlatAppearance.BorderColor = SystemColors.Highlight;
            btnAddRepairs.FlatAppearance.BorderSize = 2;
            btnAddRepairs.FlatAppearance.MouseDownBackColor = SystemColors.ActiveBorder;
            btnAddRepairs.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
            btnAddRepairs.FlatStyle = FlatStyle.Flat;
            btnAddRepairs.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnAddRepairs.ForeColor = SystemColors.Highlight;
            btnAddRepairs.Location = new Point(12, 102);
            btnAddRepairs.Margin = new Padding(3, 2, 3, 2);
            btnAddRepairs.Name = "btnAddRepairs";
            btnAddRepairs.Size = new Size(167, 33);
            btnAddRepairs.TabIndex = 14;
            btnAddRepairs.Text = "Добавить ремонт";
            btnAddRepairs.UseVisualStyleBackColor = false;
            btnAddRepairs.Click += btnAddRepairs_Click;
            // 
            // btnEditRepairs
            // 
            btnEditRepairs.BackColor = SystemColors.ControlLightLight;
            btnEditRepairs.FlatAppearance.BorderColor = SystemColors.Highlight;
            btnEditRepairs.FlatAppearance.BorderSize = 2;
            btnEditRepairs.FlatAppearance.MouseDownBackColor = SystemColors.ActiveBorder;
            btnEditRepairs.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
            btnEditRepairs.FlatStyle = FlatStyle.Flat;
            btnEditRepairs.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnEditRepairs.ForeColor = SystemColors.Highlight;
            btnEditRepairs.Location = new Point(185, 102);
            btnEditRepairs.Margin = new Padding(3, 2, 3, 2);
            btnEditRepairs.Name = "btnEditRepairs";
            btnEditRepairs.Size = new Size(167, 33);
            btnEditRepairs.TabIndex = 15;
            btnEditRepairs.Text = "Изменить ремонт";
            btnEditRepairs.UseVisualStyleBackColor = false;
            // 
            // btnDelRepairs
            // 
            btnDelRepairs.BackColor = SystemColors.ControlLightLight;
            btnDelRepairs.FlatAppearance.BorderColor = SystemColors.Highlight;
            btnDelRepairs.FlatAppearance.BorderSize = 2;
            btnDelRepairs.FlatAppearance.MouseDownBackColor = SystemColors.ActiveBorder;
            btnDelRepairs.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
            btnDelRepairs.FlatStyle = FlatStyle.Flat;
            btnDelRepairs.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnDelRepairs.ForeColor = SystemColors.Highlight;
            btnDelRepairs.Location = new Point(358, 102);
            btnDelRepairs.Margin = new Padding(3, 2, 3, 2);
            btnDelRepairs.Name = "btnDelRepairs";
            btnDelRepairs.Size = new Size(167, 33);
            btnDelRepairs.TabIndex = 16;
            btnDelRepairs.Text = "Удалить ремонт";
            btnDelRepairs.UseVisualStyleBackColor = false;
            // 
            // ViewData
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientInactiveCaption;
            ClientSize = new Size(1008, 729);
            Controls.Add(btnDelRepairs);
            Controls.Add(btnEditRepairs);
            Controls.Add(btnAddRepairs);
            Controls.Add(dataGridViewRepairsInfo);
            Controls.Add(dataGridViewRepairs);
            Controls.Add(panelHeader);
            Name = "ViewData";
            Text = "ViewData";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewRepairs).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewRepairsInfo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label labelHeader;
        private DataGridView dataGridViewRepairs;
        private DataGridView dataGridViewRepairsInfo;
        private PictureBox picLogo;
        private Button btnAddRepairs;
        private Button btnEditRepairs;
        private Button btnDelRepairs;
    }
}