using System.Collections.Generic;

namespace AG.Framework.Web.ExtJs
{

    /// <summary>
    /// <code>
    /// [{
    ///    id: 1,
    ///    text: 'A leaf Node',
    ///    leaf: true
    ///   },{
    ///    id: 2,
    ///    text: 'A folder Node',
    ///    children: [{
    ///        id: 3,
    ///        text: 'A child Node',
    ///        leaf: true
    ///    }]
    ///  }]
    /// </code>
    /// </summary>
    public class ExtJsTreeResponse<T> where T: ExtJsTreeNode<T>
    {
        public ExtJsTreeResponse()
        {
        }

        public ExtJsTreeResponse (T root)
        {
            nodes = new List<T>(1);
            nodes.Add(root);
        }

        public ExtJsTreeResponse(List<T> roots)
        {
            nodes = roots;
        }

        public virtual List<T> nodes { get; protected set; }
    }
}
