using System;

namespace BusinessLayer
{
    public class SMS : MessageSupplement
    {
        public SMS(String sender, String text)
        {
            this.sender = sender;
            this.text = text;

            //tells the decorator we want to decorate 'validate' for this object using the 'SMSDecorator'
            this.decorate(1);
        }
    }
}