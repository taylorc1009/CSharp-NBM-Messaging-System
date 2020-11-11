
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

        public String subject { get; set; }


        /// <summary>
        /// @param sender 
        /// @param text 
        /// @param subject 
        /// @return
        /// </summary>
        public StandardEmailMessage(String sender, String subject, String text, DateTime sentAt, char header)
        {
            this.sender = sender;
            this.text = text;
            this.subject = subject;
            this.sentAt = sentAt;
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