using Microsoft.AspNetCore.Mvc;
using ShopTARge24.Models.RealEstate;

namespace ShopTARge24.Controllers
{
    public class RealEstateController : Controller
    {
        public IActionResult Index()
        {
            // Näidisandmed (mock data)
            var estates = new List<RealEstateIndexViewModel>
            {
                new RealEstateIndexViewModel
                {
                    Id = Guid.NewGuid(),
                    Area = 72.5m,
                    Location = "Tallinn",
                    RoomNumber = 3,
                    BuildingType = "Apartment",
                    CreatedAt = DateTime.Now.AddMonths(-2),
                    ModifiedAt = DateTime.Now
                },
                new RealEstateIndexViewModel
                {
                    Id = Guid.NewGuid(),
                    Area = 120.0m,
                    Location = "Tartu",
                    RoomNumber = 5,
                    BuildingType = "House",
                    CreatedAt = DateTime.Now.AddYears(-1),
                    ModifiedAt = DateTime.Now.AddDays(-10)
                }
            };

            return View(estates);
        }
    }
}