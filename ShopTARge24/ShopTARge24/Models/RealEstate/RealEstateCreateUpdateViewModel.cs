using Microsoft.AspNetCore.Http;

namespace ShopTARge24.Models.RealEstate
{
    public class RealEstateCreateUpdateViewModel
    {
        public Guid? Id { get; set; }
        public decimal? Area { get; set; }
        public string? Location { get; set; }
        public int? RoomNumber { get; set; }
        public string? BuildingType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public IFormFileCollection? Files { get; set; }
        public List<RealEstateImageViewModel>? Image { get; set; }
    }

    public class RealEstateImageViewModel
    {
        public Guid ImageId { get; set; }
        public string? ImageUrl { get; set; }
    }
}