
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARge24.ApplicationServices.Services;
using ShopTARge24.Core.Dto;
using ShopTARge24.Core.ServiceInterface;
using ShopTARge24.Data;
using ShopTARge24.Models.Kindergartens;

namespace ShopTARge24.Controllers
{
    public class KindergartenController : Controller
    {
        private readonly ShopTARge24Context _context;
        private readonly IKindergartenService _kindergartenService;
        private readonly IFileService _fileService;

        public KindergartenController(
            ShopTARge24Context context,
            IKindergartenService kindergartenService,
            IFileService fileService
        )
        {
            _context = context;
            _kindergartenService = kindergartenService;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            var result = _context.Kindergartens
                .Select(x => new KindergartenIndexViewModel
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    ChildrenCount = x.ChildrenCount,
                    KindergartenName = x.KindergartenName,
                    TeacherName = x.TeacherName,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                });

            return View(result);
        }

        //create
        [HttpGet]
        public IActionResult Create()
        {
            var vm = new KindergartenCreateUpdateViewModel();
            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(KindergartenCreateUpdateViewModel vm)
        {
            var dto = new KindergartenDto
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Files = vm.Files,
                Image = vm.Image?.Select(x => new FileToDbKindergartenDto
                {
                    Id = x.Id,
                    ImageTitle = x.ImageTitle,
                    ImageData = x.ImageData,
                    KindergartenId = x.KindergartenId
                }).ToArray()
            };

            var result = await _kindergartenService.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        //update
        public async Task<IActionResult> Update(Guid id)
        {
            var kg = await _kindergartenService.DetailAsync(id);

            if (kg == null) return NotFound();

            KindergartenImageViewModel[] images = await FileToDbKindergartens(id);

            var vm = new KindergartenCreateUpdateViewModel();

            vm.Id = kg.Id;
            vm.GroupName = kg.GroupName;
            vm.ChildrenCount = kg.ChildrenCount;
            vm.KindergartenName = kg.KindergartenName;
            vm.TeacherName = kg.TeacherName;
            vm.CreatedAt = kg.CreatedAt;
            vm.UpdatedAt = kg.UpdatedAt;
            vm.Image.AddRange(images);


            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(KindergartenCreateUpdateViewModel vm)
        {
            var dto = new KindergartenDto()
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = DateTime.Now,
                Files = vm.Files, // new uploaded files
                Image = vm.Image   // existing images
            .Select(x => new FileToDbKindergartenDto
            {
                Id = x.Id,
                ImageTitle = x.ImageTitle,
                ImageData = x.ImageData,
                KindergartenId = x.KindergartenId
            }).ToArray()
            };

            var result = await _kindergartenService.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // save new uploaded files
            if (dto.Files != null && dto.Files.Count > 0)
            {
                _fileService.UploadFilesToDatabase(dto, result);
            }

            return RedirectToAction(nameof(Index));
        }

        //details
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var kg = await _kindergartenService.DetailAsync(id);

            if (kg == null) return NotFound();

            KindergartenImageViewModel[] images = await FileToDbKindergartens(id);

            var vm = new KindergartenDetailsViewModel();

            vm.Id = kg.Id;
            vm.GroupName = kg.GroupName;
            vm.ChildrenCount = kg.ChildrenCount;
            vm.KindergartenName = kg.KindergartenName;
            vm.TeacherName = kg.TeacherName;
            vm.CreatedAt = kg.CreatedAt;
            vm.UpdatedAt = kg.UpdatedAt;
            vm.Image.AddRange(images);

            return View(vm);
        }

        //delete
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var kg = await _kindergartenService.DetailAsync(id);

            if (kg == null)
            {
                return NotFound();
            }

            KindergartenImageViewModel[] images = await FileToDbKindergartens(id);

            var vm = new KindergartenDeleteViewModel();

            vm.Id = kg.Id;
            vm.GroupName = kg.GroupName;
            vm.ChildrenCount = kg.ChildrenCount;
            vm.KindergartenName = kg.KindergartenName;
            vm.TeacherName = kg.TeacherName;
            vm.CreatedAt = kg.CreatedAt;
            vm.UpdatedAt = kg.UpdatedAt;
            vm.Image.AddRange(images);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            bool deleted = await _kindergartenService.Delete(id);

            if (deleted == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<KindergartenImageViewModel[]> FileToDbKindergartens(Guid id)
        {
            return await _context.FileToDbKindergartens

                .Where(x => x.KindergartenId == id)
                .Select(y => new KindergartenImageViewModel
                {
                    Id = y.Id,
                    KindergartenId = y.Id,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,
                    Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(y.ImageData))


                }).ToArrayAsync();
        }

        [HttpPost]

        public IActionResult DeleteFile(Guid fileId)
        {
            var file = _context.FileToDbKindergartens
                .FirstOrDefault(f => f.Id == fileId);
            if (file == null)
            {
                return NotFound();
            }

            _context.FileToDbKindergartens.Remove(file);
            _context.SaveChanges();

            return RedirectToAction("Delete", new { id = file.KindergartenId });

        }

    }
}
