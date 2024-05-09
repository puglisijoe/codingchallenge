using System.Collections.Generic;
using ContactListApp.Controls.Grid;
using ContactListApp.Model;

namespace ContactListApp.Client.Data {
    public class QueryResult {
        public PageHelper PageInfo { get; set; }
        public ICollection<Contact> Contacts { get; set; }
    }
}