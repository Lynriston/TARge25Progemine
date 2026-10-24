using System;
using System.Collections.Generic;
using System.Text;
using TARge25Shop.Core.Dto.OpenWeather;

namespace TARge25Shop.Core.ServiceInterface
{
    public interface IOpenWeatherServices
    {
        Task<OpenWeatherRootDto> GetCurrentWeather(OpenWeatherRootDto dto);
    }
}
