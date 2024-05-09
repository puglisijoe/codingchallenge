using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ContactListApp.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactListApp.DataAccess {
    public class ContactContext : DbContext, ISupportUser {
        public static readonly string Contact_Version = nameof(Contact_Version);

        public static readonly string CreatedBy = nameof(CreatedBy);

        public static readonly string CreatedOn = nameof(CreatedOn);

        public static readonly string ModifiedBy = nameof(ModifiedBy);

        public static readonly string ModifiedOn = nameof(ModifiedOn);

        public static readonly string ContactListDb =
            nameof(ContactListDb).ToLower();

        private readonly ContactAuditAdapter _adapter = new ContactAuditAdapter();
        private readonly Guid _id;

        public ContactContext(DbContextOptions<ContactContext> options)
            : base(options) {
            _id = Guid.NewGuid();
            Debug.WriteLine($"{_id} context created.");
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactAudit> ContactAudits { get; set; }


        public ClaimsPrincipal User { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken token
            = default) {
            return await _adapter.ProcessContactChangesAsync(
                User, this, async () => await base.SaveChangesAsync(token));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            EntityTypeBuilder<Contact> contact = modelBuilder.Entity<Contact>();
            contact.Property<byte[]>(Contact_Version).IsRowVersion();
            contact.Property<string>(ModifiedBy);
            contact.Property<DateTimeOffset>(ModifiedOn);
            contact.Property<string>(CreatedBy);
            contact.Property<DateTimeOffset>(CreatedOn);
            base.OnModelCreating(modelBuilder);
        }

        public override void Dispose() {
            Debug.WriteLine($"{_id} context disposed.");
            base.Dispose();
        }

        public override ValueTask DisposeAsync() {
            Debug.WriteLine($"{_id} context disposed async.");
            return base.DisposeAsync();
        }
    }
}