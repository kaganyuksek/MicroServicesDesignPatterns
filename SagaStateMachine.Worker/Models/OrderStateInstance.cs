using MassTransit;
using Shared.Events.Orders;

namespace SagaStateMachine.Worker.Models
{
    public class OrderStateInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public Guid BuyerId { get; set; }
        public Guid OrderId { get; set; }
        public string OrderItems { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
