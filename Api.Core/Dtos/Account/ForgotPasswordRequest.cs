using System.ComponentModel.DataAnnotations;

namespace Api.Core.Dtos
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}