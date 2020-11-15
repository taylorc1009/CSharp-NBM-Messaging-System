
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class StandardEmailMessage : Email
    {
        public StandardEmailMessage() { }

        public StandardEmailMessage(String sender, String subject, String text, DateTime sentAt, char header)
        {
            this.sender = sender;
            this.text = text;
            this.subject = subject;
            this.sentAt = sentAt;
            this.header = header;
            this.decorate(3);
        }
    }
}