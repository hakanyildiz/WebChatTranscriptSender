using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebChatTranscriptSender.Entity;
using WebChatTranscriptSender.Helper;


namespace WebChatTranscriptSender.Service
{
    public class SiebelService
    {
        private Logger log = new Logger();

        private GeneralSettings generalSettings = GeneralSettings.CreateGeneralSettingsObject();

        internal void WriteToDB(List<Chat> chatList)
        {
            this.log.LogWrite("Writing db operation starts.", Log.LogType.Information);

            foreach(Chat chat in chatList)
            {
                this.log.LogWrite(chat.ChatHistory, Log.LogType.Warning);
            }
        }


    }
}
