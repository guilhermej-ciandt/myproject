using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using AG.Framework.Domain;
using AG.Framework.Web.ExtJs;
using Common.Logging;

namespace AG.Framework.Utils
{
    public static class LinqExtensionsMethod
    {

        /// <see cref="http://stackoverflow.com/questions/3758162/rendering-a-hierarchy-using-linq"/>
        /// <typeparam name="T"></typeparam>
        [Serializable]
        public class ArvoreGenerica<T>
        {
            public T item;
            public IList<ArvoreGenerica<T>> children;
            public int level;
            [NonSerialized]
            public ArvoreGenerica<T> parent;

            public bool HasChildren()
            {
                return children != null && children.Count() > 0;
            }

            public IEnumerable<ArvoreGenerica<T>> Ancestors()
            {
                var ancestors = parent;
                while (ancestors != null)
                {
                    yield return ancestors;
                    ancestors = ancestors.parent;
                }
            }

            public IEnumerable<ArvoreGenerica<T>> Descendents()
            {
                foreach (var child in children)
                {
                    yield return child;
                    foreach (var grandChild in child.Descendents())
                    {
                        yield return grandChild;
                    }
                }
            }

            /// <summary>
            /// Returns a collection containing this element and all child elements.
            /// </summary>
            public IEnumerable<ArvoreGenerica<T>> ChildrenAndSelf()
            {
                yield return this;

                foreach (var child in children)
                {
                    yield return child;
                }
            }

            /// <summary>
            /// Returns a collection of ancestor elements.
            /// </summary>
            public IEnumerable<ArvoreGenerica<T>> AncestorsAndSelf()
            {
                yield return this;

                foreach (var child in Ancestors())
                {
                    yield return child;
                }
            }

            /// <summary>
            /// Returns a collection containing this element and all descendant elements.
            /// </summary>
            public IEnumerable<ArvoreGenerica<T>> DescendantsAndSelf()
            {
                yield return this;

                foreach (var child in Descendents())
                {
                    yield return child;
                }
            }

            /// <summary>
            /// Returns a collection of the sibling elements before this node, in document order.
            /// </summary>
            public IEnumerable<ArvoreGenerica<T>> ElementsBeforeSelf()
            {
                if (Ancestors().FirstOrDefault() == null)
                    yield break;
                foreach (var child in Ancestors().First().children)
                {
                    if (child.Equals(this))
                        break;
                    yield return child;
                }
            }

            /// <summary>
            /// Returns a collection of the elements after this node, in document order.
            /// </summary>
            public IEnumerable<ArvoreGenerica<T>> ElementsAfterSelf()
            {
                if (Ancestors().FirstOrDefault() == null)
                    yield break;
                bool afterSelf = false;
                foreach (var child in Ancestors().First().children)
                {
                    if (afterSelf)
                        yield return child;

                    if (child.Equals(this))
                        afterSelf = true;
                }
            }

            /// <summary>
            /// Returns a collection of descendant elements.
            /// </summary>
            public IEnumerable<ArvoreGenerica<T>> Descendants<TK>()
            {
                return Descendents().Where(e => e.item is TK);
            }

            public IEnumerable<ArvoreGenerica<T>> Children<TK>()
            {
                return children.Where(e => e.item is TK);
            }

            public IEnumerable<ArvoreGenerica<T>> Children()
            {
                return children;
            }

            /// <summary>
            /// Returns a collection of children elements not of type T.
            /// </summary>
            public IEnumerable<ArvoreGenerica<T>> ChildrenNotOfType<TK>()
            {
                return children.Where(e => !(e.item is TK));
            }

            /// <summary>
            /// Returns a collection of descendant elements not of type T.
            /// </summary>
            public IEnumerable<ArvoreGenerica<T>> DescendantsNotOfType<TK>()
            {
                return Descendents().Where(e => !(e.item is TK));
            }

            /// <summary>
            /// Clone node and all properties
            /// </summary>
            /// <returns></returns>
            public ArvoreGenerica<T> Clone()
            {
                using (var stream = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, this);
                    stream.Position = 0;
                    return (ArvoreGenerica<T>)formatter.Deserialize(stream);
                }
            }
        }

        public static IEnumerable<ArvoreGenerica<T>> ByHierarchy<T>(
                this IEnumerable<T> source,
                Func<T, bool> startWith,
                Func<T, T, bool> connectBy)
        {
            return source.ByHierarchy(startWith, connectBy, null);
        }

