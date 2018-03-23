using WebChatTranscriptSender.Entity;
using WebChatTranscriptSender.Helper;
using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;


namespace WebChatTranscriptSender.Service
{
    public class Serializer
    {
        private Logger log = new Logger();

        public DBProcesses Deserialize()
        {
            try
            {
                this.log.LogWrite("Deserialize process started", Log.LogType.Information);
                string path = Assembly.GetExecutingAssembly().Location;
                FileInfo fileInfo = new FileInfo(path);
                string dir = fileInfo.DirectoryName;
                DBProcesses settings2 = null;
                XmlSerializer serializer = new XmlSerializer(typeof(DBProcesses));
                StreamReader reader2 = new StreamReader(dir + "\\ExternalFiles\\Settings.xml");
                settings2 = (DBProcesses)serializer.Deserialize(reader2);
                reader2.Close();
                reader2 = null;
                this.log.LogWrite("Deserialize operation ended successfully", Log.LogType.Information);
                return settings2;
            }
            catch (Exception ex)
            {
                this.log.LogWrite("Deserialize operation error. " + ex.ToString(), Log.LogType.Information);
                return null;
            }
        }

        public DBProcesses Serialize(DBProcesses settings)
        {
            try
            {
                this.log.LogWrite("Serialize process started", Log.LogType.Information);
                string path = Assembly.GetExecutingAssembly().Location;
                FileInfo fileInfo = new FileInfo(path);
                string dir = fileInfo.DirectoryName;
                XmlSerializer SerializerObj = new XmlSerializer(typeof(DBProcesses));
                TextWriter writeFileStream2 = new StreamWriter(dir + "\\ExternalFiles\\Settings.xml");
                SerializerObj.Serialize(writeFileStream2, settings);
                writeFileStream2.Close();
                writeFileStream2 = null;
                this.log.LogWrite("Serialize operation ended successfully", Log.LogType.Information);
                return settings;
            }
            catch (Exception ex)
            {
                this.log.LogWrite("Serialize operation error. " + ex.ToString(), Log.LogType.Error);
                return null;
            }
        }
    }
}
