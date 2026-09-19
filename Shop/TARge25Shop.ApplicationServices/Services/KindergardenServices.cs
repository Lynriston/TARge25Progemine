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
        public async Task<Kindergarden> Update(KindergardenDto dto)
        {
            Kindergarden kinderGarden = new();

            kinderGarden.Id = dto.Id;
            kinderGarden.GroupName = dto.GroupName;
            kinderGarden.ChildrenCount = dto.ChildrenCount;
            kinderGarden.KindergardenName = dto.KindergardenName;
            kinderGarden.TeacherName = dto.TeacherName;
            kinderGarden.CreatedAt = DateTime.Now;
            kinderGarden.UpdatedAt = DateTime.Now;


            //andmete salvestamine andmebaasi
            _context.Kindergardens.Update(kinderGarden);
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
            var result = await _context.Kindergardens
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Kindergardens.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
        public async Task<Kindergarden> Details(Guid id)
        {
            var result = await _context.Kindergardens
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }
    }
}
