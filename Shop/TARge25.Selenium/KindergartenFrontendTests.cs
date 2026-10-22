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
                var method = test.TestMethod.Method.GetCustomAttributes(typeof(OrderAttribute).AssemblyQualifiedName)
                .FirstOrDefault();
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


    [TestCaseOrderer("TARge25.Selenium.PriorityOrderer", "TARge25.Selenium")]
    public class KindergartenFrontendTests
    {
        public static string? thisRunGuid { get; set; }

        [Fact]
        [Order(1)]
        public void Should_NavigateToCreate_AddUniqueKindergarten_WithCorrectData_ReturnToIndex()
        {
            IWebDriver driver = SetupAndNavigateKIndex();

            IWebElement createInIndex = driver.FindElement(By.Id("CreateInIndex"));
            createInIndex.Click();

            string concreteName = InsertKindergartenData_ReturnName(driver, false);
            thisRunGuid = concreteName;

            IWebElement cu_CreateKindergarten = driver.FindElement(By.Id("CU_CreateKindergarten"));
            cu_CreateKindergarten.Click();

            Thread.Sleep(500);

            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            string kinderGartenGroupNameData = "";
            string kinderGartenChildrenCountData = "";
            string kinderGartenKindergartenNameData = "";
            string kinderGartenTeacherNameData = "";
            string kinderGartenBuiltAtData = "";

            bool doesItExist = false;
            foreach (var row in rowsOfTable)
            {
                if (row.FindElement(By.Id("IndexGroupNameKindergarten")).Text == thisRunGuid)
                {
                    doesItExist = true;
                    kinderGartenGroupNameData = thisRunGuid;
                    kinderGartenChildrenCountData = row.FindElement(By.Id("IndexChildrenCountKindergarten")).Text;
                    kinderGartenKindergartenNameData = row.FindElement(By.Id("IndexKindergartenNameKindergarten")).Text;
                    kinderGartenTeacherNameData = row.FindElement(By.Id("IndexTeacherNameKindergarten")).Text;
                    break;
                }          
            }
            driver.Quit();

            Assert.True(true == doesItExist);
            Assert.Equal(kinderGartenGroupNameData, thisRunGuid);
            Assert.Equal(kinderGartenChildrenCountData, "666");
            Assert.True(kinderGartenKindergartenNameData == "KindergertenName to Kindergarten");
            Assert.True((thisRunGuid == kinderGartenGroupNameData)
                && ("KindergertenName to Kindergarten" == kinderGartenKindergartenNameData)
                && ("666" == kinderGartenChildrenCountData));
        }

        
    
    private static IWebDriver SetupAndNavigateKIndex()
        {
            IWebDriver driver = new FirefoxDriver();
            driver.Url = "https://localhost:7232/";
            IWebElement navigateToKindergarten = driver.FindElement(By.LinkText("Kindergarten"));
            navigateToKindergarten.Click();
            return driver;
        }


        private void InsertKinderGartenData(IWebDriver driver, string name, bool isOneOrTwo = false)
        {

            if (isOneOrTwo)
            {
                IWebElement cu_GroupNameEntryKindergarten = driver.FindElement(By.Id("CU_GroupNameEntryKindergarten"));
                cu_GroupNameEntryKindergarten.Clear();
                if (name != null)
                {
                    cu_GroupNameEntryKindergarten.SendKeys(name);
                }
                else
                {
                    cu_GroupNameEntryKindergarten.SendKeys("GroupName");
                }
                IWebElement cu_ChildrenCountEntryKindergarten = driver.FindElement(By.Id("CU_ChildrenCountEntryKindergarten"));
                cu_ChildrenCountEntryKindergarten.Clear();
                cu_ChildrenCountEntryKindergarten.SendKeys("32");
                IWebElement cu_KindergartenNameEntryKindergarten = driver.FindElement(By.Id("CU_KindergartenNameEntryKindergarten"));
                cu_KindergartenNameEntryKindergarten.Clear();
                cu_KindergartenNameEntryKindergarten.SendKeys("KindergertenName");
                IWebElement cu_TeacherNameEntryKindergarten = driver.FindElement(By.Id("CU_TeacherNameEntryKindergarten"));
                cu_TeacherNameEntryKindergarten.Clear();
                cu_TeacherNameEntryKindergarten.SendKeys("TeacherName");
            }
            else
            {
                IWebElement cu_GroupNameEntryKindergarten = driver.FindElement(By.Id("CU_GroupNameEntryKindergarten"));
                cu_GroupNameEntryKindergarten.Clear();
                if (name != null)
                {
                    cu_GroupNameEntryKindergarten.SendKeys(name);
                }
                else
                {
                    cu_GroupNameEntryKindergarten.SendKeys("Added GroupName to Kindergarten");
                }
                IWebElement cu_ChildrenCountEntryKindergarten = driver.FindElement(By.Id("CU_ChildrenCountEntryKindergarten"));
                cu_ChildrenCountEntryKindergarten.Clear();
                cu_ChildrenCountEntryKindergarten.SendKeys("32");
                IWebElement cu_KindergartenNameEntryKindergarten = driver.FindElement(By.Id("CU_KindergartenNameEntryKindergarten"));
                cu_KindergartenNameEntryKindergarten.Clear();
                cu_KindergartenNameEntryKindergarten.SendKeys("Added KindergertenName to Kindergarten");
                IWebElement cu_TeacherNameEntryKindergarten = driver.FindElement(By.Id("CU_TeacherNameEntryKindergarten"));
                cu_TeacherNameEntryKindergarten.Clear();
                cu_TeacherNameEntryKindergarten.SendKeys("Adde TeacherName to Kindergarten");
            }
        }

        private string InsertKindergartenData_ReturnName(IWebDriver driver, bool isOneOrTwo = false)
        {
            Guid returnGuid = Guid.NewGuid();
            if (isOneOrTwo)
            {
                IWebElement cu_GroupNameEntryKindergarten = driver.FindElement(By.Id("CU_GroupNameEntryKindergarten"));
                cu_GroupNameEntryKindergarten.Clear();
                cu_GroupNameEntryKindergarten.SendKeys(returnGuid.ToString());
                IWebElement cu_ChildrenCountEntryKindergarten = driver.FindElement(By.Id("CU_ChildrenCountEntryKindergarten"));
                cu_ChildrenCountEntryKindergarten.Clear();
                cu_ChildrenCountEntryKindergarten.SendKeys("666");
                IWebElement cu_KindergartenNameEntryKindergarten = driver.FindElement(By.Id("CU_KindergartenNameEntryKindergarten"));
                cu_KindergartenNameEntryKindergarten.Clear();
                cu_KindergartenNameEntryKindergarten.SendKeys("KindergertenName to Kindergarten");
                IWebElement cu_TeacherNameEntryKindergarten = driver.FindElement(By.Id("CU_TeacherNameEntryKindergarten"));
                cu_TeacherNameEntryKindergarten.Clear();
                cu_TeacherNameEntryKindergarten.SendKeys("TeacherName to Kindergarten");
            }
            else
            {
                IWebElement cu_GroupNameEntryKindergarten = driver.FindElement(By.Id("CU_GroupNameEntryKindergarten"));
                cu_GroupNameEntryKindergarten.Clear();
                cu_GroupNameEntryKindergarten.SendKeys(returnGuid.ToString());
                IWebElement cu_ChildrenCountEntryKindergarten = driver.FindElement(By.Id("CU_ChildrenCountEntryKindergarten"));
                cu_ChildrenCountEntryKindergarten.Clear();
                cu_ChildrenCountEntryKindergarten.SendKeys("666");
                IWebElement cu_KindergartenNameEntryKindergarten = driver.FindElement(By.Id("CU_KindergartenNameEntryKindergarten"));
                cu_KindergartenNameEntryKindergarten.Clear();
                cu_KindergartenNameEntryKindergarten.SendKeys("KindergertenName to Kindergarten");
                IWebElement cu_TeacherNameEntryKindergarten = driver.FindElement(By.Id("CU_TeacherNameEntryKindergarten"));
                cu_TeacherNameEntryKindergarten.Clear();
                cu_TeacherNameEntryKindergarten.SendKeys("TeacherName to Kindergarten");
            }
            return returnGuid.ToString();
        }
    }
}
