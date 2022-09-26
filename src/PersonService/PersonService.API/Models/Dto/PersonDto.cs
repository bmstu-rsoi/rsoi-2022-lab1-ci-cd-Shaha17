using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PersonService.API.Models.Dto
{
    public class PersonDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("work")]
        public string? Work { get; set; }

        [JsonPropertyName("age")]
        public int? Age { get; set; }
    }
}