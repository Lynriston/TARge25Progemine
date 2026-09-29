using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;

namespace TARge25Shop.ApplicationServices.Services
{
    public class RealestateServices : IRealestateServices
    {
        private readonly TARge25ShopContext _context;

        public RealestateServices(TARge25ShopContext _context)
        {
            _context = context;
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

            _context.RealEstate.Add(realEstate);
        }
    }
}
