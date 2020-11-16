
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class SEMDecorator : ValidateDecorator
    {
        public SEMDecorator() { }
        
        public override bool validate(String sender, String subject, String message, DateTime sentAt, DateTime SIRDate, String sortCode, String nature)
        {
            return !String.IsNullOrEmpty(sender)
                && Utilities.isValidEmail(sender)
                && sender.Length <= 40
                && !String.IsNullOrEmpty(subject)
                && subject.Length <= 20
                && !String.IsNullOrEmpty(message)
                && message.Length <= 1028;
                //&& (sentAt >= DateTime.Now.AddYears(-1) && sentAt <= DateTime.Now);
        }
    }
}