using System;
using System.Windows.Forms;

namespace os_lab2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string dir1 = txtDir1.Text;
            string dir2 = txtDir2.Text;

            // Создаем процессоры для каждого каталога
            SizeProcessor p1 = new SizeProcessor(dir1);
            SizeProcessor p2 = new SizeProcessor(dir2);

            // Запускаем потоки для обработки каталогов
            Thread t1 = new Thread(() =>
            {
                p1.Process();
            });

            Thread t2 = new Thread(() =>
            {
                p2.Process();
            });

            t1.Start();
            t2.Start();

            // Агрегирующий поток
            Thread aggregator = new Thread(() =>
            {
                // Ждем завершения обоих потоков
                t1.Join();
                t2.Join();

                // Получаем результаты и обновляем форму
                Invoke(new Action(() =>
                {
                    lblResult1.Text = $"Каталог 1: {FormatBytes(p1.TotalSize)}";
                    lblResult2.Text = $"Каталог 2: {FormatBytes(p2.TotalSize)}";

                    if (p1.TotalSize > p2.TotalSize)
                    {
                        long diff = p1.TotalSize - p2.TotalSize;
                        lblCompare.Text = $"Каталог 1 больше на {FormatBytes(diff)}";
                    }
                    else if (p2.TotalSize > p1.TotalSize)
                    {
                        long diff = p2.TotalSize - p1.TotalSize;
                        lblCompare.Text = $"Каталог 2 больше на {FormatBytes(diff)}";
                    }
                    else
                    {
                        lblCompare.Text = "Каталоги имеют одинаковый объем";
                    }
                }));
            });

            aggregator.Start();
        }

        // Функция для форматирования размера в удобочитаемый вид
        private string FormatBytes(long bytes)
        {
            string[] sizes = { "Б", "КБ", "МБ", "ГБ", "ТБ" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]} ({bytes:N0} байт)";
        }
    }
}