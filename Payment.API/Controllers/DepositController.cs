using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payment.API.Models;

namespace Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        private readonly AppDbContext Context;

        public DepositController(AppDbContext context)
        {
            Context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddDeposit(Deposit data)
        {
            await Context.Deposits.AddAsync(data);
            await Context.SaveChangesAsync();

            return Accepted();
        }

        [HttpGet]
        public async Task<IActionResult> GetDeposits()
        {
            return Ok(await Context.Deposits.ToListAsync());
        }
    }
}
