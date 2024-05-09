using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using ContactListApp.BaseRepository;
using ContactListApp.Model;

namespace ContactListApp.Client.Data {
    public class WasmRepository : IBasicRepository<Contact> {
        private const string _apiPrefix = "/api/";
        private readonly HttpClient _apiClient;
        private readonly IContactFilters _controls;

        public WasmRepository(IHttpClientFactory clientFactory, IContactFilters controls) {
            _apiClient = clientFactory.CreateClient(Program.BaseClient);
            _controls = controls;
        }

        private string _apiContacts => $"{_apiPrefix}contacts/";
        private string _apiQuery => $"{_apiPrefix}query/";
        private string _Updated => "?doUpdate=true";

        public Contact Contact_Original { get; set; }

        public Contact Contact_DB { get; set; }

        public byte[] Contact_Version { get; set; }

        public async Task<Contact> AddAsync(Contact item, ClaimsPrincipal user) {
            HttpResponseMessage result = await _apiClient.PostAsJsonAsync(_apiContacts, item);
            return await result.Content.ReadFromJsonAsync<Contact>();
        }

        public async Task<bool> DeleteAsync(int id, ClaimsPrincipal user) {
            try {
                await _apiClient.DeleteAsync($"{_apiContacts}{id}");
                return true;
            }
            catch {
                return false;
            }
        }

        public Task<Contact> LoadAsync(
            int id,
            ClaimsPrincipal _,
            bool doUpdate = false) {
            if (doUpdate) return LoadAsync(id);
            return SafeGetFromJsonAsync<Contact>($"{_apiContacts}{id}");
        }


        public async Task<ICollection<Contact>> GetListAsync() {
            HttpResponseMessage result = await _apiClient.PostAsJsonAsync(
                _apiQuery, _controls);
            QueryResult queryInfo = await result.Content.ReadFromJsonAsync<QueryResult>();
            _controls.PageHelper.Refresh(queryInfo.PageInfo);
            return queryInfo.Contacts;
        }

        public async Task<Contact>
            UpdateAsync(Contact item, ClaimsPrincipal user) {
            HttpResponseMessage result = await _apiClient.PutAsJsonAsync(
                $"{_apiContacts}{item.Id}",
                item.ToConcurrencyResolver(this));
            if (result.IsSuccessStatusCode) return null;
            if (result.StatusCode == HttpStatusCode.Conflict) {
                ContactConcurrencyResolver resolver = await
                    result.Content.ReadFromJsonAsync<ContactConcurrencyResolver>();
                Contact_DB = resolver.Contact_DB;
                RepoConcurrencyException<Contact> ex = new RepoConcurrencyException<Contact>(item, new Exception()) {
                    DbEntity = resolver.Contact_DB
                };
                Contact_Version = resolver.Contact_Version; // for override
                throw ex;
            }

            throw new HttpRequestException($"Bad status code: {result.StatusCode}");
        }

        public void Attach(Contact item) {
            throw new NotImplementedException();
        }

        public Task QueryAsync(Func<IQueryable<Contact>, Task> query) {
            return GetListAsync();
        }

        public Task<TPropertyType> GetPropertyValueAsync<TPropertyType>(Contact item, string propertyName) {
            throw new NotImplementedException();
        }

        public Task SetOriginalValueForConcurrencyAsync<TPropertyType>(Contact item, string propertyName,
            TPropertyType value) {
            throw new NotImplementedException();
        }

        private async Task<TEntity> SafeGetFromJsonAsync<TEntity>(string url) {
            HttpResponseMessage result = await _apiClient.GetAsync(url);
            if (result.StatusCode == HttpStatusCode.NotFound) return default;
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadFromJsonAsync<TEntity>();
        }

        public async Task<Contact> LoadAsync(int id) {
            Contact_Original = null;
            Contact_DB = null;
            Contact_Version = null;

            ContactConcurrencyResolver result = await SafeGetFromJsonAsync<ContactConcurrencyResolver>
                ($"{_apiContacts}{id}{_Updated}");

            if (result == null) return null;

            Contact_Original = result.Contact_Original;
            Contact_Version = result.Contact_Version;

            return result.Contact_Original;
        }
    }
}