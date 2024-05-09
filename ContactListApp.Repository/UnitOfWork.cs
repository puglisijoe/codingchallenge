using System.Security.Claims;
using System.Threading.Tasks;
using ContactListApp.BaseRepository;
using ContactListApp.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ContactListApp.Repository {
    public class UnitOfWork<TContext, TEntity> :
        IUnitOfWork<TEntity>
        where TContext : DbContext, ISupportUser {
        private IRepository<TEntity, TContext> _repo;

        public UnitOfWork(
            IRepository<TEntity, TContext> repo, DbContextFactory<TContext> factory) {
            repo.PersistedContext = factory.CreateDbContext();
            _repo = repo;
        }

        public IBasicRepository<TEntity> Repo => _repo;

        public async Task CommitAsync() {
            try {
                await _repo.PersistedContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex) {
                RepoConcurrencyException<TEntity> newex = new RepoConcurrencyException<TEntity>(
                    (TEntity)ex.Entries[0].Entity, ex);
                PropertyValues dbValues = ex.Entries[0].GetDatabaseValues();

                if (dbValues == null) {
                    newex.DbEntity = default;
                }
                else {
                    newex.Contact_Version = dbValues
                        .GetValue<byte[]>(ContactContext.Contact_Version);
                    newex.DbEntity = (TEntity)dbValues.ToObject();
                    ex.Entries[0].OriginalValues.SetValues(dbValues);
                }

                throw newex;
            }
        }

        public void Dispose() {
            if (_repo != null) {
                _repo.Dispose();
                _repo = null;
            }
        }

        public void SetUser(ClaimsPrincipal user) {
            if (_repo.PersistedContext != null) _repo.PersistedContext.User = user;
        }
    }
}