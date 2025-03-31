using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace BookManager.Dtos
{
    public record class RegisterDto
    {
        [JsonPropertyName("username")]
        [Required]
        [Length(6, 9999)]
        public required string Username { get; set; }

        [JsonPropertyName("password")]
        [Required]
        [Length(8, 9999)]
        public required string Password { get; set; }

        [JsonPropertyName("first_name")]
        [Required]
        [Length(6, 9999)]
        public required string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        [Required]
        [Length(6, 9999)]
        public required string LastName { get; set; }
    };
    public record class LoginDto
    {
        [JsonPropertyName("username")]
        [Required]
        [Length(6, 9999)]
        public required string Username { get; set; }

        [JsonPropertyName("password")]
        [Required]
        [Length(8, 9999)]
        public required string Password { get; set; }
    };
}