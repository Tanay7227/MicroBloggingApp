using System.ComponentModel.DataAnnotations;

namespace Blog_MSP.Models
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select your city.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(300)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Pincode is required.")]
        [RegularExpression(@"^\d{6}$",
            ErrorMessage = "Pincode must contain exactly 6 digits.")]
        public string Pincode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select your gender.")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select at least one hobby.")]
        public List<string> Hobbies { get; set; } = new();
    }

}
