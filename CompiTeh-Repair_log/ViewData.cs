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

        public ViewData(NpgsqlConnection conn, Form menushka)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = menushka;
            connection = conn;
            this.FormClosed += WinForm_FormClosed;

            // Подписываемся на событие клика по ячейкам DataGridView
            dataGridViewRepairs.CellContentClick += dataGridViewRepairs_CellContentClick;
            dataGridViewRepairs.CellClick += DataGridViewRepairs_CellClick;


            // Инициализация DataGridView
            InitializeRepairsDataGridView();

            // Загрузка данных
            LoadRepairsData();
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
            ReceiptsForms ReceiptsForma = new ReceiptsForms(connection, this);
            ReceiptsForma.Show();
            this.Hide();
        }
    }

}