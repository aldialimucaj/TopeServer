using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopeServer.al.aldi.utils.general
{
    class ProgramAdministration
    {
        public static String getProgramPath()
        {
            String path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            String programName = System.IO.Path.GetFileNameWithoutExtension(path);
            return programName;
        }
    }
}
