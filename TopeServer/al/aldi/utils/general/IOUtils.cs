using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TopeServer.al.aldi.utils.general
{
    class IOUtils
    {
        /// <summary>
        /// Copies input stream to output stream. Closes output stream after finished.
        /// </summary>
        /// <param name="input">input stream</param>
        /// <param name="output">output stream </param>
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Close();
        }

        public static String GetUserHomeFolder()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            documents += "\\" + Program.FOLDER_NAME_UPLOAD;
            if (!Directory.Exists(documents))
            {
                Directory.CreateDirectory(documents);
            }
            return documents;
        }
    }
}
