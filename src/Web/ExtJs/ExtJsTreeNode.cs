using System;
using System.Collections.Generic;
using AG.Framework.Utils;

namespace AG.Framework.Web.ExtJs
{

    /// <summary>
    /// Representa um nó de uma árvore (TreeNode / TreePanel)
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
    /// 
    /// <seealso cref="Ext.tree.TreeNode"/>
    /// </summary>    
    public class ExtJsTreeNode<T> : ICloneable
    {
        public ExtJsTreeNode()
        {
            leaf = true;
            disabled = false;
            Checked = "undefined";
        }

        public ExtJsTreeNode(string id, string text, bool leaf)
        {
            this.id = id;
            this.text = text;
            this.leaf = leaf;
        }

        public int Level { get; internal set; }

        /// <summary>
        /// id do nó
        /// </summary>
        public virtual string id { get; set; }
        
        /// <summary>
        /// Texto/label do nó
        /// </summary>
        public virtual string text { get; set; }

        /// <summary>
        /// Css class to apply to tree node
        /// </summary>
        public virtual string cls { get; set; }

        /// <summary>
        /// Css class to apply to tree node icon
        /// </summary>
        public virtual string iconCls { get; set; }
        
        /// <summary>
        /// Identifica um nó folha
        /// </summary>
        public virtual bool leaf { get; set; }
        
        /// <summary>
        /// Flag que habilita e desabilita um nó
        /// </summary>
        public virtual bool disabled { get; set; }

        /// <summary>
        /// Flag que (des)habilita o nó para drop
        /// </summary>
        public virtual bool allowDrop { get; set; }
        
        /// <summary>
        /// true
        /// false
        /// undefined
        /// </summary>
        public virtual string Checked { get; set; }

        ///// <summary>
        ///// Nós filhos
        ///// </summary>
        public virtual IList<T> children { get; set; }

        public virtual void AddChild(T child)
        {
            if (children == null)
            {
                children = new List<T>();
                leaf = false;
            }

            children.Add(child);
        }

        public virtual void SetId(int novoId)
        {
            id = Convert.ToString(novoId);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
