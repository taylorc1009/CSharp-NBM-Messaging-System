
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class SMS : MessageSupplement
    {

        public SMS()
        {
        }


        /// <summary>
        /// @param sender 
        /// @param text 
        /// @return
        /// </summary>
        public SMS(String sender, String text, DateTime sentAt, char header)
        {
            this.sender = sender;
            this.text = text;
            this.sentAt = sentAt;
            this.header = header;
        }
    }
}