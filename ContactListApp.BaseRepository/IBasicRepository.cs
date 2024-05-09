using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ContactListApp.BaseRepository {
    public interface IBasicRepository<TEntity> {
        Task QueryAsync(Func<IQueryable<TEntity>, Task> query);

        Task<ICollection<TEntity>> GetListAsync();

        Task<TEntity> LoadAsync(int id, ClaimsPrincipal user, bool doUpdate = false);

        Task<bool> DeleteAsync(int id, ClaimsPrincipal user);

        void Attach(TEntity item);

        Task<TEntity> AddAsync(TEntity item, ClaimsPrincipal user);

        Task<TEntity> UpdateAsync(TEntity item, ClaimsPrincipal user);

        Task<TPropertyType> GetPropertyValueAsync<TPropertyType>(
            TEntity item, string propertyName);

        Task SetOriginalValueForConcurrencyAsync<TPropertyType>(
            TEntity item, string propertyName, TPropertyType value);
    }
}