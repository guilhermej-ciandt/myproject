using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using Spring.Data.NHibernate.Generic.Support;

namespace AG.Framework.Repository
{
    /// <summary>
    /// Implementacao generica de repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : HibernateDaoSupport, IRepository<T>
    {
        #region IRepository<T> Members

        public virtual T Get(object id)
        {
            return HibernateTemplate.Get<T>(id);
        }

        public virtual T SaveOrUpdate(T entity)
        {
            HibernateTemplate.SaveOrUpdate(entity);
            return entity;
        }

        public IEnumerable<T> FindAllNoFuture(DetachedCriteria criteria)
        {
            return criteria.GetExecutableCriteria(Session).List<T>();
        }

        public IEnumerable<T> FindAll(DetachedCriteria criteria)
        {
            return criteria.GetExecutableCriteria(Session).Future<T>();
        }

        public IEnumerable<T> FindAll(DetachedCriteria criteria, params Order[] orders)
        {
            if (orders != null)
            {
                foreach (var order in orders)
                {
                    criteria.AddOrder(order);
                }
            }
            return FindAll(criteria);
        }

        public IList<T> GetAll()
        {
            return HibernateTemplate.LoadAll<T>();
        }

        public IEnumerable<T> FindAll(DetachedCriteria criteria, int firstResult, int numberOfResults, params Order[] orders)
        {
            criteria.SetFirstResult(firstResult).SetMaxResults(numberOfResults);
            return FindAll(criteria, orders);
        }

        public T FindOne(DetachedCriteria criteria)
        {
            return criteria.GetExecutableCriteria(Session).UniqueResult<T>();
        }


        public virtual IFutureValue<T> FindFutureOne(DetachedCriteria criteria)
        {
            return criteria.GetExecutableCriteria(Session).FutureValue<T>();
        }

        public virtual IFutureValue<K> GetFutureScalar<K>(DetachedCriteria criteria)
        {
            return criteria.GetExecutableCriteria(Session).FutureValue<K>();
        }

        public T FindFirst(DetachedCriteria criteria)
        {
            var results = criteria.SetFirstResult(0).SetMaxResults(1).GetExecutableCriteria(Session).List<T>();
            if (results.Count > 0)
            {
                return results[0];
            }
            return default(T);
        }

        public T FindFirst(DetachedCriteria criteria, Order order)
        {
            return FindFirst(criteria.AddOrder(order));
        }

        public long Count(DetachedCriteria criteria)
        {
            return Convert.ToInt64(criteria.GetExecutableCriteria(Session)
                .SetProjection(Projections.RowCountInt64()).UniqueResult());
        }

        public IFutureValue<int> FutureCount(DetachedCriteria criteria)
        {
            return criteria.GetExecutableCriteria(Session).FutureValue<int>();
        }

        public bool Exists(DetachedCriteria criteria)
        {
            return Count(criteria) > 0;
        }

        public void Delete(T entity)
        {
            HibernateTemplate.Delete(entity);
        }

        public void Delete(DetachedCriteria criteria)
        {
            // a simple DELETE FROM ... WHERE ... would be much better, but i haven't found
            // a way to do this yet with Criteria. So now it does two roundtrips... one for
            // the query, and one with all the batched delete statements (that is, if you've
            // enabled CUD statement batching
            foreach (T entity in FindAll(criteria))
            {
                Delete(entity);
            }
        }

        public void Evict<K>(K entity)
        {
            HibernateTemplate.Evict(entity);
        }

        public T Load(object id)
        {
            return HibernateTemplate.Load<T>(id);
        }

        public T Merge(T entity)
        {
            return (T)HibernateTemplate.Merge(entity);            
        }

        public void Update(T entity)
        {
            HibernateTemplate.Update(entity);
        }

        public void SaveOrUpdateCopy(T entity)
        {
            HibernateTemplate.SaveOrUpdate(entity);
        }

        public void Flush()
        {
            HibernateTemplate.Flush();
        }

        #endregion
    }
}
