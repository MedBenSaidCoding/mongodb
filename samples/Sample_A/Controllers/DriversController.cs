
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sample_A.Services;
using Sample_A.Models;

namespace Sample_A.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriversController : ControllerBase
    {
        private readonly ILogger<DriversController> _logger;
        private readonly DriverService _driverService;

        public DriversController(ILogger<DriversController> logger, DriverService driverService)
        {
            _logger = logger;
            _driverService = driverService?? throw new ArgumentNullException(nameof(driverService));            
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var drivers = await _driverService.GetAsync();
            return Ok(drivers);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            var driver = await _driverService.GetAsync(id);

            if(driver is null) 
                return NotFound();
           
            return Ok(driver);

        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Driver driver)
        {
            try
            {
                await _driverService.CreateAsync(driver);
                _logger.LogInformation("Driver with ID {DriverId} was created successfully.", driver.Id);
                return CreatedAtAction(nameof(Get), new {id=driver.Id}, driver);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the driver.");
                return BadRequest();
            }
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] Driver driver)
        {
           
            var existingDriver = await _driverService.GetAsync(id);
            if(existingDriver is null)
            {
                   return BadRequest();
            }

            await _driverService.UpdateAsync(driver);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> RemoveAsync(string id)
        {
            var existingDriver = await _driverService.GetAsync(id);
            if(existingDriver is null)
            {
                return BadRequest();
            }

            await _driverService.RemoveAsync(id);
            return NoContent();
        }

    }
}