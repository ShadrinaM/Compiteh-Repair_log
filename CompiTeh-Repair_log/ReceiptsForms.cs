//using CompiTeh_Repair_log.Forms;
//using Npgsql;
//using System;
//using System.Data;
//using System.Windows.Forms;

//namespace CompiTeh_Repair_log
//{
//    public partial class ReceiptsForms : Form
//    {
//        private NpgsqlConnection connection;
//        private Form mainForm;
//        private DataTable clientsDataTable;
//        private DataTable devicesDataTable;
//        private int? lastAddedClientId = null;
//        private int? lastAddedDeviceId = null;

//        public ReceiptsForms(NpgsqlConnection conn, Form menushka)
//        {
//            InitializeComponent();
//            this.StartPosition = FormStartPosition.CenterScreen;
//            this.mainForm = menushka;
//            connection = conn;
//            this.FormClosed += WinForm_FormClosed;

//            // Настройка DataGridView для клиентов
//            dataGridViewClient.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
//            dataGridViewClient.MultiSelect = false;
//            dataGridViewClient.CellClick += DataGridViewClient_CellClick;
//            dataGridViewClient.ReadOnly = true; // Запрещает редактирование всех ячеек

//            // Добавляем обработчик для правой кнопки мыши
//            dataGridViewClient.MouseClick += DataGridViewClient_MouseClick;

//            // Создаем контекстное меню
//            var contextMenu = new ContextMenuStrip();

//            // Пункт "Изменить запись"
//            var editMenuItem = new ToolStripMenuItem("Изменить запись");
//            editMenuItem.Click += EditMenuItem_Click;
//            contextMenu.Items.Add(editMenuItem);

//            // Пункт "Удалить запись"
//            var deleteMenuItem = new ToolStripMenuItem("Удалить запись");
//            deleteMenuItem.Click += DeleteMenuItem_Click;
//            contextMenu.Items.Add(deleteMenuItem);

//            // Привязываем контекстное меню к DataGridView
//            dataGridViewClient.ContextMenuStrip = contextMenu;

//            // Настройка DataGridView для устройств
//            dataGridViewDevices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
//            dataGridViewDevices.MultiSelect = false;
//            dataGridViewDevices.CellClick += DataGridViewDevices_CellClick;
//            dataGridViewDevices.ReadOnly = true;

//            // Добавляем обработчик для правой кнопки мыши для устройств
//            dataGridViewDevices.MouseClick += DataGridViewDevices_MouseClick;

//            // Создаем контекстное меню для устройств
//            var deviceContextMenu = new ContextMenuStrip();

//            // Пункт "Изменить запись" для устройств
//            var editDeviceMenuItem = new ToolStripMenuItem("Изменить запись");
//            editDeviceMenuItem.Click += EditDeviceMenuItem_Click;
//            deviceContextMenu.Items.Add(editDeviceMenuItem);

//            // Пункт "Удалить запись" для устройств
//            var deleteDeviceMenuItem = new ToolStripMenuItem("Удалить запись");
//            deleteDeviceMenuItem.Click += DeleteDeviceMenuItem_Click;
//            deviceContextMenu.Items.Add(deleteDeviceMenuItem);

//            // Привязываем контекстное меню к DataGridView устройств
//            dataGridViewDevices.ContextMenuStrip = deviceContextMenu;

//            // Подписка на события изменения выбора в комбобоксах
//            comboBoxClientType.SelectedIndexChanged += comboBoxClientType_SelectedIndexChanged;
//        }

//        private void DataGridViewClient_CellClick(object sender, DataGridViewCellEventArgs e)
//        {
//            if (e.RowIndex >= 0)
//            {
//                dataGridViewClient.Rows[e.RowIndex].Selected = true;
//            }
//        }

//        private void DataGridViewDevices_CellClick(object sender, DataGridViewCellEventArgs e)
//        {
//            if (e.RowIndex >= 0)
//            {
//                dataGridViewDevices.Rows[e.RowIndex].Selected = true;
//            }
//        }

//        private void WinForm_FormClosed(object sender, FormClosedEventArgs e)
//        {
//            mainForm.Show();
//        }

//        private void btnBack_Click(object sender, EventArgs e)
//        {
//            this.Close();
//        }

