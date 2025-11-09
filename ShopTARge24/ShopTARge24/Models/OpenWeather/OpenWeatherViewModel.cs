namespace ShopTARge24.Models.OpenWeather
{
    public class OpenWeatherViewModel
    {
        public string CityName { get; set; }

        public double Temperature { get; set; }
        public double FeelsLike { get; set; }
        public int Humidity { get; set; }
        public int Pressure { get; set; }

        public double WindSpeed { get; set; }

        public string WeatherMain { get; set; }
        public string WeatherDescription { get; set; }
    }
}
