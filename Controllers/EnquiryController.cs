using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OVS360SolutionsAPI.Constants;

//using Newtonsoft.Json;
using OVS360SolutionsAPI.EFDBContext;
using OVS360SolutionsAPI.Models;
using OVS360SolutionsAPI.Repository;
using System.Text.Json;

namespace OVS360SolutionsAPI.Controllers
{
    [Route("api/enquiry")]
    [ApiController]
    public class EnquiryController : ControllerBase
    {
        /// <summary>
        /// Logger for Home Controller
        /// </summary>
        private readonly ILogger<EnquiryController> _logger;
        /// <summary>
        /// Entity Framework Core DB Context
        /// </summary>
        private readonly OVS360SolutionsDBContext _dbContext;

        /// <summary>
        /// Constructor to Initialize class
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="dbContext">DB context</param>
        public EnquiryController(ILogger<EnquiryController> logger, OVS360SolutionsDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Default Index View Action Method
        /// </summary>
        /// <returns>view action result</returns>
        [HttpGet]
        public IActionResult Get()
        {
            // Invoking Repository Methods
            var enquires = new GenericRepository<Enquiry>(_dbContext).GetEntityData() ?? new List<Enquiry>();

            // Returning response 
            return Ok(enquires);
        }

        [HttpPost]
        public IActionResult Post([FromBody] string enquiryJson)
        {
            dynamic deserialzeEnquiry;
            if (!string.IsNullOrWhiteSpace(enquiryJson))
                deserialzeEnquiry = JsonSerializer.Deserialize<Enquiry>(enquiryJson) ?? null;
            else
                throw new BadHttpRequestException(WebConstants.blankInput);


            if (deserialzeEnquiry != null)
            {
                deserialzeEnquiry.Id = Guid.NewGuid().ToString();
                deserialzeEnquiry.CreatedOn = DateTime.Now;

                // Invoking Repository Methods
                var isCreated = new GenericRepository<Enquiry>(_dbContext).CreateEntity(deserialzeEnquiry);

                // Returning response 
                return (isCreated) ? Ok(deserialzeEnquiry.Id) : BadRequest($"{WebConstants.failureMessage} {Entity.enquiry}.");
            }

            return BadRequest(WebConstants.blankInput);
        }
    }
}
