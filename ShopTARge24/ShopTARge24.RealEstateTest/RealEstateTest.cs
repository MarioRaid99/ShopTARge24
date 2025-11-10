using ShopTARge24.Core.Dto;
using ShopTARge24.Core.ServiceInterface;
using System.Threading.Tasks;

namespace ShopTARge24.RealEstateTest
{
    public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealEstate_WhenReturnResult()
        {
            // Arrange
            RealEstateDto dto = new()
            {
                Area = 120.5,
                Location = "Test Location",
                RoomNumber = 3,
                BuildingType = "Apartment",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            // Act
            var result = await Svc<IRealEstateServices>().Create(dto);

            // Assert
            Assert.NotNull(result);
        }

        //ShouldNot_GetByIdRealestate_WhenReturnsNotEqual()
        //Should_GetByIdRealestate_WhenReturnsEqual()
        //Should_DeleteByIdRealEstate_WhenDeleteRealEstate()
        //ShouldNot_DeleteByIdRealEstate_WhenDidNotDeleteRealEstate()
    }
}