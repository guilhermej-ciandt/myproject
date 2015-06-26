using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace AG.Framework.Validation
{
    /// <summary>
    /// Excecao lancada quando um parametro nao atende o contrato de um serviço.
    /// Contem um Dictionary de erros que indexa o nome do atributo e a mensagem de erro.
    /// </summary>
    public class ValidationException : ApplicationException
    {    
        public ValidationException(){}

        public ValidationException(string message)
            : base(message)
        {
        }

        public ValidationException(string message, bool adicionaErro)
            : base(message)
        {
            if (adicionaErro)
                AdicionaErro("erro", message);
        }

        public ValidationException(IDictionary<string, string> errors)
        {
            Errors = errors;
        }

        public IDictionary<string, string> Errors { get; set; }

        public override IDictionary Data 
        {
            get
            {
                return Errors as IDictionary;
            }
        
        }

        public void AdicionaErro(string key, string value)
        {
            if (Errors == null)
                Errors = new Dictionary<string, string>();
            Errors[key] = value;
        }

        public void AdicionaErro(string key, string value, string delimitador)
        {
            if (Errors == null)
                Errors = new Dictionary<string, string>();

            Errors[key] = (Errors.ContainsKey(key)) ? Errors[key] + delimitador + value : delimitador + value;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("errors", Errors);
        }

        /// <summary>
        /// Método utilitário para lançar uma ValidationException com os detalhes do
        /// erro de validação serializados como JSON na mensagem da exceção.
        /// </summary>
        /// <param name="errors">Hash com o id do erro e a mensagem da mesma.</param>
        public static void ThrowWithMessageAsJson(IDictionary<string, string> errors)
        {
            throw new ValidationException(JsonConvert.SerializeObject(errors));
        }


    }
}
