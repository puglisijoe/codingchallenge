using ContactListApp.Model;

namespace ContactListApp.Controls.Grid {
    public class ContactFilters : IContactFilters {
        public ContactFilters(IPageHelper pageHelper) {
            PageHelper = pageHelper;
        }

        public IPageHelper PageHelper { get; set; }


        public bool Loading { get; set; }

        public bool ShowFirstNameFirst { get; set; }

        public ContactFilterColumns SortColumn { get; set; } = ContactFilterColumns.Name;

        public bool SortAscending { get; set; }

        public ContactFilterColumns FilterColumn { get; set; } = ContactFilterColumns.Name;

        public string FilterText { get; set; }
    }
}