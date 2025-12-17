using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace OS_Lab3
{
    public class SnapshotWorker
    {
        // Класс для хранения информации о процессе
        public class ProcessModuleInfo
        {
            public uint ProcessID { get; set; }
            public string ProcessName { get; set; }
            public ulong TotalModuleSize { get; set; } // в байтах
            public int ModuleCount { get; set; }
            public uint ThreadCount { get; set; }
            public uint ParentProcessID { get; set; }
        }

        // Получить топ N процессов с наибольшим суммарным размером модулей
        public List<ProcessModuleInfo> GetProcessesWithLargestModules(int topCount = 10)
        {
            var allProcesses = new List<ProcessModuleInfo>();
            IntPtr snapshotProcesses = IntPtr.Zero;

            try
            {
                // Создаём снапшот всех процессов
                snapshotProcesses = CreateToolhelp32Snapshot((uint)SnapshotFlags.Process, 0);

                if (snapshotProcesses == IntPtr.Zero || snapshotProcesses.ToInt64() == -1)
                {
                    throw new Win32Exception($"Не удалось создать снапшот процессов. Код ошибки: {Marshal.GetLastWin32Error()}");
                }

                PROCESSENTRY32 processEntry = new PROCESSENTRY32();
                processEntry.dwSize = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32));

                if (Process32First(snapshotProcesses, ref processEntry))
                {
                    do
                    {
                        var processInfo = new ProcessModuleInfo
                        {
                            ProcessID = processEntry.th32ProcessID,
                            ProcessName = processEntry.szExeFile,
                            ThreadCount = processEntry.cntThreads,
                            ParentProcessID = processEntry.th32ParentProcessID
                        };

                        // Получаем информацию о модулях процесса
                        CalculateModuleSize(processInfo);

                        // Добавляем только если удалось получить информацию о модулях
                        if (processInfo.ModuleCount > 0)
                        {
                            allProcesses.Add(processInfo);
                        }

                    } while (Process32Next(snapshotProcesses, ref processEntry));
                }
                else
                {
                    throw new Win32Exception($"Win32 error code {Marshal.GetLastWin32Error()}");
                }
            }
            finally
            {
                CloseHandle(snapshotProcesses);
            }

            // Сортируем по убыванию суммарного размера модулей
            allProcesses.Sort((p1, p2) => p2.TotalModuleSize.CompareTo(p1.TotalModuleSize));

            // Возвращаем топ N процессов
            if (allProcesses.Count > topCount)
            {
                return allProcesses.GetRange(0, topCount);
            }

            return allProcesses;
        }

        // Вычислить суммарный размер модулей для процесса
        private void CalculateModuleSize(ProcessModuleInfo processInfo)
        {
            IntPtr snapshotModules = IntPtr.Zero;

            try
            {
                // Создаём снапшот модулей для текущего процесса
                snapshotModules = CreateToolhelp32Snapshot((uint)SnapshotFlags.Module, processInfo.ProcessID);

                if (snapshotModules == IntPtr.Zero || snapshotModules.ToInt64() == -1)
                {
                    return;
                }

                MODULEENTRY32 moduleEntry = new MODULEENTRY32();
                moduleEntry.dwSize = (uint)Marshal.SizeOf(typeof(MODULEENTRY32));

                ulong totalSize = 0;
                int moduleCount = 0;

                // Считаем размер всех модулей
                if (Module32First(snapshotModules, ref moduleEntry))
                {
                    do
                    {
                        // Проверяем, что модуль принадлежит нашему процессу
                        if (moduleEntry.th32ProcessID == processInfo.ProcessID)
                        {
                            totalSize += moduleEntry.modBaseSize;
                            moduleCount++;
                        }
                    }
                    while (Module32Next(snapshotModules, ref moduleEntry));
                }

                processInfo.TotalModuleSize = totalSize;
                processInfo.ModuleCount = moduleCount;
            }
            finally
            {
                if (snapshotModules != IntPtr.Zero)
                {
                    CloseHandle(snapshotModules);
                }
            }
        }

        // Форматирование размера в читаемый вид
        public static string FormatBytes(ulong bytes)
        {
            string[] suffixes = { "Б", "КБ", "МБ", "ГБ", "ТБ" };
            int suffixIndex = 0;
            double size = bytes;

            while (size >= 1024 && suffixIndex < suffixes.Length - 1)
            {
                size /= 1024;
                suffixIndex++;
            }

            return $"{size:F2} {suffixes[suffixIndex]}";
        }

        #region kernel32

        [Flags]
        private enum SnapshotFlags : uint
        {
            HeapList = 0x00000001,
            Process = 0x00000002,
            Thread = 0x00000004,
            Module = 0x00000008,
            Module32 = 0x00000010,
            Inherit = 0x80000000,
            All = 0x0000001F,
            NoHeaps = 0x40000000
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        // ------- PROCESS -------

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct PROCESSENTRY32
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ProcessID;
            public IntPtr th32DefaultHeapID;
            public uint th32ModuleID;
            public uint cntThreads;
            public uint th32ParentProcessID;
            public int pcPriClassBase;
            public uint dwFlags;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szExeFile;
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        // ------- MODULES -------

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct MODULEENTRY32
        {
            public uint dwSize;
            public uint th32ModuleID;
            public uint th32ProcessID;
            public uint GlblcntUsage;
            public uint ProccntUsage;
            public IntPtr modBaseAddr;
            public uint modBaseSize;
            public IntPtr hModule;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szModule;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szExePath;
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool Module32First(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool Module32Next(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

        #endregion
    }
}
