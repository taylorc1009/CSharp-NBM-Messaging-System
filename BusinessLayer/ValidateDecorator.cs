using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ValidateDecorator : ValidateComponent
    {

        public ValidateDecorator()
        {
        }

        protected ValidateComponent component;



        /// <summary>
        /// @param component 
        /// @return
        /// </summary>
        public void setComponent(ValidateComponent component)
        {
            // TODO implement here
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
