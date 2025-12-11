using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

class ProcessInfoAnalyzer
{
    [Flags]
    internal enum SnapshotFlags : uint
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

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
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
    private static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool Module32First(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool Module32Next(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr hObject);

    public class ProcessModuleInfo
    {
        public uint ProcessID { get; set; }
        public string ProcessName { get; set; }
        public ulong TotalModuleSize { get; set; }
        public int ModuleCount { get; set; }
        public List<ModuleInfo> Modules { get; set; }
    }

    public class ModuleInfo
    {
        public string ModuleName { get; set; }
        public string ModulePath { get; set; }
        public uint ModuleSize { get; set; }
        public IntPtr BaseAddress { get; set; }
    }

    public static List<ProcessModuleInfo> GetProcessesWithLargestModules(int topCount = 10)
    {
        var processInfos = new List<ProcessModuleInfo>();
        IntPtr processSnapshot = IntPtr.Zero;

        try
        {
            // Создаем снимок всех процессов
            processSnapshot = CreateToolhelp32Snapshot((uint)SnapshotFlags.Process, 0);

            if (processSnapshot == IntPtr.Zero || processSnapshot.ToInt64() == -1)
            {
                Console.WriteLine($"Ошибка при создании снимка процессов: {Marshal.GetLastWin32Error()}");
                return processInfos;
            }

            PROCESSENTRY32 procEntry = new PROCESSENTRY32();
            procEntry.dwSize = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32));

            // Перечисляем все процессы
            if (Process32First(processSnapshot, ref procEntry))
            {
                do
                {
                    var processInfo = new ProcessModuleInfo
                    {
                        ProcessID = procEntry.th32ProcessID,
                        ProcessName = procEntry.szExeFile,
                        Modules = new List<ModuleInfo>()
                    };

                    // Получаем модули для текущего процесса
                    GetProcessModules(processInfo);

                    if (processInfo.ModuleCount > 0)
                    {
                        processInfos.Add(processInfo);
                    }

                } while (Process32Next(processSnapshot, ref procEntry));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        finally
        {
            if (processSnapshot != IntPtr.Zero)
            {
                CloseHandle(processSnapshot);
            }
        }

        // Сортируем по убыванию суммарного размера модулей и берем топ N
        return processInfos
            .OrderByDescending(p => p.TotalModuleSize)
            .Take(topCount)
            .ToList();
    }

    private static void GetProcessModules(ProcessModuleInfo processInfo)
    {
        IntPtr moduleSnapshot = IntPtr.Zero;

        try
        {
            // Создаем снимок модулей для конкретного процесса
            moduleSnapshot = CreateToolhelp32Snapshot((uint)SnapshotFlags.Module, processInfo.ProcessID);

            if (moduleSnapshot == IntPtr.Zero || moduleSnapshot.ToInt64() == -1)
            {
                return;
            }

            MODULEENTRY32 modEntry = new MODULEENTRY32();
            modEntry.dwSize = (uint)Marshal.SizeOf(typeof(MODULEENTRY32));

            ulong totalSize = 0;
            int moduleCount = 0;

            // Перечисляем все модули процесса
            if (Module32First(moduleSnapshot, ref modEntry))
            {
                do
                {
                    // Проверяем, что модуль принадлежит нашему процессу
                    if (modEntry.th32ProcessID == processInfo.ProcessID)
                    {
                        var module = new ModuleInfo
                        {
                            ModuleName = modEntry.szModule,
                            ModulePath = modEntry.szExePath,
                            ModuleSize = modEntry.modBaseSize,
                            BaseAddress = modEntry.modBaseAddr
                        };

                        processInfo.Modules.Add(module);
                        totalSize += modEntry.modBaseSize;
                        moduleCount++;
                    }

                } while (Module32Next(moduleSnapshot, ref modEntry));
            }

            processInfo.TotalModuleSize = totalSize;
            processInfo.ModuleCount = moduleCount;
        }
        finally
        {
            if (moduleSnapshot != IntPtr.Zero)
            {
                CloseHandle(moduleSnapshot);
            }
        }
    }

    public static void PrintProcessesInfo(List<ProcessModuleInfo> processes)
    {
        Console.WriteLine("Процессы с наибольшим суммарным размером модулей:\n");
        Console.WriteLine(new string('-', 100));
        Console.WriteLine($"{"№",3} | {"PID",7} | {"Имя процесса",30} | {"Модулей",8} | {"Суммарный размер",15} | {"Ср. размер модуля",15}");
        Console.WriteLine(new string('-', 100));

        for (int i = 0; i < processes.Count; i++)
        {
            var process = processes[i];
            double avgSize = process.ModuleCount > 0 ?
                (double)process.TotalModuleSize / process.ModuleCount : 0;

            Console.WriteLine($"{i + 1,3} | {process.ProcessID,7} | {process.ProcessName,30} | " +
                              $"{process.ModuleCount,8} | {FormatBytes(process.TotalModuleSize),15} | " +
                              $"{FormatBytes((ulong)avgSize),15}");

            // Выводим топ 5 модулей по размеру для каждого процесса
            Console.WriteLine($"    Топ 5 модулей процесса {process.ProcessName}:");
            var topModules = process.Modules
                .OrderByDescending(m => m.ModuleSize)
                .Take(5)
                .ToList();

            foreach (var module in topModules)
            {
                Console.WriteLine($"        - {module.ModuleName}: {FormatBytes(module.ModuleSize)} " +
                                 $"({module.ModulePath})");
            }
            Console.WriteLine();
        }
        Console.WriteLine(new string('-', 100));
    }

    private static string FormatBytes(ulong bytes)
    {
        string[] suffixes = { "B", "KB", "MB", "GB" };
        int suffixIndex = 0;
        double size = bytes;

        while (size >= 1024 && suffixIndex < suffixes.Length - 1)
        {
            size /= 1024;
            suffixIndex++;
        }

        return $"{size:N2} {suffixes[suffixIndex]}";
    }

    public static void Main()
    {
        try
        {
            Console.WriteLine("Сбор информации о процессах...");

            // Получаем 15 процессов с наибольшим суммарным размером модулей
            var topProcesses = GetProcessesWithLargestModules(15);

            if (topProcesses.Count == 0)
            {
                Console.WriteLine("Не удалось получить информацию о процессах.");
                return;
            }

            // Выводим информацию
            PrintProcessesInfo(topProcesses);

            // Дополнительная статистика
            Console.WriteLine("\nДополнительная статистика:");
            Console.WriteLine($"Всего проанализировано процессов: {topProcesses.Count}");
            Console.WriteLine($"Процесс с наибольшим размером модулей: {topProcesses.First().ProcessName} " +
                             $"(PID: {topProcesses.First().ProcessID}) - {FormatBytes(topProcesses.First().TotalModuleSize)}");

            // Исправление: явно указываем тип для Average
            double averageTotalSize = topProcesses.Select(p => (double)p.TotalModuleSize).Average();
            Console.WriteLine($"Средний размер модулей на процесс: {FormatBytes((ulong)averageTotalSize)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Критическая ошибка: {ex.Message}");
        }
    }
}