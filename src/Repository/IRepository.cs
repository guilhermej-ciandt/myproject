using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;

namespace AG.Framework.Repository
{
    /// <summary>
    /// Repository Generico
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="http://davybrion.com/blog/2008/06/data-access-with-nhibernate/"/>
    public interface IRepository<T>
    {
        
        /// <summary>
        /// Retrieves the entity with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the entity or null if it doesn't exist</returns>
        T Get(object id);

        /// <summary>
        /// Saves or updates the given entity
        /// </summary>
        /// <param name="entity"></param>
        T SaveOrUpdate(T entity);

        /// <summary>
        /// Returns each entity that matches the given criteria not using NH´s future features
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IEnumerable<T> FindAllNoFuture(DetachedCriteria criteria);

        /// <summary>
        /// Returns each entity that matches the given criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IEnumerable<T> FindAll(DetachedCriteria criteria);

        /// <summary>
        /// Returns all entities of given type.
        /// The result may be different than in database based on
        /// filters or other locked search criteria for search.
        /// </summary>
        /// <returns></returns>
        IList<T> GetAll();

        /// <summary>
        /// Returns each entity that maches the given criteria, and orders the results
        /// according to the given Orders
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        IEnumerable<T> FindAll(DetachedCriteria criteria, params Order[] orders);

        /// <summary>
        /// Returns each entity that matches the given criteria, with support for paging,
        /// and orders the results according to the given Orders
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="firstResult"></param>
        /// <param name="numberOfResults"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        IEnumerable<T> FindAll(DetachedCriteria criteria, int firstResult, int numberOfResults, params Order[] orders);

        /// <summary>
        /// Returns the one entity that matches the given criteria. Throws an exception if
        /// more than one entity matches the criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>        
        T FindOne(DetachedCriteria criteria);

        /// <summary>
        /// Returns the first entity to match the given criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        T FindFirst(DetachedCriteria criteria);

        /// <summary>
        /// Returns the first entity to match the given criteria, ordered by the given order
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        T FindFirst(DetachedCriteria criteria, Order order);

        /// <summary>
        /// Returns the total number of entities that match the given criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        long Count(DetachedCriteria criteria);

        /// <summary>
        /// Returns true if at least one entity exists that matches the given criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        bool Exists(DetachedCriteria criteria);

        /// <summary>
        /// Deletes the given entity
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        /// Deletes every entity that matches the given criteria
        /// </summary>
        /// <param name="criteria"></param>
        void Delete(DetachedCriteria criteria);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IFutureValue<K> GetFutureScalar<K>(DetachedCriteria criteria);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IFutureValue<int> FutureCount(DetachedCriteria criteria);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IFutureValue<T> FindFutureOne(DetachedCriteria criteria);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="entity"></param>
        void Evict<K>(K entity);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Load(object id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Merge(T entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        void SaveOrUpdateCopy(T entity);

        /// <summary>
        /// 
        /// </summary>
        void Flush();
    }
}