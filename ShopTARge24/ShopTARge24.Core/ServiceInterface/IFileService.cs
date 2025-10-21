using ShopTARge24.Core.Domain;
using ShopTARge24.Core.Dto;


namespace ShopTARge24.Core.ServiceInterface
{
    public interface IFileService
    {
        // Kindergarten methods
        void FileToDbKindergartenDto(KindergartenDto dto, Kindergartens kindergarten);
        void UploadFilesToDatabase(KindergartenDto dto, Kindergartens domain);
        void DeleteFilesFromDatabaseKindergarten(Guid kindergartenId);

    }
}