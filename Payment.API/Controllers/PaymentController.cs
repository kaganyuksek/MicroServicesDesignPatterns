using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payment.API.Models;

namespace Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly AppDbContext Context;

        public PaymentController(AppDbContext context)
        {
            Context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPayments()
        {
            return Ok(await Context.Payments.ToListAsync());
        }
    }
}
