using BusinessLogic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLogic.Controller
{
    [ApiController]
    [Route("Api/v1/")]
    public class BusinessController : ControllerBase
    {
        private readonly BusinessServices _ctx;
        private readonly HttpClient _cli;
        public BusinessController(BusinessServices ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("GetData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetData()
        {
            var data = await _ctx.GetCategories();
            return Ok(data);
        }
        
        [HttpGet("ThrowErrors/{data}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ThrowErrors(int param)
        {
            var data = await _ctx.ThrowErrors(param);
            return Ok(data);
        }

        [HttpGet]
        public int GetNumber()
        {
            return _ctx.GetNumber() ;
        }

    }
}
