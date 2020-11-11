
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
        public StandardEmailMessage(String sender, String subject, String text, char header)
        {
            this.sender = sender;
            this.text = text;
            this.subject = subject;
            this.header = header;
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