using Microsoft.EntityFrameworkCore;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;

namespace TARge25Shop.ApplicationServices.Services
{
    public class KindergardenServices : IKindergardenServices
    {
        private readonly TARge25ShopContext _context;
        private readonly IFileServices _fileServices;
        public KindergardenServices(TARge25ShopContext context, IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }
        public async Task<Kindergarden> Create(KindergardenDto dto)
        {
            if (dto == null || dto.ChildrenCount < 0 || dto.ChildrenCount <= 3)
            {
                return null; // Reject negative counts
            }

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
        public async Task<Kindergarden> Update(KindergardenDto dto)
        {
            if (dto == null || dto.Id == null || dto.Id == Guid.Empty)
            {
                return null;
            }
            // 1. Fetch the existing entity from the database so it's tracked properly
            var kinderGarden = await _context.Kindergardens
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (kinderGarden == null)
            {
                return null;
            }

            // 2. Update only the mutable fields
            kinderGarden.GroupName = dto.GroupName;
            kinderGarden.ChildrenCount = dto.ChildrenCount;
            kinderGarden.KindergardenName = dto.KindergardenName;
            kinderGarden.TeacherName = dto.TeacherName;

            // 3. Keep the original CreatedAt, but update UpdatedAt
            kinderGarden.CreatedAt = dto.CreatedAt;
            kinderGarden.UpdatedAt = DateTime.UtcNow;

            // 4. Save changes (No need for _context.Kindergardens.Update()!)
            await _context.SaveChangesAsync();

            return kinderGarden;
        }
        public async Task<Kindergarden> DetailAsync(Guid id)
        {
            var kindergarden = await _context.Kindergardens
                .FirstOrDefaultAsync(x => x.Id == id);

            return kindergarden;
        }
        public async Task<Kindergarden> Delete(Guid id)
        {
            var kindergarden = await _context.Kindergardens
                .FirstOrDefaultAsync(x => x.Id == id);

            var images = await _context.FileToDatabases
                .Where(x => x.KindergardenId == id)
                .Select(y => new FileToDatabaseDto
                {
                    Id = y.Id,
                }).ToArrayAsync();

            _context.Kindergardens.Remove(kindergarden);
            await _context.SaveChangesAsync();

            return kindergarden;
        }
        public async Task<Kindergarden> Details(Guid id)
        {
            var result = await _context.Kindergardens
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }
    }
}
