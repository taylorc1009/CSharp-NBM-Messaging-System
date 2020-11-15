using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ValidateDecorator : ValidateComponent
    {
        protected ValidateComponent component;

        public void setComponent(ValidateComponent component)
        {
            this.component = component;
        }

        public override bool validate(String sender, String subject, String message, DateTime sentAt, String sortCode, String nature)
        {
            if (component != null)
                return component.validate(sender, subject, message, sentAt, sortCode, nature);
            return false;
        }

    }
}
