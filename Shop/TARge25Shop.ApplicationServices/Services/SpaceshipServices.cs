using Microsoft.EntityFrameworkCore;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;

namespace TARge25Shop.ApplicationServices.Services
{
    public class SpaceshipServices : ISpaceshipServices
    {
        private readonly TARge25ShopContext _context;
        private readonly IFileServices _fileServices;

        public SpaceshipServices(TARge25ShopContext context, IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }

        /*See meetod on vaja controlleris esile kutsuda
         * peab lisama interface, et saaks kutsuda see meetod välja*/
        public async Task<Spaceship> Create(SpaceshipDto dto)
        {
            /*Siin tuleb teha vaheinstansi dto ja domain vahel,
            *et andmed liiguvad dto-st domain objekti*/
            Spaceship spaceShip = new();

            spaceShip.Id = Guid.NewGuid();
            spaceShip.Name = dto.Name;
            spaceShip.ShipType = dto.ShipType;
            spaceShip.Crew = dto.Crew;
            spaceShip.EnginePower = dto.EnginePower;
            // Preserve any CreatedAt/UpdatedAt values provided by caller (tests may set these)
            spaceShip.CreatedAt = dto.CreatedAt ?? DateTime.Now;
            spaceShip.UpdatedAt = dto.UpdatedAt ?? DateTime.Now;
            //kui uus ankeet on loodud, siis
            //toimub ka faili salvestamine
            //saab kutsuda teise service classi meetotit

            if (spaceShip.Crew < 4)
            {
                spaceShip.Crew = 4;
            }

            _fileServices.FilesToApi(dto, spaceShip);
            if (spaceShip.EnginePower < 0)
            {
                spaceShip.Crew = 4;
            }

            //andmete salvestamine andmebaasi
            _context.Spaceships.Add(spaceShip);
            await _context.SaveChangesAsync();

            return spaceShip;
        }

        //Teha update meethod, mis v]tab vastu dto ja uuendab olemasolevat kosmoselaeva
        public async Task<Spaceship> Update(SpaceshipDto dto)
        {
            // Load existing entity so we preserve fields like CreatedAt when updating
            var existing = await _context.Spaceships.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (existing == null)
            {
                return null;
            }

            existing.Name = dto.Name;
            existing.ShipType = dto.ShipType;
            existing.Crew = dto.Crew;
            existing.EnginePower = dto.EnginePower;
            // preserve CreatedAt if not provided in dto
            existing.CreatedAt = dto.CreatedAt ?? existing.CreatedAt;
            existing.UpdatedAt = dto.UpdatedAt ?? DateTime.Now;

            //andmete salvestamine andmebaasi
            _context.Spaceships.Update(existing);
            await _context.SaveChangesAsync();

            return existing;
        }
        public async Task<Spaceship> DetailAsync(Guid id)
        {
            var spaceship = await _context.Spaceships
                .FirstOrDefaultAsync(x => x.Id == id);

            return spaceship;
        }
        public async Task<Spaceship> Delete(Guid id)
        {
            var result = await _context.Spaceships
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Spaceships.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
        public async Task<Spaceship> Details(Guid id)
        {
            var result = await _context.Spaceships
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }
    }
}
