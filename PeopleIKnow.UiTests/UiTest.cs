using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace PeopleIKnow.UiTests
{
    [Collection("Sequential")]
    public abstract class UiTest
    {
        protected IWebDriver Driver;

        protected void WaitFor(string idToFind)
        {
            new WebDriverWait(Driver, TimeSpan.FromSeconds(10)).Until(driver => driver.FindElement(By.Id(idToFind)));
        }

        protected string AddNewContact()
        {
            var firstname = DateTime.Now.Ticks.ToString();
            Driver.FindElement(By.Id("add-button")).Click();
            WaitFor("Firstname");
            Driver.FindElement(By.Id("Firstname")).SendKeys(firstname);
            Driver.FindElement(By.Id("Lastname")).SendKeys("Test");
            Driver.FindElement(By.Id("save-button")).Click();
            return firstname + " Test";
        }

        protected void Login()
        {
            Driver.Navigate().GoToUrl("https://localhost:5001/");
            Driver.FindElement(By.Id("Input_Email")).SendKeys("test@test.com");
            Driver.FindElement(By.Id("Input_Password")).SendKeys("Test1234.");
            Driver.FindElement(By.Id("Submit_Button")).Click();
            WaitFor("add-button");
            Thread.Sleep(500);
        }

        protected void OpenContact(string contactName)
        {
            var feed = Driver.FindElement(By.Id("people-feed"));
            var teasers = feed.FindElements(By.ClassName("card"));
            var contactTeaser = teasers.First(t => t.Text.Contains(contactName));
            contactTeaser.Click();
            WaitFor("Firstname");
        }
    }
}