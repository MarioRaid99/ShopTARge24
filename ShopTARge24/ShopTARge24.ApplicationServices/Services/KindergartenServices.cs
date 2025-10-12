using Microsoft.EntityFrameworkCore;
using ShopTARge24.Core.Domain;
using ShopTARge24.Core.Dto;
using ShopTARge24.Core.ServiceInterface;
using ShopTARge24.Data;

namespace ShopTARge24.ApplicationServices.Services
{
        public class KindergartenServices : IKindergartenServices
        {
            private readonly ShopTARge24Context _context;
            private readonly IFileServices _fileServices;

            public KindergartenServices
                (
                    ShopTARge24Context context,
                    IFileServices fileServices
                )
            {
                _context = context;
                _fileServices = fileServices;
            }

            public async Task<Kindergarten> Create(KindergartenDto dto)
            {
                Kindergarten kindergartens = new Kindergarten();

                kindergartens.Id = Guid.NewGuid();
                kindergartens.KindergartenName = dto.KindergartenName;
                kindergartens.GroupName = dto.GroupName;
                kindergartens.TeacherName = dto.TeacherName;
                kindergartens.ChildCount = (int)dto.ChildCount;
                kindergartens.CreatedAt = DateTime.Now;
                kindergartens.UpdatedAt = DateTime.Now;

            if (dto.Files != null)
            {
                _fileServices.KindergartenUploadFilesToDatabase(dto, kindergartens);
            }

            await _context.Kindergartens.AddAsync(kindergartens);
            await _context.SaveChangesAsync();

                return kindergartens;
            }

            public async Task<Kindergarten> Update(KindergartenDto dto)
            {
                //vaja leida doamini objekt, mida saaks mappida dto-ga
                Kindergarten kindergartens = new Kindergarten();
                
                kindergartens.Id = (Guid)dto.Id;
                kindergartens.KindergartenName = dto.KindergartenName;
                kindergartens.GroupName = dto.GroupName;
                kindergartens.TeacherName = dto.TeacherName;
                kindergartens.ChildCount = (int)dto.ChildCount;
                kindergartens.CreatedAt = (DateTime)dto.CreatedAt;
                kindergartens.UpdatedAt = DateTime.Now;

                //tuleb db-s teha andmete uuendamine jauue oleku salvestamine
                _context.Kindergartens.Update(kindergartens);
                await _context.SaveChangesAsync();

                return kindergartens;
            }

            public async Task<Kindergarten> DetailAsync(Guid id)
            {
                var result = await _context.Kindergartens
                    .FirstOrDefaultAsync(x => x.Id == id);

                return result;
            }

            public async Task<Kindergarten> Delete(Guid id)
            {
                //leida ülesse konkreetne soovitud rida, mida soovite kustutada
                var result = await _context.Kindergartens
                    .FirstOrDefaultAsync(x => x.Id == id);


                //kui rida on leitud, siis eemaldage andmebaasist
                _context.Kindergartens.Remove(result);
                await _context.SaveChangesAsync();

                return result;
            }
        }
}