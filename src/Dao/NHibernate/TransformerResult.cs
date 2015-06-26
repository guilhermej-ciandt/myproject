using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate;
using NHibernate.Properties;
using NHibernate.Transform;

namespace AG.Framework.Dao.NHibernate
{
    [Serializable]
    public class TransformerResult<T> : IResultTransformer
    {
        private const BindingFlags _flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        protected readonly Type _resultClass;
        protected readonly Dictionary<string, string> _association;
        protected readonly Dictionary<string, Func<object[], object>> _associationByFunction;
        protected readonly Dictionary<string, ISetter> _setters;
        private readonly IPropertyAccessor _propertyAccessor;
        private readonly ConstructorInfo _constructor;

        #region TODO

        protected readonly Dictionary<string, Dictionary<object, KeyValuePair<Type, Dictionary<string, string>>>> _discriminatorMapping;
        //new Dictionary<string, Dictionary<object, KeyValuePair<Type, Dictionary<string, string>>>>
        //    {
        //        {"tp_abrangencia", new Dictionary<object, KeyValuePair<Type, Dictionary<string, string>>>
        //                               {
        //                                    {"P", new KeyValuePair<Type, Dictionary<string, string>>(
        //                                        typeof(Type), new Dictionary<string, string>
        //                                            {
        //                                                {"Valor", "vl_valor"}
        //                                            }
        //                                        )
        //                                    }    
        //                               }
        //        }
        //    };

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private object TransformerFactoryClass()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private ConstructorInfo[] GetConstructors()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="association"></param>
        /// <param name="discriminatorMapping"></param>
        public TransformerResult(Dictionary<string, string> association,
            Dictionary<string, Dictionary<object, KeyValuePair<Type, Dictionary<string, string>>>> discriminatorMapping)
            : this(association)
        {
            _discriminatorMapping = discriminatorMapping;
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="association"></param>
        /// <param name="associationByFunction"></param>
        /// <param name="discriminatorMapping"></param>
        public TransformerResult(Dictionary<string, string> association, Dictionary<string, Func<object[], object>> associationByFunction,
            Dictionary<string, Dictionary<object, KeyValuePair<Type, Dictionary<string, string>>>> discriminatorMapping)
            : this(association, associationByFunction)
        {
            _discriminatorMapping = discriminatorMapping;
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="association"></param>
        public TransformerResult(Dictionary<string, string> association)
        {
            _association = association;
            _resultClass = typeof(T);
            _setters = new Dictionary<string, ISetter>(_association.Count);
            _associationByFunction = new Dictionary<string, Func<object[], object>>();

            _constructor = _resultClass.GetConstructor(_flags, null, Type.EmptyTypes, null);

            _propertyAccessor =
                new ChainedPropertyAccessor(new[]
                                                {
                                                    PropertyAccessorFactory.GetPropertyAccessor(null),
                                                    PropertyAccessorFactory.GetPropertyAccessor("field")
                                                });

            _association.ToList()
                .ForEach(
                    pair =>
                        _setters.Add(pair.Value, _propertyAccessor.GetSetter(_resultClass, pair.Key))
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="association"></param>
        /// <param name="associationByFunction"></param>
        public TransformerResult(Dictionary<string, string> association, Dictionary<string, Func<object[], object>> associationByFunction)
            : this(association)
        {
            _associationByFunction = associationByFunction;
        }

        public virtual object TransformTuple(object[] tuple, string[] aliases)
        {
            object result;

            try
            {
                result = _constructor.Invoke(null);

                Bind(tuple, aliases, result);
            }
            catch (InstantiationException e)
            {
                throw new HibernateException("Could not instantiate result class: " + _resultClass.FullName, e);
            }
            catch (MethodAccessException e)
            {
                throw new HibernateException("Could not instantiate result class: " + _resultClass.FullName, e);
            }

            return result;
        }

        protected void Bind(object[] tuple, string[] aliases, object result)
        {
            for (int i = 0; i < tuple.Length; i++)
            {
                if (!_association.ContainsValue(aliases[i])) continue;

                _setters[aliases[i]].Set(result,
                                         _associationByFunction.ContainsKey(aliases[i])
                                             ? _associationByFunction[aliases[i]].Invoke(tuple)
                                             : tuple[i]);
            }
        }

        public IList TransformList(IList collection)
        {
            return collection;
        }
    }
}
