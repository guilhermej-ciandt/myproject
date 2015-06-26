using System.Collections.Generic;

namespace AG.Framework.Validation
{
    /// <summary>
    /// Excecao usada para mostrar erros de validacao de Forms
    /// </summary>
    public class FormValidationException : ValidationException
    {

        public FormValidationException(string message)
            : base(message) {}

        public FormValidationException(IDictionary<string, string> errors) 
            : base(errors){}

    }
}
