using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ContactListApp.BaseRepository {
    public interface IUnitOfWork<TEntity> : IDisposable {
        IBasicRepository<TEntity> Repo { get; }

        void SetUser(ClaimsPrincipal user);

        Task CommitAsync();
    }
}