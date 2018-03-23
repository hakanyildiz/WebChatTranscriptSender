using WebChatTranscriptSender.Entity;
using WebChatTranscriptSender.Helper;
using WebChatTranscriptSender.Service;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace WebChatTranscriptSender.Service
{
    internal class MailService
    {
        //private MailSender mailSender = MailSender.CreateMailSenderObject();

        private Logger log = new Logger();

        private GeneralSettings generalSettings = GeneralSettings.CreateGeneralSettingsObject();

        private FileService fileService = new FileService();

        internal void SendMail(List<Chat> chatList)
        {
            this.log.LogWrite("Sending mail operation starts.", Log.LogType.Information);
            foreach (Chat chat in chatList)
            {
                string filePath = this.fileService.SaveFileAsHtml(chat);
                this.SendMail(this.generalSettings.GetAppKey("MailFrom"), this.generalSettings.GetAppKey("SenderName"), chat.Email, this.generalSettings.GetAppKey("Subject").Replace("@Transcript", chat.Transcript), chat.ChatHistory, this.generalSettings.GetAppKey("Host"), filePath, Convert.ToBoolean(this.generalSettings.GetAppKey("EnableSSL")), Convert.ToBoolean(this.generalSettings.GetAppKey("IsHtmlBody")), Convert.ToInt32(this.generalSettings.GetAppKey("Port")), true);
            }
        }

        private bool SendMail(string MailFrom = null, string SenderName = null, string MailTo = null, string Subject = null, string Body = null, string Host = null, string Attachment = null, bool? EnableSSL = default(bool?), bool? isHtmlBody = default(bool?), int? Port = default(int?), bool UseDefaultCredentials = true)
        {
            ServicePointManager.ServerCertificateValidationCallback = ((object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true);
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(MailFrom, SenderName);
            mailMessage.To.Add(MailTo);
            mailMessage.Subject = Subject;
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = Convert.ToBoolean(isHtmlBody);
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Port = Convert.ToInt32(Port);
            smtpClient.Host = Host;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = Convert.ToBoolean(EnableSSL);
            smtpClient.UseDefaultCredentials = UseDefaultCredentials;
            try
            {
                if (!string.IsNullOrEmpty(Attachment))
                {
                    mailMessage.Attachments.Add(new Attachment(Attachment));
                }
                smtpClient.Send(mailMessage);
                this.log.LogWrite("Mail successfully sent. MailTo : " + MailTo + ", Subject : " + Subject, Log.LogType.Information);
                return true;
            }
            catch (Exception ex)
            {
                this.log.LogWrite("Exception occured while sending mail. MailTo : " + MailTo + ", Subject : " + Subject + ", Exception : " + ex.ToString(), Log.LogType.Error);
                return false;
            }
        }
    }
}
