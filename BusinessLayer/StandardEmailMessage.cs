using System;

namespace BusinessLayer
{
    public class StandardEmailMessage : Email
    {
        public StandardEmailMessage(String sender, String subject, String text)
        {
            this.sender = sender;
            this.text = text;
            this.subject = subject;

            //tells the decorator we want to decorate 'validate' for this object using the 'SEMDecorator'
            this.decorate(3);
        }
    }
}