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

        public override bool validate()
        {
            if (component != null)
                return component.validate();
            return false;
        }

    }
}
