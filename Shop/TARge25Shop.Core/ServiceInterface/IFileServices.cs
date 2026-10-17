using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;

namespace TARge25Shop.Core.ServiceInterface
{
    public interface IFileServices
    {

        public void FilesToApi(SpaceshipDto dto, Spaceship domain);
    }
}
