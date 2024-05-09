using System;
using ContactListApp.Controls.Grid;
using ContactListApp.Model;

namespace ContactListApp.Server.Controllers {
    public class Contact_Filter : IContactFilters {
        public Contact_Filter() {
            PageHelper = new PageHelper();
        }

        public PageHelper PageHelper { get; set; }

        public ContactFilterColumns FilterColumn { get; set; }

        public string FilterText { get; set; }


        public bool Loading { get; set; }

        public bool ShowFirstNameFirst { get; set; }


        public bool SortAscending { get; set; }


        public ContactFilterColumns SortColumn { get; set; }

        IPageHelper IContactFilters.PageHelper {
            get => PageHelper;
            set => throw new NotImplementedException();
        }
    }
}