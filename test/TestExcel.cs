using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System;
using System.IO;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace test
{
    public partial class TestExcel : Form
    {
        public TestExcel()
        {
            InitializeComponent();
            BtnGener.Click += BtnGener_Click;
        }

        private void BtnGener_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем путь к директории приложения
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string sourceFile = Path.Combine(appDirectory, "Квитанция приема техники — копия.xls");

                // Проверяем существование исходного файла
                if (!File.Exists(sourceFile))
                {
                    MessageBox.Show("Исходный файл не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Создаем имя для копии файла с текущей датой
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                string newFileName = $"Квитанция_{currentDate}.xls";
                string newFilePath = Path.Combine(appDirectory, newFileName);

                // Копируем файл
                File.Copy(sourceFile, newFilePath, true);

                // Работаем с Excel
                Excel.Application excelApp = null;
                Excel.Workbook workbook = null;

                try
                {
                    excelApp = new Excel.Application();
                    excelApp.Visible = false; // Скрываем Excel
                    excelApp.DisplayAlerts = false; // Отключаем предупреждения

                    // Открываем файл
                    workbook = excelApp.Workbooks.Open(newFilePath);

                    // Получаем первый лист
                    Excel.Worksheet worksheet = workbook.Sheets[1];

                    // Объединяем ячейки и вставляем значения
                    MergeAndSetValue(worksheet, "E1:H1", "repair_id");
                    MergeAndSetValue(worksheet, "E3:L3", "device_type+manufacturer+model_number");
                    MergeAndSetValue(worksheet, "D4:L4", "serial_number");
                    MergeAndSetValue(worksheet, "D5:L5", "fault_description");
                    MergeAndSetValue(worksheet, "D6:L6", "completeness");
                    MergeAndSetValue(worksheet, "I7", "acceptance_date");
                    MergeAndSetValue(worksheet, "C21:E21", "full_name/organization_name");
                    MergeAndSetValue(worksheet, "C22:E22", "contact_phone");
                    MergeAndSetValue(worksheet, "B24:L24", "names");
                    MergeAndSetValue(worksheet, "G33:J33", "repair_id");
                    MergeAndSetValue(worksheet, "E36:L36", "device_type+manufacturer+model_number");
                    MergeAndSetValue(worksheet, "D37:L37", "serial_number");
                    MergeAndSetValue(worksheet, "D38:L38", "fault_description");
                    MergeAndSetValue(worksheet, "D39:L39", "completeness");
                    MergeAndSetValue(worksheet, "I40:J40", "acceptance_date");
                    MergeAndSetValue(worksheet, "C45:H45", "full_name/organization_name");

                    // Сохраняем изменения
                    workbook.Save();

                    MessageBox.Show($"Файл успешно создан: {newFileName}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                    // Освобождаем COM объекты
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

                // Добавляем нижнюю границу(линию под текстом)
                range.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                range.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                range.Borders[Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;

                // Освобождаем COM объект
                System.Runtime.InteropServices.Marshal.ReleaseComObject(range);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при работе с диапазоном {rangeAddress}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
