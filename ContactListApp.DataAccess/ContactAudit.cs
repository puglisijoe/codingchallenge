using System;
using System.ComponentModel.DataAnnotations.Schema;
using ContactListApp.Model;

namespace ContactListApp.DataAccess {
    public class ContactAudit {
        public int Id { get; set; }

        public DateTimeOffset EventTime { get; set; }
            = DateTimeOffset.UtcNow;


        public int ContactId { get; set; }


        public string User { get; set; }


        public string Action { get; set; }

        public string Changes { get; set; }

        [NotMapped] public Contact ContactRef { get; set; }
    }
}