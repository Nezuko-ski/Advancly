using System.ComponentModel.DataAnnotations;

namespace Advancly.Core.DTOs
{
    public class UserCredentials
    {
        [Required]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "First name must start with an uppercase letter and contain only letters.")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Last name must start with an uppercase letter and contain only letters.")]
        public string LastName { get; set; }

        /*[Required]
        [RegularExpression("^[A-Za-z0-9]*$", ErrorMessage = "Username must contain only letters and numbers.")]
        public string UserName { get; set; }*/

        [Required]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "Phone number must be 11 digits long.")]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "BVN must be 11 digits long.")]
        public string BVN { get; set; }

        [Required]
        [DataType(DataType.Date)] 
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Address { get; set; }
        [Required]
        [EmailAddress]
        [RegularExpression("^[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?", ErrorMessage = "Invalid email format!")]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[@$!%*?&])[A-Za-z0-9@$!%*?&]{6,}", ErrorMessage = "Invalid password format! Password must be alphanumeric and must contain at least one symbol and one uppercase letter!")]
        public string Password { get; set; }
    }
}
