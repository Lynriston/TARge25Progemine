using Microsoft.AspNetCore.Mvc;
using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;
using TARge25Shop.Data.Migrations;
using TARge25Shop.Models.Kindergarden;
using TARge25Shop.Models.Spaceship;
using Microsoft.EntityFrameworkCore;

namespace TARge25Shop.Controllers
{
    public class KindergardenController : Controller
    {

        private readonly IKindergardenServices _kindergardenServices;
        private readonly TARge25ShopContext _context;
        private readonly IFileServices _fileServices;

        public KindergardenController
            (
            IKindergardenServices kindergardenServices,
            TARge25ShopContext context,
            IFileServices fileServices
            )
        {
            _kindergardenServices = kindergardenServices;
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<IActionResult> Index()
        {
            //Kutsume teenuse välja, et saada kõik kosmoselaevad. See on 
            //asünkroone tegevus ja kasutame await.
            //constructoris tuleb välja kutsuda DbCondext, et 
            //saaksime andmeid kätte. Seejärel kutsume teenuse välja.
            var result = _context.Kindergardens
                .Select(x => new KindergardenIndexViewModel
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    ChildrenCount = x.ChildrenCount,
                    KindergardenName = x.KindergardenName,
                    TeacherName = x.TeacherName,
                    CreatedAt = x.CreatedAt,
                });


            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            KindergardenCreateUpdateViewModel result = new();

            return View("CreateUpdate", result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(KindergardenCreateUpdateViewModel vm)
        {
            var dto = new KindergardenDto
            {
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergardenName = vm.KindergardenName,
                TeacherName = vm.TeacherName,
                Files = vm.Files,
                Image = vm.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        KindergardenId = x.KindergardenId
                    })
            };

            var result = await _kindergardenServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var kindergarden = await _kindergardenServices.DetailAsync(id);

            if (kindergarden == null)
            {
                return NotFound();
            }

            KindergardenImageViewModel[] images = await FileFromDatabase(id);

            var vm = new KindergardenCreateUpdateViewModel();

            vm.Id = kindergarden.Id;
            vm.GroupName = kindergarden.GroupName;
            vm.ChildrenCount = kindergarden.ChildrenCount;
            vm.KindergardenName = kindergarden.KindergardenName;
            vm.TeacherName = kindergarden.TeacherName;
            vm.Image.AddRange(images);

            return View("CreateUpdate", vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(KindergardenCreateUpdateViewModel vm)
        {
            var dto = new KindergardenDto()
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergardenName = vm.KindergardenName,
                TeacherName = vm.TeacherName,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt,
                Files = vm.Files,
                Image = vm.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        KindergardenId = x.KindergardenId
                    })
            };
            var result = await _kindergardenServices.Update(dto);

            var kindergardenId = result.Id;

            if (result == null)
            {
                // Handle the case when the creation fails
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Details), new { id = kindergardenId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var kindergarden = await _kindergardenServices.DetailAsync(id);

            if (kindergarden == null)
            {
                return NotFound();
            }

            KindergardenImageViewModel[] images = await FileFromDatabase(id);

            var vm = new KindergardenDeleteViewModel();
        
            vm.Id = kindergarden.Id;
            vm.GroupName = kindergarden.GroupName;
            vm.ChildrenCount = kindergarden.ChildrenCount;
            vm.KindergardenName = kindergarden.KindergardenName;
            vm.TeacherName = kindergarden.TeacherName;
            vm.CreatedAt = kindergarden.CreatedAt;
            vm.UpdatedAt = kindergarden.UpdatedAt;
            vm.Image.AddRange(images);

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var kindergarden = await _kindergardenServices.Delete(id);

            if (kindergarden == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        //Teha Detaili vaate meetod
        public async Task<IActionResult> Details(Guid id)
        {
            var kindergarden = await _kindergardenServices.Details(id);
            if (kindergarden == null)
            {
                return NotFound();
            }

            KindergardenImageViewModel[] images = await FileFromDatabase(id);

            // Tuleb teha vaheinstants dbo ja vm vahel
            var vm = new KindergardenDetailsViewModel();
            vm.Id = kindergarden.Id;
            vm.GroupName = kindergarden.GroupName;
            vm.ChildrenCount = kindergarden.ChildrenCount;
            vm.KindergardenName = kindergarden.KindergardenName;
            vm.TeacherName = kindergarden.TeacherName;
            vm.CreatedAt = kindergarden.CreatedAt;
            vm.UpdatedAt = kindergarden.UpdatedAt;
            vm.Images.AddRange(images);
            
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveImage(KindergardenImageViewModel vm)
        {
            var dto = new FileToDatabaseDto()
            {
                Id = vm.ImageId
            };

            var image = await _fileServices.RemoveImageFromDatabase(dto);

            var realEstateId = image.KindergardenId;

            if (image == null)
            {
                return RedirectToAction(nameof(Index));
            }
            //muuta see niimoodi, et pärast pildi kustutamist jääks kasutaja
            //samale kinnisvara detailide lehele, mitte ei suunataks tagasi index lehele
            //return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Update), new { id = realEstateId });
        }

        private async Task<KindergardenImageViewModel[]> FileFromDatabase(Guid id)
        {
            return await _context.FileToDatabases
                .Where(x => x.KindergardenId == id)
                .Select(y => new KindergardenImageViewModel
                {
                    ImageId = y.Id,
                    ImageTitle = y.ImageTitle,
                    ImageData = y.ImageData,
                    KindergardenId = y.KindergardenId,
                    Image = string.Format("data:image/gif;base64,{0}",
                        Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync();
        }
    }
}
