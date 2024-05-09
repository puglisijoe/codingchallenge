using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContactListApp.BaseRepository;
using ContactListApp.Controls;
using ContactListApp.DataAccess;
using ContactListApp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ContactListApp.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QueryController : ControllerBase {
        private readonly IBasicRepository<Contact> _repo;
        private readonly IServiceProvider _serviceProvider;

        public QueryController(IBasicRepository<Contact> repo,
            IServiceProvider provider) {
            _repo = repo;
            _serviceProvider = provider;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(
            [FromBody] Contact_Filter filter) {
            ContactGenerator seed = _serviceProvider.GetService<ContactGenerator>();
            await seed.CheckAndSeedDatabaseAsync(User);

            GridQueryAdapter adapter = new GridQueryAdapter(filter);
            ICollection<Contact> contacts = null;
            await _repo.QueryAsync(
                async query => contacts = await adapter.FetchAsync(query));
            return new OkObjectResult(new {
                PageInfo = filter.PageHelper,
                Contacts = contacts
            });
        }
    }
}