using CompiTeh_Repair_log.Forms;
using Npgsql;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CompiTeh_Repair_log
{
    public partial class ViewData : Form
    {
        private NpgsqlConnection connection;
        private Form mainForm;
        private ContextMenuStrip repairsContextMenu;

        public ViewData(NpgsqlConnection conn, Form menushka)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = menushka;
            connection = conn;
            this.FormClosed += WinForm_FormClosed;

            // Инициализация контекстного меню
            InitializeRepairsContextMenu();

            // Подписываемся на события DataGridView
            dataGridViewRepairs.CellContentClick += dataGridViewRepairs_CellContentClick;
            dataGridViewRepairs.CellClick += DataGridViewRepairs_CellClick;
            dataGridViewRepairs.MouseClick += DataGridViewRepairs_MouseClick;

            // Инициализация DataGridView
            InitializeRepairsDataGridView();

            // Загрузка данных
            LoadRepairsData();
        }

        private void InitializeRepairsContextMenu()
        {
            repairsContextMenu = new ContextMenuStrip();

            // Первый  пункт меню - указать запчасти
            var addSparePartsItem = new ToolStripMenuItem("Указать использованные запчасти");
            addSparePartsItem.Click += AddSparePartsItem_Click;
            repairsContextMenu.Items.Add(addSparePartsItem);

            // Добавляем разделитель
            repairsContextMenu.Items.Add(new ToolStripSeparator());

            // Второй пункт меню - пометить выполненным
            var markCompletedItem = new ToolStripMenuItem("Пометить ремонт выполненным");
            markCompletedItem.Click += MarkCompletedItem_Click;
            repairsContextMenu.Items.Add(markCompletedItem);

            // Третий пункт меню - пометить выданным
            var markIssuedItem = new ToolStripMenuItem("Пометить ремонт выданным");
            markIssuedItem.Click += MarkIssuedItem_Click;
            repairsContextMenu.Items.Add(markIssuedItem);

            // Добавляем разделитель
            repairsContextMenu.Items.Add(new ToolStripSeparator());

            // Четвертый пункт меню - изменить заказ
            var editRepairItem = new ToolStripMenuItem("Изменить заказ");
            editRepairItem.Click += EditRepairItem_Click;
            repairsContextMenu.Items.Add(editRepairItem);

            // Пятый пункт меню - удалить заказ
            var deleteRepairItem = new ToolStripMenuItem("Удалить заказ");
            deleteRepairItem.Click += DeleteRepairItem_Click;
            repairsContextMenu.Items.Add(deleteRepairItem);
        }

        private void MarkCompletedItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewRepairs.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewRepairs.SelectedRows[0];
                int repairId = Convert.ToInt32(selectedRow.Cells["RepairId"].Value);

                // Проверяем статус ремонта
                string currentStatus = GetRepairStatus(repairId);
                if (currentStatus != "принят")
                {
                    MessageBox.Show("Можно пометить выполненным только ремонты со статусом 'принят'", "Ошибка",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Обновляем статус ремонта на "выполнен"
                UpdateRepairStatus(repairId, "выполнен");
                // Обновляем данные в таблице
                LoadRepairsData();

                MessageBox.Show($"Статус ремонта успешно изменен на выполнен", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void AddSparePartsItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewRepairs.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewRepairs.SelectedRows[0];
                int repairId = Convert.ToInt32(selectedRow.Cells["RepairId"].Value);

                // Проверяем статус ремонта
                string currentStatus = GetRepairStatus(repairId);
                if (currentStatus != "принят" && currentStatus != "выполнен")
                {
                    MessageBox.Show("Можно добавлять запчасти только к ремонтам со статусом 'принят' или 'выполнен'",
                                  "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Открываем форму для добавления запчастей
                var sparePartsForm = new SparePartForRepairForm(connection, this, repairId);
                sparePartsForm.ShowDialog();

                // Обновляем данные о запчастях после закрытия формы
                LoadPartsInfoForRepair(repairId);
            }
        }

        // получить статус ремонта по id 
        private string GetRepairStatus(int repairId)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                string query = "SELECT status FROM Repairs WHERE repair_id = @repairId";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@repairId", repairId);
                    return cmd.ExecuteScalar()?.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении статуса ремонта: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        private void MarkIssuedItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewRepairs.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewRepairs.SelectedRows[0];
                int repairId = Convert.ToInt32(selectedRow.Cells["RepairId"].Value);

                // Проверяем статус ремонта
                string currentStatus = GetRepairStatus(repairId);
                if (currentStatus != "выполнен")
                {
                    MessageBox.Show("Можно пометить выданным только ремонты со статусом 'выполнен'",
                                  "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Обновляем статус ремонта на "выдан"
                UpdateRepairStatus(repairId, "выдан");
                LoadRepairsData();

                MessageBox.Show("Статус ремонта успешно изменен на 'выдан'", "Успех",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        // изменить статус ремонта по id 
        private void UpdateRepairStatus(int repairId, string newStatus)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                if (newStatus == "выполнен")
                {
                    string query = "UPDATE Repairs SET status = @status::repair_status_enum WHERE repair_id = @repairId";
                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@status", newStatus);
                        cmd.Parameters.AddWithValue("@repairId", repairId);
                        cmd.ExecuteNonQuery();
                    }
                }

                if (newStatus == "выдан")
                {
                    string query = @"
                        UPDATE Repairs 
                        SET status = @status::repair_status_enum, 
                            completion_date = CASE WHEN @status::repair_status_enum = 'выдан' THEN CURRENT_DATE ELSE completion_date END
                        WHERE repair_id = @repairId";
                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@status", newStatus);
                        cmd.Parameters.AddWithValue("@repairId", repairId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении статуса ремонта: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void DataGridViewRepairs_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hit = dataGridViewRepairs.HitTest(e.X, e.Y);
                if (hit.RowIndex >= 0 && hit.RowIndex < dataGridViewRepairs.Rows.Count)
                {
                    dataGridViewRepairs.ClearSelection();
                    dataGridViewRepairs.Rows[hit.RowIndex].Selected = true;
                    repairsContextMenu.Show(dataGridViewRepairs, e.Location);
                }
            }
        }















        private void InitializeRepairsDataGridView()
        {
            dataGridViewRepairs.Columns.Clear();

            // Сначала добавляем скрытую колонку для repair_id
            dataGridViewRepairs.Columns.Add("RepairId", "Repair ID");
            dataGridViewRepairs.Columns["RepairId"].Visible = true; // Временно видим для отладки

            // Основные колонки
            dataGridViewRepairs.Columns.Add("Device", "Устройство");
            dataGridViewRepairs.Columns.Add("Client", "Заказчик");
            dataGridViewRepairs.Columns.Add("RepairStatus", "Ремонт");

            // Кнопка для запчастей
            var partsButtonColumn = new DataGridViewButtonColumn
            {
                Name = "Parts",
                HeaderText = "Запчасти",
                Text = "Просмотр",
                UseColumnTextForButtonValue = true
            };
            dataGridViewRepairs.Columns.Add(partsButtonColumn);

            // Колонка квитанции
            dataGridViewRepairs.Columns.Add("Receipt", "Квитанция");

            // Настройки
            dataGridViewRepairs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewRepairs.AllowUserToAddRows = false;
            dataGridViewRepairs.ReadOnly = true;
            dataGridViewRepairs.Columns["RepairId"].Visible = false; // Теперь скрываем

            dataGridViewRepairsInfo.AllowUserToAddRows = false; // Добавьте в инициализацию
        }

        private void LoadRepairsData()
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                string query = @"
                SELECT 
                    r.repair_id,
                    CONCAT(d.device_type, ' ', d.manufacturer, ' ', d.model_number) AS device_info,
                    CASE 
                        WHEN c.client_type = 0 THEN c.full_name
                        ELSE COALESCE(c.organization_name, 'Не указано')
                    END AS client_info,
                    r.status,
                    r.acceptance_date,
                    r.completion_date,
                    rc.receipt_id
                FROM Repairs r
                JOIN Devices d ON r.device_id = d.device_id
                JOIN Receipts rc ON r.receipt_id = rc.receipt_id
                JOIN Clients c ON rc.client_id = c.client_id
                ORDER BY r.acceptance_date DESC";

                using (var cmd = new NpgsqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    dataGridViewRepairs.Rows.Clear();

                    while (reader.Read())
                    {
                        var row = new DataGridViewRow();
                        row.CreateCells(dataGridViewRepairs);

                        row.Cells[0].Value = reader.GetInt32(0); // RepairId
                        row.Cells[1].Value = reader.GetString(1); // Device
                        row.Cells[2].Value = reader.IsDBNull(2) ? "Не указан" : reader.GetString(2); // Client

                        string status = reader.GetString(3);
                        DateTime acceptanceDate = reader.GetDateTime(4);
                        string statusInfo = status;

                        if (status == "принят")
                            statusInfo += " " + acceptanceDate.ToString("dd.MM.yyyy");
                        else if (status == "выдан" && !reader.IsDBNull(5))
                            statusInfo += " " + reader.GetDateTime(5).ToString("dd.MM.yyyy");

                        row.Cells[3].Value = statusInfo; // RepairStatus
                        row.Cells[5].Value = reader.GetInt32(6).ToString(); // Receipt

                        dataGridViewRepairs.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void dataGridViewRepairs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dataGridViewRepairs.Columns["Parts"].Index)
                return;

            var repairId = Convert.ToInt32(dataGridViewRepairs.Rows[e.RowIndex].Cells["RepairId"].Value);

            //// вывод запчастей в отдельную форму
            //ShowPartsForRepair(repairId);

            // вывод в dataGridViewRepairsInfo 
            LoadPartsInfoForRepair(repairId);

        }


        // вывод запчастей в отдельную форму
        //private void ShowPartsForRepair(int repairId)
        //{
        //    try
        //    {
        //        if (connection.State != ConnectionState.Open)
        //            connection.Open();

        //        string query = @"
        //    SELECT 
        //        name,
        //        price,
        //        quantity
        //    FROM Sparepart
        //    WHERE repair_id = @repairId
        //    ORDER BY name";

        //        using (var cmd = new NpgsqlCommand(query, connection))
        //        {
        //            cmd.Parameters.AddWithValue("@repairId", repairId);

        //            DataTable partsTable = new DataTable();
        //            partsTable.Columns.Add("Название", typeof(string));
        //            partsTable.Columns.Add("Цена", typeof(decimal));
        //            partsTable.Columns.Add("Количество", typeof(int));
        //            partsTable.Columns.Add("Сумма", typeof(decimal));

        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    string name = reader.GetString(0);
        //                    decimal price = reader.GetDecimal(1);
        //                    int quantity = reader.GetInt32(2);
        //                    decimal total = price * quantity;

        //                    partsTable.Rows.Add(name, price, quantity, total);
        //                }
        //            }

        //            // Создаем новую форму для отображения запчастей
        //            Form partsForm = new Form();
        //            partsForm.Text = $"Запчасти для ремонта ID: {repairId}";
        //            partsForm.StartPosition = FormStartPosition.CenterParent;
        //            partsForm.Size = new Size(500, 400);

        //            DataGridView partsGridView = new DataGridView();
        //            partsGridView.Dock = DockStyle.Fill;
        //            partsGridView.ReadOnly = true;
        //            partsGridView.AutoGenerateColumns = false;
        //            partsGridView.DataSource = partsTable;

        //            // Настраиваем колонки
        //            partsGridView.Columns.Add(new DataGridViewTextBoxColumn()
        //            {
        //                DataPropertyName = "Название",
        //                HeaderText = "Название запчасти",
        //                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        //            });

        //            partsGridView.Columns.Add(new DataGridViewTextBoxColumn()
        //            {
        //                DataPropertyName = "Цена",
        //                HeaderText = "Цена за единицу",
        //                DefaultCellStyle = new DataGridViewCellStyle() { Format = "N2" }
        //            });

        //            partsGridView.Columns.Add(new DataGridViewTextBoxColumn()
        //            {
        //                DataPropertyName = "Количество",
        //                HeaderText = "Количество"
        //            });

        //            partsGridView.Columns.Add(new DataGridViewTextBoxColumn()
        //            {
        //                DataPropertyName = "Сумма",
        //                HeaderText = "Общая стоимость",
        //                DefaultCellStyle = new DataGridViewCellStyle() { Format = "N2" }
        //            });

        //            // Добавляем DataGridView на форму
        //            partsForm.Controls.Add(partsGridView);

        //            // Рассчитываем и добавляем итоговую сумму
        //            decimal totalSum = partsTable.AsEnumerable()
        //                .Sum(row => row.Field<decimal>("Сумма"));

        //            Label totalLabel = new Label();
        //            totalLabel.Text = $"Итого: {totalSum:N2} руб.";
        //            totalLabel.Dock = DockStyle.Bottom;
        //            totalLabel.TextAlign = ContentAlignment.MiddleRight;
        //            totalLabel.Font = new Font(totalLabel.Font, FontStyle.Bold);
        //            partsForm.Controls.Add(totalLabel);

        //            partsForm.ShowDialog();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка при загрузке запчастей: {ex.Message}", "Ошибка",
        //                      MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();
        //    }
        //}


        private void LoadPartsInfoForRepair(int repairId)
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

                    // Очищаем и настраиваем dataGridViewRepairsInfo
                    dataGridViewRepairsInfo.Columns.Clear();
                    dataGridViewRepairsInfo.Rows.Clear();

                    // Добавляем колонки
                    dataGridViewRepairsInfo.Columns.Add("Name", "Название запчасти");
                    dataGridViewRepairsInfo.Columns.Add("Price", "Цена");
                    dataGridViewRepairsInfo.Columns.Add("Quantity", "Количество");
                    dataGridViewRepairsInfo.Columns.Add("Total", "Сумма");

                    // Настраиваем форматирование
                    dataGridViewRepairsInfo.Columns["Price"].DefaultCellStyle.Format = "N2";
                    dataGridViewRepairsInfo.Columns["Total"].DefaultCellStyle.Format = "N2";
                    dataGridViewRepairsInfo.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridViewRepairsInfo.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridViewRepairsInfo.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

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

                            dataGridViewRepairsInfo.Rows.Add(name, price, quantity, total);
                        }
                    }

                    // Добавляем строку с итогом
                    dataGridViewRepairsInfo.Rows.Add("ИТОГО:", "", "", totalSum);
                    dataGridViewRepairsInfo.Rows[dataGridViewRepairsInfo.Rows.Count - 1].DefaultCellStyle.Font =
                        new Font(dataGridViewRepairsInfo.Font, FontStyle.Bold);
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

        private void WinForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Show();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
















        private void DataGridViewRepairs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex < 0 || e.ColumnIndex != dataGridViewRepairs.Columns["Device"].Index)
            //    return;

            //int deviceId = GetDeviceIdFromRepair(e.RowIndex);
            //if (deviceId > 0)
            //{
            //    ShowDeviceDetails(deviceId);
            //}








            //if (e.RowIndex < 0) return;

            //// Обработка клика по столбцу "Заказчик"
            //if (e.ColumnIndex == dataGridViewRepairs.Columns["Client"].Index)
            //{
            //    int clientId = GetClientIdFromRepair(e.RowIndex);
            //    if (clientId > 0)
            //    {
            //        ShowClientDetails(clientId);
            //    }
            //}
            //// Обработка клика по столбцу "Устройство" (из предыдущего кода)
            //else if (e.ColumnIndex == dataGridViewRepairs.Columns["Device"].Index)
            //{
            //    int deviceId = GetDeviceIdFromRepair(e.RowIndex);
            //    if (deviceId > 0)
            //    {
            //        ShowDeviceDetails(deviceId);
            //    }
            //}




            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == dataGridViewRepairs.Columns["RepairStatus"].Index)
            {
                int repairId = Convert.ToInt32(dataGridViewRepairs.Rows[e.RowIndex].Cells["RepairId"].Value);
                ShowRepairDetails(repairId);
            }
            else if (e.ColumnIndex == dataGridViewRepairs.Columns["Client"].Index)
            {
                int clientId = GetClientIdFromRepair(e.RowIndex);
                if (clientId > 0) ShowClientDetails(clientId);
            }
            else if (e.ColumnIndex == dataGridViewRepairs.Columns["Device"].Index)
            {
                int deviceId = GetDeviceIdFromRepair(e.RowIndex);
                if (deviceId > 0) ShowDeviceDetails(deviceId);
            }

        }

        private int GetDeviceIdFromRepair(int rowIndex)
        {
            int repairId = Convert.ToInt32(dataGridViewRepairs.Rows[rowIndex].Cells["RepairId"].Value);

            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                string query = "SELECT device_id FROM Repairs WHERE repair_id = @repairId";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@repairId", repairId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении ID устройства: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void ShowDeviceDetails(int deviceId)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                string query = @"
            SELECT 
                device_type,
                manufacturer,
                model_number,
                serial_number,
                completeness,
                fault_description,
                device_notes
            FROM Devices
            WHERE device_id = @deviceId";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@deviceId", deviceId);

                    // Очищаем и настраиваем dataGridViewRepairsInfo
                    dataGridViewRepairsInfo.Columns.Clear();
                    dataGridViewRepairsInfo.Rows.Clear();

                    // Добавляем колонки для отображения параметров
                    dataGridViewRepairsInfo.Columns.Add("Parameter", "Параметр");
                    dataGridViewRepairsInfo.Columns.Add("Value", "Значение");
                    dataGridViewRepairsInfo.Columns["Parameter"].Width = 150;
                    dataGridViewRepairsInfo.Columns["Value"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            AddDeviceDetailRow("Тип устройства", reader.GetString(0));
                            AddDeviceDetailRow("Производитель", reader.GetString(1));
                            AddDeviceDetailRow("Модель", reader.GetString(2));
                            AddDeviceDetailRow("Серийный номер", reader.GetString(3));
                            AddDeviceDetailRow("Комплектность", reader.IsDBNull(4) ? "Не указана" : reader.GetString(4));
                            AddDeviceDetailRow("Неисправность", reader.IsDBNull(5) ? "Не указана" : reader.GetString(5));
                            AddDeviceDetailRow("Примечания", reader.IsDBNull(6) ? "Нет примечаний" : reader.GetString(6));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных устройства: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void AddDeviceDetailRow(string parameter, string value)
        {
            int rowIndex = dataGridViewRepairsInfo.Rows.Add(parameter, value);
            // Можно добавить стилизацию для определенных строк
            if (parameter == "Примечания")
            {
                dataGridViewRepairsInfo.Rows[rowIndex].DefaultCellStyle.Font =
                    new Font(dataGridViewRepairsInfo.Font, FontStyle.Italic);
            }
        }


























        private int GetClientIdFromRepair(int rowIndex)
        {
            int repairId = Convert.ToInt32(dataGridViewRepairs.Rows[rowIndex].Cells["RepairId"].Value);

            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                string query = @"
            SELECT rc.client_id 
            FROM Repairs r
            JOIN Receipts rc ON r.receipt_id = rc.receipt_id
            WHERE r.repair_id = @repairId";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@repairId", repairId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении ID клиента: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void ShowClientDetails(int clientId)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                string query = @"
            SELECT 
                client_type,
                full_name,
                contact_phone,
                email,
                organization_name,
                client_notes
            FROM Clients
            WHERE client_id = @clientId";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@clientId", clientId);

                    // Очищаем и настраиваем dataGridViewRepairsInfo
                    dataGridViewRepairsInfo.Columns.Clear();
                    dataGridViewRepairsInfo.Rows.Clear();

                    // Добавляем колонки
                    dataGridViewRepairsInfo.Columns.Add("Parameter", "Параметр");
                    dataGridViewRepairsInfo.Columns.Add("Value", "Значение");
                    dataGridViewRepairsInfo.Columns["Parameter"].Width = 150;
                    dataGridViewRepairsInfo.Columns["Value"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string clientType = reader.GetInt32(0) == 0 ? "Физическое лицо" : "Юридическое лицо";

                            AddClientDetailRow("Тип клиента", clientType);
                            AddClientDetailRow("ФИО / Название", reader.GetString(1));
                            AddClientDetailRow("Контактный телефон", reader.GetString(2));
                            AddClientDetailRow("Email", reader.IsDBNull(3) ? "Не указан" : reader.GetString(3));

                            if (reader.GetInt32(0) == 1) // Для юр. лиц
                            {
                                AddClientDetailRow("Название организации",
                                                reader.IsDBNull(4) ? "Не указано" : reader.GetString(4));
                            }

                            AddClientDetailRow("Примечания", reader.IsDBNull(5) ? "Нет примечаний" : reader.GetString(5));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных клиента: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void AddClientDetailRow(string parameter, string value)
        {
            int rowIndex = dataGridViewRepairsInfo.Rows.Add(parameter, value);

            // Стилизация для важных полей
            if (parameter == "ФИО / Название" || parameter == "Контактный телефон")
            {
                dataGridViewRepairsInfo.Rows[rowIndex].DefaultCellStyle.Font =
                    new Font(dataGridViewRepairsInfo.Font, FontStyle.Bold);
            }
        }
























        private void ShowRepairDetails(int repairId)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                string query = @"
            SELECT 
                r.status,
                r.acceptance_date,
                r.completion_date,
                r.work_performed,
                r.repair_notes
            FROM Repairs r
            WHERE r.repair_id = @repairId";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@repairId", repairId);

                    // Очищаем и настраиваем dataGridViewRepairsInfo
                    dataGridViewRepairsInfo.Columns.Clear();
                    dataGridViewRepairsInfo.Rows.Clear();

                    // Добавляем колонки
                    dataGridViewRepairsInfo.Columns.Add("Parameter", "Параметр");
                    dataGridViewRepairsInfo.Columns.Add("Value", "Значение");
                    dataGridViewRepairsInfo.Columns["Parameter"].Width = 200;
                    dataGridViewRepairsInfo.Columns["Value"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Основная информация о ремонте
                            AddRepairDetailRow("Статус ремонта", reader.GetString(0));
                            AddRepairDetailRow("Дата принятия", reader.GetDateTime(1).ToString("dd.MM.yyyy"));

                            if (!reader.IsDBNull(2))
                                AddRepairDetailRow("Дата завершения", reader.GetDateTime(2).ToString("dd.MM.yyyy"));
                            else
                                AddRepairDetailRow("Дата завершения", "В процессе");

                            AddRepairDetailRow("Проведённые работы", reader.GetString(3));

                            if (!reader.IsDBNull(4))
                                AddRepairDetailRow("Примечания к ремонту", reader.GetString(4));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных ремонта: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void AddRepairDetailRow(string parameter, string value)
        {
            int rowIndex = dataGridViewRepairsInfo.Rows.Add(parameter, value);

            // Стилизация важных полей
            switch (parameter)
            {
                case "Статус ремонта":
                    dataGridViewRepairsInfo.Rows[rowIndex].DefaultCellStyle.Font =
                        new Font(dataGridViewRepairsInfo.Font, FontStyle.Bold);

                    if (value == "выполнен")
                        dataGridViewRepairsInfo.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                    else if (value == "выдан")
                        dataGridViewRepairsInfo.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    break;

                case "Описание неисправности":
                    dataGridViewRepairsInfo.Rows[rowIndex].DefaultCellStyle.Font =
                        new Font(dataGridViewRepairsInfo.Font, FontStyle.Italic);
                    break;
            }
        }










        private void btnAddRepairs_Click(object sender, EventArgs e)
        {
            ReceiptsForms receiptsForm = new ReceiptsForms(connection, this);
            if (receiptsForm.ShowDialog() == DialogResult.OK)
            {
                // Обновляем данные после закрытия формы
                LoadRepairsData();
            }
        }










        // Получение receipt_id из repair_id
        private int GetReceiptIdFromRepair(int repairId)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                string query = "SELECT receipt_id FROM Repairs WHERE repair_id = @repairId";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@repairId", repairId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении ID квитанции: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        // Изменение заказа
        private void EditRepairItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewRepairs.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewRepairs.SelectedRows[0];
                int repairId = Convert.ToInt32(selectedRow.Cells["RepairId"].Value);

                // Получаем receipt_id из repair_id
                int receiptId = GetReceiptIdFromRepair(repairId);

                if (receiptId > 0)
                {
                    // Открываем форму ReceiptsForms в режиме редактирования
                    var receiptsForm = new ReceiptsForms(connection, this, true, receiptId);
                    
                    // Используем ShowDialog() вместо Show() для ожидания закрытия формы
                    if (receiptsForm.ShowDialog() == DialogResult.OK)
                    {
                        // Обновляем данные после закрытия формы редактирования
                        LoadRepairsData();
                    }

                    this.Show(); // Показываем текущую форму
                }
                else
                {
                    MessageBox.Show("Не удалось найти квитанцию для данного ремонта", "Ошибка",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }





        // Получение информации о заказе
        private (int receiptId, int repairsCount, string clientInfo) GetReceiptInfo(int repairId)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                string query = @"
        SELECT 
            r.receipt_id,
            (SELECT COUNT(*) FROM Repairs WHERE receipt_id = r.receipt_id) as repairs_count,
            CASE 
                WHEN c.client_type = 0 THEN c.full_name
                ELSE COALESCE(c.organization_name, c.full_name)
            END as client_info
        FROM Repairs rep
        JOIN Receipts r ON rep.receipt_id = r.receipt_id
        JOIN Clients c ON r.client_id = c.client_id
        WHERE rep.repair_id = @repairId";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@repairId", repairId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (
                                reader.GetInt32(0),
                                reader.GetInt32(1),
                                reader.GetString(2)
                            );
                        }
                    }
                }
                return (-1, 0, "");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении информации о заказе: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (-1, 0, "");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        // Обработчик удаления заказа
        private void DeleteRepairItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewRepairs.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewRepairs.SelectedRows[0];
                int repairId = Convert.ToInt32(selectedRow.Cells["RepairId"].Value);

                // Получаем информацию о заказе
                var (receiptId, repairsCount, clientInfo) = GetReceiptInfo(repairId);

                if (receiptId <= 0)
                {
                    MessageBox.Show("Не удалось получить информацию о заказе", "Ошибка",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Создаем сообщение подтверждения
                string message = $"При удалении заказа #{receiptId} для клиента '{clientInfo}' " +
                                $"удалятся все ремонты ({repairsCount} шт.), в него входящие.\n\n" +
                                "Вы уверены, что хотите удалить весь заказ?\n\n" +
                                "Для того чтобы удалить один ремонт, зайдите в редактирование заказа.";

                var result = MessageBox.Show(message, "Подтверждение удаления заказа",
                                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                           MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    DeleteReceiptWithRepairs(receiptId);
                }
            }
        }

        // Удаление заказа с ремонтами
        private void DeleteReceiptWithRepairs(int receiptId)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Сначала удаляем все запчасти, связанные с ремонтами этого заказа
                        string deleteSparePartsQuery = @"
                DELETE FROM Sparepart 
                WHERE repair_id IN (
                    SELECT repair_id FROM Repairs WHERE receipt_id = @receiptId
                )";

                        using (var cmd = new NpgsqlCommand(deleteSparePartsQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@receiptId", receiptId);
                            cmd.ExecuteNonQuery();
                        }

                        // 2. Удаляем все ремонты этого заказа
                        string deleteRepairsQuery = "DELETE FROM Repairs WHERE receipt_id = @receiptId";
                        using (var cmd = new NpgsqlCommand(deleteRepairsQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@receiptId", receiptId);
                            cmd.ExecuteNonQuery();
                        }

                        // 3. Удаляем саму квитанцию
                        string deleteReceiptQuery = "DELETE FROM Receipts WHERE receipt_id = @receiptId";
                        using (var cmd = new NpgsqlCommand(deleteReceiptQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@receiptId", receiptId);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                transaction.Commit();
                                MessageBox.Show("Заказ и все связанные ремонты успешно удалены", "Успех",
                                              MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Обновляем данные
                                LoadRepairsData();
                            }
                            else
                            {
                                transaction.Rollback();
                                MessageBox.Show("Не удалось найти заказ для удаления", "Ошибка",
                                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Ошибка при удалении заказа: {ex.Message}", "Ошибка",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

    }
}