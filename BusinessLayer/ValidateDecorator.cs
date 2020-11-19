using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ValidateDecorator : ValidateComponent
    {
        private ValidateComponent component;

        public void setComponent(ValidateComponent component)
        {
            this.component = component;
        }

        public override bool validate(String sender, String subject, String message, DateTime SIRDate, String sortCode, String nature)
        {
            if (component != null)
                return component.validate(sender, subject, message, SIRDate, sortCode, nature);
            return false;
        }

    }
}
