using System;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace PeopleIKnow.UiTests
{
    public class LoginTests : UiTest, IDisposable
    {
        public LoginTests()
        {
            Driver = new ChromeDriver();
        }

        public void Dispose()
        {
            Driver.Quit();
            Driver.Dispose();
        }

        [Fact]
        public void LoginSuccessfulRedirectsToDashboard()
        {
            Driver.Navigate().GoToUrl("https://localhost:5001/");
            Driver.FindElement(By.Id("Input_Email")).SendKeys("test@test.com");
            Driver.FindElement(By.Id("Input_Password")).SendKeys("Test1234.");
            Driver.FindElement(By.Id("Submit_Button")).Click();

            WaitFor("people-feed");
        }

        [Fact]
        public void LoginUnsuccessfulRedirectsToDashboard()
        {
            Driver.Navigate().GoToUrl("https://localhost:5001/");
            Driver.FindElement(By.Id("Input_Email")).SendKeys("test@test.com");
            Driver.FindElement(By.Id("Input_Password")).SendKeys("wrong_password");
            Driver.FindElement(By.Id("Submit_Button")).Click();

            WaitFor("error-summary");

            var errorSummary = Driver.FindElement(By.Id("error-summary"));
            errorSummary.Text.Should().Contain("Invalid login attempt");
        }
    }
}