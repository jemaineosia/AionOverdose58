using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace AionOverdose58.Web.Services;

public class RecaptchaSettings
{
    public string SiteKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    /// <summary>Minimum score 0.0–1.0 to pass. Google recommends 0.5.</summary>
    public float MinScore { get; set; } = 0.5f;
    /// <summary>Skip verification in Development so localhost works without being registered in Google console.</summary>
    public bool SkipInDevelopment { get; set; } = true;
}

public interface IRecaptchaService
{
    /// <summary>Verify a reCAPTCHA v3 token. Returns (Success, Score, ErrorMessage).</summary>
    Task<(bool Success, float Score, string? ErrorMessage)> VerifyAsync(string token, string action);
}

public class RecaptchaService : IRecaptchaService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly RecaptchaSettings _settings;
    private readonly ILogger<RecaptchaService> _logger;
    private readonly bool _isDevelopment;

    public RecaptchaService(
        IHttpClientFactory httpClientFactory,
        IOptions<RecaptchaSettings> settings,
        ILogger<RecaptchaService> logger,
        IWebHostEnvironment env)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settings.Value;
        _logger = logger;
        _isDevelopment = env.IsDevelopment();
    }

    public async Task<(bool Success, float Score, string? ErrorMessage)> VerifyAsync(string token, string action)
    {
        // Skip in Development so localhost testing doesn't require domain registration
        if (_isDevelopment && _settings.SkipInDevelopment)
        {
            _logger.LogInformation("[reCAPTCHA] Development mode — verification skipped (action={Action})", action);
            return (true, 1.0f, null);
        }

        // If not configured, skip gracefully
        if (string.IsNullOrWhiteSpace(_settings.SecretKey))
        {
            _logger.LogWarning("[reCAPTCHA] SecretKey not configured — verification skipped");
            return (true, 1.0f, null);
        }

        if (string.IsNullOrWhiteSpace(token))
            return (false, 0f, "Security check token missing. Please try again.");

        try
        {
            var client = _httpClientFactory.CreateClient("recaptcha");
            var response = await client.PostAsync(
                "https://www.google.com/recaptcha/api/siteverify",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["secret"]   = _settings.SecretKey,
                    ["response"] = token
                }));

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<RecaptchaApiResponse>();

            if (result is null)
                return (false, 0f, "Could not read reCAPTCHA response.");

            if (!result.Success)
            {
                var errors = result.ErrorCodes is { Count: > 0 }
                    ? string.Join(", ", result.ErrorCodes)
                    : "verification-failed";
                _logger.LogWarning("[reCAPTCHA] v3 failed: {Errors} (action={Action}, hostname={Host})",
                    errors, action, result.Hostname ?? "unknown");
                return (false, 0f, "Security check failed. Please refresh and try again.");
            }

            var passed = result.Score >= _settings.MinScore;
            _logger.LogInformation("reCAPTCHA v3 score={Score} action={Action} passed={Passed}",
                result.Score, action, passed);

            return passed
                ? (true, result.Score, null)
                : (false, result.Score, "Automated activity detected. Please try again.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "reCAPTCHA verification threw an exception");
            return (false, 0f, "Security check unavailable. Please try again.");
        }
    }

    private sealed class RecaptchaApiResponse
    {
        [JsonPropertyName("success")]    public bool Success { get; set; }
        [JsonPropertyName("score")]      public float Score { get; set; }
        [JsonPropertyName("action")]     public string? Action { get; set; }
        [JsonPropertyName("hostname")]   public string? Hostname { get; set; }
        [JsonPropertyName("error-codes")] public List<string>? ErrorCodes { get; set; }
    }
}
