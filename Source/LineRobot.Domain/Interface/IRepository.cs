using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LineRobot.Domain.Interface
{
    /// <summary>
    /// Interface IRepository
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRepository<TEntity, TKey> where TEntity : class, IIdentifier<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task Create(TEntity entity);

        /// <summary>
        /// Creates all.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        Task CreateAll(IEnumerable<TEntity> entities);

        /// <summary>
        /// Finds the by.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Task&lt;TEntity&gt;
        /// </returns>
        Task<TEntity> FindBy(object id);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        TEntity Update(TEntity entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task Delete(object id);

        /// <summary>
        /// Deletes all.
        /// </summary>
        /// <returns></returns>
        Task<long> DeleteAll();

        /// <summary>
        /// Exists the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Boolean Exist(object id);

        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <returns>
        /// Task&lt;IEnumerable&lt;TEntity&gt;&gt;
        /// </returns>
        Task<IEnumerable<TEntity>> FetchAll();

        /// <summary>
        /// Finds the by.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// Task&lt;IEnumerable&lt;TEntity&gt;&gt;
        /// </returns>
        Task<IEnumerable<TEntity>> FetchAll(Expression<Func<TEntity, bool>> filter);
    }
}