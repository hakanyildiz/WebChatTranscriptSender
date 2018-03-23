using Com.Avaya.Util.Encoders;
using WebChatTranscriptSender.Entity;
using WebChatTranscriptSender.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace WebChatTranscriptSender.Service
{
    public class ChatHistoryService
    {
        public List<Chat> PrepareData(DBProcess setting)
        {
            List<Chat> chatList = new List<Chat>();
            foreach (DataRow row in setting.Data.Rows)
            {
                chatList.Add(new Chat
                {
                    ChatHistory = this.Converter(row["TEXT"].ToString(), row["PHONE"].ToString()),
                    Email = row["EMAIL"].ToString(),
                    Transcript = row["TRANSCRIPT"].ToString(),
                    Phone = row["PHONE"].ToString()
                });
            }
            return chatList;
        }

        private string Converter(string EncodedText, string Phone)
        {
            byte[] toDecodeByteArray = Base64.Decode(EncodedText);
            string clearText5 = Encoding.UTF8.GetString(toDecodeByteArray.ToArray());
            clearText5 = ChatHistoryService.FormatDateValues(clearText5);
            string name, date;
            ChatHistoryService.FillFields(clearText5, out name, out date);
            MemoryStream memoryStream2 = new MemoryStream(Encoding.UTF8.GetBytes(clearText5));
            XPathDocument xdoc = new XPathDocument(memoryStream2);
            XslTransform trans = new XslTransform();
            string path = Assembly.GetExecutingAssembly().Location;
            FileInfo fileInfo = new FileInfo(path);
            string dir = fileInfo.DirectoryName;
            trans.Load(dir + "\\ExternalFiles\\ChatTranscript.xslt");
            memoryStream2 = new MemoryStream();
            StreamWriter sw = new StreamWriter(memoryStream2);
            trans.Transform(xdoc, null, memoryStream2);
            memoryStream2.Seek(0L, SeekOrigin.Begin);
            StreamReader srr = new StreamReader(memoryStream2);
            clearText5 = srr.ReadToEnd();
            clearText5 = clearText5.Replace("@Name", name);
            clearText5 = clearText5.Replace("@Date", date);
            return clearText5.Replace("@Phone", Phone);
        }

        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * 2];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private static void FillFields(string clearText, out string name, out string date)
        {
            name = "";
            date = "";
            try
            {
                XDocument doc = XDocument.Parse(clearText);
                IEnumerable<XElement> nameReader = doc.Elements("transcript").Elements("parameters");
                name = (from r in nameReader
                        select r.Attribute("customerInfo.login").Value).First();
                IEnumerable<XElement> dateReader = doc.Elements("transcript").Elements("call");
                date = (from r in dateReader
                        select r.Attribute("datetime").Value).First().Substring(0, 10);
            }
            catch (Exception)
            {
            }
        }

        private static string FormatDateValues(string clearText)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(clearText);
            XmlElement root = xmlDocument.DocumentElement;
            XmlNodeList nodes = root.ChildNodes;
            foreach (XmlNode item in nodes)
            {
                for (int i = 0; i < item.Attributes.Count; i++)
                {
                    if (item.Attributes[i].Name == "datetime")
                    {
                        DateTime value = DateTime.MinValue;
                        if (DateTime.TryParse(item.Attributes[i].Value, out value))
                        {
                            item.Attributes[i].Value = value.ToString("dd.MM.yyyy HH:mm:ss");
                        }
                    }
                }
            }
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlTextWriter = XmlWriter.Create(stringWriter))
                {
                    xmlDocument.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    clearText = stringWriter.GetStringBuilder().ToString();
                    clearText = clearText.Replace("<tr xmlns=\"\">", "<tr>");
                }
            }
            return clearText;
        }
    }
}
