using Npgsql;
using System;
using System.Windows.Forms;

namespace CompiTeh_Repair_log.Forms
{
    public partial class DevicesForm : Form
    {
        private readonly NpgsqlConnection connection;
        private readonly bool isEditMode;
        private readonly int deviceId;
        private readonly Form mainForm;

        public int LastAddedDeviceId { get; private set; }

        public DevicesForm(NpgsqlConnection conn, Form mainForm, bool editMode = false, int existingDeviceId = 0)
        {
            InitializeComponent();
            this.connection = conn;
            this.mainForm = mainForm;
            this.isEditMode = editMode;
            this.deviceId = existingDeviceId;
            this.LastAddedDeviceId = 0;

            this.StartPosition = FormStartPosition.CenterScreen;
            ConfigureForm();

            if (isEditMode)
                LoadExistingDevice();

            this.FormClosed += DevicesForm_FormClosed;
        }

        private void ConfigureForm()
        {
            this.Text = isEditMode ? "Редактирование устройства" : "Добавление устройства";
            labelHeader.Text = isEditMode ? "Редактирование устройства" : "Добавление устройства";
            btnOK.Text = isEditMode ? "Сохранить изменения" : "Добавить";
        }

        private void LoadExistingDevice()
        {
            try
            {
                string query = @"SELECT device_type, manufacturer, model_number, 
                           serial_number, completeness, fault_description, device_notes 
                           FROM Devices 
                           WHERE device_id = @deviceId";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@deviceId", deviceId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBoxDeviceType.Text = reader.GetString(0);
                            textBoxManufacturer.Text = reader.GetString(1);
                            textBoxModelNumber.Text = reader.GetString(2);
                            textBoxSerialNumber.Text = reader.GetString(3);
                            textBoxCompleteness.Text = reader.GetString(4); // NOT NULL поле
                            textBoxFaultDescription.Text = reader.GetString(5); // NOT NULL поле
                            textBoxDeviceNotes.Text = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных устройства: " + ex.Message);
                this.Close();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxDeviceType.Text))
            {
                MessageBox.Show("Тип устройства является обязательным полем.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxManufacturer.Text))
            {
                MessageBox.Show("Производитель является обязательным полем.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxModelNumber.Text))
            {
                MessageBox.Show("Модель является обязательным полем.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxSerialNumber.Text))
            {
                MessageBox.Show("Серийный номер является обязательным полем.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxCompleteness.Text))
            {
                MessageBox.Show("Комплектация устройства является обязательным полем.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxFaultDescription.Text))
            {
                MessageBox.Show("Характер неисправности является обязательным полем.");
                return;
            }

            try
            {
                string deviceType = textBoxDeviceType.Text;
                string manufacturer = textBoxManufacturer.Text;
                string modelNumber = textBoxModelNumber.Text;
                string serialNumber = textBoxSerialNumber.Text;
                string completeness = textBoxCompleteness.Text;
                string faultDescription = textBoxFaultDescription.Text;
                string deviceNotes = textBoxDeviceNotes.Text;

                if (isEditMode)
                    UpdateDevice(deviceType, manufacturer, modelNumber, serialNumber,
                               completeness, faultDescription, deviceNotes);
                else
                    InsertDevice(deviceType, manufacturer, modelNumber, serialNumber,
                                completeness, faultDescription, deviceNotes);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }

        private void InsertDevice(string deviceType, string manufacturer, string modelNumber,
                        string serialNumber, string completeness, string faultDescription, string deviceNotes)
        {
            string query = @"INSERT INTO Devices 
                (device_type, manufacturer, model_number, serial_number, 
                 completeness, fault_description, device_notes)
                VALUES (@deviceType, @manufacturer, @modelNumber, @serialNumber, 
                        @completeness, @faultDescription, @deviceNotes)
                RETURNING device_id";

            using (var cmd = new NpgsqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@deviceType", deviceType);
                cmd.Parameters.AddWithValue("@manufacturer", manufacturer);
                cmd.Parameters.AddWithValue("@modelNumber", modelNumber);
                cmd.Parameters.AddWithValue("@serialNumber", serialNumber);
                cmd.Parameters.AddWithValue("@completeness", completeness);
                cmd.Parameters.AddWithValue("@faultDescription", faultDescription);
                cmd.Parameters.AddWithValue("@deviceNotes",
                    string.IsNullOrEmpty(deviceNotes) ? DBNull.Value : (object)deviceNotes);

                LastAddedDeviceId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            MessageBox.Show("Устройство добавлено успешно.");
        }

        private void UpdateDevice(string deviceType, string manufacturer, string modelNumber,
                                string serialNumber, string completeness, string faultDescription, string deviceNotes)
        {
            string query = @"UPDATE Devices 
                       SET device_type = @deviceType, 
                           manufacturer = @manufacturer,
                           model_number = @modelNumber,
                           serial_number = @serialNumber,
                           completeness = @completeness,
                           fault_description = @faultDescription,
                           device_notes = @deviceNotes
                       WHERE device_id = @deviceId";

            using (var cmd = new NpgsqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@deviceType", deviceType);
                cmd.Parameters.AddWithValue("@manufacturer", manufacturer);
                cmd.Parameters.AddWithValue("@modelNumber", modelNumber);
                cmd.Parameters.AddWithValue("@serialNumber", serialNumber);
                cmd.Parameters.AddWithValue("@completeness", completeness);
                cmd.Parameters.AddWithValue("@faultDescription", faultDescription);
                cmd.Parameters.AddWithValue("@deviceNotes",
                    string.IsNullOrEmpty(deviceNotes) ? DBNull.Value : (object)deviceNotes);
                cmd.Parameters.AddWithValue("@deviceId", deviceId);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Изменения успешно сохранены.");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void DevicesForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Show();
        }
    }
}