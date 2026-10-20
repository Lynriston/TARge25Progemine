using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;

namespace TARge25Shop.ApplicationServices.Services
{
    public class CockTailServices : ICockTailServices
    {
        private readonly HttpClient _httpClient;
        public CockTailServices
            (
                HttpClient httpClient
            )
        {
            _httpClient = httpClient;
        }

        public async Task<CockTailRootDto> GetCocktails(CockTailResultDto dto)
        {
            string apiKey = "1";
            var response = await _httpClient.GetAsync($"https://www.thecocktaildb.com/api/json/v1/{apiKey}/search.php?s={dto.StrDrink}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var cocktail = JsonSerializer.Deserialize<CockTailRootDto>(json, options);

            return cocktail!;
        }
    }
}
