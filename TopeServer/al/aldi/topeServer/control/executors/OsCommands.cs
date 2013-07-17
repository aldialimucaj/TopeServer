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
        public static TopeResponse hibernatePC(TopeRequest request)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = OsCommandExecutor.hibernatePC();
            return topeResponse;
        }

        public static TopeResponse standbyPC(TopeRequest arg)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = OsCommandExecutor.standbyPC();
            return topeResponse;
        }

        public static TopeResponse powerOffPC(TopeRequest arg)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = OsCommandExecutor.powerOffPC();
            return topeResponse;
        }

        public static TopeResponse restartPC(TopeRequest arg)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = OsCommandExecutor.restartPC();
            return topeResponse;
        }

        public static TopeResponse logOffPC(TopeRequest arg)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = OsCommandExecutor.logOffPC();
            return topeResponse;
        }

        public static TopeResponse lockScreen(TopeRequest arg)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = OsCommandExecutor.lockScreen();
            return topeResponse;
        }

        public static TopeResponse turnMonitorOn(TopeRequest arg)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = OsCommandExecutor.turnMonitorOn();
            return topeResponse;
        }

        public static TopeResponse turnMonitorOff(TopeRequest arg)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = OsCommandExecutor.turnMonitorOff();
            return topeResponse;
        }

        public static TopeResponse lockInput(TopeRequest arg)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = OsCommandExecutor.lockInput();
            return topeResponse;
        }

        public static TopeResponse unlockInput(TopeRequest arg)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = OsCommandExecutor.unlockInput();
            return topeResponse;
        }

        public static TopeResponse soundMute(TopeRequest arg)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = OsCommandExecutor.soundMute();
            return topeResponse;
        }

        public static TopeResponse soundOn(TopeRequest arg)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = OsCommandExecutor.soundUnMute();
            return topeResponse;
        }

        public static TopeResponse test(TopeRequest arg)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = true;
            return topeResponse;
        }

        public static TopeResponse wakeOnLan(TopeRequest arg)
        {
            // this is a dummy, method will never be executed
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = true;
            return topeResponse;
        }
    }
}
