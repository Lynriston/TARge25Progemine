using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.Dto.OpenWeather;
using TARge25Shop.Core.ServiceInterface;

namespace TARge25Shop.ApplicationServices.Services
{
    public class OpenWeatherServices : IOpenWeatherServices
    {
        private readonly HttpClient _httpClient;
        public OpenWeatherServices
            (
                HttpClient httpClient
            )
        {
            _httpClient = httpClient;
        }

        public async Task<OpenWeatherRootDto> GetCurrentWeather(OpenWeatherRootDto dto)
        {
            string _apiKey = "59e189d7efc6bfa174e4d5e36e135c41";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={dto.cityName}&appid={_apiKey}&units=metric&lang=et";
            
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var weatherData = JsonSerializer.Deserialize<OpenWeatherRootDto>(json, options);

            return weatherData;
        }
    }
}
