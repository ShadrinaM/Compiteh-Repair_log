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
        private ContextMenuStrip contextMenuStrip;

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

            // Инициализация контекстного меню
            InitializeContextMenu();

            // Загрузка списка запчастей для этого ремонта
            LoadSparepartsForRepair();
        }

        // Инициализация контекстного меню
        private void InitializeContextMenu()
        {
            // Создаем контекстное меню
            contextMenuStrip = new ContextMenuStrip();

            // Добавляем пункт "Изменить"
            ToolStripMenuItem editMenuItem = new ToolStripMenuItem("Изменить");
            editMenuItem.Click += EditMenuItem_Click;
            contextMenuStrip.Items.Add(editMenuItem);

            // Добавляем пункт "Удалить"
            ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem("Удалить");
            deleteMenuItem.Click += DeleteMenuItem_Click;
            contextMenuStrip.Items.Add(deleteMenuItem);

            // Привязываем контекстное меню к DataGridView
            dataGridViewSparepart.ContextMenuStrip = contextMenuStrip;

            // Обрабатываем событие нажатия правой кнопки мыши
            dataGridViewSparepart.MouseDown += DataGridViewSparepart_MouseDown;
        }

        // Обработка ПКМ по DataGridViewSparepart
        private void DataGridViewSparepart_MouseDown(object sender, MouseEventArgs e)
        {
            // Показываем контекстное меню только при нажатии правой кнопки мыши
            // и только если выделена какая-то строка (кроме итоговой)
            if (e.Button == MouseButtons.Right)
            {
                var hitTest = dataGridViewSparepart.HitTest(e.X, e.Y);
                if (hitTest.RowIndex >= 0 &&
                    hitTest.RowIndex < dataGridViewSparepart.Rows.Count - 1 && // Исключаем итоговую строку
                    !dataGridViewSparepart.Rows[hitTest.RowIndex].IsNewRow)
                {
                    dataGridViewSparepart.ClearSelection();
                    dataGridViewSparepart.Rows[hitTest.RowIndex].Selected = true;
                    contextMenuStrip.Show(dataGridViewSparepart, e.Location);
                }
            }
        }

        // "Изменить" в контекстном меню
        private void EditMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewSparepart.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridViewSparepart.SelectedRows[0].Index;

                // Проверяем, что это не итоговая строка
                if (selectedIndex < dataGridViewSparepart.Rows.Count - 1)
                {
                    try
                    {
                        // Получаем sparepart_id из скрытого столбца
                        int sparepartId = Convert.ToInt32(dataGridViewSparepart.Rows[selectedIndex].Cells["sparepart_id"].Value);

                        // Открываем форму редактирования
                        var editForm = new SparepartForm(connection, this, repairId, true, sparepartId);
                        if (editForm.ShowDialog() == DialogResult.OK)
                        {
                            // Обновляем список после редактирования
                            LoadSparepartsForRepair();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при открытии формы редактирования: {ex.Message}", "Ошибка",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // "Удалить" в контекстном меню
        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewSparepart.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridViewSparepart.SelectedRows[0].Index;

                // Проверяем, что это не итоговая строка
                if (selectedIndex < dataGridViewSparepart.Rows.Count - 1)
                {
                    string sparepartName = dataGridViewSparepart.Rows[selectedIndex].Cells["Name"].Value.ToString();
                    int sparepartId = Convert.ToInt32(dataGridViewSparepart.Rows[selectedIndex].Cells["sparepart_id"].Value);

                    // Запрос подтверждения удаления
                    DialogResult result = MessageBox.Show(
                        $"Вы уверены, что хотите удалить запчасть '{sparepartName}'?",
                        "Подтверждение удаления",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        DeleteSparepart(sparepartId);
                    }
                }
            }
        }

        private void DeleteSparepart(int sparepartId)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                string query = "DELETE FROM Sparepart WHERE sparepart_id = @sparepartId";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@sparepartId", sparepartId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Запчасть успешно удалена", "Успех",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSparepartsForRepair(); // Обновляем список
                    }
                    else
                    {
                        MessageBox.Show("Запчасть не найдена", "Ошибка",
                                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении запчасти: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void LoadSparepartsForRepair()
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                string query = @"
                SELECT 
                    sparepart_id,
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

                    // Добавляем колонки (включая скрытый столбец для sparepart_id)
                    dataGridViewSparepart.Columns.Add("sparepart_id", "ID");
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

                    // Скрываем столбец с ID
                    dataGridViewSparepart.Columns["sparepart_id"].Visible = false;

                    decimal totalSum = 0;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int sparepartId = reader.GetInt32(0);        // Индекс 0: sparepart_id
                            string name = reader.GetString(1);           // Индекс 1: name
                            decimal price = reader.GetDecimal(2);        // Индекс 2: price
                            int quantity = reader.GetInt32(3);           // Индекс 3: quantity
                            decimal total = reader.GetDecimal(4);        // Индекс 4: total
                            totalSum += total;

                            dataGridViewSparepart.Rows.Add(sparepartId, name, price, quantity, total);
                        }
                    }

                    // Добавляем строку с итогом (5 столбцов, поэтому нужно 5 значений)
                    dataGridViewSparepart.Rows.Add(null, "ИТОГО:", "", "", totalSum);
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
