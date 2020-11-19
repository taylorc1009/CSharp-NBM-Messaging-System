using System;

namespace BusinessLayer
{
    public class SMSDecorator : ValidateDecorator
    {
        public SMSDecorator() { }

        //used to decorate the method 'validate' with the SMS validations
        public override bool validate(String sender, String subject, String message, DateTime SIRDate, String sortCode, String nature)
        {
            return !String.IsNullOrEmpty(sender)
                && Utilities.isValidPhoneNumber(sender)
                && !String.IsNullOrEmpty(message)
                && message.Length <= 140;
        }
    }
}