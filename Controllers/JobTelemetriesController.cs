using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROJ_API_N_NDORO.Models;

namespace PROJ_API_N_NDORO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTelemetriesController : ControllerBase
    {
        private readonly NwutechTrendsContext _context;

        public JobTelemetriesController(NwutechTrendsContext context)
        {
            _context = context;
        }

        // GET: api/JobTelemetries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobTelemetry>>> GetJobTelemetries()
        {
            return await _context.JobTelemetries.ToListAsync();
        }

        // GET: api/JobTelemetries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobTelemetry>> GetJobTelemetry(int id)
        {
            var jobTelemetry = await _context.JobTelemetries.FindAsync(id);

            if (jobTelemetry == null)
            {
                return NotFound();
            }

            return jobTelemetry;
        }

        // PUT: api/JobTelemetries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobTelemetry(int id, JobTelemetry jobTelemetry)
        {
            if (id != jobTelemetry.Id)
            {
                return BadRequest();
            }

            _context.Entry(jobTelemetry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobTelemetryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/JobTelemetries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<JobTelemetry>> PostJobTelemetry(JobTelemetry jobTelemetry)
        {
            _context.JobTelemetries.Add(jobTelemetry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobTelemetry", new { id = jobTelemetry.Id }, jobTelemetry);
        }

        // DELETE: api/JobTelemetries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobTelemetry(int id)
        {
            var jobTelemetry = await _context.JobTelemetries.FindAsync(id);
            if (jobTelemetry == null)
            {
                return NotFound();
            }

            _context.JobTelemetries.Remove(jobTelemetry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // Custom method to check if a JobTelemetry exists by ID
        private async Task<bool> JobTelemetryExistsAsync(int id)
        {
            return await _context.JobTelemetries.AnyAsync(e => e.Id == id);
        }

        // Custom method to get savings by project
        [HttpGet("GetSavingsByProject")]
        public async Task<ActionResult> GetSavingsByProject(string projectId, DateTime startDate, DateTime endDate)
        {
            var savings = await _context.JobTelemetries
                .Where(t => t.ProccesId == projectId && t.EntryDate >= startDate && t.EntryDate <= endDate)
                .GroupBy(t => t.ProccesId)
                .Select(g => new
                {
                    ProjectId = g.Key,
                    TimeSaved = g.Sum(t => t.HumanTime),
                    // Assume CostSaved is calculated from HumanTime or another property
                    CostSaved = g.Sum(t => t.HumanTime * 10) // Example calculation
                }).ToListAsync();

            return Ok(savings);
        }

        // Custom method to get savings by client
        [HttpGet("GetSavingsByClient")]
        public async Task<ActionResult> GetSavingsByClient(string clientId, DateTime startDate, DateTime endDate)
        {
            var savings = await _context.JobTelemetries
                .Where(t => t.QueueId == clientId && t.EntryDate >= startDate && t.EntryDate <= endDate)
                .GroupBy(t => t.QueueId)
                .Select(g => new
                {
                    ClientId = g.Key,
                    TimeSaved = g.Sum(t => t.HumanTime),
                    // Assume CostSaved is calculated from HumanTime or another property
                    CostSaved = g.Sum(t => t.HumanTime * 10) // Example calculation
                }).ToListAsync();

            return Ok(savings);


        }


        private bool JobTelemetryExists(int id)
        {
            return _context.JobTelemetries.Any(e => e.Id == id);
        }
    }
}
