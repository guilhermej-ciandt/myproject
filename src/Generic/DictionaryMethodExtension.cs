using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AG.Framework.Generic
{
    public static class DictionaryMethodExtension
    {

        /// <summary>
        /// Adiciona Itens em lote no dictionary em questão
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void AddRange<T, U>(this IDictionary<T, List<U>> D, IEnumerable<KeyValuePair<T, U>> V)
        {
            foreach (var kvp in V)
            {
                if (!D.ContainsKey(kvp.Key))
                {
                    D.Add(kvp.Key, new List<U>());
                }
                List<U> c = D[kvp.Key];
                if (c == null)
                {
                    c = new List<U>();
                    D[kvp.Key] = c;
                }
                c.Add(kvp.Value);
            }
        }

       public static void AddRange<T, U>(this IDictionary<T, U> D, IEnumerable<KeyValuePair<T, U>> V)
        {
            foreach (var kvp in V)
            {
                if (D.ContainsKey(kvp.Key))
                {
                    throw new ArgumentException("An item with the same key has already been added.");
                }
                D.Add(kvp);
            }
        }
    }
}
