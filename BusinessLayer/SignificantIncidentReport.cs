using System;

namespace BusinessLayer
{
    public class SignificantIncidentReport : Email
    {
        //attributes are public for serialization purposes
        public DateTime date { get; set; }
        public String sortCode { get; set; }
        public String nature { get; set; }

        public SignificantIncidentReport(String sender, DateTime date, String sortCode, String nature, String text)
        {
            this.sender = sender;
            this.date = date;
            this.sortCode = sortCode;
            this.nature = nature;

            //builds the Significant Incident Report subject with the 'date' provided
            String s = "SIR " + this.date.ToString("dd/MM/yy");
            this.subject = s;

            //builds the Significant Incident Report body with the 'sortCode' and 'nature' provided
            String t = "Sort Code: " + this.sortCode + Environment.NewLine
                    + "Nature of Incident: " + this.nature + Environment.NewLine + Environment.NewLine
                    + text;
            this.text = t;

            //tells the decorator we want to decorate 'validate' for this object using the 'SIRDecorator'
            this.decorate(4);
        }
    }
}