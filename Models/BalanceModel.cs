namespace PayementMVC.Models
{
    public class BalanceModel
    {
        public string Username { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class TransactionModel
    {
        public string TrackingId { get; set; }
        public string Username { get; set; }
        public decimal Amount { get; set; }

    }
    public class TransactionGETModel
    {
        public int Id { get; set; }
        public string TrackingId { get; set; }
        public string Username { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }

    }
}
