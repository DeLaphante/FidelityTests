using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumFocusTest
{
    [TestClass]
    public class SeleniumAccessibility
    {
        private IWebDriver driver;

        [TestInitialize]
        public void Setup()
        {
            var options = new ChromeOptions();

            options.AddArgument("--start-maximized");

            driver = new ChromeDriver(options);
        }

        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }

        [TestMethod]
        public void KeyboardUserCannotUseInput()
        {
            driver.Navigate().GoToUrl(
                "file:///C:/Users/Personal/source/repos/FidelityTests/ClickRequiredBug.html");

            var input =
                driver.FindElement(
                    By.Id("customerName"));

            input.SendKeys("John");

            Thread.Sleep(3000); // Optional: slow things down visually

            Assert.AreEqual(
                "John",
                input.GetAttribute("value"));
        }

        [TestMethod]
        public void MouseUserCanTypeAfterClick()
        {
            driver.Navigate().GoToUrl(
                  "file:///C:/Users/Personal/source/repos/FidelityTests/ClickRequiredBug.html");

            var input =
                driver.FindElement(
                    By.Id("customerName"));

            input.Click();

            input.SendKeys("John");

            Thread.Sleep(3000); // Optional: slow things down visually

            Assert.AreEqual(
                "John",
                input.GetAttribute("value"));
        }

    }
}