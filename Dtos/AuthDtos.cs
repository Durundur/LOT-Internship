using System.ComponentModel.DataAnnotations;

namespace LOT_TASK.Dtos
{
    public class AuthRequestDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, ErrorMessage = "Password must contain {2}-{1} characters.", MinimumLength = 3)]
        public string Password { get; set; }
    }

    public class AuthResponseDto : ActionResultDto
    {
        public string? JwtToken { get; set; }
        public string? RefreshToken { get; set; }

    }
    
    public class RefreshTokenRequest
    {
        [Required]
        public string JwtToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
