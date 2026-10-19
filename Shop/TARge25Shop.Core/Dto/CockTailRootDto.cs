using System.Text.Json;
using System.Text.Json.Serialization;

namespace TARge25Shop.Core.Dto
{
    public class CockTailRootDto
    {
        [JsonPropertyName("idDrink")]
        public string idDrink { get; set; }
        [JsonPropertyName("strDrink")]
        public string strDrink { get; set; }
        [JsonPropertyName("strDrinkAlternate")]
        public string strDrinkAlternate { get; set; }
        [JsonPropertyName("strTags")]
        public string strTags { get; set; }
        [JsonPropertyName("strCategory")]
        public string strCategory { get; set; }
        [JsonPropertyName("strIBA")]
        public string strIBA { get; set; }
        [JsonPropertyName("strAlcoholic")]
        public string strAlcoholic { get; set; }
        [JsonPropertyName("strGlass")]
        public string strGlass { get; set; }
        [JsonPropertyName("strInstructions")]
        public string strInstructions { get; set; }
    }
}
