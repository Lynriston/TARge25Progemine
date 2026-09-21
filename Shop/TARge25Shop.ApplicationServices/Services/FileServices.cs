using Microsoft.Extensions.Hosting;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;

namespace TARge25Shop.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {

        private readonly IHostEnvironment _webHost;
        private readonly TARge25ShopContext _context;

        public FileServices
            (
                IHostEnvironment webHost,
                TARge25ShopContext context
            )
        {
            _webHost = webHost;
            _context = context;
        }
        

        public void FilesToApi(SpaceshipDto dto, Spaceship domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                //Kui directoryt ei ole olemas, siis tee directory
                // \\wwwroot\\multipleFileUpload\\
                //tuleb kasutada webhosti
                if (!Directory.Exists(_webHost.ContentRootPath + "\\wwwroor\\multipleFileUpload\\"))
                {
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\wwwroor\\multipleFileUpload\\");
                }

                foreach (var file in dto.Files)
                {
                    string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    //tuleb kaks ülevalpool olevat muutujat kombineerida üheks
                    string filePath = Path.Combine(uploadsFolder + uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                        //tuleb Domaini teha class FileToApi,
                        //kus on muutujad Id, ExistingFilePath ja SpaceshipId
                        FileToApi path = new FileToApi
                        {
                            Id = Guid.NewGuid(),
                            ExistingFilePath = uniqueFileName,
                            SpaceshipId = domain.Id
                        };

                        //tuleb lisada context construktorisse
                        _context.FileToApis.AddAsync();
                    }
                }
            }
        }
    }
}
