using System.ComponentModel.DataAnnotations;

namespace PassportVerification.Models
{
    public class VerifiedUser
    {
        [Key]
        public int VId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PassportNumber { get; set; }
        public string Email { get;set; }

    }
}
