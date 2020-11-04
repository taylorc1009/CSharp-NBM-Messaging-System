
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class StandardEmailMessage : Email
    {

        public StandardEmailMessage()
        {
        }

        private String subject;


        /// <summary>
        /// @param sender 
        /// @param text 
        /// @param subject 
        /// @return
        /// </summary>
        public StandardEmailMessage(String sender, String text, String subject)
        {
            // TODO implement here
        }

        /// <summary>
        /// @return
        /// </summary>
        public String getSubject()
        {
            // TODO implement here
            return null;
        }

    }
}