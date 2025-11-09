﻿using Microsoft.AspNetCore.Mvc;
using ShopTARge24.ApplicationServices.Services;
using ShopTARge24.Core.Dto.OpenWeather;
using ShopTARge24.Core.ServiceInterface;
using ShopTARge24.Models.OpenWeather;

namespace ShopTARge24.Controllers
{
    public class OpenWeatherController : Controller
    {
        private readonly IOpenWeatherServices _openWeatherServices;

        public OpenWeatherController(IOpenWeatherServices openWeatherServices)
        {
            _openWeatherServices = openWeatherServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchCity(OpenWeatherSearchModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("City", "OpenWeather", new { city = model.CityName });
            }

            return View("Index", model);
        }

        [HttpGet]
        public IActionResult City(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return RedirectToAction("Index");

            OpenWeatherDto dto = new OpenWeatherDto
            {
                CityName = city
            };

            dto = _openWeatherServices.OpenWeatherResult(dto).GetAwaiter().GetResult();

            OpenWeatherViewModel vm = new OpenWeatherViewModel
            {
                CityName = dto.CityName,
                Temperature = dto.Temperature,
                FeelsLike = dto.FeelsLike,
                Humidity = dto.Humidity,
                Pressure = dto.Pressure,
                WindSpeed = dto.WindSpeed,
                WeatherMain = dto.WeatherMain,
                WeatherDescription = dto.WeatherDescription
            };

            return View("Result", vm);
        }


    }
}