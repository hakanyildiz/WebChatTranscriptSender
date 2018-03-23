using WebChatTranscriptSender.Helper;
using System;
using System.Configuration;

namespace WebChatTranscriptSender.Helper
{
    public class GeneralSettings
    {
        private Logger log = new Logger();

        private static GeneralSettings generalSettingsObject;

        private static object checkForLock = new object();

        private GeneralSettings()
        {
        }

        public static GeneralSettings CreateGeneralSettingsObject()
        {
            if (GeneralSettings.generalSettingsObject == null)
            {
                lock (GeneralSettings.checkForLock)
                {
                    if (GeneralSettings.generalSettingsObject == null)
                    {
                        GeneralSettings.generalSettingsObject = new GeneralSettings();
                    }
                }
            }
            return GeneralSettings.generalSettingsObject;
        }

        public string GetAppKey(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key].ToString();
            }
            catch (Exception ex)
            {
                this.log.LogWrite("Key : " + key + ", Exception : " + ex.ToString(), Log.LogType.Error);
                return string.Empty;
            }
        }
    }

}
