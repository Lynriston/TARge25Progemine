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

        public async Task<CockTailRootDto> CockTailResultHttpClient()
        {
            //var response on vaja ühendada https://www.thecocktaildb.com/api/json/v1/1/random.php
            var response = await _httpClient.GetAsync("https://www.thecocktaildb.com/api/json/v1/1/random.php");
            //annab veateate, kui response ei ole edukas
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                //olek ture laseb ignoreerida täpset tähe suurustundlikust JSON muutuja nimes
                PropertyNameCaseInsensitive = true
            };

            //muutuja var joke tuleb deserialiseerida
            var joke = JsonSerializer.Deserialize<CockTailRootDto>(json, options);

            //null andestav operaator(!) on turvaline viis kuna API alati tagastab JSON
            return joke!;
        }
    }
}
