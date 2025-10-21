using System.Xml;
using ShopTARge24.Core.Domain;
using ShopTARge24.Core.Dto;

namespace ShopTARge24.Core.ServiceInterface
{
    public interface IFileServices
    {
        void FilesToApi(SpaceshipDto dto, Spaceships spaceships);
        Task<FileToApi> RemoveImageFromApi(FileToApiDto dto);
        //Task<List<FileToApi>> RemoveImagesFromApi(FileApiDto[] dtos);
        void UploadFilesToDatabase(RealEstateDto dto, RealEstate domain);
    }
}