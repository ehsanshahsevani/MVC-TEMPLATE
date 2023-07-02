//  "success": true,
//  "challenge_ts": "2022-06-28T05:28:44Z",
//  "hostname": "localhost"

using System.Text.Json.Serialization;

namespace ViewModels;

public class RecaptchaViewModel : object
{
    public RecaptchaViewModel() : base()
    {
    }

    [JsonPropertyName(name: "success")]
    public bool Success { get; set; }

    [JsonPropertyName(name: "error-codes")]
    public string[]? Errors { get; set; }

    [JsonPropertyName(name: "localhost")]
    public string? Localhost{ get; set; }
}
