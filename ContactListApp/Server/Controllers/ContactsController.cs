using System;
using System.Threading.Tasks;
using ContactListApp.BaseRepository;
using ContactListApp.DataAccess;
using ContactListApp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ContactConcurrencyResolver = ContactListApp.Client.Data.ContactConcurrencyResolver;

namespace ContactListApp.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactsController : ControllerBase {
        private readonly IBasicRepository<Contact> _repo;
        private readonly IServiceProvider _serviceProvider;

        public ContactsController(IBasicRepository<Contact> repo,
            IServiceProvider provider) {
            _repo = repo;
            _serviceProvider = provider;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id,
            [FromQuery] bool doUpdate = false) {
            if (id < 1) return new NotFoundResult();
            if (doUpdate) {
                IUnitOfWork<Contact> unitOfWork = _serviceProvider.GetService<IUnitOfWork<Contact>>();
                HttpContext.Response.RegisterForDispose(unitOfWork);
                Contact result = await unitOfWork.Repo.LoadAsync(id, User, true);

                ContactConcurrencyResolver concurrencyResult = new ContactConcurrencyResolver {
                    Contact_Original = result,
                    Contact_Version = result == null
                        ? null
                        : await unitOfWork.Repo.GetPropertyValueAsync<byte[]>(
                            result, ContactContext.Contact_Version)
                };
                return new OkObjectResult(concurrencyResult);
            }
            else {
                Contact result = await _repo.LoadAsync(id, User);
                return result == null ? (IActionResult)new NotFoundResult() : new OkObjectResult(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(
            [FromBody] Contact contact) {
            return contact == null
                ? new BadRequestResult()
                : ModelState.IsValid
                    ? new OkObjectResult(await _repo.AddAsync(contact, User))
                    : (IActionResult)new BadRequestObjectResult(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id,
            [FromBody] ContactConcurrencyResolver value) {
            if (value == null || value.Contact_Original == null
                              || value.Contact_Original.Id != id)
                return new BadRequestResult();
            if (!ModelState.IsValid) return new BadRequestObjectResult(ModelState);
            IUnitOfWork<Contact> unitOfWork = _serviceProvider.GetService<IUnitOfWork<Contact>>();
            HttpContext.Response.RegisterForDispose(unitOfWork);
            unitOfWork.SetUser(User);
            unitOfWork.Repo.Attach(value.Contact_Original);
            await unitOfWork.Repo.SetOriginalValueForConcurrencyAsync(
                value.Contact_Original, ContactContext.Contact_Version, value.Contact_Version);
            try {
                await unitOfWork.CommitAsync();
                return new OkResult();
            }
            catch (RepoConcurrencyException<Contact> dbex) {
                value.Contact_DB = dbex.DbEntity;
                value.Contact_Version = dbex.Contact_Version;
                return new ConflictObjectResult(value);
            }

            return new BadRequestObjectResult(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id) {
            try {
                bool result = await _repo.DeleteAsync(id, User);
                if (result)
                    return new OkResult();
                return new NotFoundResult();
            }
            catch (Exception ex) {
                return new BadRequestObjectResult(ex);
            }
        }
    }
}