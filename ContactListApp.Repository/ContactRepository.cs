using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ContactListApp.DataAccess;
using ContactListApp.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ContactListApp.Repository {
    public class ContactRepository : IRepository<Contact, ContactContext> {
        private readonly DbContextFactory<ContactContext> _factory;
        private bool disposedValue;

        public ContactRepository(DbContextFactory<ContactContext> factory) {
            _factory = factory;
        }

        public ContactContext PersistedContext { get; set; }

        public void Attach(Contact item) {
            if (PersistedContext == null) throw new InvalidOperationException("Only valid in a unit of work.");
            PersistedContext.Attach(item);
        }

        public async Task<Contact> AddAsync(Contact item, ClaimsPrincipal user) {
            await WorkInContextAsync(context => {
                context.Contacts.Add(item);
                return Task.CompletedTask;
            }, user, true);
            return item;
        }

        public async Task<bool> DeleteAsync(int id, ClaimsPrincipal user) {
            bool? result = null;
            await WorkInContextAsync(async context => {
                Contact item = await context.Contacts.SingleOrDefaultAsync(c => c.Id == id);
                if (item == null)
                    result = false;
                else
                    context.Contacts.Remove(item);
            }, user, true);
            if (!result.HasValue) result = true;
            return result.Value;
        }

        public Task<ICollection<Contact>> GetListAsync() {
            throw new NotImplementedException();
        }

        public async Task<Contact> LoadAsync(
            int id,
            ClaimsPrincipal user,
            bool doUpdate = false) {
            Contact contact = null;
            await WorkInContextAsync(async context => {
                DbSet<Contact> contactRef = context.Contacts;
                if (doUpdate) contactRef.AsNoTracking();
                contact = await contactRef
                    .SingleOrDefaultAsync(c => c.Id == id);
            }, user);
            return contact;
        }

        public async Task QueryAsync(Func<IQueryable<Contact>, Task> query) {
            await WorkInContextAsync(async context => { await query(context.Contacts.AsNoTracking().AsQueryable()); },
                null);
        }

        public async Task<Contact> UpdateAsync(Contact item, ClaimsPrincipal user) {
            await WorkInContextAsync(context => {
                context.Contacts.Attach(item);
                return Task.CompletedTask;
            }, user, true);
            return item;
        }

        public async Task<TPropertyType> GetPropertyValueAsync<TPropertyType>(
            Contact item, string propertyName) {
            TPropertyType value = default;
            await WorkInContextAsync(context => {
                value = context.Entry(item)
                    .Property<TPropertyType>(propertyName).CurrentValue;
                return Task.CompletedTask;
            }, null);
            return value;
        }

        public async Task SetOriginalValueForConcurrencyAsync<TPropertyType>(
            Contact item,
            string propertyName,
            TPropertyType value) {
            await WorkInContextAsync(context => {
                EntityEntry<Contact> tracked = context.Entry(item);
                // we tell EF Core what version we loaded
                tracked.Property<TPropertyType>(propertyName).OriginalValue =
                    value;
                // we tell EF Core to modify entity
                tracked.State = EntityState.Modified;
                return Task.CompletedTask;
            }, null);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private async Task WorkInContextAsync(
            Func<ContactContext, Task> work,
            ClaimsPrincipal user,
            bool saveChanges = false) {
            if (PersistedContext != null) {
                if (user != null) PersistedContext.User = user;
                await work(PersistedContext);
            }
            else {
                using (ContactContext context = _factory.CreateDbContext()) {
                    context.User = user;
                    await work(context);
                    if (saveChanges) await context.SaveChangesAsync();
                }
            }
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing)
                    if (PersistedContext != null)
                        PersistedContext.Dispose();
                disposedValue = true;
            }
        }
    }
}