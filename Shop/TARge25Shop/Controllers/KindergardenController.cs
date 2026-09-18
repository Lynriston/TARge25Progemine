using Microsoft.AspNetCore.Mvc;
using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;
using TARge25Shop.Models.Kindergarden;
using TARge25Shop.Models.Spaceship;

namespace TARge25Shop.Controllers
{
    public class KindergardenController : Controller
    {

        private readonly IKindergardenServices _kindergardenServices;
        private readonly TARge25ShopContext _context;

        public KindergardenController
            (
            IKindergardenServices kindergardenServices,
            TARge25ShopContext context
            )
        {
            _kindergardenServices = kindergardenServices;
            _context = context;
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
                TeacherName = vm.TeacherName
            };

            var result = await _kindergardenServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
