using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TARge25Shop.Core.Dto.OpenWeather
{
    public class OpenWeatherRootDto
    {
        public string cityName { get; set; } = string.Empty;

        [JsonPropertyName("coord")]
        public Coord? coord { get; set; }
        [JsonPropertyName("weather")]
        public List<Weather>? weather { get; set; }
        [JsonPropertyName("@base")]
        public string? @base { get; set; }
        [JsonPropertyName("main")]
        public Main? main { get; set; }
        [JsonPropertyName("visibility")]
        public int? visibility { get; set; }
        [JsonPropertyName("wind")]
        public Wind? wind { get; set; }
        [JsonPropertyName("rain")]
        public Rain? rain { get; set; }
        [JsonPropertyName("clouds")]
        public Clouds? clouds { get; set; }
        [JsonPropertyName("dt")]
        public long? dt { get; set; }
        [JsonPropertyName("sys")]
        public Sys? sys { get; set; }
        [JsonPropertyName("timezone")]
        public int? timezone { get; set; }
        [JsonPropertyName("id")]
        public int? id { get; set; }
        [JsonPropertyName("name")]
        public string? name { get; set; }
        [JsonPropertyName("cod")]
        public int? cod { get; set; }


        public class Coord
        {
            public double lon { get; set; }
            public double lat { get; set; }
        }

        public class Main
        {
            public double temp { get; set; }
            public double feels_like { get; set; }
            public double temp_min { get; set; }
            public double temp_max { get; set; }
            public int pressure { get; set; }
            public int humidity { get; set; }
            public int sea_level { get; set; }
            public int grnd_level { get; set; }
        }

        public class Wind
        {
            public double speed { get; set; }
            public int deg { get; set; }
            public double gust { get; set; }
        }

        public class Rain
        {
            public double _1h { get; set; }
        }

        public class Clouds
        {
            public int all { get; set; }
        }

        public class Sys
        {
            public int type { get; set; }
            public int id { get; set; }
            public string? country { get; set; }
            public long sunrise { get; set; }
            public long sunset { get; set; }
        }

        public class Weather
        {
            public int id { get; set; }
            public string? main { get; set; }
            public string? description { get; set; }
            public string? icon { get; set; }
        }
    }
}
