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

            if (dto.Files != null)
            {
                _fileServices.UploadFilesToDatabase(dto, realEstate);
            }

            _context.RealEstate.Add(realEstate);
            await _context.SaveChangesAsync();

            return realEstate;
        }

        public async Task<RealEstate> Update(RealestateDto dto)
        {
            RealEstate realEstate = new();

            realEstate.Id = dto.Id;
            realEstate.Area = dto.Area;
            realEstate.Location = dto.Location;
            realEstate.RoomNumber = dto.RoomNumber;
            realEstate.BuildingType = dto.BuildingType;
            realEstate.CreatedAt = DateTime.Now;
            realEstate.ModifiedAt = DateTime.Now;

            if (dto.Files != null)
            {
                _fileServices.UploadFilesToDatabase(dto, realEstate);
            }

            _context.RealEstate.Update(realEstate);
            await _context.SaveChangesAsync();

            return realEstate;
        }

        public async Task<RealEstate> DetailAsync(Guid id)
        {
            var realEstate = await _context.RealEstate
                .FirstOrDefaultAsync(x => x.Id == id);

            return realEstate;
        }

        public async Task<RealEstate> Delete(Guid id)
        {
            var realestate = await _context.RealEstate
                .FirstOrDefaultAsync(x => x.Id == id);

            var images = await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .Select(y => new FileToDatabaseDto
                {
                    Id = y.Id,
                }).ToArrayAsync();

            await _fileServices.RemoveImagesFromDatabase(images);

            _context.RealEstate.Remove(realestate);
            await _context.SaveChangesAsync();

            return realestate;
        }
        public async Task<RealEstate> Details(Guid id)
        {
            var realestate = await _context.RealEstate
                .FirstOrDefaultAsync(x => x.Id == id);

            return realestate;
        }
    }
}
