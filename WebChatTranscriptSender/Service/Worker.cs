using WebChatTranscriptSender.Dao;
using WebChatTranscriptSender.Entity;
using WebChatTranscriptSender.Helper;
using WebChatTranscriptSender.Service;
using System;
using System.Collections.Generic;

namespace WebChatTranscriptSender.Service
{
    public class Worker
    {
        private SiebelService siebelService = new SiebelService();

        private Serializer serializer = new Serializer();

        private DBProcesses dBProcesses;

        private Logger log = new Logger();

        private ChatDao dao = new ChatDao();

        private ChatHistoryService chatHistoryService = new ChatHistoryService();


        internal void Process()
        {
            this.dBProcesses = this.serializer.Deserialize();
            this.log.LogWrite("Service starts for " + this.dBProcesses.setting.Length + " operation", Log.LogType.Information);
            try
            {
                DBProcess[] setting2 = this.dBProcesses.setting;
                foreach (DBProcess setting in setting2)
                {
                    this.log.LogWrite("Proces started for " + setting.OperationName + " operation", Log.LogType.Information);
                    setting.Data = this.dao.FillChatHistory(setting);
                    if (setting.Data == null)
                    {
                        this.log.LogWrite("No data for " + setting.OperationName + " operation", Log.LogType.Information);
                    }
                    else if (setting.Data.Rows.Count == 0)
                    {
                        this.log.LogWrite("No data for " + setting.OperationName + " operation", Log.LogType.Information);
                    }
                    else
                    {
                        int maxValue = Convert.ToInt32(setting.Data.Compute("MAX(TRANSCRIPT)", string.Empty));
                        int uniqueValue = maxValue;
                        this.log.LogWrite("Prepare data process starts.", Log.LogType.Information);
                        List<Chat> chatList = this.chatHistoryService.PrepareData(setting);
                        this.log.LogWrite("Data prepared, sending mail operation starts.", Log.LogType.Information);
                        //this.mailService.SendMail(chatList);
                        this.siebelService.WriteToDB(chatList);

                        this.log.LogWrite("Sending mail operation completed.", Log.LogType.Information);
                        setting.UniqueValue = uniqueValue;
                        setting.Data = null;
                        this.log.LogWrite("New unique value set to :" + setting.UniqueValue, Log.LogType.Information);
                        this.serializer.Serialize(this.dBProcesses);
                    }
                    this.log.LogWrite("Proces completed for " + setting.OperationName, Log.LogType.Information);
                }
            }
            catch (Exception ex)
            {
                this.log.LogWrite("Unexpected error occured. Exception : " + ex.ToString(), Log.LogType.Information);
            }
        }
    }
}
