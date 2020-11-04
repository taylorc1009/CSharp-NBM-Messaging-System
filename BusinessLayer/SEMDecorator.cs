
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
        public override bool validate()
        {
            // TODO implement here
            return false;
        }

    }
}