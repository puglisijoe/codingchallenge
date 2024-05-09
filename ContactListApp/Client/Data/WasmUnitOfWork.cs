using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ContactListApp.BaseRepository;
using ContactListApp.Model;

namespace ContactListApp.Client.Data {
    public class WasmUnitOfWork : IUnitOfWork<Contact> {
        private readonly WasmRepository _repo;

        public WasmUnitOfWork(IBasicRepository<Contact> repo) {
            _repo = repo as WasmRepository;
        }

        public Contact Contact_Original => _repo.Contact_Original;


        public Contact Contact_DB => _repo.Contact_DB;


        public bool _isConflict => _repo.Contact_DB != null;


        public byte[] Contact_Version { get; set; }


        public IBasicRepository<Contact> Repo => _repo;

        public Task CommitAsync() {
            return Repo.UpdateAsync(Contact_Original, null);
        }

        public void Dispose() { }

        public void SetUser(ClaimsPrincipal user) {
            throw new NotImplementedException();
        }
    }
}