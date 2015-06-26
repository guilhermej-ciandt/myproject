using System.Collections.Generic;

namespace AG.Framework.Web.ExtJs
{
    /// <summary>
    /// DTO para ser usado como retorno de serviços
    /// 
    /// http://www.extjs.com/deploy/dev/docs/source/Action1.html#cls-Ext.form.Action.Load
    /// </summary>
    public abstract class ExtJsActionBaseResponse
    {

        /// <summary>
        /// Indica que a requisição foi feita com sucesso
        /// </summary>
        public bool success {get; set;}

        /// <summary>
        /// Nro Total de resultados no caso de requisicao de paging, por exemplo
        /// </summary>
        public long total { get; set; }

        /// <summary>
        /// Armazena os erros de preenchimento numa requisição de Submit
        /// Deve ser indexado da seguinte forma: Nome do Field (declarado no FormPanel), texto da mensagem
        /// </summary>
        public IDictionary<string, string> errors { get; set; }

        
    }



}
