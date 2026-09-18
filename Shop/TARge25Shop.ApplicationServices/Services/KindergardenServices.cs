using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;

namespace TARge25Shop.ApplicationServices.Services
{
    public class KindergardenServices : IKindergardenServices
    {
        private readonly TARge25ShopContext _context;
        public KindergardenServices(TARge25ShopContext context)
        {
            _context = context;
        }
        public async Task<Kindergarden> Create(KindergardenDto dto)
        {
            Kindergarden kinderGarden = new();

            kinderGarden.Id = Guid.NewGuid();
            kinderGarden.GroupName = dto.GroupName;
            kinderGarden.ChildrenCount = dto.ChildrenCount;
            kinderGarden.KindergardenName = dto.KindergardenName;
            kinderGarden.TeacherName = dto.TeacherName;
            kinderGarden.CreatedAt = DateTime.Now;
            kinderGarden.UpdatedAt = DateTime.Now;

            _context.Kindergardens.Add(kinderGarden);
            await _context.SaveChangesAsync();

            return kinderGarden;
        }
    }
}
