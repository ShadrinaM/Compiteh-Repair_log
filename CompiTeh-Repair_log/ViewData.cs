using CompiTeh_Repair_log.Forms;
using Microsoft.Office.Interop.Excel;
using Npgsql;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace CompiTeh_Repair_log
{
    public partial class ViewData : Form
    {
        private NpgsqlConnection connection;

        private ContextMenuStrip repairsContextMenu;

        public ViewData()
        {
            Connect();
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosed += WinForm_FormClosed;

            // Инициализация контекстного меню
            InitializeRepairsContextMenu();

            // Подписываемся на события DataGridView
            dataGridViewRepairs.CellContentClick += dataGridViewRepairs_CellContentClick;
            dataGridViewRepairs.CellClick += DataGridViewRepairs_CellClick;
            dataGridViewRepairs.MouseClick += DataGridViewRepairs_MouseClick;

            // Инициализация DataGridView
            InitializeRepairsDataGridView();

            // Загрузка данных в DataGridView
            LoadRepairsData();
        }

        public void Connect()
        {
            // Подключение к БД
            try
            {
                connection = new NpgsqlConnection("Server=localhost;Port=5432;User ID=postgres;Password=24072012;Database=RepairLogDB");
                connection.Open();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных:\n{ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Обработчик закрытия формы
        private void WinForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //mainForm.Show();
        }

        // Возврат в предыдущее окно
        private void btnBack_Click(object sender, EventArgs e)
        {
           // this.Close();
        }

        // Инициализация контекстного меню
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

        // Инициализация dataGridViewRepairs
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
            var receiptButtonColumn = new DataGridViewButtonColumn
            {
                Name = "Receipt",
                HeaderText = "Квитанция",
                Text = "Сгенерировать",
                UseColumnTextForButtonValue = false
            };
            dataGridViewRepairs.Columns.Add(receiptButtonColumn);

            // Настройки
            dataGridViewRepairs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewRepairs.AllowUserToAddRows = false;
            dataGridViewRepairs.ReadOnly = true;
            dataGridViewRepairs.Columns["RepairId"].Visible = false; // Теперь скрываем

            dataGridViewRepairsInfo.AllowUserToAddRows = false; // Добавьте в инициализацию
        }

        // Заполнение dataGridViewRepairs
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
                    r.completion_date
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

                        int repairId = reader.GetInt32(0); // RepairId
                        row.Cells[0].Value = repairId;
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
                        row.Cells[5].Value = $"Сгенерировать квитанцию № {repairId}";

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



        /* ////////////////////////// ПКМ DataGridViewRepairs ////////////////////////// */

        // Обработка ПКМ по dataGridViewRepairs
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


        // Добавлние запчастей
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


        /* /////// CRUD ДЛЯ РЕМОНТОВ /////// */

        // Обработчик кнопки добавления заказа
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

        // Обработчик нажатия изменения заказа
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

        // Обработчик нажатия удаления заказа
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


        /* /////// СВЯЗАННОЕ С СТАТУСАМИ РЕМОНТА /////// */

        // Изменение стутуса заказа на выполнен
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

        // Изменение стутуса заказа на выдан
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

        // Изменить статус ремонта по id на заданное значение
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

        // Получить статус ремонта по id 
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



        /* ////////////////////////// ЛКМ DataGridViewRepairs ////////////////////////// */

        /* /////// Обработка ЛКМ по контенту в dataGridViewRepairs /////// */
        private void dataGridViewRepairs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var repairId = Convert.ToInt32(dataGridViewRepairs.Rows[e.RowIndex].Cells["RepairId"].Value);

            // Обработка клика по кнопке "Запчасти"
            if (e.ColumnIndex == dataGridViewRepairs.Columns["Parts"].Index)
            {
                // вывод в dataGridViewRepairsInfo 
                LoadPartsInfoForRepair(repairId);
            }
            // Обработка клика по кнопке "Квитанция"
            else if (e.ColumnIndex == dataGridViewRepairs.Columns["Receipt"].Index)
            {
                // Вызываем функцию генерации квитанции и передаем repairId
                BtnGener_Click(repairId);
            }
        }

        // Вывод запчастей в dataGridViewRepairsInfo
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
                        new System.Drawing.Font(dataGridViewRepairsInfo.Font, FontStyle.Bold);
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

        // Вывод запчастей в отдельную форму
        /*
        private void ShowPartsForRepair(int repairId)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                string query = @"
                SELECT 
                    name,
                    price,
                    quantity
                FROM Sparepart
                WHERE repair_id = @repairId
                ORDER BY name";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@repairId", repairId);

                    DataTable partsTable = new DataTable();
                    partsTable.Columns.Add("Название", typeof(string));
                    partsTable.Columns.Add("Цена", typeof(decimal));
                    partsTable.Columns.Add("Количество", typeof(int));
                    partsTable.Columns.Add("Сумма", typeof(decimal));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader.GetString(0);
                            decimal price = reader.GetDecimal(1);
                            int quantity = reader.GetInt32(2);
                            decimal total = price * quantity;

                            partsTable.Rows.Add(name, price, quantity, total);
                        }
                    }

                    // Создаем новую форму для отображения запчастей
                    Form partsForm = new Form();
                    partsForm.Text = $"Запчасти для ремонта ID: {repairId}";
                    partsForm.StartPosition = FormStartPosition.CenterParent;
                    partsForm.Size = new Size(500, 400);

                    DataGridView partsGridView = new DataGridView();
                    partsGridView.Dock = DockStyle.Fill;
                    partsGridView.ReadOnly = true;
                    partsGridView.AutoGenerateColumns = false;
                    partsGridView.DataSource = partsTable;

                    // Настраиваем колонки
                    partsGridView.Columns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Название",
                        HeaderText = "Название запчасти",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    });

                    partsGridView.Columns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Цена",
                        HeaderText = "Цена за единицу",
                        DefaultCellStyle = new DataGridViewCellStyle() { Format = "N2" }
                    });

                    partsGridView.Columns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Количество",
                        HeaderText = "Количество"
                    });

                    partsGridView.Columns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Сумма",
                        HeaderText = "Общая стоимость",
                        DefaultCellStyle = new DataGridViewCellStyle() { Format = "N2" }
                    });

                    // Добавляем DataGridView на форму
                    partsForm.Controls.Add(partsGridView);

                    // Рассчитываем и добавляем итоговую сумму
                    decimal totalSum = partsTable.AsEnumerable()
                        .Sum(row => row.Field<decimal>("Сумма"));

                    Label totalLabel = new Label();
                    totalLabel.Text = $"Итого: {totalSum:N2} руб.";
                    totalLabel.Dock = DockStyle.Bottom;
                    totalLabel.TextAlign = ContentAlignment.MiddleRight;
                    totalLabel.Font = new Font(totalLabel.Font, FontStyle.Bold);
                    partsForm.Controls.Add(totalLabel);

                    partsForm.ShowDialog();
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
        */



        /* /////// Обработка ЛКМ по ячейкам DataGridViewRepairs /////// */
        private void DataGridViewRepairs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
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



        /* /////// ПОДРОБНОСТИ УСТРОЙСТВА В dataGridViewRepairsInfo /////// */

        // Получить device_id по repair_id
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

        // Показать подробности устройства в dataGridViewRepairsInfo
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

        // Добавление в dataGridViewRepairsInfo строки для устройства
        private void AddDeviceDetailRow(string parameter, string value)
        {
            int rowIndex = dataGridViewRepairsInfo.Rows.Add(parameter, value);
            // Можно добавить стилизацию для определенных строк
            if (parameter == "Примечания")
            {
                dataGridViewRepairsInfo.Rows[rowIndex].DefaultCellStyle.Font =
                    new System.Drawing.Font(dataGridViewRepairsInfo.Font, FontStyle.Italic);
            }
        }



        /* /////// ПОДРОБНОСТИ КЛИЕНТА В dataGridViewRepairsInfo /////// */

        // Получить client_id по repair_id
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

        // Показать подробности клиента в dataGridViewRepairsInfo
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

        // Добавление в dataGridViewRepairsInfo строки для клиента
        private void AddClientDetailRow(string parameter, string value)
        {
            int rowIndex = dataGridViewRepairsInfo.Rows.Add(parameter, value);

            // Стилизация для важных полей
            if (parameter == "ФИО / Название" || parameter == "Контактный телефон")
            {
                dataGridViewRepairsInfo.Rows[rowIndex].DefaultCellStyle.Font =
                    new System.Drawing.Font(dataGridViewRepairsInfo.Font, FontStyle.Bold);
            }
        }



        /* /////// ПОДРОБНОСТИ РЕМОНТА В dataGridViewRepairsInfo /////// */

        // Показать подробности ремонта в dataGridViewRepairsInfo
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

        // Добавление в dataGridViewRepairsInfo строки для ремонта
        private void AddRepairDetailRow(string parameter, string value)
        {
            int rowIndex = dataGridViewRepairsInfo.Rows.Add(parameter, value);

            // Стилизация важных полей
            switch (parameter)
            {
                case "Статус ремонта":
                    dataGridViewRepairsInfo.Rows[rowIndex].DefaultCellStyle.Font =
                        new System.Drawing.Font(dataGridViewRepairsInfo.Font, FontStyle.Bold);

                    if (value == "выполнен")
                        dataGridViewRepairsInfo.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                    else if (value == "выдан")
                        dataGridViewRepairsInfo.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    break;

                case "Описание неисправности":
                    dataGridViewRepairsInfo.Rows[rowIndex].DefaultCellStyle.Font =
                        new System.Drawing.Font(dataGridViewRepairsInfo.Font, FontStyle.Italic);
                    break;
            }
        }



        /* ////////////////////////// ВСЁ СВЯЗАННОЕ С ГЕНЕРАЦИЕЙ КВИТАНЦИИ ////////////////////////// */

        /* Генерация файла квитанции в директорию рядом с exe */
        /*         
        private void BtnGener_Click(int repairId)
        {
            try
            {
                // Получаем путь к директории приложения
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string sourceFile = Path.Combine(appDirectory, "Квитанция_приема_техники_шаблон.xls");

                // Проверяем существование исходного файла
                if (!File.Exists(sourceFile))
                {
                    MessageBox.Show("Исходный файл не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Получаем имя клиента
                string client_name = GetClientInfoByRepairId(repairId);

                // Создаем имя для копии файла с номером ремонта
                string newFileName = $"Квитанция_ремонта_{repairId}_для_{client_name}.xls";
                string newFilePath = Path.Combine(appDirectory, newFileName);

                // Копируем файл
                File.Copy(sourceFile, newFilePath, true);

                // Работаем с Excel
                Excel.Application excelApp = null;
                Excel.Workbook workbook = null;

                try
                {
                    excelApp = new Excel.Application();
                    excelApp.Visible = false;
                    excelApp.DisplayAlerts = false;

                    workbook = excelApp.Workbooks.Open(newFilePath);
                    Excel.Worksheet worksheet = workbook.Sheets[1];

                    // Заполняем данные из базы данных на основе repairId
                    FillReceiptData(worksheet, repairId);

                    workbook.Save();
                    MessageBox.Show($"Квитанция для ремонта №{repairId} успешно создана: {newFileName}",
                                  "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    // Закрываем Excel
                    if (workbook != null)
                    {
                        workbook.Close();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                    }
                    if (excelApp != null)
                    {
                        excelApp.Quit();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                    }

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        */

        // Генерация файла квитанции в выбранную директорию
        private void BtnGener_Click(int repairId)
        {
            try
            {
                // Получаем путь к директории приложения
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string sourceFile = Path.Combine(appDirectory, "Квитанция_приема_техники_шаблон.xls");

                // Проверяем существование исходного файла
                if (!File.Exists(sourceFile))
                {
                    MessageBox.Show("Исходный файл не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Получаем имя клиента
                string client_name = GetClientInfoByRepairId(repairId);

                // Создаем имя для копии файла с номером ремонта
                string newFileName = $"Квитанция_ремонта_{repairId}_для_{client_name}.xls";

                // Диалог выбора директории
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Выберите папку для сохранения квитанции";
                    folderDialog.RootFolder = Environment.SpecialFolder.Desktop;

                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedDirectory = folderDialog.SelectedPath;
                        string newFilePath = Path.Combine(selectedDirectory, newFileName);

                        // Копируем файл
                        File.Copy(sourceFile, newFilePath, true);

                        // Работаем с Excel
                        Excel.Application excelApp = null;
                        Excel.Workbook workbook = null;

                        try
                        {
                            excelApp = new Excel.Application();
                            excelApp.Visible = false;
                            excelApp.DisplayAlerts = false;

                            workbook = excelApp.Workbooks.Open(newFilePath);
                            Excel.Worksheet worksheet = workbook.Sheets[1];

                            // Заполняем данные из базы данных на основе repairId
                            FillReceiptData(worksheet, repairId);

                            workbook.Save();
                            MessageBox.Show($"Квитанция для ремонта №{repairId} успешно создана: {newFileName}",
                                          "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        finally
                        {
                            // Закрываем Excel
                            if (workbook != null)
                            {
                                workbook.Close();
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                            }
                            if (excelApp != null)
                            {
                                excelApp.Quit();
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                            }

                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Функция для поиска информации о клиенте по ID ремонта с форматированием для названия файлов
        private string GetClientInfoByRepairId(int repairId)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                string query = @"
                SELECT 
                    c.client_type,
                    CASE 
                        WHEN c.client_type = 0 THEN c.full_name
                        ELSE COALESCE(c.organization_name, c.full_name)
                    END as client_info
                FROM Repairs r
                JOIN Receipts rc ON r.receipt_id = rc.receipt_id
                JOIN Clients c ON rc.client_id = c.client_id
                WHERE r.repair_id = @repairId";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@repairId", repairId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int clientType = reader.GetInt32(0);
                            string clientInfo = reader.GetString(1);

                            // Форматирование для названия файлов Windows
                            if (clientType == 1) // Юридическое лицо
                            {
                                // Удаляем запрещенные символы для названий файлов Windows
                                string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                                foreach (char c in invalidChars)
                                {
                                    clientInfo = clientInfo.Replace(c.ToString(), "");
                                }

                                // Обрезаем длинное название (макс. 100 символов для безопасности)
                                if (clientInfo.Length > 100)
                                {
                                    clientInfo = clientInfo.Substring(0, 100);
                                }
                            }

                            // Заменяем пробелы на подчеркивания
                            clientInfo = clientInfo.Replace(" ", "_");

                            return clientInfo;
                        }
                        else
                        {
                            return "Клиент не найден";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске клиента: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "Ошибка поиска";
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        // Форматирование ячеек для заполнения excel
        private void MergeAndSetValue(Excel.Worksheet worksheet, string rangeAddress, string value)
        {
            try
            {
                Excel.Range range = worksheet.get_Range(rangeAddress);

                // Объединяем ячейки, если диапазон состоит из нескольких ячеек
                if (range.Cells.Count > 1)
                {
                    range.Merge();
                }

                // Устанавливаем значение
                range.Value = value;

                // Устанавливаем шрифт Arial размером 12
                range.Font.Name = "Arial";
                range.Font.Size = 12;

                // Выравнивание по левому краю для H33 и H1
                if (rangeAddress == "H33" || rangeAddress == "H1")
                {
                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                }

                // Для ячеек с запчастями делаем перенос текста
                if (rangeAddress == "B24:L25")
                {
                    range.WrapText = true;
                    range.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
                }

                // Освобождаем COM объект
                System.Runtime.InteropServices.Marshal.ReleaseComObject(range);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при работе с диапазоном {rangeAddress}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Метод для заполнения данных квитанции из базы данных
        private void FillReceiptData(Excel.Worksheet worksheet, int repairId)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                // Получаем данные о запчастях
                string sparePartsQuery = @"
                SELECT name, quantity, price 
                FROM Sparepart 
                WHERE repair_id = @repairId 
                ORDER BY name";

                List<string> sparePartsList = new List<string>();
                using (var sparePartsCmd = new NpgsqlCommand(sparePartsQuery, connection))
                {
                    sparePartsCmd.Parameters.AddWithValue("@repairId", repairId);
                    using (var sparePartsReader = sparePartsCmd.ExecuteReader())
                    {
                        while (sparePartsReader.Read())
                        {
                            string name = sparePartsReader.GetString(0);
                            int quantity = sparePartsReader.GetInt32(1);
                            decimal price = sparePartsReader.GetDecimal(2);
                            sparePartsList.Add($"{name} - {quantity} шт. - {price:N2} руб.");
                        }
                    }
                }

                string sparePartsText = sparePartsList.Count > 0
                    ? string.Join(", ", sparePartsList)
                    : "Запчасти не указаны";

                // Получаем основные данные о ремонте
                string query = @"
                SELECT 
                    r.repair_id,
                    d.device_type,
                    d.manufacturer,
                    d.model_number,
                    d.serial_number,
                    d.fault_description,
                    d.completeness,
                    r.acceptance_date,
                    CASE 
                        WHEN c.client_type = 0 THEN c.full_name
                        ELSE COALESCE(c.organization_name, c.full_name)
                    END as client_info,
                    c.contact_phone
                FROM Repairs r
                JOIN Devices d ON r.device_id = d.device_id
                JOIN Receipts rc ON r.receipt_id = rc.receipt_id
                JOIN Clients c ON rc.client_id = c.client_id
                WHERE r.repair_id = @repairId";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@repairId", repairId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Получаем данные из базы
                            int repair_id = reader.GetInt32(0);
                            string device_type = reader.GetString(1);
                            string manufacturer = reader.GetString(2);
                            string model_number = reader.GetString(3);
                            string serial_number = reader.IsDBNull(4) ? "Не указан" : reader.GetString(4);
                            string fault_description = reader.IsDBNull(5) ? "Не указана" : reader.GetString(5);
                            string completeness = reader.IsDBNull(6) ? "Не указана" : reader.GetString(6);
                            DateTime acceptance_date = reader.GetDateTime(7);
                            string client_info = reader.GetString(8);
                            string contact_phone = reader.GetString(9);

                            // Формируем полные строки
                            string full_device_info = $"{device_type} {manufacturer} {model_number}";

                            // Заполняем данные в Excel согласно вашим требованиям
                            MergeAndSetValue(worksheet, "H1", repair_id.ToString()); // номер ремонта
                            MergeAndSetValue(worksheet, "E3:L3", full_device_info); // информация об устройстве
                            MergeAndSetValue(worksheet, "D4:L4", serial_number); // серийный номер
                            MergeAndSetValue(worksheet, "D5:L5", fault_description); // описание неисправности
                            MergeAndSetValue(worksheet, "D6:L6", completeness); // комплектность
                            MergeAndSetValue(worksheet, "I7", acceptance_date.ToString("dd.MM.yyyy")); // дата принятия
                            MergeAndSetValue(worksheet, "C21:E21", client_info); // информация о клиенте
                            MergeAndSetValue(worksheet, "C22:E22", contact_phone); // контактный телефон
                            MergeAndSetValue(worksheet, "B24:L25", sparePartsText); // список запчастей

                            // Дублируем данные для второй части квитанции
                            MergeAndSetValue(worksheet, "H33", repair_id.ToString()); // номер ремонта (дубликат)
                            MergeAndSetValue(worksheet, "E36:L36", full_device_info); // информация об устройстве (дубликат)
                            MergeAndSetValue(worksheet, "D37:L37", serial_number); // серийный номер (дубликат)
                            MergeAndSetValue(worksheet, "D38:L38", fault_description); // описание неисправности (дубликат)
                            MergeAndSetValue(worksheet, "D39:L39", completeness); // комплектность (дубликат)
                            MergeAndSetValue(worksheet, "I40", acceptance_date.ToString("dd.MM.yyyy")); // дата принятия (дубликат)
                            MergeAndSetValue(worksheet, "C45:H45", client_info); // информация о клиенте (дубликат)
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении данных для квитанции: {ex.Message}", "Ошибка",
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