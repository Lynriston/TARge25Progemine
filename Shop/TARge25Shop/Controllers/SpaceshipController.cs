using Microsoft.AspNetCore.Mvc;
using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Models.Spaceship;

namespace TARge25Shop.Controllers
{
    public class SpaceshipController : Controller
    {
        private readonly ISpaceshipServices _spaceshipServices;

        public SpaceshipController(ISpaceshipServices spaceshipServices)
        {
            _spaceshipServices = spaceshipServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SpaceshipCreateViewModel vm)
        {
            var dto = new SpaceshipDto
            {
                Name = vm.Name,
                ShipType = vm.ShipType,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower
            };

            /*Nüüd kutsume teenuse välja, et luua uus kosmoselaev
            *asünkroone tegevus ja kasutame await.*/
            var result = await _spaceshipServices.Create(dto);

            if (result == null)
            {
                // Handle the case when the creation fails
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