        private static IEnumerable<ArvoreGenerica<T>> ByHierarchy<T>(
            this IEnumerable<T> source,
            Func<T, bool> startWith,
            Func<T, T, bool> connectBy,
            ArvoreGenerica<T> parent)
        {
            int level = (parent == null ? 0 : parent.level + 1);

            if (source == null)
                throw new ArgumentNullException("source");

            if (startWith == null)
                throw new ArgumentNullException("startWith");

            if (connectBy == null)
                throw new ArgumentNullException("connectBy");

            foreach (T value in from item in source
                                where startWith(item)
                                select item)
            {
                var children = new List<ArvoreGenerica<T>>();

                ArvoreGenerica<T> @new = new ArvoreGenerica<T>
                                             {
                                                 level = level,
                                                 parent = parent,
                                                 item = value,
                                                 children = children //.AsReadOnly()
                                             };

                children.AddRange(source.ByHierarchy(possibleSub => connectBy(value, possibleSub), connectBy, @new));

                yield return @new;
            }
        }

        public static IEnumerable<T> ByHierarchyExtJs<T>(
            this IEnumerable<T> source,
            Func<T, bool> startWith,
            Func<T, T, bool> connectBy)
            where T : ExtJsTreeNode<T>, new()
        {
            return source.ByHierarchyExtJs(startWith, connectBy, null);
        }

        private static IEnumerable<T> ByHierarchyExtJs<T>(
            this IEnumerable<T> source,
            Func<T, bool> startWith,
            Func<T, T, bool> connectBy,
            T parent)
            where T : ExtJsTreeNode<T>, new()
        {
            int level = (parent == null ? 0 : parent.Level + 1);

            if (source == null)
                throw new ArgumentNullException("source");

            if (startWith == null)
                throw new ArgumentNullException("startWith");

            if (connectBy == null)
                throw new ArgumentNullException("connectBy");

            foreach (T value in from item in source
                                where startWith(item)
                                select item)
            {
                var children = new List<T>();

                var newNode = (T)value.Clone();

                newNode.Level = level;
                newNode.children = children.AsReadOnly();

                children.AddRange(source.ByHierarchyExtJs(possibleSub => connectBy(value, possibleSub), connectBy, newNode));

                newNode.leaf = (newNode.children.Count == 0);

                yield return newNode;
            }
        }

        public static IEnumerable<Node> ByHierarchyNode<T>(
            this IEnumerable<T> source,
            Func<T, bool> startWith,
            Func<T, T, bool> connectBy)
            where T : Node
        {
            return source.ByHierarchyNode(startWith, connectBy, null);
        }

        private static IEnumerable<Node> ByHierarchyNode<T>(
            this IEnumerable<T> source,
            Func<T, bool> startWith,
            Func<T, T, bool> connectBy,
            T parent)
            where T : Node
        {
            int level = (parent == null ? 0 : parent.Level + 1);

            if (source == null)
                throw new ArgumentNullException("source");

            if (startWith == null)
                throw new ArgumentNullException("startWith");

            if (connectBy == null)
                throw new ArgumentNullException("connectBy");

            foreach (T value in from item in source
                                where startWith(item)
                                select item)
            {
                var children = new List<Node>();

                var newNode = (T)value.Clone();

                newNode.Level = level;
                newNode.Children = children;
                newNode.Parent = parent;

                children.AddRange(source.ByHierarchyNode(possibleSub => connectBy(value, possibleSub), connectBy, newNode));

                newNode.Leaf = (newNode.Children.Count == 0);

                yield return newNode;
            }
        }

        /// <summary>
        /// Compara objetos
        /// </summary>
        /// <seealso cref="http://stackoverflow.com/questions/543482/linq-select-distinct-with-anonymous-types"/>
        /// <typeparam name="T"></typeparam>
        public class DelegateComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _equals;
            private readonly Func<T, int> _hashCode;

            public DelegateComparer(Func<T, T, bool> equals, Func<T, int> hashCode)
            {
                _equals = equals;
                _hashCode = hashCode;
            }

            public bool Equals(T x, T y)
            {
                return _equals(x, y);
            }

            public int GetHashCode(T obj)
            {
                return _hashCode != null ? _hashCode(obj) : obj.GetHashCode();
            }
        }

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> items, 
            Func<T, T, bool> equals, Func<T, int> hashCode)
        {
            return items.Distinct(new DelegateComparer<T>(equals, hashCode));
        }

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> items,
            Func<T, T, bool> equals)
        {
            return items.Distinct(new DelegateComparer<T>(equals, null));
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> first, IEnumerable<T> second, 
            Func<T, T, bool> equals, Func<T, int> hashCode)
        {
            return first.Except(second, new DelegateComparer<T>(equals, hashCode));
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> first, IEnumerable<T> second, 
            Func<T, T, bool> equals)
        {
            return first.Except(second, new DelegateComparer<T>(equals, null));
        }

        public static bool Contains<T>(this IEnumerable<T> source, T value,
            Func<T, T, bool> equals, Func<T, int> hashCode)
        {
            return source.Contains(value, new DelegateComparer<T>(equals, hashCode));
        }

        public static bool Contains<T>(this IEnumerable<T> source, T value,
            Func<T, T, bool> equals)
        {
            return source.Contains(value, new DelegateComparer<T>(equals, null));
        }
    }
}
