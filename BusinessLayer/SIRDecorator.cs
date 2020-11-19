using System;
using System.Linq;

namespace BusinessLayer
{
    public class SIRDecorator : ValidateDecorator
    {
        public SIRDecorator() { }

        //used to decorate the method 'validate' with the Significant Incident Report validations
        public override bool validate(String sender, String subject, String message, DateTime SIRDate, String sortCode, String nature)
        {
            //specifies the only accepted incident 'natures'
            string[] natures = { "ATM Theft", "Bomb Threat", "Cash Loss", "Customer Attack", "Intelligence", "Raid", "Staff Abuse", "Staff Attack", "Suspicious Incident", "Terrorism", "Theft" };
            
            return !String.IsNullOrEmpty(sender)
                && Utilities.isValidEmail(sender)
                && sender.Length <= 40
                && !String.IsNullOrEmpty(message)
                && message.Length <= 1028
                && (SIRDate >= DateTime.Now.AddYears(-1) && SIRDate <= DateTime.Now)
                && !String.IsNullOrEmpty(sortCode)
                && Utilities.isValidSortCode(sortCode)
                && !String.IsNullOrEmpty(nature)
                && natures.Contains(nature);
        }
    }
}