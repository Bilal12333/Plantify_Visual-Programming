using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlantifyAdmin.Services
{
    public class PlantDiseaseResult
    {
        [JsonPropertyName("plant_type")]
        public string PlantType { get; set; } = "";

        [JsonPropertyName("detected_plant")]
        public string detected_plant { get; set; } = "";

        [JsonPropertyName("disease")]
        public string disease { get; set; } = "";

        [JsonPropertyName("confidence")]
        public double confidence { get; set; }

        [JsonPropertyName("status")]
        public string status { get; set; } = "";

        [JsonPropertyName("top3")]
        public List<Top3Item> top3 { get; set; } = new();

        [JsonPropertyName("ai_analysis")]
        public string ai_analysis { get; set; } = "";

        [JsonPropertyName("error")]
        public string? error { get; set; }
    }

    public class Top3Item
    {
        [JsonPropertyName("name")]
        public string name { get; set; } = "";

        [JsonPropertyName("confidence")]
        public double confidence { get; set; }
    }

    public class PlantDiseaseService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PlantDiseaseService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<PlantDiseaseResult> PredictAsync(Stream imageStream, string fileName)
        {
            try
            {
                var apiUrl = _configuration["PlantAI:ApiUrl"];

                using var content = new MultipartFormDataContent();
                using var streamContent = new StreamContent(imageStream);
                content.Add(streamContent, "image", fileName);

                var response = await _httpClient.PostAsync($"{apiUrl}/predict", content);
                var json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<PlantDiseaseResult>(json);
                return result ?? new PlantDiseaseResult { error = "Empty response" };
            }
            catch (Exception ex)
            {
                return new PlantDiseaseResult { error = ex.Message };
            }
        }
    }
}