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
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            Driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(5);
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);
        }

        public void Dispose()
        {
            Driver.Quit();
            Driver.Dispose();
        }

        [Fact]
        public void NewContactIsShownInTheFeed()
        {
            Login();
            var contactName = AddNewContact();

            var feed = Driver.FindElement(By.Id("people-feed"));
            var teasers = feed.FindElements(By.ClassName("card"));
            teasers.Should().Contain(c => c.Text == contactName);
        }
    }
}