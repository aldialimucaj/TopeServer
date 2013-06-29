using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Threading;

namespace TopeServer.al.aldi.utils.general
{
    public class UtilsCommandExecutor
    {
        [DllImport("User32.dll", SetLastError = true)]
        public static extern int MessageBox(int hWnd, String text, String caption, uint type);

        internal static bool showMsg(string p)
        {
            //SystemSounds.Beep.Play();
            MessageBox(0, p, "Tope Server", (uint)(0x00000040L | 0x00040000L));
            return true;
        }

        internal static bool beep(string p)
        {
            SystemSounds.Beep.Play();
            return true;
        }

        internal static bool readOutLoud(string p)
        {
            SpeechSynthesizer ss = new SpeechSynthesizer();
            ss.SpeakAsync(p);
            return true;
        }

        internal static String readClipBoard()
        {
            String text = null;
            Exception threadEx = null;
            Thread staThread = new Thread(
                delegate()
                {
                    try
                    {
                        text = Clipboard.GetText();
                    }

                    catch (Exception ex)
                    {
                        threadEx = ex;
                    }
                }
            );
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
            return text;
        }
    }
}
