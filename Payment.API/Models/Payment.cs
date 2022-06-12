namespace Payment.API.Models
{
    public record Payment
    {
        public Guid Id { get; init; }
        public Guid BuyerId { get; init; }
        public decimal Amount { get; init; }
        public DateTime Date { get; init; }

        public Payment(Guid buyerId, decimal amount, DateTime date)
        {
            Id = Guid.NewGuid();
            BuyerId = buyerId;
            Amount = amount;
            Date = date;
        }

        public Payment()
        {

        }
    }
}
