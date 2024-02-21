namespace PayementMVC.Models
{
    public class BalanceViewModel
    {
        public string Username { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class TransactionViewModel
    {
        public string TrackingId { get; set; }
        public string Username { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }

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
