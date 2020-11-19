using System;

namespace BusinessLayer
{
    public class ValidateDecorator : ValidateComponent
    {
        //stores an inctance of the decorator provided to 'setComponent'
        private ValidateComponent component;

        public void setComponent(ValidateComponent component)
        {
            this.component = component;
        }

        public override bool validate(String sender, String subject, String message, DateTime SIRDate, String sortCode, String nature)
        {
            //uses the overridden 'validate' method, with the mothod of whatever decorator is present
            if (component != null)
                return component.validate(sender, subject, message, SIRDate, sortCode, nature);
            return false;
        }

    }
}
