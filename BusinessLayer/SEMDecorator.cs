using System;

namespace BusinessLayer
{
    public class SEMDecorator : ValidateDecorator
    {
        public SEMDecorator() { }
        
        //used to decorate the method 'validate' with the Standard Email Message validations
        public override bool validate(String sender, String subject, String message, DateTime SIRDate, String sortCode, String nature)
        {
            return !String.IsNullOrEmpty(sender)
                && Utilities.isValidEmail(sender)
                && sender.Length <= 40
                && !String.IsNullOrEmpty(subject)
                && subject.Length <= 20
                && !String.IsNullOrEmpty(message)
                && message.Length <= 1028;
        }
    }
}