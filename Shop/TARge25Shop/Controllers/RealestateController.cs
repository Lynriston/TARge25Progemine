using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;
using TARge25Shop.Models.RealEstate;


namespace TARge25Shop.Controllers
{
    public class RealestateController : Controller
    {

        private readonly IRealestateServices _realestateServices;
        private readonly TARge25ShopContext _context;
        private readonly IFileServices _fileServices;


        public RealestateController
            (
                IRealestateServices realestateServices,
                TARge25ShopContext context,
                IFileServices fileServices
            )
        {
            _realestateServices = realestateServices;
            _context = context;
            _fileServices = fileServices;
        }
        public async Task<IActionResult> Index()
        {
            var result = _context.RealEstate
                .Select(x => new RealestateIndexViewModel
                {
                    Id = x.Id,
                    Area = x.Area,
                    Location = x.Location,
                    RoomNumber = x.RoomNumber,
                    BuildingType = x.BuildingType,
                });

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            RealestateCreateUpdateViewModel result = new();

            return View("CreateUpdate", result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RealestateCreateUpdateViewModel vm)
        {
            var dto = new RealestateDto
            {
                Area = vm.Area,
                Location = vm.Location,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                Files = vm.Files,
                Image = vm.Images
                    .Select( x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        RealEstateId = x.RealEstateId
                    }).ToArray()
            };

            var result = await _realestateServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var realestate = await _realestateServices.DetailAsync(id);

            if (realestate == null)
            {
                return NotFound();
            }

            RealestateImageViewModel[] images = await FileFromDatabase(id);

            var vm = new RealestateCreateUpdateViewModel();

            vm.Id = realestate.Id;
            vm.Area = realestate.Area;
            vm.Location = realestate.Location;
            vm.BuildingType = realestate.BuildingType;
            vm.CreatedAt = realestate.CreatedAt;
            vm.ModifiedAt = realestate.ModifiedAt;
            vm.Images.AddRange(images);

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RealestateCreateUpdateViewModel vm)
        {
            var dto = new RealestateDto()
            {
                Id = vm.Id,
                Area = vm.Area,
                Location = vm.Location,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt,
                Files = vm.Files,
                Image = vm.Images
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        RealEstateId = x.RealEstateId
                    }).ToArray()

            };
            var result = await _realestateServices.Update(dto);

            var realEstateId = result.Id;

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Details), new { id = realEstateId });
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var realestate = await _realestateServices.DetailAsync(id);

            if (realestate == null)
            {
                return NotFound();
            }

            RealestateImageViewModel[] images = await FileFromDatabase(id);

            var vm = new RealestateDeleteViewModel();

            vm.Id = realestate.Id;
            vm.Area = realestate.Area;
            vm.Location = realestate.Location;
            vm.BuildingType = realestate.BuildingType;
            vm.CreatedAt = realestate.CreatedAt;
            vm.ModifiedAt = realestate.ModifiedAt;
            vm.Image.AddRange(images);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var realestate = await _realestateServices.Delete(id);

            if (realestate == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var realestate = await _realestateServices.Details(id);
            if (realestate == null)
            {
                return NotFound();
            }

            RealestateImageViewModel[] images = await FileFromDatabase(id);

            var vm = new RealEstateDetailsViewModel();

            vm.Id = realestate.Id;
            vm.Area = realestate.Area;
            vm.Location = realestate.Location;
            vm.RoomNumber = realestate.RoomNumber;
            vm.BuildingType = realestate.BuildingType;
            vm.CreatedAt = realestate.CreatedAt;
            vm.ModifiedAt = realestate.ModifiedAt;
            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(RealestateImageViewModel vm)
        {
            var dto = new FileToDatabaseDto()
            {
                Id = vm.ImageId
            };

            var image = await _fileServices.RemoveImageFromDatabase(dto);

            var realEstateId = image.RealEstateId;

            if (image == null)
            {
                return RedirectToAction(nameof(Index));
            }
            //muuta see niimoodi, et pärast pildi kustutamist jääks kasutaja
            //samale kinnisvara detailide lehele, mitte ei suunataks tagasi index lehele
            //return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Update), new { id = realEstateId });
        }

        private async Task<RealestateImageViewModel[]> FileFromDatabase(Guid id)
        {
            return await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .Select(y => new RealestateImageViewModel
                {
                    ImageId = y.Id,
                    ImageTitle = y.ImageTitle,
                    ImageData = y.ImageData,
                    RealEstateId = y.RealEstateId,
                    Image = string.Format("data:image/gif;base64,{0}",
                        Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync();
        }
    }
}
