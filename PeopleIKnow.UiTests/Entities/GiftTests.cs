using System;
using System.Linq;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace PeopleIKnow.UiTests.Entities
{
    public class GiftTests : UiTest, IDisposable
    {
        public GiftTests()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            Driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(10);
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
        }

        public void Dispose()
        {
            Driver.Quit();
            Driver.Dispose();
        }

        [Fact]
        public void AddGift_ShowsGiftInPanel()
        {
            Login();
            var contactName = AddNewContact();
            var feed = Driver.FindElement(By.Id("people-feed"));
            var teasers = feed.FindElements(By.ClassName("card"));
            var contactTeaser = teasers.First(t => t.Text.Contains(contactName));
            contactTeaser.Click();
            WaitFor("Firstname");
            Driver.FindElement(By.Id("add-gift")).Click();
            WaitFor("Description");
            Driver.FindElement(By.Id("Description")).SendKeys("Awesome Gift");
            Driver.FindElement(By.Id("GiftType")).SendKeys("Given");
            Driver.FindElement(By.Id("save-gift-button")).Click();
            WaitFor("Firstname");
            var giftPanel = Driver.FindElement(By.Id("gift-panel"));

            var awesomeGift = giftPanel.FindElements(By.TagName("input"))
                .Where(t => t.GetProperty("value") == "Awesome Gift").ToList();
            awesomeGift.Should().HaveCount(1);
            awesomeGift.First().Should().NotBeNull();
        }
    }
}