
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class SMS : MessageSupplement
    {
        public SMS() { }

        public SMS(String sender, String text, DateTime sentAt)
        {
            this.sender = sender;
            this.text = text;
            this.sentAt = sentAt;
            this.decorate(1);
        }
    }
}