using System;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace PeopleIKnow.UiTests
{
    public class CreateContactTests : IDisposable
    {
        private readonly IWebDriver _driver;

        public CreateContactTests()
        {
            _driver = new ChromeDriver();
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        [Fact]
        public void NewContactIsShownInTheFeed()
        {
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(10);
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            Login();
            WaitFor("add-button");
            AddNewContact();

            var feed = _driver.FindElement(By.Id("people-feed"));
            var teasers = feed.FindElements(By.ClassName("card"));
            teasers.Should().Contain(c => c.Text == "Harry James Potter");
        }

        private void WaitFor(string idToFind)
        {
            new WebDriverWait(_driver, TimeSpan.FromSeconds(10)).Until(driver => driver.FindElement(By.Id(idToFind)));
        }

        private void AddNewContact()
        {
            _driver.FindElement(By.Id("add-button")).Click();
            WaitFor("Firstname");
            _driver.FindElement(By.Id("Firstname")).SendKeys("Harry");
            _driver.FindElement(By.Id("Middlename")).SendKeys("James");
            _driver.FindElement(By.Id("Lastname")).SendKeys("Potter");
            _driver.FindElement(By.Id("save-button")).Click();
        }

        private void Login()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/");
            _driver.FindElement(By.Id("Input_Email")).SendKeys("test@test.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Test1234.");
            _driver.FindElement(By.Id("Submit_Button")).Click();
        }
    }
}