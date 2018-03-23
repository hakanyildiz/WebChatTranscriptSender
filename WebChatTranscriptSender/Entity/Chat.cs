using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebChatTranscriptSender.Entity
{
    public class Chat
    {
        private string chatHistory;

        private string transcript;

        private string email;

        private string phone;

        public string ChatHistory
        {
            get
            {
                return this.chatHistory;
            }
            set
            {
                this.chatHistory = value;
            }
        }

        public string Transcript
        {
            get
            {
                return this.transcript;
            }
            set
            {
                this.transcript = value;
            }
        }

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = value;
            }
        }

        public string Phone
        {
            get
            {
                return this.phone;
            }
            set
            {
                this.phone = value;
            }
        }
    }
}
