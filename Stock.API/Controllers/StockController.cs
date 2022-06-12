using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.API.Models;

namespace Stock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly AppDbContext Context;

        public StockController(AppDbContext context)
        {
            Context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Context.Stocks.ToListAsync());
        }
    }
}
