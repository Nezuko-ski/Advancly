namespace Advancly.Core.DTOs
{
    public class TransactionRequest
    {
        public string RequestID { get; set; }

        public string SourceAccount { get; set; }

        public string DestAccount { get; set; }

        public decimal Amount { get; set; }

        public string Narration { get; set; }
        public DateTime Date { get; set; }
    }
}
