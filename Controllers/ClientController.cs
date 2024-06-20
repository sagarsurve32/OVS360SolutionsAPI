using Microsoft.AspNetCore.Mvc;
using OVS360SolutionsAPI.Constants;

//using Newtonsoft.Json;
using OVS360SolutionsAPI.EFDBContext;
using OVS360SolutionsAPI.Models;
using OVS360SolutionsAPI.Repository;
using System.Text.Json;

namespace OVS360SolutionsAPI.Controllers
{
    [Route("api/client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        /// <summary>
        /// Logger for Home Controller
        /// </summary>
        private readonly ILogger<ClientController> _logger;
        /// <summary>
        /// Entity Framework Core DB Context
        /// </summary>
        private readonly OVS360SolutionsDBContext _dbContext;

        /// <summary>
        /// Constructor to Initialize class
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="dbContext">DB context</param>
        public ClientController(ILogger<ClientController> logger, OVS360SolutionsDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [NonAction]
        public IEnumerable<Client> GetClients()
        {
            // Invoking Repository Methods
            return new GenericRepository<Client>(_dbContext).GetEntityData() ?? new List<Client>();
        }

        [NonAction]
        public Client GetClientById(string clientId)
        {
            // Invoking Repository Methods
            return GetClients().Where(client => client.Id == clientId).FirstOrDefault() ?? null;
        }

        /// <summary>
        /// Default Index View Action Method
        /// </summary>
        /// <returns>view action result</returns>
        [HttpGet]
        public IActionResult Get()
        {
            // Returning response 
            return Ok(GetClients());
        }

        [HttpPost]
        [Route("save")]
        public IActionResult PostClient([FromBody] string clientJson)
        {
            var deserialzeClient = JsonSerializer.Deserialize<Client>(clientJson);
            deserialzeClient.Id = Guid.NewGuid().ToString();
            deserialzeClient.CreatedOn = DateTime.Now;

            //Invoking Repository Methods
            var isCreated = new GenericRepository<Client>(_dbContext).CreateEntity(deserialzeClient);

            // Returning response 
            return (isCreated) ? Ok(deserialzeClient.Id) : BadRequest(deserialzeClient.Id);
        }

        [HttpPut]
        public IActionResult Put([FromBody] string clientJson)
        {
            var deserialzeClient = JsonSerializer.Deserialize<Client>(clientJson);

            //get existing client
            var client = GetClientById(deserialzeClient.Id);

            client.Address = deserialzeClient.Address;
            client.Email = deserialzeClient.Email;
            client.PhoneNumber = deserialzeClient?.PhoneNumber;

            // Invoking Repository Methods
            var isUpdated = new GenericRepository<Client>(_dbContext).UpdateEntity(client);

            // Returning response 
            return (isUpdated) ? Ok(isUpdated) : BadRequest(isUpdated);
        }


        [HttpDelete]
        [Route("{clientId}")]
        public IActionResult Delete(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new BadHttpRequestException(WebConstants.blankInput);
            }

            //get existing client
            var client = GetClientById(clientId);

            // Invoking Repository Methods
            var isDeleted = new GenericRepository<Client>(_dbContext).DeleteEntity(client);

            // Returning response 
            return (isDeleted) ? Ok(isDeleted) : BadRequest(isDeleted);
        }
    }
}
