
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class StandardEmailMessage : Email
    {
        public StandardEmailMessage() { }

        public StandardEmailMessage(String sender, String subject, String text)
        {
            this.sender = sender;
            this.text = text;
            this.subject = subject;
            this.decorate(3);
        }
    }
}