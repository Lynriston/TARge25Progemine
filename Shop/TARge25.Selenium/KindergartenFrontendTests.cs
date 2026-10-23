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
            Assert.Equal("666", kinderGartenChildrenCountData);
            Assert.True(kinderGartenKindergartenNameData == "KindergertenName to Kindergarten");
            Assert.True((thisRunGuid == kinderGartenGroupNameData)
                && ("KindergertenName to Kindergarten" == kinderGartenKindergartenNameData)
                && ("666" == kinderGartenChildrenCountData));
        }

        [Fact]
        [Order(2)]
        public void Should_NavigateToDetails_OfThisKindergarten_WithPreviouslyCorrectData()
        {
            IWebDriver driver = SetupAndNavigateKIndex();

            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool isObjectFound = false;

            foreach (var row in rows)
            {
                var locatedElement = row.FindElement(By.Id("IndexGroupNameKindergarten"));
                if (locatedElement.Text == thisRunGuid)
                {
                    isObjectFound = true;
                    var rowelement = row.FindElement(By.Id("IndexActionKindergarten"));
                    var button = rowelement.FindElement(By.Id("IndexKindergartenDetails"));
                    button.Click();

                    //Thread.Sleep(500);
                    break;
                }
            }
            IWebElement details_KindergartenId = driver.FindElement(By.Id("Details_KindergartenId"));
            var details_id = details_KindergartenId.Text;

            IWebElement details_KindergartenGroupName = driver.FindElement(By.Id("Details_KindergartenGroupName"));
            var details_groupName = details_KindergartenGroupName.Text;

            IWebElement details_KindergartenChildrenCount = driver.FindElement(By.Id("Details_KindergartenChildrenCount"));
            var details_childrenCount = details_KindergartenChildrenCount.Text;

            IWebElement details_KindergartenName = driver.FindElement(By.Id("Details_KindergartenName"));
            var details_kindergartenName = details_KindergartenName.Text;

            IWebElement details_KindergartenTeacherName = driver.FindElement(By.Id("Details_KindergartenTeacherName"));
            var details_teacherName = details_KindergartenTeacherName.Text;

            driver.Quit();

            Assert.True(isObjectFound);
            Assert.NotNull(details_id);
            Assert.NotEmpty(details_groupName);
            Assert.NotNull(details_childrenCount);
            Assert.True(!string.IsNullOrEmpty(details_childrenCount));
            Assert.Equal(thisRunGuid, details_groupName);
            Assert.Equal("KindergertenName to Kindergarten", details_kindergartenName);
            Assert.Equal("666", details_childrenCount);
            Assert.Equal("TeacherName to Kindergarten", details_teacherName);
            Assert.True(int.Parse(details_childrenCount).GetType() == typeof(int));
        }

        [Fact]
        [Order(3)]
        public void Should_UpdateDetails_OfThisKindergarten_WithNewData()
        {
            IWebDriver driver = SetupAndNavigateKIndex();

            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool doesItExist = false;

            foreach (var row in rows)
            {
                var locatedElement = row.FindElement(By.Id("IndexGroupNameKindergarten"));
                if (locatedElement.Text == thisRunGuid)
                {
                    doesItExist = true;
                    var rowElement = row.FindElement(By.Id("IndexActionKindergarten"));
                    var button = rowElement.FindElement(By.Id("IndexKindergartenUpdate"));
                    button.Click();
                    //Thread.Sleep(500);
                    break;
                }
            }
            InsertKinderGartenData(driver, thisRunGuid, true);

            IWebElement cu_CreateKindergarten = driver.FindElement(By.Id("CU_UpdateKindergarten"));
            cu_CreateKindergarten.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var tableOfIndex = wait.Until(d => d.FindElement(By.Id("IndexTable")));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            string kinderGartenGroupNameData = "";
            string kinderGartenChildrenCountData = "";
            string kinderGartenKindergartenNameData = "";
            string kinderGartenTeacherNameData = "";
            doesItExist = false;
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

            Assert.True(doesItExist);
            Assert.NotNull(kinderGartenGroupNameData);
            Assert.Equal(thisRunGuid, kinderGartenGroupNameData);
            Assert.False(string.IsNullOrEmpty(kinderGartenKindergartenNameData));
            Assert.NotEqual("Adde TeacherName to Kindergarten", kinderGartenTeacherNameData);
            Assert.True(kinderGartenTeacherNameData == "TeacherName");
            Assert.False(string.IsNullOrEmpty(kinderGartenKindergartenNameData));
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
