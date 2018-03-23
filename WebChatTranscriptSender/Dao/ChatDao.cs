using WebChatTranscriptSender.Entity;
using WebChatTranscriptSender.Helper;
using System;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
namespace WebChatTranscriptSender.Dao
{
    public class ChatDao
    {
        private DBProcess set = new DBProcess();

        private GeneralSettings genSet = GeneralSettings.CreateGeneralSettingsObject();

        private Logger log = new Logger();

        internal DataTable FillChatHistory(DBProcess setting)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            this.log.LogWrite("Query : " + setting.DataFromSource, Log.LogType.Information);
            try
            {
                OdbcConnection con = new OdbcConnection(setting.SourceConString);
                OdbcDataAdapter da = new OdbcDataAdapter(setting.DataFromSource, con);
                da.SelectCommand.CommandTimeout = 240;
                da.SelectCommand.Parameters.AddWithValue("TRANSCRIPT", setting.UniqueValue);
                DataTable dt = new DataTable();
                da.Fill(dt);
                this.log.LogWrite("Data Count : " + dt.Rows.Count, Log.LogType.Information);
                sw.Stop();
                this.log.LogWrite("Time Elapsed : " + sw.ElapsedMilliseconds, Log.LogType.Information);
                return dt;
            }
            catch (Exception ex)
            {
                this.log.LogWrite("Error at getting data. Error : " + ex.ToString(), Log.LogType.Error);
                return null;
            }
        }
    }
}
