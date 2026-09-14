using Microsoft.AspNetCore.Mvc;
using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;
using TARge25Shop.Models.Spaceship;

namespace TARge25Shop.Controllers
{
    public class SpaceshipController : Controller
    {
        private readonly ISpaceshipServices _spaceshipServices;
        private readonly TARge25ShopContext _context;

        public SpaceshipController
            (
            ISpaceshipServices spaceshipServices, 
            TARge25ShopContext context
            )
        {
            _spaceshipServices = spaceshipServices;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //Kutsume teenuse välja, et saada kõik kosmoselaevad. See on 
            //asünkroone tegevus ja kasutame await.
            //constructoris tuleb välja kutsuda DbCondext, et 
            //saaksime andmeid kätte. Seejärel kutsume teenuse välja.
            var result = _context.Spaceships
                .Select(x => new SpaceshipIndexViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ShipType = x.ShipType,
                    CreatedAt = x.CreatedAt,
                    Crew = x.Crew,
                });


            return View(result);
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
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var spaceship = await _spaceshipServices.DetailAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var vm = new SpaceshipUpdateViewModel
            {
                Id = spaceship.Id,
                Name = spaceship.Name,
                ShipType = spaceship.ShipType,
                Crew = spaceship.Crew,
                EnginePower = spaceship.EnginePower
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(SpaceshipUpdateViewModel vm)
        {
            var dto = new SpaceshipDto()
            {
                Id = vm.Id,
                Name = vm.Name,
                ShipType = vm.ShipType,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt
            };
            var result = await _spaceshipServices.Update(dto);

            if (result == null)
            {
                // Handle the case when the creation fails
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            return View();
        }
    }
}
