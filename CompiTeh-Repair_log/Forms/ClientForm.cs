using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;

namespace CompiTeh_Repair_log.Forms
{
    public partial class ClientForm : Form
    {
        private readonly NpgsqlConnection connection;
        private readonly bool isEditMode;
        private readonly int clientId;
        private readonly Form mainForm;

        // Добавляем свойство для хранения ID добавленного клиента
        public int LastAddedClientId { get; private set; }

        public ClientForm(NpgsqlConnection conn, Form menushka, bool editMode = false, int existingClientId = 0)
        {
            InitializeComponent();
            this.connection = conn;
            this.mainForm = menushka;
            this.isEditMode = editMode;
            this.clientId = existingClientId;
            this.LastAddedClientId = 0;

            this.StartPosition = FormStartPosition.CenterScreen;
            ConfigureForm();

            if (isEditMode)
                LoadExistingClient();

            this.FormClosed += WinForm_FormClosed;
        }

        private void ConfigureForm()
        {
            this.Text = isEditMode ? "Редактирование клиента" : "Добавление клиента";
            labelHeader.Text = isEditMode ? "Редактирование клиента" : "Добавление клиента";
            btnOK.Text = isEditMode ? "Сохранить изменения" : "Добавить";
        }

        private void LoadExistingClient()
        {
            try
            {
                string query = @"SELECT client_type, full_name, contact_phone, 
                               organization_name, email, client_notes 
                               FROM Clients 
                               WHERE client_id = @clientId";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@clientId", clientId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            comboBoxClientType.SelectedIndex = reader.GetInt32(0);
                            textBoxFullName.Text = reader.GetString(1);
                            textBoxContactPhone.Text = reader.GetString(2);
                            textBoxOrganizationName.Text = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            textBoxEmail.Text = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            textBoxClientNotes.Text = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных клиента: " + ex.Message);
                this.Close();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxFullName.Text))
            {
                MessageBox.Show("ФИО является обязательным полем.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxContactPhone.Text))
            {
                MessageBox.Show("Контактный телефон является обязательным полем.");
                return;
            }

            try
            {
                int clientType = comboBoxClientType.SelectedIndex;
                string fullName = textBoxFullName.Text;
                string contactPhone = textBoxContactPhone.Text;
                string organizationName = clientType == 1 ? textBoxOrganizationName.Text : null;
                string email = textBoxEmail.Text;
                string clientNotes = textBoxClientNotes.Text;

                if (isEditMode)
                    UpdateClient(clientType, fullName, contactPhone, organizationName, email, clientNotes);
                else
                    InsertClient(clientType, fullName, contactPhone, organizationName, email, clientNotes);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }

        private void InsertClient(int clientType, string fullName, string contactPhone,
                        string organizationName, string email, string clientNotes)
        {
            string query = @"INSERT INTO Clients 
                    (client_type, full_name, contact_phone, organization_name, email, client_notes)
                    VALUES (@clientType, @fullName, @contactPhone, @organizationName, @email, @clientNotes)
                    RETURNING client_id";

            using (var cmd = new NpgsqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@clientType", clientType);
                cmd.Parameters.AddWithValue("@fullName", fullName);
                cmd.Parameters.AddWithValue("@contactPhone", contactPhone);

                // Для юридических лиц (clientType = 1) добавляем organizationName
                // Для физических лиц (clientType = 0) передаем NULL
                cmd.Parameters.AddWithValue("@organizationName",
                    clientType == 1 ? (object)(organizationName ?? "") : DBNull.Value);

                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@clientNotes",
                    string.IsNullOrEmpty(clientNotes) ? DBNull.Value : (object)clientNotes);

                LastAddedClientId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            MessageBox.Show("Клиент добавлен успешно.");
        }

        private void UpdateClient(int clientType, string fullName, string contactPhone,
                                string organizationName, string email, string clientNotes)
        {
            string query = @"UPDATE Clients 
                           SET client_type = @clientType, 
                               full_name = @fullName,
                               contact_phone = @contactPhone,
                               organization_name = @organizationName,
                               email = @email,
                               client_notes = @clientNotes
                           WHERE client_id = @clientId";

            using (var cmd = new NpgsqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@clientType", clientType);
                cmd.Parameters.AddWithValue("@fullName", fullName);
                cmd.Parameters.AddWithValue("@contactPhone", contactPhone);
                cmd.Parameters.AddWithValue("@organizationName",
                    string.IsNullOrEmpty(organizationName) ? DBNull.Value : (object)organizationName);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@clientNotes",
                    string.IsNullOrEmpty(clientNotes) ? DBNull.Value : (object)clientNotes);
                cmd.Parameters.AddWithValue("@clientId", clientId);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Изменения успешно сохранены.");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void WinForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Show();
        }

        private void comboBoxClientType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFieldsVisibility();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            UpdateFieldsVisibility();
        }

        private void UpdateFieldsVisibility()
        {
            // Проверяем по индексу (0 - физическое, 1 - юридическое)
            bool isLegalEntity = comboBoxClientType.SelectedIndex == 1;

            labelOrganizationName.Visible = isLegalEntity;
            textBoxOrganizationName.Visible = isLegalEntity;
        }
    }
}