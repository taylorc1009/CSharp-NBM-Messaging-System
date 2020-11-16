
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class SMSDecorator : ValidateDecorator
    {
        public SMSDecorator() { }

        public override bool validate(String sender, String subject, String message, DateTime SIRDate, String sortCode, String nature)
        {
            return !String.IsNullOrEmpty(sender)
                && Utilities.isValidPhoneNumber(sender)
                && sender.Length <= 12
                && !String.IsNullOrEmpty(message)
                && message.Length <= 140;
        }
    }
}