using AG.Framework.Repository;

namespace AG.Framework.Dao.NHibernate
{
    /// <summary>
    /// Base class for data access operations.
    /// </summary>
    public abstract class HibernateDao<TEntity, TId> : Repository<TEntity>
        where TEntity : class 
    {
    }
}
