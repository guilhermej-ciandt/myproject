namespace AG.Framework.Web.ExtJs
{
    public class FileUploaderResponse<T> : ExtJsActionSubmitResponse
    {

        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Numa requisi��o de load, cont�m o objeto com os atributos que
        /// popular�o o Form
        /// </summary>
        public T data { get; set; }
    }
}