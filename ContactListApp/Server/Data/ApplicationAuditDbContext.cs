using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ContactListApp.Server.Data {
    public class ApplicationAuditDbContext : ApplicationDbContext {
        private readonly AuditAdapter _adapter = new AuditAdapter();

        public ApplicationAuditDbContext(DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions) { }

        public DbSet<UserAudit> Audits { get; set; }

        public override Task<int> SaveChangesAsync(
            CancellationToken token = default) {
            _adapter.Snap(this);
            return base.SaveChangesAsync(token);
        }
    }
}