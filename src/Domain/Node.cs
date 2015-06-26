using System;
using System.Collections.Generic;
using System.Linq;

namespace AG.Framework.Domain
{
    public class Node : ICloneable
    {
        public virtual int Id { get; set; }
        public virtual int? IdPai { get; set; }
        public virtual Node Parent { get; set; }
        public virtual IList<Node> Children { get; set; }
        public virtual int Level { get; set; }
        public virtual bool Leaf { get; set; }
        private bool _visivel = true;
        public virtual bool Visivel
        {
            get { return _visivel; }
            set { _visivel = value; }
        }

        public virtual bool Colapsed { get; set; }

        public virtual bool IsRootNode
        {
            get { return Parent == null;}            
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }


        /// <summary>
        /// Returns a collection containing this element and all descendant elements.
        /// </summary>
        public virtual IEnumerable<Node> DescendantsAndSelf()
        {
            yield return this;

            foreach (var child in Descendents())
            {
                yield return child;
            }
        }

        public virtual IEnumerable<Node> Descendents()
        {
            foreach (var child in Children)
            {
                yield return child;
                foreach (var grandChild in child.Descendents())
                {
                    yield return grandChild;
                }
            }
        }

        /// <summary>
        /// Returns a collection of children elements not of type T.
        /// </summary>
        public virtual IEnumerable<Node> ChildrenNotOfType<TK>()
        {
            return Children.Where(e => !(e is TK));
        }

        public virtual IEnumerable<Node> ChildrenByType<TK>()
        {
            return Children.Where(e => e is TK);
        }

        /// <summary>
        /// Returns a collection of descendant elements.
        /// </summary>
        public virtual IEnumerable<Node> Descendants<TK>()
        {
            return Descendents().Where(e => e is TK);
        }

        protected int ContaFilhos()
        {
            return Children != null ? Children.Count : 0;
        }

        protected int ContaFilhos<T>()
        {
            return Children != null ? Children.Count(x=> x is T) : 0;
        }
        
        protected int ContaFilhos<T>(bool visibilidade)
        {
            return Children != null ? Children.Count(x=> x is T && x.Visivel) : 0;
        }

        public virtual bool Possui<T>()
        {
            return ContaFilhos<T>() > 0;
        }

        public virtual IEnumerable<T> GetFilhos<T>()
        {
            return Children==null ? null: Children.Where(child => child is T).Cast<T>();
        }

        public virtual void AdicionaFilhos(IList<Node> filhos)
        {
            foreach (var filho in filhos)
            {
                AdicionaFilho(filho);
            }
        }

        public virtual void AdicionaFilho(Node filho)
        {
            if (Children == null)
            {
                Children = new List<Node>();
            }
            filho.Parent = this;
            filho.IdPai = this.Id;
            Children.Add(filho);
        }

        public virtual bool AvoExpandido
        {
            get
            {
                if (Parent == null) return true;
                var avo = Parent.Parent;
                return avo == null || !(avo.Colapsed);
            }
        }

        public virtual bool PaiExpandido
        {
            get
            {
                if (Parent == null) return true;
                return !(Parent.Colapsed);
            }
        }

        private bool _consideraNaMontagemDaArvore = true;
        public virtual bool ConsideraNaMontagemDaArvore
        {
            set { _consideraNaMontagemDaArvore = value; }
            get
            {
                if (_consideraNaMontagemDaArvore) return _consideraNaMontagemDaArvore;
                return Parent == null ? _consideraNaMontagemDaArvore : Parent.ConsideraNaMontagemDaArvore;
            }
        }
    }
}
