namespace Shared.Common
{
    public class MessageQueueConst
    {
        public static class OrderService
        {
            public const string OrderCreatedEvent = "order-saga";
        }

        public static class StockService 
        {
            public const string StockReservedEvent = "stock-reserved-queue";
            public const string StockNotReservedEvent = "stock-not-reserved-queue";
        }

        public static class PaymentService
        {
            public const string PaymentSucceededEvent = "payment-succeeded-queue";
            public const string PaymentFailedEvent = "payment-failed-queue";
            public const string StockPaymentFailedEvent = "stock-payment-failed-queue";
        }
    }
}
