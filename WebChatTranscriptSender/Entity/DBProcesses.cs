using WebChatTranscriptSender.Entity;
using System;
using System.Xml.Serialization;
namespace WebChatTranscriptSender.Entity
{
    [Serializable]
    [XmlRoot("DBProcessCollection")]
    public class DBProcesses
    {
        [XmlArrayItem("DBProcess", typeof(DBProcess))]
        [XmlArray("DBProcesses")]
        public DBProcess[] setting
        {
            get;
            set;
        }
    }
}
