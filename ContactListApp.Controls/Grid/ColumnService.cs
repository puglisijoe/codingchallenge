using System.Collections.Generic;
using ContactListApp.Model;

namespace ContactListApp.Controls.Grid {
    public class ColumnService {
        private readonly Dictionary<ContactFilterColumns, string> _columnMappings =
            new Dictionary<ContactFilterColumns, string> {
                { ContactFilterColumns.City, "d-none d-sm-block col-lg-1 col-sm-3" },
                { ContactFilterColumns.Name, "col-8 col-lg-2 col-sm-3" },
                { ContactFilterColumns.Phone, "d-none d-sm-block col-lg-2 col-sm-2" },
                { ContactFilterColumns.State, "d-none d-sm-block col-sm-1" },
                { ContactFilterColumns.Street, "d-none d-lg-block col-lg-3" },
                { ContactFilterColumns.ZipCode, "d-none d-sm-block col-sm-2" }
            };


        public string EditColumn => "col-4 col-sm-1";
        public string DeleteConfirmation => "col-lg-9 col-sm-8";

        public string GetClassForColumn(ContactFilterColumns column) {
            return _columnMappings[column];
        }
    }
}