using HowTo.Services.Interfaces;
using HowTo.Shared.Version;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HowTo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly IVersionService _versionService;

        public VersionController(IVersionService versionService) { 
            _versionService = versionService;
        }

        [HttpPost]
        public async Task<IActionResult> Compare(VersionComparisonModel comparisonModel)
        {
            try
            {
                var response = await _versionService.Compare(comparisonModel);
                return Ok(response);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
