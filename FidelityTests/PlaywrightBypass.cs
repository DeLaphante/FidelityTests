using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

    [TestMethod]
    public async Task Playwright_Waits_For_Overlay_To_Disappear()
    {
        await page.GotoAsync(
            "file:///C:/Users/Personal/source/repos/FidelityTests/OverlayBug.html"
        );

        // First button creates invisible overlay
        await page.Locator("#button1").ClickAsync();

        // Playwright will wait/retry here
        // until the overlay disappears
        await page.Locator("#button2").ClickAsync();

        // Verify final state
        var status =
            await page.Locator("#status").InnerTextAsync();

        Assert.AreEqual(
            "Second button clicked successfully",
            status
        );

        // Pause so behavior is visible
        await page.WaitForTimeoutAsync(3000);
    }

    [TestMethod]
    public async Task Playwright_Requeries_After_Rerender()
    {
        await page.GotoAsync(
            "file:///C:/Users/Personal/source/repos/FidelityTests/ReRenderBug.html"
        );

        // Lazy locator (does NOT hold old DOM node)
        var firstResult =
            page.Locator("li").First;

        // Wait long enough for full re-render
        await page.WaitForTimeoutAsync(3000);

        // Playwright re-scans DOM at click time
        await firstResult.ClickAsync();

        // Verify click succeeded
        var status =
            await page.Locator("#status")
                .InnerTextAsync();

        Assert.AreEqual(
            "Clicked: First Search Result",
            status
        );

        // Pause so behavior is visible
        await page.WaitForTimeoutAsync(3000);
    }

    [TestMethod]
    public async Task Playwright_Should_Continue_Typing_After_Focus_Is_Stolen()
    {
        await page.GotoAsync(
            "file:///C:/Users/Personal/source/repos/FidelityTests/FocusBug.html"
        );

        var username =
            page.Locator("#username");

        // Focus input
        await username.ClickAsync();

        // Start typing before focus steal occurs
        await username.FillAsync(
            "PlaywrightTyping");

        // Get final value
        var finalValue =
            await username.InputValueAsync();

        // Playwright often recovers/re-focuses
        // and continues typing
        Assert.AreEqual(
            "PlaywrightTyping",
            finalValue
        );

        // Show visible result
        await page.WaitForTimeoutAsync(3000);
    }

    [TestMethod]
    public async Task Playwright_Should_Wait_And_Click_Button()
    {
        await page.GotoAsync(
            "file:///C:/Users/Personal/source/repos/FidelityTests/MissedClickBug.html"
        );

        var button =
            page.Locator("#submitButton");

        // Playwright internally:
        // - retries hover/click logic
        // - re-resolves locator
        // - waits for stability

        await button.ClickAsync();

        // Verify click eventually succeeded
        var status =
            await page.Locator("#status")
                .InnerTextAsync();

        Assert.AreEqual(
            "Button clicked successfully",
            status
        );

        // Pause so behavior is visible
        await page.WaitForTimeoutAsync(3000);
    }
}