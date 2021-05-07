using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace DbgviewDiscord
{
    public static class Injector
    {
        private static Mem mem;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern int GetProcAddress(IntPtr hModule, string procName);
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess,
        IntPtr lpThreadAttributes, uint dwStackSize, int lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        //classical Loadlibrary injection
        public static void Inject(Process proc)
        {
            var dllPath = Path.GetFullPath("dbgviewhook.dll");
            if (!File.Exists(dllPath))
            {
                Console.WriteLine("Hook dll not found!");
                Console.ReadLine();
                Environment.Exit(0);
            }

            mem = new Mem(proc);
            int loadLibraryPtr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            int allocAddr = mem.AllocateExecutableMemory((uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))));
            mem.WriteBytes(allocAddr, Encoding.Default.GetBytes(dllPath));
            CreateRemoteThread(mem.processHandle, IntPtr.Zero, 0, loadLibraryPtr, (IntPtr)allocAddr, 0, IntPtr.Zero);
        }


    }
}
