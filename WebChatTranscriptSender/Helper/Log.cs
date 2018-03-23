using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WebChatTranscriptSender.Helper
{

    public class Log
    {
        public enum LogType
        {
            Information = 1,
            Warning,
            Error
        }

        private static Log logObject;

        private static readonly object checkForLock = new object();

        private TextWriter textWriter;

        private Log()
        {
        }

        public static Log CreateLogObject()
        {
            if (Log.logObject == null)
            {
                lock (Log.checkForLock)
                {
                    if (Log.logObject == null)
                    {
                        Log.logObject = new Log();
                    }
                }
            }
            return Log.logObject;
        }

        public void LogWrite(string txt, LogType eventLogEntryType, string LogPath)
        {
            try
            {
                txt = string.Format(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff") + "|" + eventLogEntryType.ToString() + "|" + txt);
                GeneralSettings generalSettings = GeneralSettings.CreateGeneralSettingsObject();
                string text = "";
                text = ((!string.IsNullOrEmpty(LogPath)) ? LogPath : generalSettings.GetAppKey("LogPath"));
                if (!Directory.Exists(text))
                {
                    Directory.CreateDirectory(text);
                }
                int num = 0;
                while (true)
                {
                    try
                    {
                        num++;
                        this.textWriter = new StreamWriter($"{text}\\{DateTime.Now:yyyyMMdd}.txt", true);
                        lock (this.textWriter)
                        {
                            this.textWriter.WriteLine(txt);
                            this.textWriter.Close();
                        }
                        return;
                    }
                    catch
                    {
                        if (num >= 100)
                        {
                            return;
                        }
                    }
                }
            }
            catch
            {
            }
        }
    }
}
