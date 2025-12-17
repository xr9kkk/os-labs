using System;
using System.Threading;
using System.Windows.Forms;

namespace OS_Lab3
{
    public partial class SnapshotForm : Form
    {
        // поток для вычислений
        private Thread snapshotThread;
        // количество процессов для вывода
        private int topCount = 10;

        public SnapshotForm()
        {
            InitializeComponent();
        }

        // кнопка "Найти процессы"
        private void buttonStart_Click(object sender, EventArgs e)
        {
            // если поток уже работает — выходим
            if (snapshotThread != null && snapshotThread.IsAlive)
                return;

            // проверяем введённое количество процессов
            if (!int.TryParse(textBoxTopCount.Text, out topCount) || topCount <= 0)
            {
                MessageBox.Show("Введите корректное число (больше 0)!", "Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            snapshotThread = new Thread(() =>
            {
                try
                {
                    // создаём объект, работающий со снапшотами
                    SnapshotWorker worker = new SnapshotWorker();

                    // получаем список процессов с наибольшим размером модулей
                    var processes = worker.GetProcessesWithLargestModules(topCount);

                    // Очищаем вывод в UI потоке
                    BeginInvoke(new MethodInvoker(() =>
                    {
                        richTextBoxProcesses.Clear();
                    }));

                    if (processes.Count > 0)
                    {
                        // Заголовок таблицы
                        BeginInvoke(new MethodInvoker(() =>
                        {
                            richTextBoxProcesses.AppendText("Процессы с наибольшим суммарным размером модулей:\n");
                            richTextBoxProcesses.AppendText(new string('=', 80) + "\n");
                            richTextBoxProcesses.AppendText(string.Format("{0,-4} {1,-8} {2,-30} {3,-15} {4,-10} {5,-10}\n",
                                "№", "PID", "Имя процесса", "Размер модулей", "Модулей", "Потоков"));
                            richTextBoxProcesses.AppendText(new string('-', 80) + "\n");
                        }));

                        // Выводим информацию о процессах
                        for (int i = 0; i < processes.Count; i++)
                        {
                            var process = processes[i];

                            BeginInvoke(new MethodInvoker(() =>
                            {
                                string formattedSize = SnapshotWorker.FormatBytes(process.TotalModuleSize);

                                richTextBoxProcesses.AppendText(string.Format("{0,-4} {1,-8} {2,-30} {3,-15} {4,-10} {5,-10}\n",
                                    i + 1,
                                    process.ProcessID,
                                    System.IO.Path.GetFileName(process.ProcessName),
                                    formattedSize,
                                    process.ModuleCount,
                                    process.ThreadCount));
                            }));
                        }

                        // Статистика
                        BeginInvoke(new MethodInvoker(() =>
                        {
                            richTextBoxProcesses.AppendText(new string('=', 80) + "\n");
                            richTextBoxProcesses.AppendText($"Всего найдено процессов: {processes.Count}\n");

                            if (processes.Count > 0)
                            {
                                ulong totalSize = 0;
                                foreach (var proc in processes)
                                {
                                    totalSize += proc.TotalModuleSize;
                                }

                                richTextBoxProcesses.AppendText($"Общий размер модулей: {SnapshotWorker.FormatBytes(totalSize)}\n");
                                richTextBoxProcesses.AppendText($"Процесс с наибольшим размером: {System.IO.Path.GetFileName(processes[0].ProcessName)} " +
                                                                $"({SnapshotWorker.FormatBytes(processes[0].TotalModuleSize)})\n");
                            }
                        }));
                    }
                    else
                    {
                        BeginInvoke(new MethodInvoker(() =>
                        {
                            richTextBoxProcesses.AppendText("Не удалось получить информацию о процессах\n");
                        }));
                    }
                }
                catch (Exception ex)
                {
                    BeginInvoke(new MethodInvoker(() =>
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка выполнения",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            });

            // запуск поиска
            snapshotThread.Start();
        }

        // закрытие формы
        private void SnapshotForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // если поток работает — дождаться его завершения
            if (snapshotThread != null && snapshotThread.IsAlive)
            {
                snapshotThread.Join(1000); // ждём максимум 1 секунду
            }
        }
    }
}
