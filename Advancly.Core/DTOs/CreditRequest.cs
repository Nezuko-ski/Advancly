namespace Advancly.Core.DTOs
{
    public class CreditRequest
    {
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }

        public string RequestId { get; set; }
    }
}
