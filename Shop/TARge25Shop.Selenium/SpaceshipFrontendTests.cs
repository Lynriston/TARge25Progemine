using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TARge25Shop.Selenium
{
    public class OrderAttribute : Attribute
    {
        public int Value { get; }
        public OrderAttribute(int value)
        {
            Value = value;
        }
    }

    public class PriorityOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
            where TTestCase : ITestCase
        {
            return testCases.OrderBy(test =>
            {
                var method = test.TestMethod.Method.GetCustomAttributes(typeof(OrderAttribute)).FirstOrDefault();
                if (method == null)
                {
                    return int.MaxValue;
                }
                else
                {
                    return method.GetNamedArgument<int>(nameof(OrderAttribute.Value));
                }
            }).ToList();
        }
    }

    [TestCaseOrderer("TARge25Shop.Selenium.PriorityOrderer", "TARge25Shop.Selenium")]
    public class SpaceshipFrontendTests : IDisposable
    {
        private readonly IWebDriver _driver;

        public SpaceshipFrontendTests()
        {
            // Initialize driver once for the whole test class workflow
            _driver = new FirefoxDriver();
            NavigateToSIndex();
        }

        private void NavigateToSIndex()
        {
            _driver.Url = "https://127.0.0.1:7232/";
            var navigateToSpaceship = _driver.FindElement(By.LinkText("Spaceship"));
            navigateToSpaceship.Click();
        }

        [Fact]
        [Order(1)]
        public void Should_NavigateToCreate_AddSpaceShip_WithCorrectData_ReturnToIndex()
        {
            IWebElement createInIndex = _driver.FindElement(By.Id("CreateInIndex"));
            createInIndex.Click();

            InsertSpaceShipData(_driver, false);

            IWebElement cu_CreateSpaceship = _driver.FindElement(By.Id("CU_CreateSpaceship"));
            cu_CreateSpaceship.Click();

            IWebElement indexNameSpaceship = _driver.FindElement(By.Id("IndexNameSpaceship"));
            var spaceShipNameData = indexNameSpaceship.Text;
            IWebElement indexTypeSpaceship = _driver.FindElement(By.Id("IndexTypeSpaceship"));
            var spaceShipTypeData = indexTypeSpaceship.Text;
            IWebElement indexCrewSpaceship = _driver.FindElement(By.Id("IndexCrewSpaceship"));
            var spaceShipCrewData = indexCrewSpaceship.Text;

            Assert.Equal("i add name for spaceship", spaceShipNameData);
            Assert.True(spaceShipTypeData == "i add ship type for spaceship");
            Assert.Equal("12345", spaceShipCrewData);
        }

        [Fact]
        [Order(2)]
        public void Should_NavigateToDetails_OfASpaceShip_WithPreviouslyCorrectData()
        {
            // Ensure we are back on the index page if needed
            NavigateToSIndex();

            var table = _driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);

            foreach (var row in rows)
            {
                var locatedelement = row.FindElement(By.Id("IndexNameSpaceship"));
                if (locatedelement.Text == "i add name for spaceship")
                {
                    var rowelement = row.FindElement(By.Id("IndexActionsSpaceship"));
                    var button = rowelement.FindElement(By.Id("IndexSpaceshipDetails"));
                    button.Click();
                    Thread.Sleep(500);
                    break;
                }
            }

            IWebElement details_SpaceshipId = _driver.FindElement(By.Id("Details_SpaceshipId"));
            var details_id = details_SpaceshipId.Text;

            IWebElement details_SpaceshipName = _driver.FindElement(By.Id("Details_SpaceshipName"));
            var details_name = details_SpaceshipName.Text;

            IWebElement details_SpaceshipType = _driver.FindElement(By.Id("Details_SpaceshipType"));
            var details_type = details_SpaceshipType.Text;

            IWebElement details_SpaceshipCrew = _driver.FindElement(By.Id("Details_SpaceshipCrew"));
            var details_crew = details_SpaceshipCrew.Text;

            IWebElement details_SpaceshipPower = _driver.FindElement(By.Id("Details_SpaceshipPower"));
            var details_power = details_SpaceshipPower.Text;

            Assert.NotNull(details_id);
            Assert.Equal("i add name for spaceship", details_name);
            Assert.Equal("i add ship type for spaceship", details_type);
            Assert.Equal("12345", details_crew);
            Assert.Equal("666667", details_power);
        }

        [Fact]
        [Order(3)]
        public void Should_UpdateDetails_OfASpaceShip_WithNewData()
        {
            NavigateToSIndex();

            var table = _driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);

            foreach (var row in rows)
            {
                var locatedelement = row.FindElement(By.Id("IndexNameSpaceship"));
                if (locatedelement.Text == "i add name for spaceship")
                {
                    var rowelement = row.FindElement(By.Id("IndexActionsSpaceship"));
                    var button = rowelement.FindElement(By.Id("IndexSpaceshipUpdate"));
                    button.Click();
                    Thread.Sleep(500);
                    break;
                }
            }

            InsertSpaceShipData(_driver, true);

            IWebElement cu_UpdateSpaceship = _driver.FindElement(By.Id("CU_UpdateSpaceship"));
            cu_UpdateSpaceship.Click();
        }

        [Fact]
        [Order(4)]
        public void Should_DeleteSpaceship_FromAppVIADeleteView()
        {
            NavigateToSIndex();

            var table = _driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);

            foreach (var row in rows)
            {
                var locatedelement = row.FindElement(By.Id("IndexNameSpaceship"));
                if (locatedelement.Text == "DISISENJUNAEM")
                {
                    var rowelement = row.FindElement(By.Id("IndexActionsSpaceship"));
                    var button = rowelement.FindElement(By.Id("IndexSpaceshipDelete"));
                    button.Click();
                    Thread.Sleep(500);
                    break;
                }
            }
            var clickthisbutton = _driver.FindElement(By.Id("Delete_Delete"));
            clickthisbutton.Click();

            // Re-fetch table to get updated row count
            var updatedTable = _driver.FindElement(By.Id("IndexTable"));
            int rowCount = updatedTable.FindElements(By.TagName("tr")).ToList().Count();
            Assert.True(rowCount < 2);
        }

        private void InsertSpaceShipData(IWebDriver driver, bool isOneOrTwo = false)
        {
            if (isOneOrTwo)
            {
                IWebElement cu_NameEntrySpaceship = driver.FindElement(By.Id("CU_NameEntrySpaceship"));
                cu_NameEntrySpaceship.Clear();
                cu_NameEntrySpaceship.SendKeys("DISISENJUNAEM");
                IWebElement cu_ShipTypeEntrySpaceship = driver.FindElement(By.Id("CU_ShipTypeEntrySpaceship"));
                cu_ShipTypeEntrySpaceship.Clear();
                cu_ShipTypeEntrySpaceship.SendKeys("completely unheard of haunebu");
                IWebElement cu_CrewEntrySpaceship = driver.FindElement(By.Id("CU_CrewEntrySpaceship"));
                cu_CrewEntrySpaceship.Clear();
                cu_CrewEntrySpaceship.SendKeys("67890");
                IWebElement cu_EnginePowerEntrySpaceship = driver.FindElement(By.Id("CU_EnginePowerEntrySpaceship"));
                cu_EnginePowerEntrySpaceship.Clear();
                cu_EnginePowerEntrySpaceship.SendKeys("444");
            }
            else
            {
                IWebElement cu_NameEntrySpaceship = driver.FindElement(By.Id("CU_NameEntrySpaceship"));
                cu_NameEntrySpaceship.Clear();
                cu_NameEntrySpaceship.SendKeys("i add name for spaceship");
                IWebElement cu_ShipTypeEntrySpaceship = driver.FindElement(By.Id("CU_ShipTypeEntrySpaceship"));
                cu_ShipTypeEntrySpaceship.Clear();
                cu_ShipTypeEntrySpaceship.SendKeys("i add ship type for spaceship");
                IWebElement cu_CrewEntrySpaceship = driver.FindElement(By.Id("CU_CrewEntrySpaceship"));
                cu_CrewEntrySpaceship.Clear();
                cu_CrewEntrySpaceship.SendKeys("12345");
                IWebElement cu_EnginePowerEntrySpaceship = driver.FindElement(By.Id("CU_EnginePowerEntrySpaceship"));
                cu_EnginePowerEntrySpaceship.Clear();
                cu_EnginePowerEntrySpaceship.SendKeys("666667");
            }
        }

        public void Dispose()
        {
            try
            {
                _driver?.Quit();
                _driver?.Dispose();
            }
            catch { }
        }
    }
}