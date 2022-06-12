namespace Payment.API.Models
{
    public class Deposit
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }

        public Deposit(Guid userId, decimal balance)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Balance = balance;
        }

        public Deposit()
        {

        }
    }
}
