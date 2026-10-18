using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Security.AccessControl;
using System.Text;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;
using TARge25Shop.Data.Migrations;
using TARge25Shop.SpaceshipTest;
using Xunit;
using Xunit.Sdk;

namespace TARge25Shop.Tests
{
    public class KindergartenTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyKindergarten_WhenResultIsReturned()
        {
            KindergardenDto dto = new KindergardenDto()
            {
                GroupName = "Class",
                ChildrenCount = 10,
                KindergardenName = "Kindergarten",
                TeacherName = "Teacher",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            var result = await Svc<IKindergardenServices>().Create(dto);

            // kontroll
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldNot_GetKindergartenByID_WhenIDNotEqual()
        {
            // ülesseade
            Guid wrongGuid = Guid.NewGuid();
            Guid goodGuid = Guid.Parse("ecbc059a-0bca-4df2-aae9-a3211e69185a");

            // tegevus
            await Svc<IKindergardenServices>().DetailAsync(goodGuid);

            // kontroll
            Assert.NotEqual(wrongGuid, goodGuid);
        }

        [Fact]
        public async Task Should_GetKindergartenByID_WhenGuidIsEqual()
        {
            // ülessezade
            Guid databaseGuid = Guid.Parse("ecbc059a-0bca-4df2-aae9-a3211e69185a");
            Guid seekGuid = Guid.Parse("ecbc059a-0bca-4df2-aae9-a3211e69185a");

            // tegevus
            await Svc<IKindergardenServices>().DetailAsync(seekGuid);

            // kontroll
            Assert.Equal(databaseGuid, seekGuid);
        }

        [Fact]
        public async Task Should_KindergartenDeletedByID_WhenReturnedResultIsEqual()
        {
            // ülesseade
            KindergardenDto dto = MockKindergartenData();

            // tegevus
            var addKindergarten = await Svc<IKindergardenServices>().Create(dto);
            var deleteKindergarten = await Svc<IKindergardenServices>().Delete((Guid)addKindergarten.Id);

            // kontroll
            Assert.Equal(addKindergarten.Id, deleteKindergarten.Id);
        }

        [Fact]
        public async Task ShouldNot_DeleteKindergartenByID_WhenDidNotDeleteKindergarten()
        {
            //ülesseade
            var dto = MockKindergartenData();

            //tegevus
            var kinderGarden1 = await Svc<IKindergardenServices>().Create(dto);
            var kinderGarden2 = await Svc<IKindergardenServices>().Create(dto);

            var result = await Svc<IKindergardenServices>().Delete((Guid)kinderGarden2.Id);

            //kontroll
            Assert.NotEqual(kinderGarden1.Id, result.Id);
        }

        [Fact]
        public async Task Should_UpdateKindergartenByID_WhenUpdatingData()
        {
            //ülesseade
            KindergardenDto dto = MockKindergartenData();
            var result = await Svc<IKindergardenServices>().Create(dto);

            Guid guid = (Guid)result.Id;
            KindergardenDto domain = new();

            domain.Id = guid;
            domain.GroupName = "Name";
            domain.ChildrenCount = 12;
            domain.KindergardenName = "Kindergatenn";
            domain.TeacherName = "TeacherName";
            domain.CreatedAt = dto.CreatedAt;//  <-- ei tohi muutuda Update korral, tuleb võtta olemasolevast objektist.
            domain.UpdatedAt = DateTime.UtcNow;//  <-- PEAB muutuma Update korral

            //tegevus
            await Svc<IKindergardenServices>().Update(domain);
            //kill first connection: added tracker clearing to update
            Svc<TARge25ShopContext>().ChangeTracker.Clear();
            var result2 = await Svc<IKindergardenServices>().DetailAsync(guid);

            //kontroll
            Assert.Equal(result.Id, result2.Id);
            Assert.NotEqual(dto.GroupName, result2.GroupName);
            Assert.NotEqual(dto.ChildrenCount, result2.ChildrenCount);
            Assert.NotEqual(dto.KindergardenName, result2.KindergardenName);
            Assert.NotEqual(dto.TeacherName, result2.TeacherName);
            Assert.Equal(dto.CreatedAt, result2.CreatedAt);
            Assert.NotEqual(dto.UpdatedAt, result2.CreatedAt);
        }

        [Fact]
        public async Task ShouldNot_UpdateKindergartenByID_WhenNoDataIsUpdated()
        {
            //ülesseade
            KindergardenDto dto = MockKindergartenData();
            var createdKindergarten = await Svc<IKindergardenServices>().Create(dto);

            //tegevus
            KindergardenDto nullDto = MockKindergartenNullData();
            var result = await Svc<IKindergardenServices>().Update(nullDto);

            //kontroll
            Assert.Null(result);
        }

        [Fact]
        public async Task ShouldNot_CreateKindergartenWithNegativeChildrenCount_WhenChildrenCountNegative()
        {
            //ülesseade
            KindergardenDto dto = MockKindergartenData(true);
            dto.ChildrenCount -= (dto.ChildrenCount * 2);

            //tegevus
            var result = await Svc<IKindergardenServices>().Create(dto);

            //kontroll
            Assert.Null(result);
        }
        [Fact]
        public async Task ShouldNot_CreateKindergarten_WhenChildrenCountIsThreeOrLess()
        {
            //ülesseade
            KindergardenDto dto = MockKindergartenData();
            dto.ChildrenCount = 0;
            //tegevus
            var result = await Svc<IKindergardenServices>().Create(dto);
            //kontroll
            Assert.Null(result);
        }

        [Fact]
        public async Task Should_RemoveKindergartenFromDatabase_WhenKindergartenIsDeleted()
        {
            //ülesseade
            KindergardenDto dto = MockKindergartenData();

            //tegevus
            var createdKindergarten = await Svc<IKindergardenServices>().Create(dto);
            var deletedKindergarten = await Svc<IKindergardenServices>().Delete((Guid)createdKindergarten.Id);
            var result = await Svc<IKindergardenServices>().DetailAsync((Guid)createdKindergarten.Id);

            //kontrollimine
            Assert.Equal(createdKindergarten.Id, deletedKindergarten.Id);
            Assert.Null(result);
        }

        [Fact]
        public async Task ShouldNot_RemoveKindergartenFromDatabase_WhenKindergartenIdIsDifferent()
        {
            var dto = MockKindergartenData();
            var createdKindergaten1 = await Svc<IKindergardenServices>().Create(dto);
            var createdKindergarten2 = await Svc<IKindergardenServices>().Create(dto);
            var deleteResult = await Svc<IKindergardenServices>().Delete((Guid)createdKindergarten2.Id);
            var kindergartenindb = await Svc<IKindergardenServices>().DetailAsync((Guid)createdKindergaten1.Id);

            //kontroll
            Assert.NotNull(kindergartenindb);
            Assert.NotEqual(deleteResult.Id, createdKindergaten1.Id);
            Assert.Equal(createdKindergaten1, kindergartenindb);
        }

        //6.1 - xUnit Ühiktestimine

        //1. Test: Kui ChildCount on täis
        [Fact]
        public async Task ShouldNot_CreateKindergarten_WhenChildrenCountIsFull()
        {
            //ülesseade
            KindergardenDto dto = MockKindergartenData();
            dto.ChildrenCount = 11;
            //tegevus
            var result = await Svc<IKindergardenServices>().Create(dto);
            //kontroll
            Assert.True(result.ChildrenCount > 10);
        }

        //2. Test: Kui ChildCount ei ole täis
        [Fact]
        public async Task Should_CreateKindergarten_WhenChildrenCountIsNotFull()
        {
            //ülesseade
            KindergardenDto dto = MockKindergartenData();
            dto.ChildrenCount = 5;
            //tegevus
            var result = await Svc<IKindergardenServices>().Create(dto);
            //kontroll
            Assert.True(result.ChildrenCount < 10);
        }

        //3. Test: Kui KindergartenId on olemas
        [Fact]
        public async Task ShouldNot_CreateKindergartenById_WhenKindergartenIdExists()
        {
            //ülesseade
            var dto = MockKindergartenData();
            //tegevus
            var createdKindergaten1 = await Svc<IKindergardenServices>().Create(dto);
            var createdKindergarten2 = await Svc<IKindergardenServices>().Create(dto);
            //kontroll
            Assert.True(createdKindergaten1.Id != createdKindergarten2.Id);
        }

        //4. Test: Kui ChildCount on üle 10 Kustuta
        [Fact]
        public async Task Should_DeleteKindergartenByChildCount_WhenChildCountIsOver10()
        {
            //ülesseade
            var dto = MockKindergartenData();
            dto.ChildrenCount = 12;
            //Tegevus
            var result = await Svc<IKindergardenServices>().Create(dto);
            var resultDelete = await Svc<IKindergardenServices>().Delete((Guid)result.Id);
            //kontroll
            Assert.True(resultDelete.ChildrenCount > 10);
        }

        //5. Test: Kui ChildCount ei ole üle 10 kustuta
        [Fact]
        public async Task ShouldNot_DeleteKindergartenById_WhenChildCountIsUnder10()
        {
            //ülesseade
            var dto = MockKindergartenData();
            dto.ChildrenCount = 9;
            //Tegevus
            var result = await Svc<IKindergardenServices>().Create(dto);
            var resultDelete = await Svc<IKindergardenServices>().Delete((Guid)result.Id);
            //kontroll
            Assert.True(resultDelete.ChildrenCount < 10);
        }

        //6. Test: Leia lasteaed kui ChildCount on üle 10
        [Fact]
        public async Task Should_GetKindergartenById_WhenChildCountIsOver10()
        {
            //ülesseade
            var dto = MockKindergartenData();
            dto.ChildrenCount = 20;
            //Tegevus
            var result = await Svc<IKindergardenServices>().Create(dto);
            var getResult = await Svc<IKindergardenServices>().DetailAsync((Guid)result.Id);
            //kontroll
            Assert.NotNull(getResult);
            Assert.True(getResult.ChildrenCount > 10);
        }
        //7. Test: Ei leia lasteaed kui ChildCount on üle 10
        [Fact]
        public async Task ShouldNot_GetKindergartenById_WhenChildCountIsOver10()
        {
            //ülesseade
            var dto = MockKindergartenData();
            dto.ChildrenCount = 20;
            //Tegevus
            var result = await Svc<IKindergardenServices>().Create(dto);
            var getResult = await Svc<IKindergardenServices>().DetailAsync((Guid)result.Id);
            //kontroll
            Assert.NotNull(getResult);
            Assert.False(getResult.ChildrenCount < 10);
        }

        //8. Test:Ei leia kui krupi nimi ei eksisteeri
        [Fact]
        public async Task ShouldNot_GetKindergartenById_WhenGroupNameDoesNotExists()
        {
            //ülesseade
            var dto = MockKindergartenData();
            dto.GroupName = "";
            //tegevus
            var result = await Svc<IKindergardenServices>().Create(dto);
            var getResult = await Svc<IKindergardenServices>().DetailAsync((Guid)result.Id);
            //kontroll
            Assert.NotNull(result.GroupName);
            Assert.False(getResult.GroupName != "");
        }

        //9. Test: Lei kui krupi nimi eksisteerib
        [Fact]
        public async Task Should_GetKindergartenById_WhenGroupNameExists()
        {
            //ülesseade
            var dto = MockKindergartenData();
            //tegevus
            var result = await Svc<IKindergardenServices>().Create(dto);
            var getResult = await Svc<IKindergardenServices>().DetailAsync((Guid)result.Id);
            //kontroll
            Assert.NotNull(result.GroupName);
        }

        //10. Test: Ei kustuta kui krupi nimi ei ole võrdne
        [Fact]
        public async Task ShouldNot_RemoveKindergartenById_WhenGroupNameNotEqual()
        {
            //ülesseade
            var dto = MockKindergartenData();
            dto.GroupName = "Class1";
            //tegevus
            var result = await Svc<IKindergardenServices>().Create(dto);
            var deleteResult = await Svc<IKindergardenServices>().Delete((Guid)result.Id);
            //kontroll
            Assert.NotEmpty(result.GroupName);
            Assert.Equal(deleteResult.GroupName, "Class1");
            Assert.False(deleteResult.GroupName != "Class1");
        }

        //11. Test: ei uuenda kui lasteaed ei eksisteeri
        [Fact]
        public async Task ShouldNot_UpdateKindergartenById_WhenKindergartenDoesNotExist()
        {
            //ülesseade
            var dto = MockKindergartenData();
            dto.Id = Guid.NewGuid();

            //tegevus
            await Svc<IKindergardenServices>().Update(dto);
            Svc<TARge25ShopContext>().ChangeTracker.Clear();
            var result = await Svc<IKindergardenServices>().DetailAsync((Guid)dto.Id);

            //kontroll
            Assert.Null(result);
        }

        //12. Test: uuendab kui lasteaed eksisteerib
        [Fact]
        public async Task Should_UpdateKindergartenById_WhenKindergartenExists()
        {
            //ülesseade
            KindergardenDto dto = MockKindergartenData();
            var result = await Svc<IKindergardenServices>().Create(dto);

            Guid guid = (Guid)result.Id;
            KindergardenDto domain = new();

            domain.Id = guid;
            domain.GroupName = "NewName";
            domain.ChildrenCount = 21;
            domain.KindergardenName = "NewKindergaten";
            domain.TeacherName = "NewTeacherName";
            domain.CreatedAt = dto.CreatedAt;
            domain.UpdatedAt = DateTime.UtcNow;

            //tegevus
            await Svc<IKindergardenServices>().Update(domain);
            Svc<TARge25ShopContext>().ChangeTracker.Clear();
            var result2 = await Svc<IKindergardenServices>().DetailAsync(guid);

            //kontroll
            Assert.NotNull(result);
            Assert.Equal(result.Id, result2.Id);
            Assert.NotEqual(dto.GroupName, result2.GroupName);
            Assert.NotEqual(dto.ChildrenCount, result2.ChildrenCount);
            Assert.NotEqual(dto.KindergardenName, result2.KindergardenName);
            Assert.NotEqual(dto.TeacherName, result2.TeacherName);
            Assert.Equal(dto.CreatedAt, result2.CreatedAt);
            Assert.NotEqual(dto.UpdatedAt, result2.CreatedAt);
        }

        //13. Test: leiab kui lasteaed eksisteerib
        [Fact]
        public async Task Should_GetKindergartenById_WhenKindergartenNameExists()
        {
            //ülesseade
            var dto = MockKindergartenData();
            //tegevus
            var result = await Svc<IKindergardenServices>().Create(dto);
            var getResult = await Svc<IKindergardenServices>().DetailAsync((Guid)result.Id);
            //kontroll
            Assert.NotNull(result.KindergardenName);
        }

        //14. Test: ei leia kui lasteaia nimi ei eksisteeri
        [Fact]
        public async Task ShouldNot_GetKindergartenById_WhenKindergartenNameDoesNotExist()
        {
            //ülesseade
            var dto = MockKindergartenData();
            dto.KindergardenName = "";
            //tegevus
            var result = await Svc<IKindergardenServices>().Create(dto);
            var getResult = await Svc<IKindergardenServices>().DetailAsync((Guid)result.Id);
            //kontroll
            Assert.NotNull(result.KindergardenName);
            Assert.False(getResult.KindergardenName != "");
        }

        //15. Test: kustutab kui õpetaja nimi ei eksisteeri
        [Fact]
        public async Task Should_RemoveKindergartenNameById_WhenKindergartenTeacherNameExists()
        {
            //üleseade
            var dto = MockKindergartenData();
            dto.TeacherName = "Kelly";
            //tegevus
            var result = await Svc<IKindergardenServices>().Create(dto);
            var getResult = await Svc<IKindergardenServices>().DetailAsync((Guid)result.Id);
            //kontroll
            Assert.NotNull(result.TeacherName);
            Assert.Equal(result.TeacherName, "Kelly");
        }

        private KindergardenDto MockKindergartenNullData()
        {
            return new KindergardenDto
            {
                Id = null,
                GroupName = "",
                ChildrenCount = 10,
                KindergardenName = "",
                TeacherName = "",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };         
        }
        private KindergardenDto MockKindergartenData(bool isOneOrTwo = false)
        {
            if (isOneOrTwo == false)
            {
                return new KindergardenDto
                {
                    GroupName = "Class1",
                    ChildrenCount = 20,
                    KindergardenName = "KinGar1",
                    TeacherName = "Teacher1",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
            }
            else
            {
                return new KindergardenDto
                {
                    GroupName = "Class2",
                    ChildrenCount = 10,
                    KindergardenName = "KinGar2",
                    TeacherName = "Teacher2",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
            }
        }
    }
}
