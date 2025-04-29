using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Core.Services;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;

namespace RickAndMortyApi.Controllers
{
    [Route("umbraco/api/rickmorty")]
    public class RickAndMortyController : UmbracoApiController
    {
        private readonly IContentService _contentService;
        private readonly IHttpClientFactory _httpClientFactory;

        public RickAndMortyController(IContentService contentService, IHttpClientFactory httpClientFactory)
        {
            _contentService = contentService;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportCharacters()
        {
            List<Character> allCharacters = new List<Character>();

            int created = 0;
            int updated = 0;

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://rickandmortyapi.com/api/character");

            if (!response.IsSuccessStatusCode)
                return BadRequest("API call failed");

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<ApiResponse>(json);

            if (data?.Results == null || !data.Results.Any())
            {
                return BadRequest("No character data returned from API.");
            }

            allCharacters.AddRange(data.Results);

            // Step 1: Find or create the Character Container
            var container = _contentService.GetRootContent()
                .FirstOrDefault(x => x.ContentType.Alias == "characterContainer");

            if (container == null)
            {
                container = _contentService.Create("Character Container", -1, "characterContainer");
                _contentService.SaveAndPublish(container);
            }

            var existingCharacters = _contentService.GetPagedChildren(container.Id, 0, int.MaxValue, out var totalChildren)
                        .ToList();

            while (data.Info.Next != null)
            {
                response = await client.GetAsync(data.Info.Next);

                if (!response.IsSuccessStatusCode)
                    return BadRequest("API call failed");

                
                json = await response.Content.ReadAsStringAsync();
                
                data = JsonSerializer.Deserialize<ApiResponse>(json);

                if (data?.Results == null || !data.Results.Any())
                {
                    return BadRequest("No character data returned from API.");
                }

                allCharacters.AddRange(data.Results);

            }

            foreach (var character in allCharacters)
            {
                // Look for existing node by rickAndMortyId
                var existingNode = existingCharacters
                    .FirstOrDefault(x => x.GetValue<string>("rickAndMortyId") == character.Id.ToString());

                if (existingNode != null)
                {
                    // Update existing node
                    updated++;
                    existingNode.SetValue("status", character.Status);
                    existingNode.SetValue("species", character.Species);
                    existingNode.SetValue("gender", character.Gender);
                    _contentService.SaveAndPublish(existingNode);
                }

                else
                {
                    // Create new node
                    created++;
                    var node = _contentService.Create(character.Name, container.Id, "character");

                    node.SetValue("status", character.Status);
                    node.SetValue("species", character.Species);
                    node.SetValue("gender", character.Gender);
                    node.SetValue("rickAndMortyId", character.Id.ToString());

                    _contentService.SaveAndPublish(node);
                }
            }

            return Ok($"Import complete! {created} created, {updated} updated.");
        }

    }

    // Models used for JSON deserialization
    public class ApiResponse
    {
        [JsonPropertyName("results")]
        public List<Character> Results { get; set; }

        [JsonPropertyName("info")]
        public ApiInfo Info { get; set; }
    }

    public class ApiInfo
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("pages")]
        public int Pages { get; set; }

        [JsonPropertyName("next")]
        public string Next { get; set; }

        [JsonPropertyName("prev")]
        public string Prev { get; set; }
    }

    public class Character
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("species")]
        public string Species { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }
    }
}
