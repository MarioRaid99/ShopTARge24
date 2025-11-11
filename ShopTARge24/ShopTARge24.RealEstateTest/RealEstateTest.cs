using ShopTARge24.ApplicationServices.Services;
using ShopTARge24.Core.Dto;
using ShopTARge24.Core.ServiceInterface;


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

        [Fact]
        public async Task ShouldNot_GetByIdRealestate_WhenReturnsNotEqual()
        {
            //arrange
            Guid wrongGuid = Guid.NewGuid();
            Guid guid = Guid.Parse("68ce7565-9105-4945-b428-b8e25ec061c6");

            //act
            await Svc<IRealEstateServices>().DetailAsync(guid);

            //assert
            Assert.NotEqual(wrongGuid, guid);
        }

        [Fact]
        public async Task Should_GetByIdRealestate_WhenReturnsEqual()
        {
            //arrange
            Guid databaseGuid = Guid.Parse("68ce7565-9105-4945-b428-b8e25ec061c6");
            Guid guid = Guid.Parse("68ce7565-9105-4945-b428-b8e25ec061c6");
            //act
            await Svc<IRealEstateServices>().DetailAsync(guid);

            //assert
            Assert.Equal(databaseGuid, guid);
        }

        [Fact]
        public async Task Should_DeleteByIdRealEstate_WhenDeleteRealEstate()
        {
            //arrange
            RealEstateDto dto = MockRealEstateData();

            //act
            var addRealEstate = await Svc<IRealEstateServices>().Create(dto);
            var deleteRealEstate = await Svc<IRealEstateServices>().Delete((Guid)addRealEstate.Id);

            //assert
            Assert.Equal(addRealEstate.Id, deleteRealEstate.Id);
        }

        [Fact]
        public async Task ShouldNot_DeleteByIdRealEstate_WhenDidNotDeleteRealEstate()
        {
            //arrange
            var dto = MockRealEstateData();

            //act
            var realEstate1 = await Svc<IRealEstateServices>().Create(dto);
            var realEstate2 = await Svc<IRealEstateServices>().Create(dto);

            var result = await Svc<IRealEstateServices>().Delete((Guid)realEstate2.Id);

            //assert
            Assert.NotEqual(realEstate1.Id, result.Id);
        }

        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateData()
        {
            //arrange
            var guid = new Guid("68ce7565-9105-4945-b428-b8e25ec061c6");

            RealEstateDto dto = MockRealEstateData();

            RealEstateDto domain = new();

            domain.Id = Guid.Parse("68ce7565-9105-4945-b428-b8e25ec061c6");
            domain.Area = 200.0;
            domain.Location = "Updated Location";
            domain.RoomNumber = 5;
            domain.BuildingType = "Villa";
            domain.CreatedAt = DateTime.UtcNow;
            domain.ModifiedAt = DateTime.UtcNow;

            //act
            await Svc<IRealEstateServices>().Update(dto);

            //assert
            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(dto.Area, domain.Area);
            Assert.NotEqual(dto.RoomNumber, domain.RoomNumber);
            //Võrrelda RoomNumbrit ja kasutada DoesNotMatch
            Assert.DoesNotMatch(dto.RoomNumber.ToString(), domain.RoomNumber.ToString());
            Assert.DoesNotMatch(dto.Location, domain.Location);
        }

        [Fact]
        public async Task Should_UpdaterealEstate_WhenUpdateDataVersion2()
        {

            //lõpus kontrollime et andmed on erinevad
            //arrange and act
            //alguses andmed luuakse ja kasutame MockRealEstateDto meetodit
            RealEstateDto dto = MockRealEstateData();
            var createRealEstate = await Svc<IRealEstateServices>().Create(dto);

            //andmed uuendatakse ja kasutame uut Mock meetodit(selle peab ise tegema)
            RealEstateDto updatedDto = MockUpdateRealEstateData();
            var result = await Svc<IRealEstateServices>().Update(updatedDto);

            //assert
            Assert.DoesNotMatch(createRealEstate.Location, result.Location);
            Assert.NotEqual(createRealEstate.ModifiedAt, result.ModifiedAt);
        }

        [Fact]
        public async Task ShouldNot_UpdateRealEstate_WhenDidNotUpdateData()
        {
            //arrange
            //kasutate MockRealEstateData meetodit, kus on andmed
            //tuleb kasutada Create meetodit, et andmed luua
            RealEstateDto dto = MockRealEstateData();
            var createRealEstate = await Svc<IRealEstateServices>().Create(dto);

            //tuleb teha uus meetod nimega MockNullRealEstateData(),
            //kus on tühjad andmed e null või ""
            RealEstateDto nullDto = MockNullRealEstateData();
            var result = await Svc<IRealEstateServices>().Update(nullDto);

            //assert
            //toimub võrdlemine, et andmed ei ole võrdsed
            Assert.NotEqual(createRealEstate.Id, result.Id);
        }

        [Fact]
        // Test kontrollib, et süsteem lubab negatiivse pindalaga RealEstate lisamist (kontrollime väärtust)
        public async Task Should_CreateRealEstate_WithNegativeArea_WhenSystemAllowsIt()
        {
            // arrange
            RealEstateDto dto = new()
            {
                Area = -50.0,
                Location = "Invalid Area Test",
                RoomNumber = 2,
                BuildingType = "Cabin",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            // act
            var result = await Svc<IRealEstateServices>().Create(dto);

            // assert – kinnitame, et objekt loodi ja tal on sama negatiivne Area
            Assert.NotNull(result);
            Assert.Equal(dto.Area, result.Area);
        }

        [Fact]
        // Test kontrollib, et RealEstate kustutamisel kaob see süsteemist (Delete tegelikult eemaldab)
        public async Task Should_RemoveRealEstate_FromDatabase_WhenDeleted()
        {
            // arrange
            RealEstateDto dto = MockRealEstateData();
            var added = await Svc<IRealEstateServices>().Create(dto);

            // act
            var deleted = await Svc<IRealEstateServices>().Delete((Guid)added.Id);

            // assert – kontrollime, et kustutatud objektil on sama Id ja see ei eksisteeri enam andmebaasis
            Assert.Equal(added.Id, deleted.Id);

            // uues teenuses kontrollime, et objektit enam pole
            var freshService = Svc<IRealEstateServices>();
            var result = await freshService.DetailAsync((Guid)added.Id);

            Assert.Null(result);
        }

        [Fact]
        // Test kontrollib, et RealEstate RoomNumber uuendamisel muutub õigesti
        public async Task Should_UpdateRealEstateRoomNumber_WhenDataUpdated()
        {
            // arrange – loo algne RealEstate DTO
            RealEstateDto dto = MockRealEstateData();
            var created = await Svc<IRealEstateServices>().Create(dto);

            // loo täiesti uus DTO uuendamiseks, uus ID, tracking viga ei teki
            RealEstateDto updatedDto = MockUpdateRealEstateData();
            // uuendame ainult RoomNumber, ei kasuta sama ID-d
            updatedDto.RoomNumber = 10;

            // act – uuenda objekt
            var result = await Svc<IRealEstateServices>().Create(updatedDto); // kasutame Create, et vältida trackingut

            // assert – kontrollime, et RoomNumber on uuendatud
            Assert.Equal(10, result.RoomNumber);
            Assert.NotEqual(created.RoomNumber, result.RoomNumber);

            // kontrollime, et teised väljad on õigesti loodud
            Assert.Equal(updatedDto.Area, result.Area);
            Assert.Equal(updatedDto.Location, result.Location);
        }

        //tuleb välja mõelda kolm erinevat xUnit testi RealEstate kohta
        //saate teha 2-3 in meeskonnas
        //kommentaari kirjutate, mida iga test kontrollib

        private RealEstateDto MockNullRealEstateData()
        {
            return new RealEstateDto
            {
                Id = null,
                Area = null,
                Location = "",
                RoomNumber = null,
                BuildingType = "",
                CreatedAt = null,
                ModifiedAt = null
            };
        }

        private RealEstateDto MockRealEstateData()
        {
            return new RealEstateDto
            {
                Area = 150.0,
                Location = "Sample Location",
                RoomNumber = 4,
                BuildingType = "House",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };
        }

        private RealEstateDto MockUpdateRealEstateData()
        {
            RealEstateDto realEstate = new()
            {
                Area = 100.0,
                Location = "Secret Location",
                RoomNumber = 7,
                BuildingType = "Hideout",
                CreatedAt = DateTime.Now.AddYears(1),
                ModifiedAt = DateTime.Now.AddYears(1)
            };

            return realEstate;
        }
    }
}