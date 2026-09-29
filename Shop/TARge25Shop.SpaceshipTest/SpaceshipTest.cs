using System;
using System.Collections.Generic;
using System.Text;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using Xunit;

namespace TARge25Shop.SpaceshipTest
{
    public class SpaceshipTest : TestBase
    {

        [Fact]//Fact t'histab 'ra [hte testi xUnit raamistikus
        //ShouldNot kirjeldab öra kas test on tavaline või negatiivne
        //AddEmptySpaceShip kirjeldab ära mida parasjagu üritatakse testialuse objektiga teha
        //WhenResultIsReturned kirjeldab mis tingimusel tulemust kontrollitakse, peale tegevust
        //Selles testis kontrollitakse et Kosmoselaeva lisamisel 
        //ei tohiks saadud tulemus olla tühi. Jälgi seda sõnastusviisi
        public async Task ShouldNot_AddEmptySpaceShip_WhenResultIsReturned()
        {
            //Ülesseade 
            SpaceshipDto dto = new SpaceshipDto()
            {
                Name = "X AE a L 12 wfoaewfo",
                ShipType = "Lendav taldrik",
                Crew = 666,
                EnginePower = 69,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            //Tegutsemine
            var result = await Svc<ISpaceshipServices>().Create(dto);

            //Kontroll
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldNot_GetSpaceShipByID_WhenIDNotEqual()
        {
            // Ülesseade
            Guid wrongGuid = Guid.NewGuid();
            Guid goodGuid = Guid.Parse("a6c5a2e3-20a9-4956-915e-d289b8f5e15b");

            //Tegevus
            await Svc<ISpaceshipServices>().DetailAsync(goodGuid);

            //Kontroll
            Assert.NotEqual(wrongGuid, goodGuid);
        }
        //Seleta kodus lahti, nagu eelnevate testide laused, eesti keelde, selle testi oma ka...
        [Fact]
        public async Task Should_GetSpaceshipByID_WhenGuidIsEqual()
        {
            //Ülesseade
            Guid databaseGuid = Guid.Parse("a6c5a2e3-20a9-4956-915e-d289b8f5e15b");
            Guid seekGuid = Guid.Parse("a6c5a2e3-20a9-4956-915e-d289b8f5e15b");

            //Tegevus
            await Svc<ISpaceshipServices>().DetailAsync(seekGuid);

            Assert.Equal(databaseGuid, seekGuid);
        }

        [Fact]
        public async Task Should_SpaceshipDeletedById_WhenReturnedResultIsEqual()
        {
            //Ülesseade
            SpaceshipDto dto = MockSpaceshipData();

            //Tegevus
            var addSpaceship = await Svc<ISpaceshipServices>().Create(dto);
            var deleteSpaceship = await Svc<ISpaceshipServices>().Delete((Guid)addSpaceship.Id);

            //Kontroll
            Assert.Equal(addSpaceship.Id, deleteSpaceship.Id);
        }

        [Fact]
        public async Task ShouldNot_DeleteSpaceshipById_WhenDidNotDeleteSpaceship()
        {
            //Ülesseade
            var dto = MockSpaceshipData();

            //Tegevus
            var spaceShip1 = await Svc<ISpaceshipServices>().Create(dto);
            var spaceShip2 = await Svc<ISpaceshipServices>().Create(dto);

            var result = await Svc<ISpaceshipServices>().Delete((Guid)spaceShip2.Id);

            //Kontroll
            Assert.NotEqual(spaceShip1.Id, result.Id)
        }

        /* ]leval testid all abimeetodid */

        private SpaceshipDto MockSpaceshipData(bool isOneOrTwo = false)
        {
            if (isOneOrTwo == false)
            {
                return new SpaceshipDto
                {
                    Name = "X AE a L 12 wfoaewfo",
                    ShipType = "Lendav taldrik",
                    Crew = 666,
                    EnginePower = 69,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
            }
            else
            {
                return new SpaceshipDto
                {
                    Name = "Rakett69",
                    ShipType = "Lendav kauss",
                    Crew = 420,
                    EnginePower = 007,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

            }
            
        }
    }
}
