using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CompiTeh_Repair_log.Forms;
using Npgsql;

namespace CompiTeh_Repair_log
{
    public partial class Menu : Form
    {
        public NpgsqlConnection con;
        public Menu()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
        }
        // Преднастройка формы
        private void Menu_Load(object sender, EventArgs e)
        {
            // Подключение к БД
            try
            {
                con = new NpgsqlConnection("Server=localhost;Port=5432;User ID=postgres;Password=24072012;Database=RepairLogDB");
                con.Open();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных:\n{ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClientForma_Click(object sender, EventArgs e)
        {
            ClientForm ClientForma = new ClientForm(con, this);
            ClientForma.Show();
            this.Hide();
        }

        private void btnViewData_Click(object sender, EventArgs e)
        {
            ViewData ViewDataForm = new ViewData(con, this);
            ViewDataForm.Show();
            this.Hide();
        }

        private void btnDevicesForm_Click(object sender, EventArgs e)
        {
            DevicesForm DevicesForma = new DevicesForm(con, this);
            DevicesForma.Show();
            this.Hide();
        }

        private void btnPeriodReport_Click(object sender, EventArgs e)
        {
            ReceiptsForms ReceiptsForma = new ReceiptsForms(con, this);
            ReceiptsForma.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SparepartForm ReceiptsForma = new SparepartForm(con, this, 6);
            ReceiptsForma.Show();
            this.Hide();
        }
    }
}
