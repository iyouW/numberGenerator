namespace numberPool.Controller
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using numberPool.App.Services.NumberPool;
    using numberPool.App.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class NumberPoolController : ControllerBase
    {
        private readonly INumberPoolService _service;

        public NumberPoolController(INumberPoolService service)
        {
            _service = service;
        }

        [HttpGet("rent")]
        public Task<long> Rent()
        {
            return _service.RentAsync();
        }

        [HttpPost("return")]
        public Task Return(ReturnNumberRequest request)
        {
            return _service.ReturnAsync(request.Number);
        }
    }
}