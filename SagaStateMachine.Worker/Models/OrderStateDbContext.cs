using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace SagaStateMachine.Worker.Models
{
    public class OrderStateDbContext : SagaDbContext
    {
        public OrderStateDbContext(DbContextOptions<OrderStateDbContext> options) : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations 
        { 
            get 
            {
                yield return new OrderStateMap();
            } 
        }
    }
}
