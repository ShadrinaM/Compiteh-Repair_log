using System;
using System.Windows.Forms;
using System.IO;
using Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;

namespace test
{
    public partial class Form1 : Form
    {
        private string documentPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Квитанция.docx"
        );

        public Form1()
        {
            InitializeComponent();
        }

        // Генерация документа
        private void GenerateDocument()
        {
            Microsoft.Office.Interop.Word.Application wordApp = null;
            Document doc = null;

            try
            {
                wordApp = new Microsoft.Office.Interop.Word.Application();
                wordApp.Visible = false; // Скрыть Word

                doc = wordApp.Documents.Add();

                // Добавляем текст "Квитанция"
                Paragraph para1 = doc.Content.Paragraphs.Add();
                para1.Range.Text = "Квитанция";
                para1.Range.Font.Bold = 1;
                para1.Range.Font.Size = 16;
                para1.Format.SpaceAfter = 24;
                para1.Range.InsertParagraphAfter();

                // Добавляем строку с подписью
                Paragraph para2 = doc.Content.Paragraphs.Add();
                para2.Range.Text = "дата _________        подпись ___________";
                para2.Range.Font.Size = 14;
                para2.Range.InsertParagraphAfter();

                // Сохраняем документ
                doc.SaveAs(documentPath);
                MessageBox.Show("Документ создан: " + documentPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            finally
            {
                // Закрываем документ и Word
                if (doc != null)
                {
                    doc.Close(false);
                    Marshal.ReleaseComObject(doc);
                }
                if (wordApp != null)
                {
                    wordApp.Quit();
                    Marshal.ReleaseComObject(wordApp);
                }
            }
        }

        // Открытие документа
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (File.Exists(documentPath))
            {
                System.Diagnostics.Process.Start(documentPath);
            }
            else
            {
                MessageBox.Show("Документ не найден. Создайте его сначала.");
            }
        }

        // Печать документа
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!File.Exists(documentPath))
            {
                MessageBox.Show("Документ не найден. Создайте его сначала.");
                return;
            }

            Microsoft.Office.Interop.Word.Application wordApp = null;
            Document doc = null;

            try
            {
                wordApp = new Microsoft.Office.Interop.Word.Application();
                doc = wordApp.Documents.Open(documentPath);

                // Показываем диалог печати
                wordApp.Dialogs[WdWordDialog.wdDialogFilePrint].Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка печати: " + ex.Message);
            }
            finally
            {
                if (doc != null)
                {
                    doc.Close(false);
                    Marshal.ReleaseComObject(doc);
                }
                if (wordApp != null)
                {
                    wordApp.Quit();
                    Marshal.ReleaseComObject(wordApp);
                }
            }
        }

        // Кнопка "Создать документ"
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateDocument();
        }
    }
}