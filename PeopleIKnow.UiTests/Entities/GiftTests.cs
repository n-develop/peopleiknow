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
            OpenContact(contactName);
            var giftName = AddGift();
            var giftPanel = Driver.FindElement(By.Id("gift-panel"));

            var awesomeGift = giftPanel.FindElements(By.TagName("input"))
                .Where(t => t.GetProperty("value") == giftName).ToList();
            awesomeGift.Should().HaveCount(1);
            awesomeGift.First().Should().NotBeNull();
        }

        private string AddGift()
        {
            var giftName = DateTime.Now.Ticks.ToString();
            Driver.FindElement(By.Id("add-gift")).Click();
            WaitFor("Description");
            Driver.FindElement(By.Id("Description")).SendKeys(giftName);
            Driver.FindElement(By.Id("GiftType")).SendKeys("Given");
            Driver.FindElement(By.Id("save-gift-button")).Click();
            WaitFor("Firstname");
            return giftName;
        }
    }
}