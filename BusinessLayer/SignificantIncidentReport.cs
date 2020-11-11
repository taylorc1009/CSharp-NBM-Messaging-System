
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class SignificantIncidentReport : Email
    {

        public SignificantIncidentReport()
        {
        }

        public DateTime date { get; set; }

        public String sortCode { get; set; }

        public String nature { get; set; }


        /// <summary>
        /// @param sender 
        /// @param text 
        /// @param date 
        /// @param sortCode 
        /// @param nature 
        /// @return
        /// </summary>
        public SignificantIncidentReport(String sender, DateTime date, String sortCode, String nature, String text, DateTime sentAt, char header)
        {
            this.sender = sender;
            this.date = date;
            this.sortCode = sortCode;
            this.nature = nature;
            this.text = text;
            this.sentAt = sentAt;
            this.header = header;
        }
    }
}