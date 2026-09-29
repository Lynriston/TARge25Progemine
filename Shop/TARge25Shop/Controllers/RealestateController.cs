using Microsoft.AspNetCore.Mvc;
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


        public RealestateController
            (
                IRealestateServices realestateServices,
                TARge25ShopContext context
            )
        {
            _realestateServices = realestateServices;
            _context = context;
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

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Create(RealestateCreateUpdateViewModel vm)
        {
            var dto = new RealestateDto
            {
                Area = vm.Area,
                Location = vm.Location,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };

            var result = await _realestateServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
