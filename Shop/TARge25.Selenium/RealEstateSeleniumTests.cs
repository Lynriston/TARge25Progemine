using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TARge25.Selenium
{
    [TestCaseOrderer("TARge25.Selenium.PriorityOrderer", "TARge25.Selenium")]
    public class RealEstateSeleniumTests
    {
        public static string? thisRunGuid { get; set; }

        //Loob Uue Real Estate ning kontrollib kas andmed on õiged
        [Fact]
        [Order(5)]
        public void Should_NavigateToCreate_AddNewRealEstate_WithCorrectData_ReturnToIndex()
        {
            IWebDriver driver = SetupAndNavigateRIndex();

            IWebElement createInIndex = driver.FindElement(By.Id("CreateInIndex"));
            createInIndex.Click();

            string concreteName = InsertRealEstateData_ReturnName(driver, false);
            thisRunGuid = concreteName;

            IWebElement cu_CreateRealEstate = driver.FindElement(By.Id("CU_CreateRealEstate"));
            cu_CreateRealEstate.Click();

            Thread.Sleep(500);

            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            string realEstateNameData = "";
            string realEstateAreaData = "";
            string realEstateLocationData = "";
            string realEstateRoomNumberData = "";
            string realEstateBuildingTypeData = "";
            bool doesItExist = false;
            foreach (var row in rowsOfTable)
            {
                if (row.FindElement(By.Id("IndexNameRealEstate")).Text == thisRunGuid)
                {
                    doesItExist = true;
                    realEstateNameData = row.FindElement(By.Id("IndexNameRealEstate")).Text;
                    realEstateAreaData = row.FindElement(By.Id("IndexAreaRealEstate")).Text;
                    realEstateLocationData = row.FindElement(By.Id("IndexLocationRealEstate")).Text;
                    realEstateRoomNumberData = row.FindElement(By.Id("IndexRoomNumberRealEstate")).Text;
                    realEstateBuildingTypeData = row.FindElement(By.Id("IndexBuildingTypeRealEstate")).Text;
                    break;
                }
            }
            driver.Quit();

            Assert.True(true == doesItExist);
            Assert.Equal(thisRunGuid, realEstateNameData);
            Assert.True(thisRunGuid == realEstateNameData);
            Assert.True("13" == realEstateAreaData);
            Assert.Equal("13", realEstateAreaData);
            Assert.True("Current Location" == realEstateLocationData);
            Assert.True("House" == realEstateBuildingTypeData);
        }

        [Fact]
        [Order(6)]
        public void Should_NavigateToDetails_OfThisRealEstate_WithPreviouslyCorrectData()
        {
            IWebDriver driver = SetupAndNavigateRIndex();

            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool isObjectFound = false;

            foreach (var row in rows)
            {
                var locatedElement = row.FindElement(By.Id("IndexNameRealEstate"));
                if (locatedElement.Text == thisRunGuid)
                {
                    isObjectFound = true;
                    var rowelement = row.FindElement(By.Id("IndexActionRealEstate"));
                    var button = rowelement.FindElement(By.Id("IndexRealEstateDetails"));
                    button.Click();

                    //Thread.Sleep(500);
                    break;
                }
            }
            IWebElement details_RealEstateId = driver.FindElement(By.Id("Details_REId"));
            var details_id = details_RealEstateId.Text;

            IWebElement details_RealEstateName = driver.FindElement(By.Id("Details_REName"));
            var details_Name = details_RealEstateName.Text;

            IWebElement details_RealEstateArea = driver.FindElement(By.Id("Details_REArea"));
            var details_Area = details_RealEstateArea.Text;

            IWebElement details_RealEstateLocation = driver.FindElement(By.Id("Details_RELocation"));
            var details_Location = details_RealEstateLocation.Text;

            IWebElement details_RealEstateRoomNumber = driver.FindElement(By.Id("Details_RERoomNr"));
            var details_roomNr = details_RealEstateRoomNumber.Text;
            
            IWebElement details_RealEstateBuildingType = driver.FindElement(By.Id("Details_REBuildType"));
            var details_buildingType = details_RealEstateBuildingType.Text;

            driver.Quit();

            Assert.True(isObjectFound);
            Assert.NotNull(details_id);
            Assert.NotEmpty(details_Name);
            Assert.NotNull(details_roomNr);
            Assert.True(!string.IsNullOrEmpty(details_roomNr));
            Assert.Equal(thisRunGuid, details_Name);
            Assert.Equal("13", details_Area);
            Assert.Equal("22", details_roomNr);
            Assert.Equal("House", details_buildingType);
            Assert.True(int.Parse(details_roomNr).GetType() == typeof(int));
        }

        [Fact]
        [Order(7)]
        public void Should_UpdateDetails_OfThisRealEstate_WithNewData()
        {
            IWebDriver driver = SetupAndNavigateRIndex();

            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool doesItExist = false;

            foreach (var row in rows)
            {
                var locatedElement = row.FindElement(By.Id("IndexNameRealEstate"));
                if (locatedElement.Text == thisRunGuid)
                {
                    doesItExist = true;
                    var rowElement = row.FindElement(By.Id("IndexActionRealEstate"));
                    var button = rowElement.FindElement(By.Id("IndexRealEstateUpdate"));
                    button.Click();
                    //Thread.Sleep(500);
                    break;
                }
            }
            InsertRealEstateData(driver, thisRunGuid, true);

            IWebElement cu_CreatRealEstate = driver.FindElement(By.Id("CU_UpdateRealEstate"));
            cu_CreatRealEstate.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var tableOfIndex = wait.Until(d => d.FindElement(By.Id("IndexTable")));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            string realEstateNameData = "";
            string realEstateAreaData = "";
            string realEstateLocationData = "";
            string realEstateRoomNumberData = "";
            string realEstateBuildingTypeData = "";
            doesItExist = false;
            foreach (var row in rowsOfTable)
            {
                if (row.FindElement(By.Id("IndexNameRealEstate")).Text == thisRunGuid)
                {
                    doesItExist = true;
                    realEstateNameData = thisRunGuid;
                    realEstateAreaData = row.FindElement(By.Id("IndexAreaRealEstate")).Text;
                    realEstateLocationData = row.FindElement(By.Id("IndexLocationRealEstate")).Text;
                    realEstateRoomNumberData = row.FindElement(By.Id("IndexRoomNumberRealEstate")).Text;
                    realEstateBuildingTypeData = row.FindElement(By.Id("IndexBuildingTypeRealEstate")).Text;
                    break;
                }
            }
            driver.Quit();

            Assert.True(doesItExist);
            Assert.NotNull(realEstateNameData);
            Assert.Equal(thisRunGuid, realEstateNameData);
            Assert.False(string.IsNullOrEmpty(realEstateAreaData));
            Assert.NotEqual("Current Location", realEstateLocationData);
            Assert.True(realEstateRoomNumberData == "13");
        }

        [Fact]
        [Order(8)]
        public void Should_DeleteThisRealEstate_FromDbByDeleteView()
        {
            IWebDriver driver = SetupAndNavigateRIndex();

            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool doesItExist = false;

            foreach (var row in rows)
            {
                var locatedElement = row.FindElement(By.Id("IndexNameRealEstate"));
                if (locatedElement.Text == thisRunGuid)
                {
                    doesItExist = true;
                    var rowElement = row.FindElement(By.Id("IndexActionRealEstate"));
                    var button = rowElement.FindElement(By.Id("IndexRealEstateDelete"));
                    button.Click();
                    //Thread.Sleep(500);
                    break;
                }
            }
            var clickBtn = driver.FindElement(By.Id("delete_confirm"));
            clickBtn.Click();
            bool isDeleted = false;
            table = driver.FindElement(By.Id("IndexTable"));
            rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            foreach (var row in rows)
            {
                var elementName = row.FindElement(By.Id("IndexNameRealEstate")).Text;
                if (elementName == thisRunGuid)
                {
                    isDeleted = false;
                    break;
                }
                else
                {
                    isDeleted = true;
                }
            }
            driver.Quit();

            Assert.True(isDeleted);
            Assert.False(!isDeleted);
        }

        private static IWebDriver SetupAndNavigateRIndex()
        {
            //firefoxi käskiv ja juhtiv draiver
            IWebDriver driver = new FirefoxDriver();
            //aadress millele draiver navigeerib
            driver.Url = "https://localhost:7232/";
            //lehelt otsitav element
            IWebElement navigateToRealEstate = driver.FindElement(By.LinkText("Real Estate"));
            //selle elemendiga tehtav tegevus
            navigateToRealEstate.Click();
            return driver;
        }


        private void InsertRealEstateData(IWebDriver driver, string name, bool isOneOrTwo = false)
        {

            if (isOneOrTwo)
            {
                IWebElement cu_NameEntryRealEstate = driver.FindElement(By.Id("CU_NameEntryRealEstate"));
                cu_NameEntryRealEstate.Clear();
                if (name != null)
                {
                    cu_NameEntryRealEstate.SendKeys(name);
                }
                else
                {
                    cu_NameEntryRealEstate.SendKeys("Name for Real Estate");
                }
                IWebElement cu_AreaEntryRealEstate = driver.FindElement(By.Id("CU_AreaEntryRealEstate"));
                cu_AreaEntryRealEstate.Clear();
                cu_AreaEntryRealEstate.SendKeys("314");
                IWebElement cu_LocationEntryRealEstate = driver.FindElement(By.Id("CU_LocationEntryRealEstate"));
                cu_LocationEntryRealEstate.Clear();
                cu_LocationEntryRealEstate.SendKeys("Random Location");
                IWebElement cu_RoomNumberEntryRealEstate = driver.FindElement(By.Id("CU_RoomNumberEntryRealEstate"));
                cu_RoomNumberEntryRealEstate.Clear();
                cu_RoomNumberEntryRealEstate.SendKeys("13");
                IWebElement cu_BuildingTypeEntryRealEstate = driver.FindElement(By.Id("CU_BuildingTypeEntryRealEstate"));
                cu_BuildingTypeEntryRealEstate.Clear();
                cu_BuildingTypeEntryRealEstate.SendKeys("Apartment");
            }
            else
            {
                IWebElement cu_NameEntryRealEstate = driver.FindElement(By.Id("CU_NameEntryRealEstate"));
                cu_NameEntryRealEstate.Clear();
                if (name != null)
                {
                    cu_NameEntryRealEstate.SendKeys(name);
                }
                else
                {
                    cu_NameEntryRealEstate.SendKeys("Another Name for Real Estate");
                }
                IWebElement cu_AreaEntryRealEstate = driver.FindElement(By.Id("CU_AreaEntryRealEstate"));
                cu_AreaEntryRealEstate.Clear();
                cu_AreaEntryRealEstate.SendKeys("13");
                IWebElement cu_LocationEntryRealEstate = driver.FindElement(By.Id("CU_LocationEntryRealEstate"));
                cu_LocationEntryRealEstate.Clear();
                cu_LocationEntryRealEstate.SendKeys("Current Location");
                IWebElement cu_RoomNumberEntryRealEstate = driver.FindElement(By.Id("CU_RoomNumberEntryRealEstate"));
                cu_RoomNumberEntryRealEstate.Clear();
                cu_RoomNumberEntryRealEstate.SendKeys("22");
                IWebElement cu_BuildingTypeEntryRealEstate = driver.FindElement(By.Id("CU_BuildingTypeEntryRealEstate"));
                cu_BuildingTypeEntryRealEstate.Clear();
                cu_BuildingTypeEntryRealEstate.SendKeys("House");
            }
        }

        private string InsertRealEstateData_ReturnName(IWebDriver driver, bool isOneOrTwo = false)
        {
            Guid returnGuid = Guid.NewGuid();
            if (isOneOrTwo)
            {
                IWebElement cu_NameEntryRealEstate = driver.FindElement(By.Id("CU_NameEntryRealEstate"));
                cu_NameEntryRealEstate.Clear();
                cu_NameEntryRealEstate.SendKeys(returnGuid.ToString());
                IWebElement cu_AreaEntryRealEstate = driver.FindElement(By.Id("CU_AreaEntryRealEstate"));
                cu_AreaEntryRealEstate.Clear();
                cu_AreaEntryRealEstate.SendKeys("314");
                IWebElement cu_LocationEntryRealEstate = driver.FindElement(By.Id("CU_LocationEntryRealEstate"));
                cu_LocationEntryRealEstate.Clear();
                cu_LocationEntryRealEstate.SendKeys("Random Location");
                IWebElement cu_RoomNumberEntryRealEstate = driver.FindElement(By.Id("CU_RoomNumberEntryRealEstate"));
                cu_RoomNumberEntryRealEstate.Clear();
                cu_RoomNumberEntryRealEstate.SendKeys("13");
                IWebElement cu_BuildingTypeEntryRealEstate = driver.FindElement(By.Id("CU_BuildingTypeEntryRealEstate"));
                cu_BuildingTypeEntryRealEstate.Clear();
                cu_BuildingTypeEntryRealEstate.SendKeys("Apartment");
            }
            else
            {
                IWebElement cu_NameEntryRealEstate = driver.FindElement(By.Id("CU_NameEntryRealEstate"));
                cu_NameEntryRealEstate.Clear();
                cu_NameEntryRealEstate.SendKeys(returnGuid.ToString());
                IWebElement cu_AreaEntryRealEstate = driver.FindElement(By.Id("CU_AreaEntryRealEstate"));
                cu_AreaEntryRealEstate.Clear();
                cu_AreaEntryRealEstate.SendKeys("13");
                IWebElement cu_LocationEntryRealEstate = driver.FindElement(By.Id("CU_LocationEntryRealEstate"));
                cu_LocationEntryRealEstate.Clear();
                cu_LocationEntryRealEstate.SendKeys("Current Location");
                IWebElement cu_RoomNumberEntryRealEstate = driver.FindElement(By.Id("CU_RoomNumberEntryRealEstate"));
                cu_RoomNumberEntryRealEstate.Clear();
                cu_RoomNumberEntryRealEstate.SendKeys("22");
                IWebElement cu_BuildingTypeEntryRealEstate = driver.FindElement(By.Id("CU_BuildingTypeEntryRealEstate"));
                cu_BuildingTypeEntryRealEstate.Clear();
                cu_BuildingTypeEntryRealEstate.SendKeys("House");
            }
            return returnGuid.ToString();
        }
    }
}
