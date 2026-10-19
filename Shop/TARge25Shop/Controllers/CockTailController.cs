using Microsoft.AspNetCore.Mvc;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Models.CockTail;
using TARge25Shop.Core.Dto;
using Microsoft.EntityFrameworkCore;
using TARge25Shop.Data;

namespace TARge25Shop.Controllers
{
    public class CockTailController : Controller
    {
        private readonly ICockTailServices _cockTailServices;

        public CockTailController
            (
                ICockTailServices cockTailServices
            )
        {
            _cockTailServices = cockTailServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Cocktail()
        {
            var cocktail = await _cockTailServices.CockTailResultHttpClient();

            CockTailViewModel vm = new();

            vm.idDrink = cocktail.idDrink;
            vm.strDrink = cocktail.strDrink;
            vm.strDrinkAlternate = cocktail.strDrinkAlternate;
            vm.strTags = cocktail.strTags;
            vm.strCategory = cocktail.strCategory;
            vm.strIBA = cocktail.strIBA;
            vm.strAlcoholic = cocktail.strAlcoholic;
            vm.strGlass = cocktail.strGlass;
            vm.strInstructions = cocktail.strInstructions;

            return View(vm);
        }
    }
}
