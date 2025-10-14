using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShopTARge24.Core.Domain;
using ShopTARge24.Core.Dto;
using ShopTARge24.Core.ServiceInterface;
using ShopTARge24.Data;

namespace ShopTARge24.ApplicationServices.Services
{
    public class FileService : IFileService
    {
        private readonly IHostEnvironment _webHost;
        private readonly ShopTARge24Context _context;

        public FileService(
            IHostEnvironment webHost,
            ShopTARge24Context context)
        {
            _webHost = webHost;
            _context = context;
        }

        public void FileToDbKindergartenDto(KindergartenDto dto, Kindergartens domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                if (!Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"))
                {
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\");
                }

                foreach (var file in dto.Files)
                {
                    string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "multipleFileUpload");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                        FileToKindergarten path = new FileToKindergarten
                        {
                            Id = Guid.NewGuid(),
                            ExistingFilePath = uniqueFileName,
                            KindergartenId = domain.Id
                        };
                        _context.FileToKindergartens.AddAsync(path);
                    }
                }
            }
        }


        public void UploadFilesToDatabase(KindergartenDto dto, Kindergartens domain)
        {
            //toimub kontroll, kas on vähemalt üks fail või mitu
            if (dto.Files != null && dto.Files.Count > 0)
            {
                //tuleb kasutada foreachi et mitu faili ülesse laadida
                foreach (var file in dto.Files)
                {
                    //foreachi sees tuleb kasutada using-t
                    using (var target = new MemoryStream())
                    {
                        FileToDbKindergarten files = new FileToDbKindergarten()
                        {
                            Id = Guid.NewGuid(),
                            ImageTitle = file.FileName,
                            KindergartenId = domain.Id

                        };

                        file.CopyTo(target);
                        files.ImageData = target.ToArray();

                        _context.FileToDbKindergartens.Add(files);
                    }
                }
                _context.SaveChanges();
            }
        }

        public async Task<FileToKindergarten> RemoveImageFromApi(FileApiKindergartenDto dto)
        {
            //kui soovin kustutada, siis pean l'bi Id pildi ülesse otsima
            var imageId = await _context.FileToKindergartens
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            //kus asuvad pildid, mida hakatakse kustutama
            var filePath = _webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"
                + imageId.ExistingFilePath;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _context.FileToKindergartens.Remove(imageId);
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<List<FileToKindergarten>> RemoveImagesFromApi(FileApiKindergartenDto[] dtos)
        {
            foreach (var dto in dtos)
            {
                var imageId = await _context.FileToKindergartens
                    .FirstOrDefaultAsync(x => x.ExistingFilePath == dto.ExistingFilePath);

                var filePath = _webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"
                    + imageId.ExistingFilePath;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                _context.FileToKindergartens.Remove(imageId);
                await _context.SaveChangesAsync();
            }

            return null;
        }

        public void DeleteFilesFromDatabaseKindergarten(Guid kindergartenId)
        {
            var files = _context.FileToDbKindergartens
                .Where(f => f.KindergartenId == kindergartenId)
                .ToList();

            if (files.Any())
            {
                _context.FileToDbKindergartens.RemoveRange(files);
                _context.SaveChanges();
            }
        }

        public void DeleteSingleFileFromDatabase(Guid fileId)
        {
            var file = _context.FileToDbKindergartens
                .FirstOrDefault(f => f.Id == fileId);

            if (file != null)
            {
                _context.FileToDbKindergartens.Remove(file);
                _context.SaveChanges();
            }
        }

    }
}