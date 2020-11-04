
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

        private DateTime date;

        private String sortCode;

        private String nature;


        /// <summary>
        /// @param sender 
        /// @param text 
        /// @param date 
        /// @param sortCode 
        /// @param nature 
        /// @return
        /// </summary>
        public SignificantIncidentReport(String sender, String text, DateTime date, String sortCode, String nature)
        {
            // TODO implement here
        }

        /// <summary>
        /// @return
        /// </summary>
        public DateTime getDate()
        {
            // TODO implement here
            return new DateTime();
        }

        /// <summary>
        /// @return
        /// </summary>
        public String getSortCode()
        {
            // TODO implement here
            return null;
        }

        /// <summary>
        /// @return
        /// </summary>
        public String getNature()
        {
            // TODO implement here
            return null;
        }

    }
}