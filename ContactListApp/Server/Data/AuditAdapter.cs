using System.Collections.Generic;
using ContactListApp.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ContactListApp.Server.Data {
    public class AuditAdapter {
        public void Snap(ApplicationAuditDbContext context) {
            List<UserAudit> audits = new List<UserAudit>();
            ChangeTracker tracker = context.ChangeTracker;
            foreach (EntityEntry<ApplicationUser> item in tracker.Entries<ApplicationUser>())
                if (item.State == EntityState.Added ||
                    item.State == EntityState.Deleted ||
                    item.State == EntityState.Modified) {
                    UserAudit audit = new UserAudit(item.State.ToString(), item.Entity);
                    if (item.State == EntityState.Modified) {
                        bool wasConfirmed =
                            (bool)item.OriginalValues[nameof(ApplicationUser.EmailConfirmed)];
                        if (wasConfirmed == false && item.Entity.EmailConfirmed) audit.Action = "Email Confirmed";
                    }

                    audits.Add(audit);
                }

            if (audits.Count > 0) context.Audits.AddRange(audits);
        }
    }
}