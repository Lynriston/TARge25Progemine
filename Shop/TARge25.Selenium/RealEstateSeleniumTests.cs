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
                IWebElement cu_AreaEntryRealEstate = driver.FindElement(By.Id("CU_AreaEntryRealEstate"));
                cu_AreaEntryRealEstate.Clear();
                if (name != null)
                {
                    cu_AreaEntryRealEstate.SendKeys(name);
                }
                else
                {
                    cu_AreaEntryRealEstate.SendKeys("314");
                }
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
                IWebElement cu_AreaEntryRealEstate = driver.FindElement(By.Id("CU_AreaEntryRealEstate"));
                cu_AreaEntryRealEstate.Clear();
                if (name != null)
                {
                    cu_AreaEntryRealEstate.SendKeys(name);
                }
                else
                {
                    cu_AreaEntryRealEstate.SendKeys("13");
                }
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
