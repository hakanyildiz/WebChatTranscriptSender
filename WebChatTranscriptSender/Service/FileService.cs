using WebChatTranscriptSender.Entity;
using WebChatTranscriptSender.Helper;
using System;
using System.IO;
using System.Text;

namespace WebChatTranscriptSender.Service
{
    public class FileService
    {
        private GeneralSettings generalSettings = GeneralSettings.CreateGeneralSettingsObject();

        private Logger log = new Logger();

        internal string SaveFileAsHtml(Chat chat)
        {
            try
            {
                if (!Directory.Exists(this.generalSettings.GetAppKey("ChatHistoryPath")))
                {

                    Directory.CreateDirectory(this.generalSettings.GetAppKey("ChatHistoryPath"));
                }
                string fileName = this.generalSettings.GetAppKey("ChatHistoryPath") + chat.Transcript + "_" + DateTime.Now.ToFileTime() + ".html";
                StreamWriter file = new StreamWriter(fileName, true, Encoding.UTF8);
                file.WriteLine(chat.ChatHistory);
                file.Close();
                this.log.LogWrite("File successfully created. Transcript : " + chat.Transcript + ", File : " + fileName, Log.LogType.Information);
                return fileName;
            }
            catch (Exception)
            {
                this.log.LogWrite("File write error. Transcript : " + chat.Transcript, Log.LogType.Error);
                return string.Empty;
            }
        }
    }
}
