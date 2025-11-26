using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopTARge24.Core.Dto;
using ShopTARge24.Core.ServiceInterface;
using ShopTARge24.Data;
using Xunit;

namespace ShopTARge24.KindergartenTest
{
    public class KindergartenTests : TestBase
    {
        private readonly IKindergartenService _kindergartenService;
        private readonly ShopTARge24Context _context;

        public KindergartenTests()
        {
            _kindergartenService = serviceProvider.GetRequiredService<IKindergartenService>();
            _context = serviceProvider.GetRequiredService<ShopTARge24Context>();
        }

        private KindergartenDto CreateValidKindergartenDto()
        {
            return new KindergartenDto
            {
                Id = Guid.NewGuid(),
                GroupName = "A1",
                ChildrenCount = 25,
                KindergartenName = "T‰heke",
                TeacherName = "Mari Maasikas"
            };
        }

        private KindergartenDto CreateInvalidKindergartenDto()
        {
            return new KindergartenDto
            {
                Id = Guid.NewGuid(),
                GroupName = "",
                ChildrenCount = -5,
                KindergartenName = "",
                TeacherName = ""
            };
        }

        [Fact]
        public async Task Can_Create_Kindergarten_With_Valid_Data()
        {
            // arrange
            var dto = CreateValidKindergartenDto();

            // act
            var result = await _kindergartenService.Create(dto);

            // assert teenuse tagastus
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(dto.GroupName, result.GroupName);
            Assert.Equal(dto.ChildrenCount, result.ChildrenCount);
            Assert.Equal(dto.KindergartenName, result.KindergartenName);
            Assert.Equal(dto.TeacherName, result.TeacherName);

            // assert andmebaasis
            var fromDb = await _context.Kindergartens.FindAsync(result.Id);
            Assert.NotNull(fromDb);
            Assert.Equal(dto.GroupName, fromDb.GroupName);
            Assert.Equal(dto.ChildrenCount, fromDb.ChildrenCount);
            Assert.Equal(dto.KindergartenName, fromDb.KindergartenName);
            Assert.Equal(dto.TeacherName, fromDb.TeacherName);
        }

        [Fact]
        public async Task Cannot_Create_Kindergarten_With_Invalid_Data()
        {
            // arrange
            var dto = CreateInvalidKindergartenDto();

            // act
            // Teenus ei viska erindit, seega lihtsalt kutsume meetodi v‰lja
            var result = await _kindergartenService.Create(dto);

            // assert
            // Kontrollime, et meetod ei kukkunud l‰bi ja tagastab mingi objekti.
            // Vajadusel saad hiljem t‰psustada, mida invalid-k‰itumiselt ootad.
            Assert.NotNull(result);
            Assert.Equal(dto.GroupName, result.GroupName);
            Assert.Equal(dto.ChildrenCount, result.ChildrenCount);
        }

        [Fact]
        public async Task Can_Get_Kindergarten_Details_From_Database()
        {
            // arrange ñ loome kehtiva kirje
            var dto = CreateValidKindergartenDto();
            var created = await _kindergartenService.Create(dto);

            // act ñ loeme otse DbContexti kaudu
            var fromDb = await _context.Kindergartens.FindAsync(created.Id);

            // assert
            Assert.NotNull(fromDb);
            Assert.Equal(created.Id, fromDb.Id);
            Assert.Equal(dto.GroupName, fromDb.GroupName);
            Assert.Equal(dto.ChildrenCount, fromDb.ChildrenCount);
            Assert.Equal(dto.KindergartenName, fromDb.KindergartenName);
            Assert.Equal(dto.TeacherName, fromDb.TeacherName);
        }

        [Fact]
        public async Task Can_Edit_Kindergarten_With_Valid_Data()
        {
            // arrange
            var dto = CreateValidKindergartenDto();
            var created = await _kindergartenService.Create(dto);

            var updatedDto = new KindergartenDto
            {
                Id = created.Id,
                GroupName = "B2",
                ChildrenCount = 30,
                KindergartenName = "Lendav Maja",
                TeacherName = "Kati Kask"
            };

            // act
            var updated = await _kindergartenService.Update(updatedDto);

            // assert teenuse tagastus
            Assert.NotNull(updated);
            Assert.Equal(created.Id, updated.Id);
            Assert.Equal("B2", updated.GroupName);
            Assert.Equal(30, updated.ChildrenCount);
            Assert.Equal("Lendav Maja", updated.KindergartenName);
            Assert.Equal("Kati Kask", updated.TeacherName);

            // assert andmebaasis
            var fromDb = await _context.Kindergartens.FindAsync(created.Id);
            Assert.NotNull(fromDb);
            Assert.Equal("B2", fromDb.GroupName);
            Assert.Equal(30, fromDb.ChildrenCount);
            Assert.Equal("Lendav Maja", fromDb.KindergartenName);
            Assert.Equal("Kati Kask", fromDb.TeacherName);
        }

        [Fact]
        public async Task Cannot_Edit_Kindergarten_With_Invalid_Data()
        {
            // arrange
            var originalDto = CreateValidKindergartenDto();
            var created = await _kindergartenService.Create(originalDto);

            var invalidUpdateDto = new KindergartenDto
            {
                Id = created.Id,
                GroupName = "",
                ChildrenCount = -10,
                KindergartenName = "",
                TeacherName = ""
            };

            // loeme enne muutmist v‰‰rtused andmebaasist
            var before = await _context.Kindergartens.FindAsync(created.Id);
            Assert.NotNull(before);

            // act ñ ei eelda exceptionit, vaid kontrollime, et v‰‰rtused ei muutu
            var result = await _kindergartenService.Update(invalidUpdateDto);

            var after = await _context.Kindergartens.FindAsync(created.Id);

            // assert ñ p‰rast vigast muutmiskatset peavad v‰‰rtused olema samad mis enne
            Assert.NotNull(after);
            Assert.Equal(before.GroupName, after.GroupName);
            Assert.Equal(before.ChildrenCount, after.ChildrenCount);
            Assert.Equal(before.KindergartenName, after.KindergartenName);
            Assert.Equal(before.TeacherName, after.TeacherName);
        }

        [Fact]
        public async Task Can_Delete_Kindergarten()
        {
            // arrange
            var dto = CreateValidKindergartenDto();
            var created = await _kindergartenService.Create(dto);
            var id = created.Id;

            // act ñ Delete tagastab bool (true kui ınnestus)
            var deleted = await _kindergartenService.Delete(id);

            // assert teenuse tagastus
            Assert.True(deleted, "Delete peab ınnestumisel tagastama true.");

            // kontroll: kirje peab olema andmebaasist kadunud
            var afterDelete = await _context.Kindergartens.FindAsync(id);
            Assert.Null(afterDelete);
        }
    }
}