using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PlantifyAdmin.Services
{
    public class GroqService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GroqService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> AskPlantAssistant(string userMessage)
        {
            try
            {
                var intent = await ClassifyIntent(userMessage);

                if (intent == PlantIntent.Other)
                {
                    return "🌿 I mainly focus on plants, gardening, farming, and ecosystems.";
                }

                var systemPrompt = intent switch
                {
                    PlantIntent.Plant => "You are a Plant AI assistant...",
                    PlantIntent.Ecosystem => "You are an Ecology AI assistant...",
                    _ => "You are a helpful plant assistant."
                };

                var apiKey = _configuration["GroqApiKey"];

                var requestBody = new
                {
                    model = "llama-3.1-8b-instant",
                    messages = new[]
                    {
                        new { role = "system", content = systemPrompt },
                        new { role = "user", content = userMessage }
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);

                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    "https://api.groq.com/openai/v1/chat/completions"
                );

                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", apiKey);

                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return "API ERROR: " + responseContent;

                using var doc = JsonDocument.Parse(responseContent);

                return doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString()
                    ?? "No response";
            }
            catch (Exception ex)
            {
                return "ERROR: " + ex.Message;
            }
        }

        private async Task<PlantIntent> ClassifyIntent(string message)
        {
            var apiKey = _configuration["GroqApiKey"];

            var classifierPrompt = new
            {
                model = "llama-3.1-8b-instant",
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content =
                        "Classify into PLANT, ECOSYSTEM, OTHER. Return only one word."
                    },
                    new { role = "user", content = message }
                }
            };

            var json = JsonSerializer.Serialize(classifierPrompt);

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.groq.com/openai/v1/chat/completions"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return PlantIntent.Plant;

            using var doc = JsonDocument.Parse(content);

            var result = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString()
                ?.Trim()
                .ToUpper();

            return result switch
            {
                "PLANT" => PlantIntent.Plant,
                "ECOSYSTEM" => PlantIntent.Ecosystem,
                _ => PlantIntent.Other
            };
        }
    }

    public enum PlantIntent
    {
        Plant,
        Ecosystem,
        Other
    }
}