using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopeServer.al.aldi.topeServer.control.executors;
using TopeServer.al.aldi.topeServer.model;

namespace TopeServer.al.aldi.topeServer.control.executors
{
    class OsCommands
    {
        public static bool hibernatePC(TopeRequest request)
        {
            return OsCommandExecutor.hibernatePC();
        }

        public static bool standbyPC(TopeRequest arg)
        {
            return OsCommandExecutor.standbyPC();
        }

        public static bool powerOffPC(TopeRequest arg)
        {
            return OsCommandExecutor.powerOffPC();
        }

        public static bool restartPC(TopeRequest arg)
        {
            return OsCommandExecutor.restartPC();
        }

        public static bool logOffPC(TopeRequest arg)
        {
            return OsCommandExecutor.logOffPC();
        }

        public static bool lockScreen(TopeRequest arg)
        {
            return OsCommandExecutor.lockScreen();
        }

        public static bool turnMonitorOn(TopeRequest arg)
        {
            return OsCommandExecutor.turnMonitorOn();
        }

        public static bool turnMonitorOff(TopeRequest arg)
        {
            return OsCommandExecutor.turnMonitorOff();
        }

        public static bool lockInput(TopeRequest arg)
        {
            return OsCommandExecutor.lockInput();
        }

        public static bool unlockInput(TopeRequest arg)
        {
            return OsCommandExecutor.unlockInput();
        }

        public static bool soundMute(TopeRequest arg)
        {
            return OsCommandExecutor.soundMute();
        }

        public static bool soundOn(TopeRequest arg)
        {
            return OsCommandExecutor.soundUnMute();
        }

        public static bool test(TopeRequest arg)
        {
            return true;
        }
    }
}
