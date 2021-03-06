﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CoreAudioApi;
using System.Windows.Forms;

namespace TopeServer.al.aldi.topeServer.control.executors
{
    class OsCommandExecutor
    {
        /* Hibernate Standby */
        [DllImport("Powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

        /* Shutdown Restart*/
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool ExitWindowsEx(int flag, int reason);

        /* Locking the workstation*/
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool LockWorkStation();

        /* For input block/unblock */
        [System.Runtime.InteropServices.DllImportAttribute("User32.dll", EntryPoint = "BlockInput")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool BlockInput([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)] bool fBlockIt);

        /* For the monitor interaction */
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        /* Planed for sound*/
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
   
        /* Keyboard emulator */


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
            Process.Start("shutdown.exe", "-s -t 00");
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

        public static bool logonUser(string sUsername, string sDomain, string sPassword)
        {
            return true;
        }

        public static bool restartPC()
        {
            ExitWindowsEx(2, 0); // 2 = restart
            Process.Start("shutdown.exe", "-r -t 00");
            return true;
        }

        public static bool turnMonitorOn(bool turnOn)
        {
            int WM_SYSCOMMAND = 0x112;
            int SC_MONITORPOWER = 0xF170;
            const int MONITOR_ON = -1;
            const int MONITOR_OFF = 2;
            //const int MONITOR_STANBY = 1;

            int retValue = SendMessage(new IntPtr(0xFFFF), WM_SYSCOMMAND, SC_MONITORPOWER, turnOn ? MONITOR_ON : MONITOR_OFF);

            return true;
        }

        public static bool turnMonitorOn()
        {
            return turnMonitorOn(true);
        }

        public static bool turnMonitorOff()
        {
            return turnMonitorOn(false);
        }

        /* ************ INPUT ************ */
        public static bool lockInput(bool input)
        {
            return BlockInput(input);
        }

        public static bool lockInput()
        {
            return lockInput(true);
        }

        public static bool unlockInput()
        {
            return lockInput(false);
        }

        public static bool simInputPressEnter()
        {
            SendKeys.SendWait("{ENTER}");
            return true;
        }

        public static bool simInputPressEscape()
        {
            SendKeys.SendWait("{ESC}");
            return true;
        }

        public static bool simInputPressTheSpacebar()
        {
            SendKeys.SendWait(" ");
            return true;
        }

        public static bool simInputPressBackspace()
        {
            SendKeys.SendWait("{BS}");
            return true;
        }
        public static bool simInputPressPageUp()
        {
            SendKeys.SendWait("{PGUP}");
            return true;
        }

        public static bool simInputPressPageDown()
        {
            SendKeys.SendWait("{PGDN}");
            return true;
        }

        public static bool simInputPressShiftF5()
        {
            SendKeys.SendWait("+{F5}");
            return true;
        }

        public static bool simInputPressPeriod()
        {
            SendKeys.SendWait(".");
            return true;
        }

        public static bool simInputPressRight()
        {
            SendKeys.SendWait("{RIGHT}");
            return true;
        }

        public static bool simInputPressLeft()
        {
            SendKeys.SendWait("{LEFT}");
            return true;
        }
        
        public static bool simInputPressKeyF()
        {
            SendKeys.SendWait("f");
            return true;
        }

        public static bool simInputPressKeyT()
        {
            SendKeys.SendWait("t");
            return true;
        }

        // CTRL

        public static bool simInputPressCtrlLeft()
        {
            SendKeys.SendWait("^{LEFT}");
            return true;
        }
        public static bool simInputPressCtrlRight()
        {
            SendKeys.SendWait("^{RIGHT}");
            return true;
        }

        public static bool simInputPressCtrlUp()
        {
            SendKeys.SendWait("^{UP}");
            return true;
        }
        public static bool simInputPressCtrlDown()
        {
            SendKeys.SendWait("^{DOWN}");
            return true;
        }
        public static bool simInputPressCtrlW()
        {
            SendKeys.SendWait("^w");
            return true;
        }
        public static bool simInputPressCtrlTab()
        {
            SendKeys.SendWait("^{TAB}");
            return true;
        }

        // ALT
        public static bool simInputPressAltF4()
        {
            SendKeys.SendWait("%{F4}");
            return true;
        }
        public static bool simInputPressAltTab()
        {
            SendKeys.SendWait("%{TAB}");
            return true;
        }

        /* ************ SOUND ************ */
        public static bool soundSwap(bool status)
        {
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
            MMDevice device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
            for (int i = 0; i < device.AudioSessionManager.Sessions.Count; i++)
            {
                AudioSessionControl session = device.AudioSessionManager.Sessions[i];
                if (session.State == AudioSessionState.AudioSessionStateActive)
                {
                    AudioMeterInformation mi = session.AudioMeterInformation;
                    SimpleAudioVolume vol = session.SimpleAudioVolume;
                    vol.Mute = status;
                }
            }

            return true;
        }

        public static bool soundMute()
        {
            return soundSwap(true);
        }

        public static bool soundUnMute()
        {
            return soundSwap(false);
        }
    }
}
