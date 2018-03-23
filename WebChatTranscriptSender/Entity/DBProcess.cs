using System;
using System.Data;
using System.Xml.Serialization;

namespace WebChatTranscriptSender.Entity
{

    [Serializable]
    public class DBProcess
    {
        [XmlElement("OperationName")]
        public string OperationName
        {
            get;
            set;
        }

        [XmlElement("DataFromSource")]
        public string DataFromSource
        {
            get;
            set;
        }

        [XmlElement("SourceConString")]
        public string SourceConString
        {
            get;
            set;
        }

        [XmlElement("UniqueValue")]
        public int UniqueValue
        {
            get;
            set;
        }

        public DataTable Data
        {
            get;
            set;
        }
    }

}

