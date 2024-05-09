using System;
using ContactListApp.BaseRepository;
using ContactListApp.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ContactListApp.Repository {
    public interface IRepository<TEntity, TContext> :
        IDisposable,
        IBasicRepository<TEntity> where TContext : DbContext, ISupportUser {
        TContext PersistedContext { get; set; }
    }
}