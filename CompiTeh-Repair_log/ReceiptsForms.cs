using CompiTeh_Repair_log.Forms;
using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;

namespace CompiTeh_Repair_log
{
    public partial class ReceiptsForms : Form
    {
        private NpgsqlConnection connection;
        private Form mainForm;
        private DataTable clientsDataTable;
        private DataTable devicesDataTable;
        private int? lastAddedClientId = null;
        private int? lastAddedDeviceId = null;

        public ReceiptsForms(NpgsqlConnection conn, Form menushka)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = menushka;
            connection = conn;
            this.FormClosed += WinForm_FormClosed;

            // Настройка DataGridView для клиентов
            dataGridViewClient.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewClient.MultiSelect = false;
            dataGridViewClient.CellClick += DataGridViewClient_CellClick;
            dataGridViewClient.ReadOnly = true; // Запрещает редактирование всех ячеек

            // Добавляем обработчик для правой кнопки мыши
            dataGridViewClient.MouseClick += DataGridViewClient_MouseClick;

            // Создаем контекстное меню
            var contextMenu = new ContextMenuStrip();

            // Пункт "Изменить запись"
            var editMenuItem = new ToolStripMenuItem("Изменить запись");
            editMenuItem.Click += EditMenuItem_Click;
            contextMenu.Items.Add(editMenuItem);

            // Пункт "Удалить запись"
            var deleteMenuItem = new ToolStripMenuItem("Удалить запись");
            deleteMenuItem.Click += DeleteMenuItem_Click;
            contextMenu.Items.Add(deleteMenuItem);

            // Привязываем контекстное меню к DataGridView
            dataGridViewClient.ContextMenuStrip = contextMenu;

            // Настройка DataGridView для устройств
            dataGridViewDevices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewDevices.MultiSelect = false;
            dataGridViewDevices.CellClick += DataGridViewDevices_CellClick;
            dataGridViewDevices.ReadOnly = true;

            // Добавляем обработчик для правой кнопки мыши для устройств
            dataGridViewDevices.MouseClick += DataGridViewDevices_MouseClick;

            // Создаем контекстное меню для устройств
            var deviceContextMenu = new ContextMenuStrip();

            // Пункт "Изменить запись" для устройств
            var editDeviceMenuItem = new ToolStripMenuItem("Изменить запись");
            editDeviceMenuItem.Click += EditDeviceMenuItem_Click;
            deviceContextMenu.Items.Add(editDeviceMenuItem);

            // Пункт "Удалить запись" для устройств
            var deleteDeviceMenuItem = new ToolStripMenuItem("Удалить запись");
            deleteDeviceMenuItem.Click += DeleteDeviceMenuItem_Click;
            deviceContextMenu.Items.Add(deleteDeviceMenuItem);

            // Привязываем контекстное меню к DataGridView устройств
            dataGridViewDevices.ContextMenuStrip = deviceContextMenu;

            // Подписка на события изменения выбора в комбобоксах
            comboBoxClientType.SelectedIndexChanged += comboBoxClientType_SelectedIndexChanged;
        }

