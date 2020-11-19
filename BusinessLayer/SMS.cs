
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class SMS : MessageSupplement
    {
        public SMS(String sender, String text)
        {
            this.sender = sender;
            this.text = text;
            this.decorate(1);
        }
    }
}