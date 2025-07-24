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
            "���������.docx"
        );

        public Form1()
        {
            InitializeComponent();
        }

        // ��������� ���������
        private void GenerateDocument()
        {
            Microsoft.Office.Interop.Word.Application wordApp = null;
            Document doc = null;

            try
            {
                wordApp = new Microsoft.Office.Interop.Word.Application();
                wordApp.Visible = false; // ������ Word

                doc = wordApp.Documents.Add();

                // ��������� ����� "���������"
                Paragraph para1 = doc.Content.Paragraphs.Add();
                para1.Range.Text = "���������";
                para1.Range.Font.Bold = 1;
                para1.Range.Font.Size = 16;
                para1.Format.SpaceAfter = 24;
                para1.Range.InsertParagraphAfter();

                // ��������� ������ � ��������
                Paragraph para2 = doc.Content.Paragraphs.Add();
                para2.Range.Text = "���� _________        ������� ___________";
                para2.Range.Font.Size = 14;
                para2.Range.InsertParagraphAfter();

                // ��������� ��������
                doc.SaveAs(documentPath);
                MessageBox.Show("�������� ������: " + documentPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("������: " + ex.Message);
            }
            finally
            {
                // ��������� �������� � Word
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

        // �������� ���������
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (File.Exists(documentPath))
            {
                System.Diagnostics.Process.Start(documentPath);
            }
            else
            {
                MessageBox.Show("�������� �� ������. �������� ��� �������.");
            }
        }

        // ������ ���������
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!File.Exists(documentPath))
            {
                MessageBox.Show("�������� �� ������. �������� ��� �������.");
                return;
            }

            Microsoft.Office.Interop.Word.Application wordApp = null;
            Document doc = null;

            try
            {
                wordApp = new Microsoft.Office.Interop.Word.Application();
                doc = wordApp.Documents.Open(documentPath);

                // ���������� ������ ������
                wordApp.Dialogs[WdWordDialog.wdDialogFilePrint].Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("������ ������: " + ex.Message);
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

        // ������ "������� ��������"
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateDocument();
        }
    }
}