using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TopeServer.al.aldi.topeServer.control.executors;

namespace TopeServer.al.aldi.utils.general
{
    public class ProgCommandExecutor
    {
        public static bool openBrowserWithUrl(String url)
        {
            Process process = Process.Start(url);
            try
            {
                if (null != process)
                {
                    Process.GetProcessById(process.Id);
                }
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }

        public static bool appInputSimulation(String key)
        {
            if (key.Equals("#ENTER"))
            {
                OsCommandExecutor.simInputPressEnter();
            }
            if (key.Equals("#ESCAPE"))
            {
                OsCommandExecutor.simInputPressEscape();
            }
            if(key.Equals("#SPACE")) 
            {
                OsCommandExecutor.simInputPressTheSpacebar();
            }
            if (key.Equals("#BACKSPACE"))
            {
                OsCommandExecutor.simInputPressBackspace();
            }
            if (key.Equals("#SHIFTF5"))
            {
                OsCommandExecutor.simInputPressShiftF5();
            }
            if (key.Equals("#PERIOD"))
            {
                OsCommandExecutor.simInputPressPeriod();
            }
            if (key.Equals("#RIGHT"))
            {
                OsCommandExecutor.simInputPressRight();
            }
            if (key.Equals("#LEFT"))
            {
                OsCommandExecutor.simInputPressLeft();
            }
            if (key.Equals("#PAGE_UP"))
            {
                OsCommandExecutor.simInputPressPageUp();
            }
            if (key.Equals("#PAGE_DOWN"))
            {
                OsCommandExecutor.simInputPressPageDown();
            }
            if (key.Equals("#f"))
            {
                OsCommandExecutor.simInputPressKeyF();
            }
            if (key.Equals("#t"))
            {
                OsCommandExecutor.simInputPressKeyT();
            }

            // CTRL
            
            if (key.Equals("#CTRL-RIGHT"))
            {
                OsCommandExecutor.simInputPressCtrlRight();
            }
            if (key.Equals("#CTRL-LEFT"))
            {
                OsCommandExecutor.simInputPressCtrlLeft();
            }
            if (key.Equals("#CTRL-UP"))
            {
                OsCommandExecutor.simInputPressCtrlUp();
            }
            if (key.Equals("#CTRL-DOWN"))
            {
                OsCommandExecutor.simInputPressCtrlDown();
            }
            if (key.Equals("#CTRL-W"))
            {
                OsCommandExecutor.simInputPressCtrlW();
            }
            if (key.Equals("#CTRL-TAB"))
            {
                OsCommandExecutor.simInputPressCtrlTab();
            }

            // ALT
            if (key.Equals("#ALT-F4"))
            {
                OsCommandExecutor.simInputPressAltF4();
            }
            if (key.Equals("#ALT-TAB"))
            {
                OsCommandExecutor.simInputPressAltTab();
            }
            return true;
        }

    }
}
