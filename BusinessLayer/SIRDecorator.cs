
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class SIRDecorator : ValidateDecorator
    {
        public SIRDecorator() { }

        public override bool validate(String sender, String subject, String message, DateTime sentAt, DateTime SIRDate, String sortCode, String nature)
        {
            string[] natures = { "ATM Theft", "Bomb Threat", "Cash Loss", "Customer Attack", "Intelligence", "Raid", "Staff Abuse", "Staff Attack", "Suspicious Incident", "Terrorism", "Theft" };
            return !String.IsNullOrEmpty(sender)
                && Utilities.isValidEmail(sender)
                && sender.Length <= 40
                /*&& !String.IsNullOrEmpty(subject)
                && (subject.Substring(0, 4) == "SIR " && DateTime.TryParseExact(subject.Substring(4), "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))*/
                && !String.IsNullOrEmpty(message)
                //&& checkSIRStandard(message)
                //&& (sentAt >= DateTime.Now.AddYears(-1) && sentAt <= DateTime.Now)
                && (SIRDate >= DateTime.Now.AddYears(-1) && SIRDate <= DateTime.Now)
                && !String.IsNullOrEmpty(sortCode)
                && Utilities.isValidSortCode(sortCode)
                && !String.IsNullOrEmpty(nature)
                && natures.Contains(nature);
        }

        /*private bool checkSIRStandard(String message)
        {
            String[] lineOne, lineTwo, natures = { "ATM Theft", "Bomb Threat", "Cash Loss", "Customer Attack", "Intelligence", "Raid", "Staff Abuse", "Staff Attack", "Suspicious Incident", "Terrorism", "Theft" };
            int i = message.IndexOf('\n', 0), j = message.IndexOf('\n', i + 1);

            lineOne = message.Substring(0, i).Split(' ');
            lineTwo = message.Substring(j, message.IndexOf('\n', j + 1)).Split(' ');

            return lineOne[0] == "Sort Code: "
                && Utilities.isValidSortCode(lineOne[1])
                && lineTwo[0] == "Nature of Incident"
                && natures.Contains(lineTwo[1]);
        }*/
    }
}