using System.Collections.Generic;
using System.Text.Json.Serialization;


// Models used for JSON deserialization

namespace RickAndMortyUmbraco.Models.RickAndMorty
{
    public class ApiResponse
    {
        [JsonPropertyName("results")]
        public List<Character>? Results { get; set; }

        [JsonPropertyName("info")]
        public ApiInfo? Info { get; set; }
    }

    public class ApiInfo
    {
        [JsonPropertyName("next")]
        public string? Next { get; set; }

        [JsonPropertyName("prev")]
        public string? Prev { get; set; }
    }

    public class Character
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("species")]
        public string? Species { get; set; }

        [JsonPropertyName("gender")]
        public string? Gender { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("episode")]
        public List<string>? Episode { get; set; }
    }

}