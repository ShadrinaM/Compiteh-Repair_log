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
    public partial class SparepartForm : Form
    {
        private readonly NpgsqlConnection connection;
        private readonly bool isEditMode;
        private readonly int sparepartId;
        private readonly Form mainForm;
        private readonly int? repairId;

        public int LastAddedSparepartId { get; private set; }

        public SparepartForm(NpgsqlConnection conn, Form mainForm, int? repairId = null, bool editMode = false, int existingSparepartId = 0)
        {
            InitializeComponent();
            this.connection = conn;
            this.mainForm = mainForm;
            this.repairId = repairId;
            this.isEditMode = editMode;
            this.sparepartId = existingSparepartId;
            this.LastAddedSparepartId = 0;

            this.StartPosition = FormStartPosition.CenterScreen;
            ConfigureForm();

            if (isEditMode)
                LoadExistingSparepart();

            this.FormClosed += SparepartForm_FormClosed;
        }

        private void ConfigureForm()
        {
            this.Text = isEditMode ? "Редактирование запчасти" : "Добавление запчасти";
            labelHeader.Text = isEditMode ? "Редактирование запчасти" : "Добавление запчасти";
            btnOK.Text = isEditMode ? "Сохранить изменения" : "Добавить";
        }

        private void LoadExistingSparepart()
        {
            try
            {
                string query = @"SELECT name, price, quantity 
                           FROM Sparepart 
                           WHERE sparepart_id = @sparepartId";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@sparepartId", sparepartId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBoxName.Text = reader.GetString(0);
                            numericPrice.Value = reader.GetDecimal(1);
                            numericQuantity.Value = reader.GetInt32(2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных запчасти: " + ex.Message);
                this.Close();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                MessageBox.Show("Название запчасти является обязательным полем.");
                return;
            }

            if (numericPrice.Value <= 0)
            {
                MessageBox.Show("Цена должна быть больше нуля.");
                return;
            }

            if (numericQuantity.Value <= 0)
            {
                MessageBox.Show("Количество должно быть больше нуля.");
                return;
            }

            try
            {
                string name = textBoxName.Text;
                decimal price = numericPrice.Value;
                int quantity = (int)numericQuantity.Value;

                if (isEditMode)
                    UpdateSparepart(name, price, quantity);
                else
                    InsertSparepart(name, price, quantity);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }

        private void InsertSparepart(string name, decimal price, int quantity)
        {
            string query = @"INSERT INTO Sparepart 
                (repair_id, name, price, quantity)
                VALUES (@repairId, @name, @price, @quantity)
                RETURNING sparepart_id";

            using (var cmd = new NpgsqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@repairId", repairId.HasValue ? (object)repairId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@quantity", quantity);

                LastAddedSparepartId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            MessageBox.Show("Запчасть добавлена успешно.");
        }

        private void UpdateSparepart(string name, decimal price, int quantity)
        {
            string query = @"UPDATE Sparepart 
                       SET name = @name, 
                           price = @price,
                           quantity = @quantity
                       WHERE sparepart_id = @sparepartId";

            using (var cmd = new NpgsqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@sparepartId", sparepartId);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Изменения успешно сохранены.");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void SparepartForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Show();
        }
    }
}
