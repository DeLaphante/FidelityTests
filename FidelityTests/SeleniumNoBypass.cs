using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace SeleniumFocusTest
{
    [TestClass]
    public class SeleniumNoBypass
    {
        private IWebDriver driver;

        [TestInitialize]
        public void Setup()
        {
            var options = new ChromeOptions();

            // Optional: slow things down visually
            options.AddArgument("--start-maximized");

            driver = new ChromeDriver(options);
        }

        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }

        [TestMethod]
        public void Should_Interact_With_Tabindex_Minus1_Elements()
        {
            driver.Navigate().GoToUrl(
                "file:///C:/Users/Personal/source/repos/FidelityTests/TypingBug.html"
            );

            // Locate input
            var input = driver.FindElement(By.TagName("input"));

            // Selenium can still type into it directly
            input.SendKeys("Selenium bypassed keyboard focus");

            // Locate button
            var button = driver.FindElement(By.TagName("button"));

            // Selenium can still click it directly
            button.Click();

            // Pause so you can visually observe
            Thread.Sleep(3000);

            // Assertion
            Assert.AreEqual(
                "Selenium bypassed keyboard focus",
                input.GetAttribute("value")
            );
        }

        [TestMethod]
        public void Tab_Key_Should_Not_Focus_Input()
        {
            driver.Navigate().GoToUrl(
                "file:///C:/Users/Personal/source/repos/FidelityTests/TypingBug.html"
            );

            // Send TAB key to page
            new Actions(driver)
                .SendKeys(Keys.Tab)
                .Perform();

            Thread.Sleep(3000);

            // Get active element tag name
            var activeTag = driver.SwitchTo().ActiveElement()
                .TagName;

            Assert.AreEqual("a", activeTag.ToLower());
        }

        [TestMethod]
        public void Selenium_Should_Expose_Overlay_Bug()
        {
            driver.Navigate().GoToUrl(
                "file:///C:/Users/Personal/source/repos/FidelityTests/OverlayBug.html"
            );

            // First click creates invisible overlay
            driver.FindElement(By.Id("button1")).Click();

            // Selenium immediately detects interception
            driver.FindElement(By.Id("button2")).Click();

        }

        [TestMethod]
        public void Selenium_Should_Expose_Stale_Element_Bug()
        {
            driver.Navigate().GoToUrl(
                "file:///C:/Users/Personal/source/repos/FidelityTests/ReRenderBug.html"
            );

            // Selenium stores actual DOM node reference
            var firstResult = new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(
                    d => d.FindElement(By.XPath("(//li)[1]")
                )
            );

            // Wait long enough for full re-render
            Thread.Sleep(3000);

            // Old DOM node no longer exists
            firstResult.Click();
        }

        [TestMethod]
        public void Selenium_Should_Expose_Focus_Stealing_Bug()
        {
            driver.Navigate().GoToUrl(
                "file:///C:/Users/Personal/source/repos/FidelityTests/FocusBug.html"
            );

            // Wait for input
            var username = new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(
                    d => d.FindElement(By.Id("username")
                )
            );

            // Focus input
            username.SendKeys("SeleniumTyping");


            // If focus is stolen mid-typing,
            // Selenium will stop typing into input
            var finalValue =
                username.GetAttribute("value");

            Assert.AreEqual(
                "SeleniumTyping",
                finalValue
            );
        }
    }
}