        private void DataGridViewClient_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridViewClient.Rows[e.RowIndex].Selected = true;
            }
        }

        private void DataGridViewDevices_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridViewDevices.Rows[e.RowIndex].Selected = true;
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

        private void btnAddClient_Click(object sender, EventArgs e)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            ClientForm clientForm = new ClientForm(connection, this);
            if (clientForm.ShowDialog() == DialogResult.OK)
            {
                lastAddedClientId = clientForm.LastAddedClientId;

                if (comboBoxClientType.SelectedIndex == -1)
                {
                    LoadClients();
                }
                else
                {
                    comboBoxClientType_SelectedIndexChanged(null, null);
                }
            }
        }

        // Обработчик клика правой кнопкой мыши
        private void DataGridViewClient_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dataGridViewClient.HitTest(e.X, e.Y);
                if (hti.RowIndex >= 0)
                {
                    dataGridViewClient.Rows[hti.RowIndex].Selected = true;
                }
            }
        }

        // Обработчик пункта меню "Изменить запись"
        private void EditMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewClient.SelectedRows.Count == 0) return;

            var selectedRow = dataGridViewClient.SelectedRows[0];
            if (selectedRow.Cells["client_id"].Value == null) return;

            int clientId = Convert.ToInt32(selectedRow.Cells["client_id"].Value);

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            // Открываем форму в режиме редактирования
            ClientForm clientForm = new ClientForm(connection, this, true, clientId);
            if (clientForm.ShowDialog() == DialogResult.OK)
            {
                // Обновляем данные после редактирования
                if (comboBoxClientType.SelectedIndex == -1)
                {
                    LoadClients();
                }
                else
                {
                    comboBoxClientType_SelectedIndexChanged(null, null);
                }
            }
        }

        // Обработчик пункта меню "Удалить запись"
        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewClient.SelectedRows.Count == 0) return;

            var selectedRow = dataGridViewClient.SelectedRows[0];
            if (selectedRow.Cells["client_id"].Value == null) return;

            int clientId = Convert.ToInt32(selectedRow.Cells["client_id"].Value);
            string clientName = selectedRow.Cells["full_name"].Value.ToString();

            // Запрос подтверждения удаления
            var confirmResult = MessageBox.Show(
                $"Вы уверены, что хотите удалить клиента '{clientName}'?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    // Проверяем, есть ли связанные записи в других таблицах
                    bool hasDependencies = CheckClientDependencies(clientId);

                    if (hasDependencies)
                    {
                        MessageBox.Show("Нельзя удалить клиента, так как существуют связанные записи (например, ремонты).",
                            "Ошибка удаления",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    // Удаляем клиента
                    string deleteQuery = "DELETE FROM Clients WHERE client_id = @clientId";
                    using (var cmd = new NpgsqlCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@clientId", clientId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Клиент успешно удален.",
                                "Успех",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            // Обновляем данные после удаления
                            if (comboBoxClientType.SelectedIndex == -1)
                            {
                                LoadClients();
                            }
                            else
                            {
                                comboBoxClientType_SelectedIndexChanged(null, null);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении клиента: {ex.Message}",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        // Проверка зависимостей клиента в других таблицах
        private bool CheckClientDependencies(int clientId)
        {
            try
            {
                // Проверяем наличие связанных записей в таблице квитанций
                string checkReceiptsQuery = "SELECT COUNT(*) FROM Receipts WHERE client_id = @clientId";
                using (var cmd = new NpgsqlCommand(checkReceiptsQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@clientId", clientId);
                    long receiptsCount = (long)cmd.ExecuteScalar();
                    if (receiptsCount > 0) return true;
                }

                // Проверяем наличие связанных записей в таблице ремонтов (через квитанции)
                // Это не обязательно, так как ON DELETE CASCADE в Receipts позаботится об этом
                // Но оставлю для примера
                string checkRepairsQuery = @"
            SELECT COUNT(*) 
            FROM Repairs r
            JOIN Receipts rc ON r.receipt_id = rc.receipt_id
            WHERE rc.client_id = @clientId";

                using (var cmd = new NpgsqlCommand(checkRepairsQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@clientId", clientId);
                    long repairsCount = (long)cmd.ExecuteScalar();
                    if (repairsCount > 0) return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке зависимостей: {ex.Message}",
                              "Ошибка",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
                return true; // В случае ошибки считаем, что зависимости есть
            }
        }

        private void btnAddDevices_Click(object sender, EventArgs e)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            DevicesForm devicesForm = new DevicesForm(connection, this);
            if (devicesForm.ShowDialog() == DialogResult.OK)
            {
                lastAddedDeviceId = devicesForm.LastAddedDeviceId;

                LoadDevices();

            }
        }

        // Обработчик клика правой кнопкой мыши для устройств
        private void DataGridViewDevices_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dataGridViewDevices.HitTest(e.X, e.Y);
                if (hti.RowIndex >= 0)
                {
                    dataGridViewDevices.Rows[hti.RowIndex].Selected = true;
                }
            }
        }

        // Обработчик пункта меню "Изменить запись" для устройств
        private void EditDeviceMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewDevices.SelectedRows.Count == 0) return;

            var selectedRow = dataGridViewDevices.SelectedRows[0];
            if (selectedRow.Cells["device_id"].Value == null) return;

            int deviceId = Convert.ToInt32(selectedRow.Cells["device_id"].Value);

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            // Открываем форму в режиме редактирования
            DevicesForm devicesForm = new DevicesForm(connection, this, true, deviceId);
            if (devicesForm.ShowDialog() == DialogResult.OK)
            {
                // Обновляем данные после редактирования
                LoadDevices();
            }
        }

        // Обработчик пункта меню "Удалить запись" для устройств
        private void DeleteDeviceMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewDevices.SelectedRows.Count == 0) return;

            var selectedRow = dataGridViewDevices.SelectedRows[0];
            if (selectedRow.Cells["device_id"].Value == null) return;

            int deviceId = Convert.ToInt32(selectedRow.Cells["device_id"].Value);
            string deviceName = $"{selectedRow.Cells["manufacturer"].Value} {selectedRow.Cells["model_number"].Value}";

            // Запрос подтверждения удаления
            var confirmResult = MessageBox.Show(
                $"Вы уверены, что хотите удалить устройство '{deviceName}'?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    // Проверяем, есть ли связанные записи в других таблицах
                    bool hasDependencies = CheckDeviceDependencies(deviceId);

                    if (hasDependencies)
                    {
                        MessageBox.Show("Нельзя удалить устройство, так как существуют связанные записи (например, ремонты).",
                            "Ошибка удаления",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    // Удаляем устройство
                    string deleteQuery = "DELETE FROM Devices WHERE device_id = @deviceId";
                    using (var cmd = new NpgsqlCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@deviceId", deviceId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Устройство успешно удалено.",
                                "Успех",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            // Обновляем данные после удаления
                            LoadDevices();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении устройства: {ex.Message}",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        // Проверка зависимостей устройства в других таблицах
        private bool CheckDeviceDependencies(int deviceId)
        {
            try
            {
                // Проверяем наличие связанных записей в таблице Ремонты
                string checkQuery = "SELECT COUNT(*) FROM Repairs WHERE device_id = @deviceId";
                using (var cmd = new NpgsqlCommand(checkQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@deviceId", deviceId);
                    long count = (long)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch
            {
                return true; // В случае ошибки считаем, что зависимости есть
            }
        }

        private void ViewData_Load(object sender, EventArgs e)
        {
            LoadClients();
            LoadDevices();
            comboBoxClientType.SelectedIndex = 0;
        }

        // Загрузка клиентов в dataGridViewClient
        private void LoadClients(int? clientType = null)
        {
            try
            {
                string query = "SELECT client_id, client_type, full_name, contact_phone, organization_name, email FROM Clients";

                if (clientType.HasValue)
                {
                    query += " WHERE client_type = @clientType";
                }

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    if (clientType.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@clientType", clientType.Value);
                    }

                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        clientsDataTable = new DataTable();
                        adapter.Fill(clientsDataTable);

                        dataGridViewClient.AutoGenerateColumns = false;
                        dataGridViewClient.DataSource = clientsDataTable;

                        dataGridViewClient.Columns.Clear();

                        dataGridViewClient.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "client_id",
                            HeaderText = "ID",
                            Name = "client_id",
                            Visible = false
                        });

                        dataGridViewClient.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "full_name",
                            HeaderText = "ФИО/Название",
                            Name = "full_name"
                        });

                        dataGridViewClient.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "contact_phone",
                            HeaderText = "Телефон",
                            Name = "contact_phone"
                        });

                        dataGridViewClient.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "email",
                            HeaderText = "Email",
                            Name = "email"
                        });

                        if (lastAddedClientId.HasValue)
                        {
                            SelectClientInGrid(lastAddedClientId.Value);
                            lastAddedClientId = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке клиентов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Загрузка устройств в dataGridViewDevices
        private void LoadDevices(string deviceTypeFilter = null)
        {
            try
            {
                string query = @"SELECT device_id, device_type, manufacturer, model_number, 
                              serial_number, completeness, device_notes FROM Devices";

                if (!string.IsNullOrEmpty(deviceTypeFilter))
                {
                    query += " WHERE device_type = @deviceType";
                }

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(deviceTypeFilter))
                    {
                        cmd.Parameters.AddWithValue("@deviceType", deviceTypeFilter);
                    }

                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        devicesDataTable = new DataTable();
                        adapter.Fill(devicesDataTable);

                        dataGridViewDevices.AutoGenerateColumns = false;
                        dataGridViewDevices.DataSource = devicesDataTable;

                        dataGridViewDevices.Columns.Clear();

                        dataGridViewDevices.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "device_id",
                            HeaderText = "ID",
                            Name = "device_id",
                            Visible = false
                        });

                        dataGridViewDevices.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "device_type",
                            HeaderText = "Тип устройства",
                            Name = "device_type"
                        });

                        dataGridViewDevices.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "manufacturer",
                            HeaderText = "Производитель",
                            Name = "manufacturer"
                        });

                        dataGridViewDevices.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "model_number",
                            HeaderText = "Модель",
                            Name = "model_number"
                        });

                        dataGridViewDevices.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "serial_number",
                            HeaderText = "Серийный номер",
                            Name = "serial_number"
                        });

                        dataGridViewDevices.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "completeness",
                            HeaderText = "Комплектность",
                            Name = "completeness"
                        });

                        dataGridViewDevices.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "serial_number",
                            HeaderText = "Серийный номер",
                            Name = "device_notes"
                        });

                        if (lastAddedDeviceId.HasValue)
                        {
                            SelectDeviceInGrid(lastAddedDeviceId.Value);
                            lastAddedDeviceId = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке устройств: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectClientInGrid(int clientId)
        {
            foreach (DataGridViewRow row in dataGridViewClient.Rows)
            {
                if (Convert.ToInt32(row.Cells["client_id"].Value) == clientId)
                {
                    row.Selected = true;
                    dataGridViewClient.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }

        private void SelectDeviceInGrid(int deviceId)
        {
            foreach (DataGridViewRow row in dataGridViewDevices.Rows)
            {
                if (Convert.ToInt32(row.Cells["device_id"].Value) == deviceId)
                {
                    row.Selected = true;
                    dataGridViewDevices.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }

        private void comboBoxClientType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxClientType.SelectedIndex == 0) // Физический
            {
                LoadClients(0);
            }
            else if (comboBoxClientType.SelectedIndex == 1) // Юридический
            {
                LoadClients(1);
            }
        }






    }
}