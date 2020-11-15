
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class SMSDecorator : ValidateDecorator
    {
        public SMSDecorator() { }

        public override bool validate(String sender, String subject, String message, DateTime sentAt, String sortCode, String nature)
        {
            
            return false;
        }
    }
}