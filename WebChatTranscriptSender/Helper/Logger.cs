using WebChatTranscriptSender.Helper;
using System;

namespace WebChatTranscriptSender.Helper
{
    public class Logger
    {
        private Log log = Log.CreateLogObject();

        public void LogWrite(string Message, Log.LogType LogType)
        {
            GeneralSettings generalSettings = GeneralSettings.CreateGeneralSettingsObject();
            if (Convert.ToBoolean(generalSettings.GetAppKey("OpenLog")))
            {
                string logPath = generalSettings.GetAppKey("LogPath");
                this.log.LogWrite(Message, LogType, logPath);
            }
        }
    }
}
