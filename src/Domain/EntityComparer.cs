using System.Collections.Generic;

namespace AG.Framework.Domain
{
    public class EntityComparer<T> :  IEqualityComparer<IEntity<T>>
        where T : IEntity<T>
    {
        #region IEqualityComparer<IEntity<T>> Members

        public bool Equals(IEntity<T> x, IEntity<T> y)
        {
            return x.HasSameIdentityAs((T) y);
        }

        public int GetHashCode(IEntity<T> obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }


}
