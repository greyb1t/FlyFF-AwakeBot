using System;
using System.Runtime.InteropServices;

namespace FlyFF_AwakeBot
{
    public struct EnumWindowProcess
    {
        public int processId;
        public IntPtr windowHandle;
    }

    public class Process
    {
        public string ProcessName { get; set; }

        public int ProcessId { get; set; }

        public IntPtr Handle { get; set; }

        public IntPtr WindowHandle { get; set; }

        public Process() { }

        public Process(string processName, int processId, IntPtr handle, IntPtr windowHandle)
        {
            ProcessName = processName;
            ProcessId = processId;
            Handle = handle;
            WindowHandle = windowHandle;
        }

        /// <summary>
        /// Gets the window handle from a process id
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        static public IntPtr GetWindowHandle(int processId)
        {
            EnumWindowProcess ewp = new EnumWindowProcess();
            ewp.processId = processId;

            // Allocate a pointer to pass to the function
            IntPtr pEwpBlock = Marshal.AllocHGlobal(Marshal.SizeOf(ewp));
            // Convert the pointer to the target struct format
            Marshal.StructureToPtr(ewp, pEwpBlock, false);

            // Pass the allocated pointer block
            Win32API.EnumWindows(EnumWindowsCallback, pEwpBlock);

            // Convert back into struct from block
            EnumWindowProcess newEwp = (EnumWindowProcess)Marshal.PtrToStructure(pEwpBlock, typeof(EnumWindowProcess));

            Marshal.FreeHGlobal(pEwpBlock);

            return newEwp.windowHandle;
        }

        /// <summary>
        /// A callback function for the GetWindowHandle() function
        /// </summary>
        /// <param name="hWindow"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        unsafe static private bool EnumWindowsCallback(IntPtr hWindow, IntPtr lParam)
        {
            Win32API.GetWindowThreadProcessId(hWindow, out uint procId);

            EnumWindowProcess *windowProcess = (EnumWindowProcess *)lParam.ToPointer();

            if (procId == windowProcess->processId)
            {
                windowProcess->windowHandle = hWindow;
                return false;
            }

            return true;
        }
    }
}
