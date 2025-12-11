using os1LabForm;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace os1LabForm
{
    public partial class Form1 : Form
    {
        private List<WorkerThread> workers;
        private List<Buffer<int>> buffers;
        private Random random;
        private int workerCounter;

        // Настройки по умолчанию
        private int itemsPerWorker = 5;
        private int spawnInterval = 3000;

        public Form1()
        {
            InitializeComponent();
            InitializeApplication();
        }

        private void InitializeApplication()
        {
            workers = new List<WorkerThread>();
            buffers = new List<Buffer<int>>();
            random = new Random();
            workerCounter = 0;

            // Установка значений по умолчанию
            writerItemsNumeric.Value = itemsPerWorker;
            readerItemsNumeric.Value = itemsPerWorker;
            spawnIntervalNumeric.Value = spawnInterval;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            // Получение настроек из интерфейса
            itemsPerWorker = (int)writerItemsNumeric.Value;
            spawnInterval = (int)spawnIntervalNumeric.Value;

            spawnTimer.Interval = spawnInterval;
            spawnTimer.Start();
            updateTimer.Start();

            startButton.Enabled = false;
            stopButton.Enabled = true;
            spawnNowButton.Enabled = true;
            pauseAllButton.Enabled = true;

            LogMessage("=== Система запущена ===");
            LogMessage($"Настройки: каждый поток обрабатывает {itemsPerWorker} элементов");
            LogMessage("Главный поток будет создавать потоки в случайные моменты времени");
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            spawnTimer.Stop();
            updateTimer.Stop();

            foreach (var worker in workers.ToArray())
            {
                worker.Stop();
            }

            foreach (var buffer in buffers.ToArray())
            {
                buffer.Deactivate();
            }

            CleanupCompletedWorkers();

            startButton.Enabled = true;
            stopButton.Enabled = false;
            spawnNowButton.Enabled = false;
            pauseAllButton.Enabled = false;
            pauseAllButton.Text = "Пауза всех читателей";

            LogMessage("=== Система остановлена ===");
            UpdateWorkersList();
        }

        private void PauseAllButton_Click(object sender, EventArgs e)
        {
            bool anyPaused = false;
            foreach (var worker in workers)
            {
                if (worker is Reader reader && reader.IsAlive && !reader.IsPaused)
                {
                    anyPaused = true;
                    break;
                }
            }

            if (anyPaused)
            {
                // Паузируем всех активных читателей
                foreach (var worker in workers)
                {
                    if (worker is Reader reader)
                    {
                        reader.Pause();
                    }
                }
                pauseAllButton.Text = "Возобновить всех";
                LogMessage("Все читатели приостановлены");
            }
            else
            {
                // Возобновляем всех приостановленных читателей
                foreach (var worker in workers)
                {
                    if (worker is Reader reader)
                    {
                        reader.Resume();
                    }
                }
                pauseAllButton.Text = "Пауза всех читателей";
                LogMessage("Все читатели возобновлены");
            }
        }

        private void SpawnNowButton_Click(object sender, EventArgs e)
        {
            SpawnRandomWorker();
        }

        private void SpawnRandomThread(object sender, EventArgs e)
        {
            SpawnRandomWorker();
        }


        private void SpawnRandomWorker()
        {
            workerCounter++;

            // Случайно выбираем тип потока (писатель или читатель)
            bool isWriter = random.Next(0, 2) == 0; // 50% вероятность

            // Получаем или создаем буфер для этого типа потока
            Buffer<int> buffer = GetOrCreateBufferForThreadType(isWriter);

            WorkerThread worker;
            if (isWriter)
            {
                worker = new Writer(workerCounter, buffer, itemsPerWorker, LogMessage);
                LogMessage($"=== Создан Писатель {workerCounter} ===");
            }
            else
            {
                worker = new Reader(workerCounter, buffer, itemsPerWorker, LogMessage);
                LogMessage($"=== Создан Читатель {workerCounter} ===");
            }

            if (isWriter)
            {
                buffer.HasWriter = true;
            }
            else
            {
                buffer.HasReader = true;
            }

            workers.Add(worker);
            worker.Start();

            LogMessage($"Поток запланировал обработку {itemsPerWorker} элементов");
            UpdateWorkersList();
        }


        private Buffer<int> GetOrCreateBufferForThreadType(bool isWriter)
        {
            // 1) Ищем буфер, где есть противоположный поток, но нет нашего
            foreach (var buffer in buffers)
            {
                if (isWriter)
                {
                    if (buffer.HasReader && !buffer.HasWriter)
                    {
                        buffer.HasWriter = true;
                        LogMessage($"[Буфер] Писатель подключён к существующему буферу (читатель уже есть)");
                        return buffer;
                    }
                }
                else
                {
                    if (buffer.HasWriter && !buffer.HasReader)
                    {
                        buffer.HasReader = true;
                        LogMessage($"[Буфер] Читатель подключён к существующему буферу (писатель уже есть)");
                        return buffer;
                    }
                }
            }

            // 2) Если такого буфера нет — создаём новый
            var newBuffer = new Buffer<int>(5);
            if (isWriter) newBuffer.HasWriter = true;
            else newBuffer.HasReader = true;

            buffers.Add(newBuffer);

            LogMessage(
                isWriter
                ? $"[Буфер] Создан новый буфер #{buffers.Count}: добавлен Писатель (читатель требуется)"
                : $"[Буфер] Создан новый буфер #{buffers.Count}: добавлен Читатель (писатель требуется)"
            );

            return newBuffer;
        }



        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateUI();
        }



        private void UpdateUI()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateUI));
                return;
            }

            CleanupCompletedWorkers();

            int activeWorkers = 0;
            int activeWriters = 0;
            int activeReaders = 0;
            int activePairs = 0;

            foreach (var worker in workers)
            {
                if (worker.IsAlive)
                {
                    activeWorkers++;
                    if (worker is Writer) activeWriters++;
                    if (worker is Reader) activeReaders++;
                }
            }

            foreach (var buffer in buffers)
            {
                if (buffer.HasWriter && buffer.HasReader)
                {
                    activePairs++;
                }
            }

            statusLabel.Text = $"Активных пар: {activePairs} | Писателей: {activeWriters} | Читателей: {activeReaders} | Всего создано пар: {workerCounter}";

            UpdateWorkersList();
        }

        private void CleanupCompletedWorkers()
        {
            for (int i = workers.Count - 1; i >= 0; i--)
            {
                if (!workers[i].IsAlive)
                    workers.RemoveAt(i);
            }

            for (int i = buffers.Count - 1; i >= 0; i--)
            {
                var buf = buffers[i];

                if (!buf.HasWriter && !buf.HasReader)
                {
                    buffers.RemoveAt(i);
                    LogMessage($"[Буфер] Удалён буфер #{i + 1}, так как пара завершилась");
                }
            }
        }



        private void LogMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(LogMessage), message);
                return;
            }

            logTextBox.AppendText(message + Environment.NewLine);
            logTextBox.ScrollToCaret();
        }



        //private void UpdateWorkersList()
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new MethodInvoker(UpdateWorkersList));
        //        return;
        //    }

        //    // Очищаем оба списка
        //    workersListBox.Items.Clear();
        //    buffersListBox.Items.Clear();

        //    // Заполняем список потоков
        //    foreach (var worker in workers)
        //    {
        //        if (worker.IsAlive)
        //        {
        //            workersListBox.Items.Add(worker.GetStatus());
        //        }
        //    }

        //    // Заполняем список буферов с информацией о парах
        //    for (int i = 0; i < buffers.Count; i++)
        //    {
        //        var buffer = buffers[i];
        //        int writers = CountWritersInBuffer(buffer);
        //        int readers = CountReadersInBuffer(buffer);

        //        if (writers > 0 || readers > 0)
        //        {
        //            string bufferInfo = $"Буфер {i + 1}: {buffer.Count}/{buffer.MaxSize} элементов | ";

        //            if (writers == 1 && readers == 1)
        //            {
        //                bufferInfo += "Полная пара (П+Ч)";
        //            }
        //            else if (writers == 1 && readers == 0)
        //            {
        //                bufferInfo += "Только писатель (ожидает читателя)";
        //            }
        //            else if (writers == 0 && readers == 1)
        //            {
        //                bufferInfo += " Только читатель (ожидает писателя)";
        //            }
        //            else
        //            {
        //                bufferInfo += "Некорректное состояние";
        //            }

        //            // Добавляем информацию о конкретных потоках
        //            List<string> connectedThreads = new List<string>();
        //            foreach (var worker in workers)
        //            {
        //                if (worker.IsAlive && GetWorkerBuffer(worker) == buffer)
        //                {
        //                    if (worker is Writer writer)
        //                        connectedThreads.Add($"Писатель {writer.WriterId}");
        //                    else if (worker is Reader reader)
        //                        connectedThreads.Add($"Читатель {reader.ReaderId}");
        //                }
        //            }

        //            if (connectedThreads.Count > 0)
        //            {
        //                bufferInfo += $" | Потоки: {string.Join(", ", connectedThreads)}";
        //            }

        //            buffersListBox.Items.Add(bufferInfo);
        //        }
        //    }

        //    // Если нет активных буферов, показываем сообщение
        //    if (buffersListBox.Items.Count == 0)
        //    {
        //        buffersListBox.Items.Add("Нет активных буферов");
        //    }
        //}

        private void UpdateWorkersList()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateWorkersList));
                return;
            }

            workersListBox.Items.Clear();
            buffersListBox.Items.Clear();

            // --- 1. Потоки ---
            foreach (var worker in workers)
            {
                if (worker.IsAlive)
                    workersListBox.Items.Add(worker.GetStatus());
            }

            // --- 2. Буферы ---
            for (int i = 0; i < buffers.Count; i++)
            {
                var buffer = buffers[i];

                // если буфер никто не использует — пропускаем
                if (!buffer.HasWriter && !buffer.HasReader)
                    continue;

                string bufferInfo = $"Буфер {i + 1}: {buffer.Count}/{buffer.MaxSize} элементов | ";

                // состояние пары
                if (buffer.HasWriter && buffer.HasReader)
                    bufferInfo += "Полная пара (П+Ч)";
                else if (buffer.HasWriter && !buffer.HasReader)
                    bufferInfo += "Только писатель (ожидает читателя)";
                else if (!buffer.HasWriter && buffer.HasReader)
                    bufferInfo += "Только читатель (ожидает писателя)";
                else
                    bufferInfo += "Некорректное состояние";

                // --- 3. Список потоков, которые используют этот буфер ---
                List<string> threads = new List<string>();

                foreach (var worker in workers)
                {
                    if (!worker.IsAlive)
                        continue;

                    // Определяем, работает ли этот worker с буфером
                    if (worker is Writer writer && writer.Buffer == buffer)
                        threads.Add($"Писатель {writer.WriterId}");

                    else if (worker is Reader reader && reader.Buffer == buffer)
                        threads.Add($"Читатель {reader.ReaderId}");
                }

                if (threads.Count > 0)
                    bufferInfo += $" | Потоки: {string.Join(", ", threads)}";

                buffersListBox.Items.Add(bufferInfo);
            }

            // --- 4. Если буферов нет ---
            if (buffersListBox.Items.Count == 0)
                buffersListBox.Items.Add("Нет активных буферов");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            spawnTimer?.Stop();
            updateTimer?.Stop();

            foreach (var worker in workers)
            {
                worker.Stop();
            }

            foreach (var buffer in buffers)
            {
                buffer.Deactivate();
            }
        }

        private void ClearLogButton_Click(object sender, EventArgs e)
        {
            logTextBox.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

     

        private void buttonAddWriter_Click(object sender, EventArgs e)
        {
            workerCounter++;
            bool isWriter = true;

            Buffer<int> buffer = GetOrCreateBufferForThreadType(isWriter);

            Writer worker = new Writer(workerCounter, buffer, itemsPerWorker, LogMessage);

            LogMessage($"=== Создан Писатель {workerCounter} ===");

            buffer.HasWriter = true;
            workers.Add(worker);
            worker.Start();

            LogMessage($"Поток запланировал обработку {itemsPerWorker} элементов");
            UpdateWorkersList();
        }


        private void buttonAddReader_Click(object sender, EventArgs e)
        {
            workerCounter++;
            bool isWriter = false;

            Buffer<int> buffer = GetOrCreateBufferForThreadType(isWriter);

            Reader worker = new Reader(workerCounter, buffer, itemsPerWorker, LogMessage);

            LogMessage($"=== Создан Читатель {workerCounter} ===");

            buffer.HasReader = true;
            workers.Add(worker);
            worker.Start();

            LogMessage($"Поток запланировал обработку {itemsPerWorker} элементов");
            UpdateWorkersList();
        }

    }
}