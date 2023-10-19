using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Advancly.Domain.Entitities
{
    public class Transaction
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public string RequestID { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Source Account Number must be 10 digits")]
        public string SourceAccount { get; set; }


        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Destination Account Number must be 10 digits")]
        public string DestAccount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [RegularExpression(@"^\d{1,15}(\.\d{0,2})?$")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        public string Narration { get; set; }
        [JsonIgnore]
        public DateTime TimeStamp { get; set; }

        [JsonIgnore]
        public ulong RowVersion { get; set; }

    }
}
