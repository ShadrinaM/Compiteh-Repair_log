using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompiTeh_Repair_log.Forms
{
    public partial class SparePartForRepairForm : Form
    {
        private readonly NpgsqlConnection connection;
        private readonly int repairId;
        private readonly Form mainForm;

        public SparePartForRepairForm(NpgsqlConnection conn, Form mainForm, int repairId)
        {
            InitializeComponent();
            this.connection = conn;
            this.mainForm = mainForm;
            this.repairId = repairId;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Настройка кнопок
            btnOK.Click += BtnOK_Click;
            btnCancel.Click += BtnCancel_Click;
            btnAddSparepart.Click += BtnAddSparepart_Click;

            // Загрузка списка запчастей для этого ремонта
            LoadSparepartsForRepair();
        }

        private void LoadSparepartsForRepair()
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                string query = @"
                SELECT 
                    name,
                    price,
                    quantity,
                    (price * quantity) as total
                FROM Sparepart
                WHERE repair_id = @repairId
                ORDER BY name";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@repairId", repairId);

                    // Очищаем и настраиваем dataGridViewSparepart
                    dataGridViewSparepart.Columns.Clear();
                    dataGridViewSparepart.Rows.Clear();

                    // Добавляем колонки
                    dataGridViewSparepart.Columns.Add("Name", "Название запчасти");
                    dataGridViewSparepart.Columns.Add("Price", "Цена");
                    dataGridViewSparepart.Columns.Add("Quantity", "Количество");
                    dataGridViewSparepart.Columns.Add("Total", "Сумма");

                    // Настраиваем форматирование
                    dataGridViewSparepart.Columns["Price"].DefaultCellStyle.Format = "N2";
                    dataGridViewSparepart.Columns["Total"].DefaultCellStyle.Format = "N2";
                    dataGridViewSparepart.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridViewSparepart.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridViewSparepart.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    decimal totalSum = 0;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader.GetString(0);
                            decimal price = reader.GetDecimal(1);
                            int quantity = reader.GetInt32(2);
                            decimal total = reader.GetDecimal(3);
                            totalSum += total;

                            dataGridViewSparepart.Rows.Add(name, price, quantity, total);
                        }
                    }

                    // Добавляем строку с итогом
                    dataGridViewSparepart.Rows.Add("ИТОГО:", "", "", totalSum);
                    dataGridViewSparepart.Rows[dataGridViewSparepart.Rows.Count - 1].DefaultCellStyle.Font =
                        new Font(dataGridViewSparepart.Font, FontStyle.Bold);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке запчастей: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
            
        private void BtnAddSparepart_Click(object sender, EventArgs e)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            var sparepartForm = new SparepartForm(connection, this, repairId);
            if (sparepartForm.ShowDialog() == DialogResult.OK)
            {
                // Обновляем список запчастей после добавления новой
                LoadSparepartsForRepair();
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