//        private void btnAddClient_Click(object sender, EventArgs e)
//        {
//            if (connection.State != ConnectionState.Open)
//            {
//                connection.Open();
//            }
//            ClientForm clientForm = new ClientForm(connection, this);
//            if (clientForm.ShowDialog() == DialogResult.OK)
//            {
//                lastAddedClientId = clientForm.LastAddedClientId;

//                if (comboBoxClientType.SelectedIndex == -1)
//                {
//                    LoadClients();
//                }
//                else
//                {
//                    comboBoxClientType_SelectedIndexChanged(null, null);
//                }
//            }
//        }

//        // Обработчик клика правой кнопкой мыши
//        private void DataGridViewClient_MouseClick(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Right)
//            {
//                var hti = dataGridViewClient.HitTest(e.X, e.Y);
//                if (hti.RowIndex >= 0)
//                {
//                    dataGridViewClient.Rows[hti.RowIndex].Selected = true;
//                }
//            }
//        }

//        // Обработчик пункта меню "Изменить запись"
//        private void EditMenuItem_Click(object sender, EventArgs e)
//        {
//            if (dataGridViewClient.SelectedRows.Count == 0) return;

//            var selectedRow = dataGridViewClient.SelectedRows[0];
//            if (selectedRow.Cells["client_id"].Value == null) return;

//            int clientId = Convert.ToInt32(selectedRow.Cells["client_id"].Value);

//            if (connection.State != ConnectionState.Open)
//            {
//                connection.Open();
//            }

//            // Открываем форму в режиме редактирования
//            ClientForm clientForm = new ClientForm(connection, this, true, clientId);
//            if (clientForm.ShowDialog() == DialogResult.OK)
//            {
//                // Обновляем данные после редактирования
//                if (comboBoxClientType.SelectedIndex == -1)
//                {
//                    LoadClients();
//                }
//                else
//                {
//                    comboBoxClientType_SelectedIndexChanged(null, null);
//                }
//            }
//        }

//        // Обработчик пункта меню "Удалить запись"
//        private void DeleteMenuItem_Click(object sender, EventArgs e)
//        {
//            if (dataGridViewClient.SelectedRows.Count == 0) return;

//            var selectedRow = dataGridViewClient.SelectedRows[0];
//            if (selectedRow.Cells["client_id"].Value == null) return;

//            int clientId = Convert.ToInt32(selectedRow.Cells["client_id"].Value);
//            string clientName = selectedRow.Cells["full_name"].Value.ToString();

//            // Запрос подтверждения удаления
//            var confirmResult = MessageBox.Show(
//                $"Вы уверены, что хотите удалить клиента '{clientName}'?",
//                "Подтверждение удаления",
//                MessageBoxButtons.YesNo,
//                MessageBoxIcon.Warning);

//            if (confirmResult == DialogResult.Yes)
//            {
//                try
//                {
//                    if (connection.State != ConnectionState.Open)
//                    {
//                        connection.Open();
//                    }

//                    // Проверяем, есть ли связанные записи в других таблицах
//                    bool hasDependencies = CheckClientDependencies(clientId);

//                    if (hasDependencies)
//                    {
//                        MessageBox.Show("Нельзя удалить клиента, так как существуют связанные записи (например, ремонты).",
//                            "Ошибка удаления",
//                            MessageBoxButtons.OK,
//                            MessageBoxIcon.Error);
//                        return;
//                    }

//                    // Удаляем клиента
//                    string deleteQuery = "DELETE FROM Clients WHERE client_id = @clientId";
//                    using (var cmd = new NpgsqlCommand(deleteQuery, connection))
//                    {
//                        cmd.Parameters.AddWithValue("@clientId", clientId);
//                        int rowsAffected = cmd.ExecuteNonQuery();

//                        if (rowsAffected > 0)
//                        {
//                            MessageBox.Show("Клиент успешно удален.",
//                                "Успех",
//                                MessageBoxButtons.OK,
//                                MessageBoxIcon.Information);

