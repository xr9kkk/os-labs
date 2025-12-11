using os1LabForm;
using System;
using System.Threading;

namespace os1LabForm
{
    public class Writer : WorkerThread
    {
        private readonly Buffer<int> buffer;
        private readonly int writerId;
        private readonly int itemsToWrite;
        private Thread thread;
        private bool isRunning;
        private readonly Random random;
        private readonly Action<string> logAction;
        private int itemsWritten;


        public Writer(int id, Buffer<int> buffer, int itemsCount, Action<string> logAction)
        {
            writerId = id;
            this.buffer = buffer;
            this.itemsToWrite = itemsCount;
            this.logAction = logAction;
            random = new Random();
            itemsWritten = 0;
        }

        public Buffer<int> Buffer => buffer;

        public override void Start()
        {
            if (thread != null && thread.IsAlive)
                return;

            isRunning = true;
            thread = new Thread(WriteProcess);
            thread.Name = $"Writer-{writerId}";
            thread.Start();

            Log($"Писатель {writerId} запущен (запланировано записей: {itemsToWrite})");
        }

        public override void Stop()
        {
            isRunning = false;
            thread?.Join(1000);
        }

        private void WriteProcess()
        {
            itemsWritten = 0;

            while (isRunning && itemsWritten < itemsToWrite)
            {
                try
                {
                    int data = random.Next(1, 1000);

                    if (buffer.Put(data))
                    {
                        itemsWritten++;
                        Log($"Писатель {writerId} добавил: {data} ({itemsWritten}/{itemsToWrite}) [буфер: {buffer.Count}/{buffer.MaxSize}]");
                    }
                    else
                    {
                        if (!buffer.IsActive)
                        {
                            Log($"Писатель {writerId} - буфер деактивирован, завершение работы");
                            break;
                        }
                    }

                    Thread.Sleep(random.Next(500, 1500));
                }
            
                catch (ThreadInterruptedException)
                {
                    break;
                }
                finally
                {
                    buffer.HasWriter = false;
                    Log($"Писатель {writerId} освободил буфер");
                }
            }

            Log($"Писатель {writerId} завершил работу. Всего записано: {itemsWritten}/{itemsToWrite}");
        }

        private void Log(string message)
        {
            logAction?.Invoke($"{DateTime.Now:HH:mm:ss} - {message}");
        }

        public override bool IsAlive => thread?.IsAlive == true;
        public int WriterId => writerId;
        public int ItemsWritten => itemsWritten;
        public int ItemsToWrite => itemsToWrite;

        public override string GetStatus()
        {
            string status = IsAlive ? "Активен" : "Завершен";
            return $"Писатель {writerId}: {status} | Прогресс: {itemsWritten}/{itemsToWrite} | Буфер: {buffer.Count}/{buffer.MaxSize}";
        }
    }
}