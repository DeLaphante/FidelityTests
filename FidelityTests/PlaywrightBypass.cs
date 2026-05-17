using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace PlaywrightFocusTest;

[TestClass]
public class PlaywrightBypass : PageTest
{
    // Use local fields instead of assigning the base's read-only properties
    private IBrowser? browser;
    private IBrowserContext? context;
    private IPage? page;

    [TestInitialize]
    public async Task Setup()
    {
        // Recreate browser in headed mode
        browser = await Playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 500
            });

        context = await browser.NewContextAsync();

        page = await context.NewPageAsync();
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        if (context is not null)
            await context.CloseAsync();

        if (browser is not null)
            await browser.CloseAsync();
    }

    [TestMethod]
    public async Task Should_Interact_With_Tabindex_Minus1_Elements()
    {
        await page!.GotoAsync(
            "file:///C:/Users/Personal/source/repos/FidelityTests/TypingBug.html"
        );

        // Fill input that cannot receive TAB focus
        var input = page.Locator("input");

        await input.FillAsync(
            "Playwright bypassed keyboard focus"
        );

        // Click button that cannot receive TAB focus
        var button = page.Locator("button");

        await button.ClickAsync();

        // Pause so you can visually see it
        await page.WaitForTimeoutAsync(3000);

        // Assertion
        await Expect(input)
            .ToHaveValueAsync(
                "Playwright bypassed keyboard focus"
            );
    }

    [TestMethod]
    public async Task Tab_Key_Should_Not_Focus_Input()
    {
        await page!.GotoAsync(
            "file:///C:/Users/Personal/source/repos/FidelityTests/TypingBug.html"
        );

        // Press TAB
        await page.Keyboard.PressAsync("Tab");

        // Pause so you can visually observe focus behavior
        await page.WaitForTimeoutAsync(3000);

        // Get currently focused element
        var activeTag = await page.EvaluateAsync<string>(
            "document.activeElement.tagName"
        );

        Assert.AreEqual("A", activeTag);
    }
}