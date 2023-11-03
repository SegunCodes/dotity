using System.ComponentModel.DataAnnotations;

namespace dotity.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}