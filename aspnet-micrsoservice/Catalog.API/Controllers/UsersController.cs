using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Catalog.API.Model;
using Catalog.API.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ILogger = DnsClient.Internal.ILogger;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;


        public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<User>> GetUsers() => Ok(_userRepository.Get(p => true));

        [HttpGet("[action]/{name}")]
        [ProducesResponseType(typeof(IEnumerable<User>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<User>> GetByName(string name)
        {
            return Ok(_userRepository.Get(u => u.Name.ToLowerInvariant().Contains(name.ToLowerInvariant())));
        }

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogError("user could not found");
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<User>> CreateUser([Bind("Name,Surname,Age")][FromBody] User user)
        { 
            return Ok(await _userRepository.AddAsync(user));
        }

        [HttpPut("{id:length(24)}")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<User>> UpdateUser(string id, [Bind("Name,Surname,Age")][FromBody] User user)
        {
            return Ok(await _userRepository.UpdateAsync(id, user));
        }


        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<User>> Delete(string id)
        {
            return Ok(await _userRepository.DeleteAsync(id));
        }
    }
}
