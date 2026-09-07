using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;

namespace TARge25Shop.Core.ServiceInterface
{
    public interface ISpaceshipServices
    {
        Task<Spaceship> Create(SpaceshipDto dto);
    }
}