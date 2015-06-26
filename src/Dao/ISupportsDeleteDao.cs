using System;

namespace AG.Framework.Dao
{
    /// <summary>
    /// Role interface for DAOs that support deletion of entities.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    [Obsolete("Use IRepository")] 
    public interface ISupportsDeleteDao<TEntity>
    {
        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        void Delete(TEntity entity);
    }
}