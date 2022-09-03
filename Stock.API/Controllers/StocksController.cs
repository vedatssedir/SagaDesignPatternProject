using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.API.Models;

namespace Stock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly MainDbContext _mainDbContext;
        public StocksController(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mainDbContext.StockItems.ToListAsync());
        }


    }
}
