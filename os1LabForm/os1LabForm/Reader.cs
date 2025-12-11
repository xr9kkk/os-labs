using os1LabForm;
using System;
using System.Threading;

namespace os1LabForm
{
    public class Reader : WorkerThread
    {
        private readonly Buffer<int> buffer;
        private readonly int readerId;
        private readonly int itemsToRead;
        private Thread thread;
        private bool isRunning;
        private bool isPaused;
        private readonly object pauseLock = new object();
        private readonly Random random;
        private readonly Action<string> logAction;
        private int itemsRead;

        public Reader(int id, Buffer<int> buffer, int itemsCount, Action<string> logAction)
        {
            readerId = id;
            this.buffer = buffer;
            this.itemsToRead = itemsCount;
            this.logAction = logAction;
            random = new Random();
            itemsRead = 0;
        }

        public Buffer<int> Buffer => buffer;

        public override void Start()
        {
            if (thread != null && thread.IsAlive)
                return;

            isRunning = true;
            isPaused = false;
            thread = new Thread(ReadProcess);
            thread.Name = $"Reader-{readerId}";
            thread.Start();

            Log($"Читатель {readerId} запущен (запланировано чтений: {itemsToRead})");
        }

        public override void Stop()
        {
            isRunning = false;
            Resume();
            thread?.Join(1000);
        }

        public void Pause()
        {
            lock (pauseLock)
            {
                isPaused = true;
                Log($"Читатель {readerId} приостановлен");
            }
        }

        public void Resume()
        {
            lock (pauseLock)
            {
                isPaused = false;
                Monitor.PulseAll(pauseLock);
                Log($"Читатель {readerId} возобновлен");
            }
        }

        private void ReadProcess()
        {
            itemsRead = 0;

            while (isRunning && itemsRead < itemsToRead)
            {
                try
                {
                    CheckPaused();

                    try
                    {
                        int data = buffer.Take();
                        itemsRead++;
                        Log($"Читатель {readerId} извлек: {data} ({itemsRead}/{itemsToRead}) [буфер: {buffer.Count}/{buffer.MaxSize}]");
                    }
                    catch (InvalidOperationException)
                    {
                        Log($"Читатель {readerId} - буфер деактивирован, завершение работы");
                        break;
                    }

                    Thread.Sleep(random.Next(500, 1500));
                }
                catch (ThreadInterruptedException)
                {
                    break;
                }
                finally
                {
                    buffer.HasReader = false;
                    Log($"Читатель {readerId} освободил буфер");
                }
            }

            Log($"Читатель {readerId} завершил работу. Всего прочитано: {itemsRead}/{itemsToRead}");
        }

        private void CheckPaused()
        {
            lock (pauseLock)
            {
                while (isPaused && isRunning)
                {
                    Monitor.Wait(pauseLock);
                }
            }
        }

        private void Log(string message)
        {
            logAction?.Invoke($"{DateTime.Now:HH:mm:ss} - {message}");
        }

        public override bool IsAlive => thread?.IsAlive == true;
        public bool IsPaused => isPaused;
        public int ReaderId => readerId;
        public int ItemsRead => itemsRead;
        public int ItemsToRead => itemsToRead;

        public override string GetStatus()
        {
            string status = IsAlive ? "Активен" : "Завершен";
            string pauseStatus = IsPaused ? " (Пауза)" : "";
            return $"Читатель {readerId}: {status}{pauseStatus} | Прогресс: {itemsRead}/{itemsToRead} | Буфер: {buffer.Count}/{buffer.MaxSize}";
        }
    }
}