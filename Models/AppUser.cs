using Microsoft.AspNetCore.Identity;

namespace Blog_MSP.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public string Hobbies { get; set; } = string.Empty;
    }
}