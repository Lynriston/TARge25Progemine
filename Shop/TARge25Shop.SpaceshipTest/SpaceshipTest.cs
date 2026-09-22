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
    }
}
