namespace AG.Framework.Web.ExtJs
{
    public class FileUploaderResponse<T> : ExtJsActionSubmitResponse
    {

        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Numa requisição de load, contém o objeto com os atributos que
        /// popularão o Form
        /// </summary>
        public T data { get; set; }
    }
}