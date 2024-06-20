using Microsoft.AspNetCore.Mvc;
using OVS360SolutionsAPI.Constants;

//using Newtonsoft.Json;
using OVS360SolutionsAPI.EFDBContext;
using OVS360SolutionsAPI.Models;
using OVS360SolutionsAPI.Repository;
using System.Text.Json;

namespace OVS360SolutionsAPI.Controllers
{
    [Route("api/job")]
    [ApiController]
    public class JobController : ControllerBase
    {
        /// <summary>
        /// Logger for Home Controller
        /// </summary>
        private readonly ILogger<JobController> _logger;
        /// <summary>
        /// Entity Framework Core DB Context
        /// </summary>
        private readonly OVS360SolutionsDBContext _dbContext;

        /// <summary>
        /// Constructor to Initialize class
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="dbContext">DB context</param>
        public JobController(ILogger<JobController> logger, OVS360SolutionsDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [NonAction]
        public IEnumerable<Job> GetJobs()
        {
            // Invoking Repository Methods
            return new GenericRepository<Job>(_dbContext).GetEntityData() ?? new List<Job>();
        }

        [NonAction]
        public Job GetJobById(string jobId)
        {
            // Invoking Repository Methods
            return GetJobs().Where(job => job.Id == jobId).FirstOrDefault() ?? null;
        }

        /// <summary>
        /// Default Index View Action Method
        /// </summary>
        /// <returns>view action result</returns>
        [HttpGet]
        public IActionResult Get()
        {
            // Returning response 
            return Ok(GetJobs());
        }

        [HttpPost]
        [Route("save")]
        public IActionResult PostJob([FromBody] string jobJson)
        {
            var deserialzeJob = JsonSerializer.Deserialize<Job>(jobJson);
            deserialzeJob.Id = Guid.NewGuid().ToString();
            deserialzeJob.CreatedOn = DateTime.Now;

            //Invoking Repository Methods
            var isCreated = new GenericRepository<Job>(_dbContext).CreateEntity(deserialzeJob);

            // Returning response 
            return (isCreated) ? Ok(deserialzeJob.Id) : BadRequest(deserialzeJob.Id);
        }

        [HttpPut]
        public IActionResult Put([FromBody] string jobJson)
        {
            var deserialzeJob = JsonSerializer.Deserialize<Job>(jobJson);

            //get existing job
            var job = GetJobById(deserialzeJob.Id);

            job.Description = deserialzeJob.Description;

            // Invoking Repository Methods
            var isUpdated = new GenericRepository<Job>(_dbContext).UpdateEntity(job);

            // Returning response 
            return (isUpdated) ? Ok(isUpdated) : BadRequest(isUpdated);
        }


        [HttpDelete]
        [Route("{jobId}")]
        public IActionResult Delete(string jobId)
        {
            if (string.IsNullOrEmpty(jobId))
            {
                throw new BadHttpRequestException(WebConstants.blankInput);
            }

            //get existing job
            var job = GetJobById(jobId);

            // Invoking Repository Methods
            var isDeleted = new GenericRepository<Job>(_dbContext).DeleteEntity(job);

            // Returning response 
            return (isDeleted) ? Ok(isDeleted) : BadRequest(isDeleted);
        }
    }
}
