using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DbgviewDiscord
{
    public class Mem
    {
        
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, long lpBaseAddress, byte[] lpBuffer, uint dwSize, uint lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, int lpBaseAddress, byte[] lpBuffer, uint nSize, uint lpNumberOfBytesWritten);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);


        public const int MEM_COMMIT = 0x1000;
        public const int MEM_RESERVE = 0x2000;
        public const int PAGE_EXECUTE_READWRITE = 0x40;


        public IntPtr processHandle;
        public Process process;
        public long MainModuleBaseAddress
        {
            get => (long)process.MainModule.BaseAddress;
        }

        public Mem(Process proc)
        {
            process = proc;
            processHandle = OpenProcess(2035711U, false, proc.Id);
        }

        public void WriteBytes(int address, byte[] bytes)
        {
            WriteProcessMemory(processHandle, address, bytes, (uint)bytes.Length, 0);
        }

        public int AllocateExecutableMemory(uint size)
        {
            return (int)VirtualAllocEx(processHandle, IntPtr.Zero, size, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);
        }


    }
}
