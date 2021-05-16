using System;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace PeopleIKnow.UiTests
{
    public class CreateContactTests : UiTest, IDisposable
    {
        public CreateContactTests()
        {
            Driver = new ChromeDriver();
        }

        public void Dispose()
        {
            Driver.Quit();
            Driver.Dispose();
        }

        [Fact]
        public void NewContactIsShownInTheFeed()
        {
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            Driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(10);
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            Login();
            WaitFor("add-button");
            AddNewContact();

            var feed = Driver.FindElement(By.Id("people-feed"));
            var teasers = feed.FindElements(By.ClassName("card"));
            teasers.Should().Contain(c => c.Text == "Harry James Potter");
        }

        private void AddNewContact()
        {
            Driver.FindElement(By.Id("add-button")).Click();
            WaitFor("Firstname");
            Driver.FindElement(By.Id("Firstname")).SendKeys("Harry");
            Driver.FindElement(By.Id("Middlename")).SendKeys("James");
            Driver.FindElement(By.Id("Lastname")).SendKeys("Potter");
            Driver.FindElement(By.Id("save-button")).Click();
        }

        private void Login()
        {
            Driver.Navigate().GoToUrl("https://localhost:5001/");
            Driver.FindElement(By.Id("Input_Email")).SendKeys("test@test.com");
            Driver.FindElement(By.Id("Input_Password")).SendKeys("Test1234.");
            Driver.FindElement(By.Id("Submit_Button")).Click();
        }
    }
}