using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OVS360SolutionsAPI.Constants;

//using Newtonsoft.Json;
using OVS360SolutionsAPI.EFDBContext;
using OVS360SolutionsAPI.Models;
using OVS360SolutionsAPI.Repository;
using System.Text.Json;
using Files = OVS360SolutionsAPI.Models.File;
using ovsFiles = System.IO;

namespace OVS360SolutionsAPI.Controllers
{
    [Route("api/candidate")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        /// <summary>
        /// Logger for Home Controller
        /// </summary>
        private readonly ILogger<CandidateController> _logger;
        /// <summary>
        /// Entity Framework Core DB Context
        /// </summary>
        private readonly OVS360SolutionsDBContext _dbContext;

        /// <summary>
        /// Constructor to Initialize class
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="dbContext">DB context</param>
        public CandidateController(ILogger<CandidateController> logger, OVS360SolutionsDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [NonAction]
        public IEnumerable<Candidate> GetCandidates()
        {
            // Invoking Repository Methods
            return new GenericRepository<Candidate>(_dbContext).GetEntityData() ?? new List<Candidate>();
        }

        [NonAction]
        public Candidate GetCandidateById(string candidateId)
        {
            // Invoking Repository Methods
            return GetCandidates().Where(cand => cand.Id == candidateId).FirstOrDefault() ?? null;
        }

        /// <summary>
        /// Default Index View Action Method
        /// </summary>
        /// <returns>view action result</returns>
        [HttpGet]
        public IActionResult Get()
        {
            // Returning response 
            return Ok(GetCandidates());
        }

        [HttpPost]
        [Route("resume/{candidateId}")]
        public IActionResult PostResume([FromForm] Files PhysicalResume, string candidateId)
        {
            //get existing candidate
            var candidate = GetCandidateById(candidateId);
            var resumeDocument = PhysicalResume.FormFile;
            var fileName = $"{candidateId}{resumeDocument.FileName}";

            if (!string.IsNullOrEmpty(candidate.ResumeId))
            {
                //delete already exisitng resume
                string resumePath = Path.Combine(Directory.GetCurrentDirectory(), WebConstants.ResumeDirectory, candidate.ResumeId);

                if (System.IO.File.Exists(resumePath))
                {
                    ovsFiles.File.Delete(resumePath);
                }
            }            
            
            string path = Path.Combine(Directory.GetCurrentDirectory(), WebConstants.ResumeDirectory, fileName);
            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                resumeDocument.CopyTo(stream);
            }

            candidate.ResumeId = fileName;

            // Invoking Repository Methods
            var isUpdated = new GenericRepository<Candidate>(_dbContext).UpdateEntity(candidate);

            return Ok(isUpdated);
        }

        [HttpPost]
        [Route("save")]
        public IActionResult PostCandidate([FromBody] string candidateJson)
        {
            var deserialzeCandidate = JsonSerializer.Deserialize<Candidate>(candidateJson);
            deserialzeCandidate.Id = Guid.NewGuid().ToString();
            deserialzeCandidate.CreatedOn = DateTime.Now;

            //Invoking Repository Methods
            var isCreated = new GenericRepository<Candidate>(_dbContext).CreateEntity(deserialzeCandidate);

            // Returning response 
            return (isCreated) ? Ok(deserialzeCandidate.Id) : BadRequest(deserialzeCandidate.Id);
        }

        [HttpPut]
        public IActionResult Put([FromBody] string candidateJson)
        {
            var deserialzeCandidate = JsonSerializer.Deserialize<Candidate>(candidateJson);

            //get existing candidate
            var candidate = GetCandidateById(deserialzeCandidate.Id);

            candidate.RelevantExperience = deserialzeCandidate.RelevantExperience;
            candidate.Skills = deserialzeCandidate.Skills;

            // Invoking Repository Methods
            var isUpdated = new GenericRepository<Candidate>(_dbContext).UpdateEntity(candidate);

            // Returning response 
            return (isUpdated) ? Ok(isUpdated) : BadRequest(isUpdated);
        }


        [HttpDelete]
        [Route("{candidateId}")]
        public IActionResult Delete(string candidateId)
        {
            if (string.IsNullOrEmpty(candidateId))
            {
                throw new BadHttpRequestException(WebConstants.blankInput);
            }

            //get existing candidate
            var candidate = GetCandidateById(candidateId);

            string resumePath = Path.Combine(Directory.GetCurrentDirectory(), WebConstants.ResumeDirectory, candidate.ResumeId);

            if (System.IO.File.Exists(resumePath))
            {
                ovsFiles.File.Delete(resumePath);
            }

            // Invoking Repository Methods
            var isDeleted = new GenericRepository<Candidate>(_dbContext).DeleteEntity(candidate);

            // Returning response 
            return (isDeleted) ? Ok(isDeleted) : BadRequest(isDeleted);
        }
    }
}
