using TARge25Shop.Core.Dto;
using TARge25Shop.Core.Domain;

namespace TARge25Shop.Core.ServiceInterface
{
    public interface IRealestateServices
    {
        Task<RealEstate> Create(RealestateDto dto);
        Task<RealEstate> Update(RealestateDto dto);
        Task<RealEstate> DetailAsync(Guid dto);
        Task<RealEstate> Delete(Guid dto);
        Task<RealEstate> Details(Guid dto);
    }
}
