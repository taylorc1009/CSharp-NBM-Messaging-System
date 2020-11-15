
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class StandardEmailMessage : Email
    {
        public StandardEmailMessage() { }

        public StandardEmailMessage(String sender, String subject, String text, DateTime sentAt)
        {
            this.sender = sender;
            this.text = text;
            this.subject = subject;
            this.sentAt = sentAt;
            this.decorate(3);
        }
    }
}