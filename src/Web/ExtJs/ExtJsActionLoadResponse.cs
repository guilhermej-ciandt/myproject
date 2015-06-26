namespace AG.Framework.Web.ExtJs
{
    /// <summary>
    /// 
    /// </summary>
    public class ExtJsActionLoadResponse<T> : ExtJsActionBaseResponse
    {
        /// <summary>
        /// Numa requisição de load, contém o objeto com os atributos que
        /// popularão o Form
        /// </summary>
        public T data { get; set; }

        /// <summary>
        /// Mensagem de erro no caso de problemas no processamento de requisições Load 
        /// </summary>
        public string errorMessage { get; set; }

    }

}
