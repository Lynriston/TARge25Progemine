using Microsoft.AspNetCore.Mvc;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Models.Email;

namespace TARge25Shop.Controllers
{
    public class EmailController : Controller
    {
        //kontstruktori kaudu saan kasutada IEmailServices meetodit
        private readonly IEmailServices _emailService;
        public EmailController
            (
                IEmailServices emailService
            )
        {
            _emailService = emailService;
        }
        // GET: EmailController
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //Vaja teha meethod, mis võtab vastu EmailDto objekti
        //ja kutsuda välja _emailServices.SendEmail meetodi
        //ning teha vahe instants, mis vahetab EmailDto ja ViesModeli vahel
        public IActionResult SendEmail(EmailViewModel vm)
        {
            var files = Request.Form.Files.Any() ? Request.Form.Files.ToList() : new List<IFormFile>();
            var dto = new EmailDto
            {
                To = vm.To,
                Subject = vm.Subject,
                Body = vm.Body,
                Attachments = files
            };
            _emailService.SendEmail(dto);
            return RedirectToAction(nameof(Index));
        }
    }
}