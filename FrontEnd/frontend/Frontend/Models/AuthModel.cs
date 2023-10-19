using System.Text.Json;
using System.Text.Json.Serialization;

namespace Frontend.Models
{
    public class AuthResultModel
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
        [JsonPropertyName("current_user")]
        public string currentUsers { get; set; } = string.Empty;

        public CurrentUserModel currentUsersModel { get { return !string.IsNullOrEmpty(this.currentUsers) ? JsonSerializer.Deserialize<CurrentUserModel>(this.currentUsers ?? "") ?? new CurrentUserModel() : new CurrentUserModel(); } }
    }

}
