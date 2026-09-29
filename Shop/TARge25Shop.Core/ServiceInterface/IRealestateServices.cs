using TARge25Shop.Core.Dto;
using TARge25Shop.Core.Domain;

namespace TARge25Shop.Core.ServiceInterface
{
    public interface IRealestateServices
    {
        Task<RealEstate> Create(RealestateDto dto);
    }
}
