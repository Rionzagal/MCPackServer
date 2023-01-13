using System.ComponentModel.DataAnnotations;

namespace MCPackServer.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        public bool EmailConfirmed { get; set; }
        [Required]
        [StringLength(250)]
        public string FirstName { get; set; }
        [StringLength(250)]
        public string MiddleName { get; set; }
        [Required]
        [StringLength(250)]
        public string FatherSurname { get; set; }
        [Required]
        [StringLength(250)]
        public string MotherSurname { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
    }
}
