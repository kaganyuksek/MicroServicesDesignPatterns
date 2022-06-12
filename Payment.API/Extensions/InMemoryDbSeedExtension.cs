using Payment.API.Models;

namespace Payment.API
{
    public static class InMemoryDbSeedExtension
    {
        /// <summary>
        /// Seed for memory database.
        /// </summary>
        public static IApplicationBuilder UseMemoryDbSeed(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Deposits.Add(new Payment.API.Models.Deposit(Guid.NewGuid(), 100));
                context.SaveChanges();
            }

            return app;
        }
    }
}
