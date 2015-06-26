using System.Collections.Generic;

namespace AG.Framework.Validation
{
    public class ConfirmValidationException: ValidationException
    {

        public ConfirmValidationException(string message)
            : base(message) {}

        public ConfirmValidationException(IDictionary<string, string> errors) 
            : base(errors){}

    }
}
