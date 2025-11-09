using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTARge24.Core.Dto.OpenWeather
{
    public class OpenWeatherDto
    {
        public string CityName { get; set; }
        public string Country { get; set; }

        public double Temperature { get; set; }
        public double FeelsLike { get; set; }
        public int Humidity { get; set; }
        public int Pressure { get; set; }

        public double WindSpeed { get; set; }

        public string WeatherMain { get; set; }
        public string WeatherDescription { get; set; }

        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
