using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Advancly.Domain.Entitities
{
    public class AdvanclyUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BVN { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AccountNumber { get; set; }
        [Column(TypeName = "decimal (18, 2)")]
        public decimal Balance { get; set; }
        public string Address { get; set; }
    }
}
