using System.ComponentModel.DataAnnotations;

namespace Api.Core.Dtos
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; }
    }
}