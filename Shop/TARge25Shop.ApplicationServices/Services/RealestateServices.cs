using Microsoft.EntityFrameworkCore;
using System.Xml;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;

namespace TARge25Shop.ApplicationServices.Services
{
    public class RealestateServices : IRealestateServices
    {
        private readonly TARge25ShopContext _context;
        private readonly IFileServices _fileServices;

        public RealestateServices(TARge25ShopContext context, IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<RealEstate> Create(RealestateDto dto)
        {
            RealEstate realEstate = new();

            realEstate.Id = Guid.NewGuid();
            realEstate.Area = dto.Area;
            realEstate.Location = dto.Location;
            realEstate.RoomNumber = dto.RoomNumber;
            realEstate.BuildingType = dto.BuildingType;
            realEstate.CreatedAt = DateTime.Now;
            realEstate.ModifiedAt = DateTime.Now;
            _fileServices.FilesToApi(dto, realEstate);

            _context.RealEstate.Add(realEstate);
            await _context.SaveChangesAsync();

            return realEstate;
        }

        public async Task<RealEstate> Update(RealestateDto dto)
        {
            RealEstate realEstate = new();

            realEstate.Id = Guid.NewGuid();
            realEstate.Area = dto.Area;
            realEstate.Location = dto.Location;
            realEstate.RoomNumber = dto.RoomNumber;
            realEstate.BuildingType = dto.BuildingType;
            realEstate.CreatedAt = DateTime.Now;
            realEstate.ModifiedAt = DateTime.Now;
            _fileServices.FilesToApi(dto, realEstate);

            _context.RealEstate.Update(realEstate);
            await _context.SaveChangesAsync();

            return realEstate;
        }

        public async Task<RealEstate> DetailAsync(Guid id)
        {
            var realestate = await _context.RealEstate
                .FirstOrDefaultAsync(x => x.Id == id);

            return realestate;
        }

        public async Task<RealEstate> Delete(Guid id)
        {
            var result = await _context.RealEstate
                .FirstOrDefaultAsync(x => x.Id == id);

            //var images muutuja alt otsib ülesse pildid
            var images = await _context.FileToApis
                .Where(x => x.RealEstateId == id)
                .Select(y => new FileToApiDto
                {
                    Id = y.Id,
                    ExistingFilePath = y.ExistingFilePath,
                    RealEstateId = y.RealEstateId
                }).ToArrayAsync();
            //ja kutsub välja removeImagesFromApi
            await _fileServices.RemoveImagesFromApi(images);

            _context.RealEstate.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
        public async Task<RealEstate> Details(Guid id)
        {
            var result = await _context.RealEstate
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }
    }
}
