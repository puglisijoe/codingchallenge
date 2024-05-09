using ContactListApp.Model;

namespace ContactListApp.Client.Data {
    public class ContactConcurrencyResolver {
        public byte[] Contact_Version { get; set; }
        public Contact Contact_Original { get; set; }
        public Contact Contact_DB { get; set; }
    }
}