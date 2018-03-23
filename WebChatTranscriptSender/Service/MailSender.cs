using System;
using System.Net;
using System.Net.Mail;
using WebChatTranscriptSender.Helper;

namespace WebChatTranscriptSender.Service
{
    public class MailSender
    {
        private Log log = Log.CreateLogObject();

        private GeneralSettings genSet = GeneralSettings.CreateGeneralSettingsObject();

        private static MailSender mailSenderObject;

        private static object checkForLock = new object();

        private MailSender()
        {
        }

        public static MailSender CreateMailSenderObject()
        {
            if (MailSender.mailSenderObject == null)
            {
                lock (MailSender.checkForLock)
                {
                    if (MailSender.mailSenderObject == null)
                    {
                        MailSender.mailSenderObject = new MailSender();
                    }
                }
            }
            return MailSender.mailSenderObject;
        }

        public bool SendMail(string MailFrom, string MailTo, string Subject, string Body, string Host, string Pass, string Attachment, bool? EnableSSL, bool? isHtmlBody, int? Port)
        {
            this.SetParameters(ref MailFrom, ref MailTo, ref Subject, ref Body, ref isHtmlBody, ref Port, ref Host, ref Pass, ref Attachment, ref EnableSSL);
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(MailFrom, MailFrom);
            mailMessage.To.Add(MailTo);
            mailMessage.Subject = Subject;
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = Convert.ToBoolean(isHtmlBody);
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Port = Convert.ToInt32(Port);
            smtpClient.Host = Host;
            smtpClient.EnableSsl = Convert.ToBoolean(EnableSSL);
            smtpClient.Credentials = new NetworkCredential(MailFrom, Pass);
            try
            {
                if (string.IsNullOrEmpty(Attachment))
                {
                    mailMessage.Attachments.Add(new Attachment(Attachment));
                }
                smtpClient.Send(mailMessage);
                this.log.LogWrite("Mail sent successfully. MailTo : " + MailTo + ", Subject : " + mailMessage.Subject, Log.LogType.Information, "");
                return true;
            }
            catch (Exception ex)
            {
                this.log.LogWrite("Error at sending mail." + MailTo + ", Subject : " + mailMessage.Subject + ", Error : " + ex.ToString(), Log.LogType.Error, "");
                return false;
            }
        }

        private void SetParameters(ref string MailFrom, ref string MailTo, ref string Subject, ref string Body, ref bool? isHtmlBody, ref int? Port, ref string Host, ref string Pass, ref string Attachment, ref bool? EnableSSL)
        {
            if (string.IsNullOrEmpty(MailFrom))
            {
                MailFrom = this.genSet.GetAppKey("Email");
            }
            if (string.IsNullOrEmpty(MailTo))
            {
                MailTo = this.genSet.GetAppKey("Email");
            }
            if (string.IsNullOrEmpty(Subject))
            {
                Subject = this.genSet.GetAppKey("Email");
            }
            if (string.IsNullOrEmpty(Body))
            {
                Body = this.genSet.GetAppKey("Email");
            }
            if (!isHtmlBody.HasValue)
            {
                isHtmlBody = Convert.ToBoolean(this.genSet.GetAppKey("Email"));
            }
            if (!Port.HasValue)
            {
                Port = Convert.ToInt32(this.genSet.GetAppKey("Email"));
            }
            if (string.IsNullOrEmpty(Host))
            {
                Host = this.genSet.GetAppKey("Email");
            }
            if (string.IsNullOrEmpty(Pass))
            {
                Pass = this.genSet.GetAppKey("Email");
            }
            if (string.IsNullOrEmpty(Attachment))
            {
                Attachment = this.genSet.GetAppKey("Email");
            }
            if (!EnableSSL.HasValue)
            {
                EnableSSL = Convert.ToBoolean(this.genSet.GetAppKey("Email"));
            }
        }
    }

}
