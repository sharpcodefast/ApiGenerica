using System.ComponentModel.DataAnnotations;

namespace Api.Core.Dtos
{
    public class ValidateResetTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}