//                            // Обновляем данные после удаления
//                            if (comboBoxClientType.SelectedIndex == -1)
//                            {
//                                LoadClients();
//                            }
//                            else
//                            {
//                                comboBoxClientType_SelectedIndexChanged(null, null);
//                            }
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show($"Ошибка при удалении клиента: {ex.Message}",
//                        "Ошибка",
//                        MessageBoxButtons.OK,
//                        MessageBoxIcon.Error);
//                }
//            }
//        }

//        // Проверка зависимостей клиента в других таблицах
//        private bool CheckClientDependencies(int clientId)
//        {
//            try
//            {
//                // Проверяем наличие связанных записей в таблице квитанций
//                string checkReceiptsQuery = "SELECT COUNT(*) FROM Receipts WHERE client_id = @clientId";
//                using (var cmd = new NpgsqlCommand(checkReceiptsQuery, connection))
//                {
//                    cmd.Parameters.AddWithValue("@clientId", clientId);
//                    long receiptsCount = (long)cmd.ExecuteScalar();
//                    if (receiptsCount > 0) return true;
//                }

//                // Проверяем наличие связанных записей в таблице ремонтов (через квитанции)
//                // Это не обязательно, так как ON DELETE CASCADE в Receipts позаботится об этом
//                // Но оставлю для примера
//                string checkRepairsQuery = @"
//            SELECT COUNT(*) 
//            FROM Repairs r
//            JOIN Receipts rc ON r.receipt_id = rc.receipt_id
//            WHERE rc.client_id = @clientId";

//                using (var cmd = new NpgsqlCommand(checkRepairsQuery, connection))
//                {
//                    cmd.Parameters.AddWithValue("@clientId", clientId);
//                    long repairsCount = (long)cmd.ExecuteScalar();
//                    if (repairsCount > 0) return true;
//                }

//                return false;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Ошибка при проверке зависимостей: {ex.Message}",
//                              "Ошибка",
//                              MessageBoxButtons.OK,
//                              MessageBoxIcon.Error);
//                return true; // В случае ошибки считаем, что зависимости есть
//            }
//        }

//        private void btnAddDevices_Click(object sender, EventArgs e)
//        {
//            if (connection.State != ConnectionState.Open)
//            {
//                connection.Open();
//            }
//            DevicesForm devicesForm = new DevicesForm(connection, this);
//            if (devicesForm.ShowDialog() == DialogResult.OK)
//            {
//                lastAddedDeviceId = devicesForm.LastAddedDeviceId;

//                LoadDevices();

//            }
//        }

//        // Обработчик клика правой кнопкой мыши для устройств
//        private void DataGridViewDevices_MouseClick(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Right)
//            {
//                var hti = dataGridViewDevices.HitTest(e.X, e.Y);
//                if (hti.RowIndex >= 0)
//                {
//                    dataGridViewDevices.Rows[hti.RowIndex].Selected = true;
//                }
//            }
//        }

//        // Обработчик пункта меню "Изменить запись" для устройств
//        private void EditDeviceMenuItem_Click(object sender, EventArgs e)
//        {
//            if (dataGridViewDevices.SelectedRows.Count == 0) return;

//            var selectedRow = dataGridViewDevices.SelectedRows[0];
//            if (selectedRow.Cells["device_id"].Value == null) return;

//            int deviceId = Convert.ToInt32(selectedRow.Cells["device_id"].Value);

//            if (connection.State != ConnectionState.Open)
//            {
//                connection.Open();
//            }

//            // Открываем форму в режиме редактирования
//            DevicesForm devicesForm = new DevicesForm(connection, this, true, deviceId);
//            if (devicesForm.ShowDialog() == DialogResult.OK)
//            {
//                // Обновляем данные после редактирования
//                LoadDevices();
//            }
//        }

//        // Обработчик пункта меню "Удалить запись" для устройств
//        private void DeleteDeviceMenuItem_Click(object sender, EventArgs e)
//        {
//            if (dataGridViewDevices.SelectedRows.Count == 0) return;

//            var selectedRow = dataGridViewDevices.SelectedRows[0];
//            if (selectedRow.Cells["device_id"].Value == null) return;

//            int deviceId = Convert.ToInt32(selectedRow.Cells["device_id"].Value);
//            string deviceName = $"{selectedRow.Cells["manufacturer"].Value} {selectedRow.Cells["model_number"].Value}";

//            // Запрос подтверждения удаления
//            var confirmResult = MessageBox.Show(
//                $"Вы уверены, что хотите удалить устройство '{deviceName}'?",
//                "Подтверждение удаления",
//                MessageBoxButtons.YesNo,
//                MessageBoxIcon.Warning);

//            if (confirmResult == DialogResult.Yes)
//            {
//                try
//                {
//                    if (connection.State != ConnectionState.Open)
//                    {
//                        connection.Open();
//                    }

//                    // Проверяем, есть ли связанные записи в других таблицах
//                    bool hasDependencies = CheckDeviceDependencies(deviceId);

//                    if (hasDependencies)
//                    {
//                        MessageBox.Show("Нельзя удалить устройство, так как существуют связанные записи (например, ремонты).",
//                            "Ошибка удаления",
//                            MessageBoxButtons.OK,
//                            MessageBoxIcon.Error);
//                        return;
//                    }

//                    // Удаляем устройство
//                    string deleteQuery = "DELETE FROM Devices WHERE device_id = @deviceId";
//                    using (var cmd = new NpgsqlCommand(deleteQuery, connection))
//                    {
//                        cmd.Parameters.AddWithValue("@deviceId", deviceId);
//                        int rowsAffected = cmd.ExecuteNonQuery();

//                        if (rowsAffected > 0)
//                        {
//                            MessageBox.Show("Устройство успешно удалено.",
//                                "Успех",
//                                MessageBoxButtons.OK,
//                                MessageBoxIcon.Information);

//                            // Обновляем данные после удаления
//                            LoadDevices();
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show($"Ошибка при удалении устройства: {ex.Message}",
//                        "Ошибка",
//                        MessageBoxButtons.OK,
//                        MessageBoxIcon.Error);
//                }
//            }
//        }

//        // Проверка зависимостей устройства в других таблицах
//        private bool CheckDeviceDependencies(int deviceId)
//        {
//            try
//            {
//                // Проверяем наличие связанных записей в таблице Ремонты
//                string checkQuery = "SELECT COUNT(*) FROM Repairs WHERE device_id = @deviceId";
//                using (var cmd = new NpgsqlCommand(checkQuery, connection))
//                {
//                    cmd.Parameters.AddWithValue("@deviceId", deviceId);
//                    long count = (long)cmd.ExecuteScalar();
//                    return count > 0;
//                }
//            }
//            catch
//            {
//                return true; // В случае ошибки считаем, что зависимости есть
//            }
//        }

//        private void ViewData_Load(object sender, EventArgs e)
//        {
//            LoadClients();
//            LoadDevices();
//            comboBoxClientType.SelectedIndex = 0;
//        }

//        // Загрузка клиентов в dataGridViewClient
//        private void LoadClients(int? clientType = null)
//        {
//            try
//            {
//                string query = "SELECT client_id, client_type, full_name, contact_phone, organization_name, email FROM Clients";

//                if (clientType.HasValue)
//                {
//                    query += " WHERE client_type = @clientType";
//                }

//                using (var cmd = new NpgsqlCommand(query, connection))
//                {
//                    if (clientType.HasValue)
//                    {
//                        cmd.Parameters.AddWithValue("@clientType", clientType.Value);
//                    }

//                    using (var adapter = new NpgsqlDataAdapter(cmd))
//                    {
//                        clientsDataTable = new DataTable();
//                        adapter.Fill(clientsDataTable);

//                        dataGridViewClient.AutoGenerateColumns = false;
//                        dataGridViewClient.DataSource = clientsDataTable;

//                        dataGridViewClient.Columns.Clear();

//                        dataGridViewClient.Columns.Add(new DataGridViewTextBoxColumn()
//                        {
//                            DataPropertyName = "client_id",
//                            HeaderText = "ID",
//                            Name = "client_id",
//                            Visible = false
//                        });

//                        dataGridViewClient.Columns.Add(new DataGridViewTextBoxColumn()
//                        {
//                            DataPropertyName = "full_name",
//                            HeaderText = "ФИО/Название",
//                            Name = "full_name"
//                        });

//                        dataGridViewClient.Columns.Add(new DataGridViewTextBoxColumn()
//                        {
//                            DataPropertyName = "contact_phone",
//                            HeaderText = "Телефон",
//                            Name = "contact_phone"
//                        });

//                        dataGridViewClient.Columns.Add(new DataGridViewTextBoxColumn()
//                        {
//                            DataPropertyName = "email",
//                            HeaderText = "Email",
//                            Name = "email"
//                        });

//                        if (lastAddedClientId.HasValue)
//                        {
//                            SelectClientInGrid(lastAddedClientId.Value);
//                            lastAddedClientId = null;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Ошибка при загрузке клиентов: {ex.Message}", "Ошибка",
//                    MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        // Загрузка устройств в dataGridViewDevices
//        private void LoadDevices(string deviceTypeFilter = null)
//        {
//            try
//            {
//                string query = @"SELECT device_id, device_type, manufacturer, model_number, 
//                              serial_number, completeness, device_notes FROM Devices";

//                if (!string.IsNullOrEmpty(deviceTypeFilter))
//                {
//                    query += " WHERE device_type = @deviceType";
//                }

//                using (var cmd = new NpgsqlCommand(query, connection))
//                {
//                    if (!string.IsNullOrEmpty(deviceTypeFilter))
//                    {
//                        cmd.Parameters.AddWithValue("@deviceType", deviceTypeFilter);
//                    }

//                    using (var adapter = new NpgsqlDataAdapter(cmd))
//                    {
//                        devicesDataTable = new DataTable();
//                        adapter.Fill(devicesDataTable);

//                        dataGridViewDevices.AutoGenerateColumns = false;
//                        dataGridViewDevices.DataSource = devicesDataTable;

//                        dataGridViewDevices.Columns.Clear();

//                        dataGridViewDevices.Columns.Add(new DataGridViewTextBoxColumn()
//                        {
//                            DataPropertyName = "device_id",
//                            HeaderText = "ID",
//                            Name = "device_id",
//                            Visible = false
//                        });

//                        dataGridViewDevices.Columns.Add(new DataGridViewTextBoxColumn()
//                        {
//                            DataPropertyName = "device_type",
//                            HeaderText = "Тип устройства",
//                            Name = "device_type"
//                        });

//                        dataGridViewDevices.Columns.Add(new DataGridViewTextBoxColumn()
//                        {
//                            DataPropertyName = "manufacturer",
//                            HeaderText = "Производитель",
//                            Name = "manufacturer"
//                        });

//                        dataGridViewDevices.Columns.Add(new DataGridViewTextBoxColumn()
//                        {
//                            DataPropertyName = "model_number",
//                            HeaderText = "Модель",
//                            Name = "model_number"
//                        });

//                        dataGridViewDevices.Columns.Add(new DataGridViewTextBoxColumn()
//                        {
//                            DataPropertyName = "serial_number",
//                            HeaderText = "Серийный номер",
//                            Name = "serial_number"
//                        });

//                        dataGridViewDevices.Columns.Add(new DataGridViewTextBoxColumn()
//                        {
//                            DataPropertyName = "completeness",
//                            HeaderText = "Комплектность",
//                            Name = "completeness"
//                        });

//                        dataGridViewDevices.Columns.Add(new DataGridViewTextBoxColumn()
//                        {
//                            DataPropertyName = "serial_number",
//                            HeaderText = "Серийный номер",
//                            Name = "device_notes"
//                        });

//                        if (lastAddedDeviceId.HasValue)
//                        {
//                            SelectDeviceInGrid(lastAddedDeviceId.Value);
//                            lastAddedDeviceId = null;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Ошибка при загрузке устройств: {ex.Message}", "Ошибка",
//                    MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void SelectClientInGrid(int clientId)
//        {
//            foreach (DataGridViewRow row in dataGridViewClient.Rows)
//            {
//                if (Convert.ToInt32(row.Cells["client_id"].Value) == clientId)
//                {
//                    row.Selected = true;
//                    dataGridViewClient.FirstDisplayedScrollingRowIndex = row.Index;
//                    break;
//                }
//            }
//        }

//        private void SelectDeviceInGrid(int deviceId)
//        {
//            foreach (DataGridViewRow row in dataGridViewDevices.Rows)
//            {
//                if (Convert.ToInt32(row.Cells["device_id"].Value) == deviceId)
//                {
//                    row.Selected = true;
//                    dataGridViewDevices.FirstDisplayedScrollingRowIndex = row.Index;
//                    break;
//                }
//            }
//        }

//        private void comboBoxClientType_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (comboBoxClientType.SelectedIndex == 0) // Физический
//            {
//                LoadClients(0);
//            }
//            else if (comboBoxClientType.SelectedIndex == 1) // Юридический
//            {
//                LoadClients(1);
//            }
//        }

//        private void btnOK_Click(object sender, EventArgs e)
//        {
//            // Проверяем, выбран ли клиент
//            if (dataGridViewClient.SelectedRows.Count == 0)
//            {
//                MessageBox.Show("Пожалуйста, выберите клиента", "Ошибка",
//                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            // Проверяем, есть ли устройства для квитанции
//            if (dataGridViewDevices.Rows.Count == 0)
//            {
//                MessageBox.Show("Пожалуйста, добавьте хотя бы одно устройство", "Ошибка",
//                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//        }
//    }
//}










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

        // Добавляем временный список для хранения ID добавленных устройств
        private List<int> tempDeviceIds = new List<int>();

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
            dataGridViewClient.ReadOnly = true;

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

            // Инициализируем пустой источник данных для устройств
            devicesDataTable = new DataTable();
            dataGridViewDevices.DataSource = devicesDataTable;

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

                // Добавляем ID устройства во временный массив
                if (lastAddedDeviceId.HasValue)
                {
                    tempDeviceIds.Add(lastAddedDeviceId.Value);
                }

                // Загружаем только устройства из временного массива
                LoadTempDevices();
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
                LoadTempDevices();
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
                            LoadTempDevices();
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







        // Модифицируем метод ViewData_Load
        private void ViewData_Load(object sender, EventArgs e)
        {
            LoadClients();
            // Не загружаем все устройства при открытии формы
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

        // Загрузка всех устройств в dataGridViewDevices
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


        // Новый метод для загрузки только выбранных устройств
        private void LoadTempDevices()
        {
            if (tempDeviceIds.Count == 0)
            {
                devicesDataTable.Clear();
                return;
            }

            try
            {
                string query = @"SELECT device_id, device_type, manufacturer, model_number, 
                              serial_number, completeness, device_notes FROM Devices
                              WHERE device_id = ANY(@deviceIds)";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@deviceIds", tempDeviceIds.ToArray());

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
                            DataPropertyName = "device_notes",
                            HeaderText = "Примечания",
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Проверяем, выбран ли клиент
            if (dataGridViewClient.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите клиента", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверяем, есть ли устройства для квитанции
            if (tempDeviceIds.Count == 0)
            {
                MessageBox.Show("Пожалуйста, добавьте хотя бы одно устройство", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                // Начинаем транзакцию
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Получаем ID выбранного клиента
                        var selectedClientRow = dataGridViewClient.SelectedRows[0];
                        int clientId = Convert.ToInt32(selectedClientRow.Cells["client_id"].Value);

                        // 1. Создаем запись в таблице Receipts
                        string insertReceiptQuery = @"
                    INSERT INTO Receipts (client_id, doc_path) 
                    VALUES (@clientId, '') 
                    RETURNING receipt_id";

                        int receiptId;
                        using (var cmd = new NpgsqlCommand(insertReceiptQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@clientId", clientId);
                            receiptId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // 2. Для каждого устройства из tempDeviceIds создаем запись в таблице Repairs
                        foreach (int deviceId in tempDeviceIds)
                        {
                            // Проверяем, что ID устройства валидный
                            if (deviceId <= 0)
                            {
                                throw new Exception($"Некорректный ID устройства: {deviceId}");
                            }

                            // Проверяем существование устройства в базе
                            string checkDeviceQuery = "SELECT 1 FROM Devices WHERE device_id = @deviceId";
                            using (var checkCmd = new NpgsqlCommand(checkDeviceQuery, connection, transaction))
                            {
                                checkCmd.Parameters.AddWithValue("@deviceId", deviceId);
                                var deviceExists = checkCmd.ExecuteScalar();

                                if (deviceExists == null)
                                {
                                    throw new Exception($"Устройство с ID {deviceId} не найдено в базе данных");
                                }
                            }

                            string insertRepairQuery = @"
                        INSERT INTO Repairs (device_id, receipt_id, work_performed, acceptance_date, status)
                        VALUES (@deviceId, @receiptId, 'Принято в ремонт', @acceptanceDate, 'принят')";

                            using (var cmd = new NpgsqlCommand(insertRepairQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@deviceId", deviceId);
                                cmd.Parameters.AddWithValue("@receiptId", receiptId);
                                cmd.Parameters.AddWithValue("@acceptanceDate", DateTime.Today);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Если все успешно - коммитим транзакцию
                        transaction.Commit();

                        MessageBox.Show("Квитанция и ремонты успешно созданы", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Очищаем список устройств после успешного создания
                        tempDeviceIds.Clear();
                        devicesDataTable.Clear();
                    }
                    catch (Exception ex)
                    {
                        // В случае ошибки откатываем транзакцию
                        try { transaction.Rollback(); } catch { }
                        MessageBox.Show($"Ошибка при создании квитанции: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
















