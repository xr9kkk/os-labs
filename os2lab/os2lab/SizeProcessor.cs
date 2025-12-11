using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace os_lab2
{
    public class SizeProcessor
    {
        private readonly string rootPath;
        private long totalSize;

        public long TotalSize => totalSize;

        public SizeProcessor(string path)
        {
            rootPath = path ?? throw new ArgumentNullException(nameof(path));
        }

        public void Process()
        {
            if (!Directory.Exists(rootPath)) return;
            CalculateDirectorySize(rootPath);
        }

        private void CalculateDirectorySize(string path)
        {
            WIN32_FIND_DATA data;
            IntPtr handle = FindFirstFile(Path.Combine(path, "*"), out data);

            if (handle == INVALID_HANDLE_VALUE)
                return;

            try
            {
                do
                {
                    string current = data.cFileName;
                    if (current == "." || current == "..")
                        continue;

                    bool isDir = (data.dwFileAttributes & FileAttributes.Directory) != 0;
                    string fullPath = Path.Combine(path, current);

                    if (isDir)
                    {
                        // Рекурсивно обрабатываем подкаталоги
                        CalculateDirectorySize(fullPath);
                    }
                    else
                    {
                        // Считаем размер файла
                        // nFileSizeLow содержит младшие 32 бита размера файла
                        // nFileSizeHigh содержит старшие 32 бита размера файла
                        long fileSize = ((long)data.nFileSizeHigh << 32) | data.nFileSizeLow;
                        Interlocked.Add(ref totalSize, fileSize);
                    }

                } while (FindNextFile(handle, out data));
            }
            finally
            {
                FindClose(handle);
            }
        }

        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct WIN32_FIND_DATA
        {
            public FileAttributes dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public uint dwReserved0;
            public uint dwReserved1;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FindClose(IntPtr hFindFile);
    }
}