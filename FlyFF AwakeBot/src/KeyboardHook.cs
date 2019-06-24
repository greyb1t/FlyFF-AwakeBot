using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using FlyFF_AwakeBot;

class KeyboardHook
{
    private static IntPtr _hookHandle = IntPtr.Zero;

    // We need to have this as a class variable to prevent it from being garbage collected
    private static Win32.LowLevelKeyboardProc _proc = KeyboardCallback;

    public static IntPtr RegisterHook()
    {
        using (Process currentProcess = Process.GetCurrentProcess())
        {
            using (ProcessModule mainModule = currentProcess.MainModule)
            {
                return Win32.SetWindowsHookEx(Win32.WH_KEYBOARD_LL, _proc, IntPtr.Zero, 0);
            }
        }
    }

    public static IntPtr KeyboardCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode < 0)
            return Win32.CallNextHookEx(_hookHandle, nCode, wParam, lParam);

        if (wParam == (IntPtr)Win32.WM_KEYDOWN)
        {
            var kbStruct = (Win32.KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(Win32.KBDLLHOOKSTRUCT));

            if (kbStruct.vkCode == (uint)Keys.End)
            {
                AwakeBotUserInterface.AwakeningRoutine.StopBot();

                var t = new Thread(() =>
                {
                    AwakeBotUserInterface.AwakeningRoutine.WaitAwakeningThreadFinish();
                });

                t.IsBackground = true;
                t.Start();
            }
        }

        return Win32.CallNextHookEx(_hookHandle, nCode, wParam, lParam);
    }
}