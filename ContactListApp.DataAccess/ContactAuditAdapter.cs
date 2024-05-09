using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using ContactListApp.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ContactListApp.DataAccess {
    public class ContactAuditAdapter {
        private static readonly string Unknown = nameof(Unknown);

        public async Task<int> ProcessContactChangesAsync(
            ClaimsPrincipal currentUser,
            ContactContext context,
            Func<Task<int>> saveChangesAsync) {
            string user = Unknown;

            if (currentUser != null) {
                Claim name = currentUser.Claims.FirstOrDefault(
                    c => c.Type == ClaimTypes.NameIdentifier);

                if (name != null)
                    user = name.Value;
                else if (!string.IsNullOrWhiteSpace(currentUser.Identity.Name)) user = currentUser.Identity.Name;
            }

            List<ContactAudit> audits = new List<ContactAudit>();
            foreach (EntityEntry<Contact> item in context.ChangeTracker.Entries<Contact>())
                if (item.State == EntityState.Modified ||
                    item.State == EntityState.Added ||
                    item.State == EntityState.Deleted) {
                    if (item.State == EntityState.Added) {
                        item.Property<string>(ContactContext.CreatedBy).CurrentValue =
                            user;
                        item.Property<DateTimeOffset>(ContactContext.CreatedOn).CurrentValue =
                            DateTimeOffset.UtcNow;
                    }

                    Contact dbVal = null;
                    if (item.State == EntityState.Modified) {
                        PropertyValues db = await item.GetDatabaseValuesAsync();
                        dbVal = db.ToObject() as Contact;
                        item.Property<string>(ContactContext.ModifiedBy).CurrentValue =
                            user;
                        item.Property<DateTimeOffset>(ContactContext.ModifiedOn).CurrentValue =
                            DateTimeOffset.UtcNow;
                    }

                    PropertyChanges<Contact> changes = new PropertyChanges<Contact>(item, dbVal);
                    ContactAudit audit = new ContactAudit {
                        ContactId = item.Entity.Id,
                        Action = item.State.ToString(),
                        User = user,
                        Changes = JsonSerializer.Serialize(changes),
                        ContactRef = item.Entity
                    };

                    audits.Add(audit);
                }

            if (audits.Count > 0)
                // save
                context.ContactAudits.AddRange(audits);

            int result = await saveChangesAsync();
            bool secondSave = false;

            foreach (ContactAudit audit in audits.Where(a => a.ContactId == 0).ToList()) {
                secondSave = true;
                audit.ContactId = audit.ContactRef.Id;
                context.Entry(audit).State = EntityState.Modified;
            }

            if (secondSave) await saveChangesAsync();

            return result;
        }
    }
}