
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class SIRDecorator : ValidateDecorator
    {
        public SIRDecorator() { }

        public override bool validate()
        {
            // TODO implement here
            return false;
        }
    }
}