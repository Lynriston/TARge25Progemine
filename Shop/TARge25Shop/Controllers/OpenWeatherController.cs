using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.Dto.OpenWeather;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;
using TARge25Shop.Models.OpenWeather;
using static TARge25Shop.Models.OpenWeather.OpenWeatherViewModel;

namespace TARge25Shop.Controllers
{
    public class OpenWeatherController : Controller
    {
        private readonly IOpenWeatherServices _openWeatherServices;

        public OpenWeatherController
            (
                IOpenWeatherServices openWeatherServices
            )
        {
            _openWeatherServices = openWeatherServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentWeather(OpenWeatherRootDto dto)
        {
            var weatherRes = await _openWeatherServices.GetCurrentWeather(dto);
            
            if (weatherRes == null)
            {
                return View("Index");
            }

            OpenWeatherViewModel vm = new()
            {
                coord = new OpenWeatherViewModel.Coord
                {
                    lon = weatherRes.coord?.lon ?? 0,
                    lat = weatherRes.coord?.lat ?? 0
                },
                weather = weatherRes.weather?.Select(w => new Weather
                {
                    id = w.id,
                    main = w.main,
                    description = w.description,
                    icon = w.icon
                }).ToList(),
                @base = weatherRes.@base,
                main = new OpenWeatherViewModel.Main
                {
                    temp = weatherRes.main?.temp ?? 0,
                    feels_like = weatherRes.main?.feels_like ?? 0,
                    temp_min = weatherRes.main?.temp_min ?? 0,
                    temp_max = weatherRes.main?.temp_max ?? 0,
                    pressure = weatherRes.main?.pressure ?? 0,
                    humidity = weatherRes.main?.humidity ?? 0,
                    sea_level = weatherRes.main?.sea_level ?? 0,
                    grnd_level = weatherRes.main?.grnd_level ?? 0
                },
                visibility = weatherRes.visibility ?? 0,
                wind = new OpenWeatherViewModel.Wind
                {
                    speed = weatherRes.wind?.speed ?? 0,
                    deg = weatherRes.wind?.deg ?? 0,
                    gust = weatherRes.wind?.gust ?? 0
                },
                clouds = new OpenWeatherViewModel.Clouds
                {
                    all = weatherRes.clouds?.all ?? 0
                },
                dt = weatherRes.dt ?? 0,
                sys = new OpenWeatherViewModel.Sys
                {
                    type = weatherRes.sys?.type ?? 0,
                    id = weatherRes.sys?.id ?? 0,
                    country = weatherRes.sys?.country,
                    sunrise = weatherRes.sys?.sunrise ?? 0,
                    sunset = weatherRes.sys?.sunset ?? 0
                },
                timezone = weatherRes.timezone ?? 0,
                id = weatherRes.id ?? 0,
                name = weatherRes.name,
                cod = weatherRes.cod ?? 0
            };

            return View("GetCurrentWeather", vm);
        }
    }
}
