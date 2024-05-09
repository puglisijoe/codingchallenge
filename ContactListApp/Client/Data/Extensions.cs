using ContactListApp.Model;

namespace ContactListApp.Client.Data {
    public static class Extensions {
        public static void Refresh(this IPageHelper helper, IPageHelper newData) {
            helper.PageSize = newData.PageSize;
            helper.PageItems = newData.PageItems;
            helper.Page = newData.Page;
            helper.TotalItemCount = newData.TotalItemCount;
        }

        public static ContactConcurrencyResolver ToConcurrencyResolver(
            this Contact contact, WasmRepository repo) {
            return new ContactConcurrencyResolver {
                Contact_Original = contact,
                Contact_Version = repo.Contact_Version
            };
        }
    }
}