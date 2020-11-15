
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class SEMDecorator : ValidateDecorator
    {

        public SEMDecorator()
        {
        }

        /// <summary>
        /// @return
        /// </summary>
        public override bool validate(String sender, String subject, String message, DateTime sentAt, String sortCode, String nature)
        {
            // TODO implement here
            return false;
        }

    }
}