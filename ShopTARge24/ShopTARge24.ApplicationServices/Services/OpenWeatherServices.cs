
using ShopTARge24.Core.Dto.OpenWeather;
using ShopTARge24.Core.ServiceInterface;
using System.Text.Json;

namespace ShopTARge24.ApplicationServices.Services
{
    public class OpenWeatherServices : IOpenWeatherServices
    {
        public async Task<OpenWeatherDto> OpenWeatherResult(OpenWeatherDto dto)
        {
            string apiKey = "a5ff179e13420806f083ec6730e09a67";

            // get coordinates
            string geoUrl = $"http://api.openweathermap.org/geo/1.0/direct?q={dto.CityName}&appid={apiKey}";

            using (var client = new HttpClient())
            {
                var geoResponse = await client.GetAsync(geoUrl);
                string geoJson = await geoResponse.Content.ReadAsStringAsync();

                // deserialize directly to List
                var geoData = JsonSerializer.Deserialize<List<GeoLocationDto>>(geoJson);

                if (geoData == null || geoData.Count == 0)
                    return dto;

                dto.Lat = geoData[0].Lat;
                dto.Lon = geoData[0].Lon;
                dto.Country = geoData[0].Country;
            }

            // using lat/lon here
            string weatherUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={dto.Lat}&lon={dto.Lon}&units=metric&appid={apiKey}";

            using (var clientWeather = new HttpClient())
            {
                var httpResponseWeather = await clientWeather.GetAsync(weatherUrl);
                string jsonWeather = await httpResponseWeather.Content.ReadAsStringAsync();

                var weatherRootDto = JsonSerializer.Deserialize<OpenWeatherRootDto>(jsonWeather);

                dto.CityName = weatherRootDto.Name;
                dto.Temperature = weatherRootDto.Main.Temp;
                dto.FeelsLike = weatherRootDto.Main.Feels_Like;
                dto.Humidity = weatherRootDto.Main.Humidity;
                dto.Pressure = weatherRootDto.Main.Pressure;

                dto.WeatherMain = weatherRootDto.Weather[0].Main;
                dto.WeatherDescription = weatherRootDto.Weather[0].Description;

                dto.WindSpeed = weatherRootDto.Wind.Speed;
            }

            return dto;
        }
    }
}