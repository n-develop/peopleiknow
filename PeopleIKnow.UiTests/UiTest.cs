using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PeopleIKnow.UiTests
{
    public abstract class UiTest
    {
        protected IWebDriver Driver;


        protected void WaitFor(string idToFind)
        {
            new WebDriverWait(Driver, TimeSpan.FromSeconds(10)).Until(driver => driver.FindElement(By.Id(idToFind)));
        }
    }
}