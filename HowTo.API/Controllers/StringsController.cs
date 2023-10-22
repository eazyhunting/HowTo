using HowTo.Services.Interfaces;
using HowTo.Shared.Strings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HowTo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StringsController : ControllerBase
    {
        private readonly IStringService _stringService;
        public StringsController(IStringService stringService) 
        { 
            _stringService = stringService;
        }

        [HttpPost("stuff")]
        public async Task<IActionResult> Stuff(StuffStringModel model)
        {
            try
            {
                var result = 
                    await _stringService.StuffStringAsync(model.Input, model.Interval, model.Value);
                return Ok(result);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
