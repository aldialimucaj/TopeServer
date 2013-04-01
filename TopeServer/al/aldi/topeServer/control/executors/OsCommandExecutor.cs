using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TopeServer.al.aldi.topeServer.control.executors
{
    class OsCommandExecutor
    {
        [DllImport("Powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool ExitWindowsEx(int flag, int reason);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool LockWorkStation();

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "BlockInput")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool BlockInput([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)] bool fBlockIt);


        public static bool hibernatePC() 
        {
            SetSuspendState(true, true, false);
            return true;
        }

        public static bool standbyPC()
        {
            SetSuspendState(false, true, false);
            return true;
        }

        public static bool powerOffPC()
        {
            ExitWindowsEx(1, 0); // 1 = poweroff
            return true;
        }

        public static bool logOffPC()
        {
            ExitWindowsEx(4, 0); // 4 = forced log off 
            return true;
        }

        public static bool lockScreen()
        {
            LockWorkStation(); // log off but save session
            return true;
        }
        public static bool restartPC()
        {
            ExitWindowsEx(2, 0); // 2 = restart
            return true;
        }

        /* ************ INPUT ************ */
        public static bool lockInput(bool input)
        {
            return BlockInput(input);
        }

    }
}
