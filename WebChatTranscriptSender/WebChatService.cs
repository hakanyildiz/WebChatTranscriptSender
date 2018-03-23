using WebChatTranscriptSender.Helper;
using WebChatTranscriptSender.Service;
using System;
using System.ComponentModel;
using System.ServiceProcess;
using System.Threading;
using System.Timers;

namespace WebChatTranscriptSender
{
    partial class WebChatService : ServiceBase
    {
        private Logger log = new Logger();

        private ChatHistoryService chatHistoryService = new ChatHistoryService();

        private System.Timers.Timer tmrProcess = new System.Timers.Timer();

        private GeneralSettings generalSettings = GeneralSettings.CreateGeneralSettingsObject();

        private Worker worker = new Worker();

        public WebChatService()
        {
            this.StartProcess(null);
            this.InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                this.log.LogWrite("WebChatTranscriptSender named service started", Log.LogType.Information);
                this.tmrProcess = new System.Timers.Timer();
                this.tmrProcess.Interval = 2000.0;
                this.log.LogWrite("First Start : " + this.tmrProcess.Interval + " ms later", Log.LogType.Information);
                this.tmrProcess.Elapsed += this.tmrProcess_Elapsed;
                this.tmrProcess.Start();
            }
            catch (Exception ex)
            {
                this.log.LogWrite("Unexpected error while service starts. " + ex.ToString(), Log.LogType.Error);
            }
        }

        private void tmrProcess_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.tmrProcess.Interval = (double)Convert.ToInt32(this.generalSettings.GetAppKey("TimerInterval"));
            this.tmrProcess.Stop();
            try
            {
                this.log.LogWrite("Process starts", Log.LogType.Information);
                Thread th = new Thread(this.StartProcess);
                th.ApartmentState = ApartmentState.STA;
                th.IsBackground = true;
                th.Start(null);
            }
            catch (Exception ex)
            {
                this.log.LogWrite("Unexpected error (Timer process). " + ex.ToString(), Log.LogType.Error);
            }
            this.tmrProcess.Start();
        }

        private void StartProcess(object data)
        {
            this.log.LogWrite("DBOperation Sync operation started", Log.LogType.Information);
            this.worker.Process();
            this.log.LogWrite("DBOperation Sync operation completed", Log.LogType.Information);
        }

        protected override void OnStop()
        {
            this.log.LogWrite("Service stops", Log.LogType.Warning);
        }
    }
}
