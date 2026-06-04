# Selenium vs Playwright Interaction Fidelity Tests

C#/.NET tests comparing Selenium WebDriver and Playwright interaction behavior. Demonstrates differences in keyboard navigation, tabindex accessibility issues, blocked key events, focus handling, overlay interception behavior, stale element behavior, focus stealing behavior, hover instability behavior, hidden interaction contracts, semantic interaction drift, and how direct automation APIs and locator healing can bypass real user interaction paths and hide accessibility, UX, or business logic defects.

## Files

The HTML files used for the interaction experiments are located in the root of the repository:

- `TypingBug.html`
- `OverlayBug.html`
- `ReRenderBug.html`
- `FocusBug.html`
- `MissedClickBug.html`
- `ClickRequiredBug.html`
- `AISemanticBug.html`

These files intentionally contain accessibility, interaction, rendering, and semantic behavior defects used to compare framework behavior.

## Included Test Scenarios

### Typing Fidelity Test

Demonstrates how:
- real keyboard typing can be broken
- Playwright `.fill()` can still pass by bypassing parts of the keyboard interaction pipeline
- Selenium `.SendKeys()` exposes the broken typing behavior more directly

### Overlay Interception Test

Demonstrates how:
- an invisible overlay temporarily blocks a button
- keyboard focus becomes trapped inside the overlay
- keyboard users cannot TAB to the second button
- Selenium immediately throws `ElementClickInterceptedException`
- Playwright waits/retries until the overlay disappears and then passes

This highlights how interaction abstraction and auto-waiting heuristics can mask transient UI defects and accessibility issues.

### Stale Element Re-Render Test

Demonstrates how:
- a background API update completely re-renders a results list
- Selenium throws `StaleElementReferenceException`
- Playwright re-queries the DOM using lazy locators and clicks the new element successfully

This highlights how automatic locator healing can mask:
- flickering UI bugs
- unstable rendering behavior
- interrupted user interaction flows
- hidden DOM mutation problems

### Focus Stealing Test

Demonstrates how:
- a background side-effect or asynchronous update steals focus away from an active form field
- sequential keyboard interaction becomes interrupted
- Selenium exposes the broken focus continuity during typing
- Playwright `.fill()` still succeeds because it bypasses continuous keyboard focus dependency

This highlights how direct value injection can mask:
- focus management bugs
- interrupted typing flows
- broken keyboard interaction behavior
- accessibility focus issues

### Missed Click Stability Test

Demonstrates how:
- hovering a button causes it to unexpectedly relocate on the page
- the original interaction path becomes invalid
- Selenium exposes the unstable interaction surface more directly
- Playwright waits, re-evaluates the locator, and successfully clicks the relocated element

This highlights how automatic actionability checks and stability heuristics can mask:
- hover instability bugs
- layout shift issues
- micro-movement interaction defects
- unstable click targets
- precision interaction problems affecting real users

### Bonus: Click Required Accessibility Test

Demonstrates how:
- an input field silently requires a mouse click before typing works
- keyboard users can TAB into the field but cannot enter text
- screen reader and keyboard-only users are impacted
- Selenium `.SendKeys()` exposes the defect immediately
- a test that performs `.Click()` before typing can unintentionally hide the defect

The experiment intentionally creates an interaction contract violation where:
- the input appears normal
- the field receives focus correctly
- typing is blocked unless a mouse click occurs first
- every new focus session requires another click

A high-fidelity keyboard interaction exposes the defect:

```csharp
var input =
    driver.FindElement(
        By.Id("customerName"));

input.SendKeys("John");

Assert.AreEqual(
    "",
    input.GetAttribute("value"),
    "Keyboard user should not be able to type."
);
```

The issue can easily be masked by introducing a click:

```csharp
input.Click();
input.SendKeys("John");
```

This highlights how test design choices can influence defect visibility and how hidden mouse dependencies can remain undiscovered when automation assumes a click-based interaction path.

The experiment demonstrates defects involving:
- keyboard accessibility
- hidden interaction contracts
- mouse-dependent behavior
- focus vs activation discrepancies
- screen reader accessibility issues
- interaction fidelity
- real user workflow validation

### Bonus: AI Semantic Drift Test

Demonstrates how:
- the visible button text remains unchanged
- IDs and semantic attributes dynamically swap underneath the UI
- the visible "Submit Order" button can secretly execute cancellation logic
- a locator healing system may still find and interact with a "similar" element successfully
- weak assertions may falsely report the workflow as passing

The experiment intentionally creates semantic drift where:
- the page visually appears valid
- the interaction still succeeds
- but the underlying business meaning has changed

A deterministic semantic assertion exposes the defect:

```csharp
Assert.AreEqual(
    "Order Submitted",
    status,
    "Visible 'Submit Order' button executed the wrong business action."
);
```

This highlights how AI-assisted locator recovery and probabilistic healing can preserve interaction continuity while masking:
- semantic corruption
- business logic drift
- incorrect event binding
- hidden transactional defects
- behavioral contract violations

It also demonstrates the difference between:
- structural similarity
- semantic correctness

## Technologies

- C#
- .NET 8
- MSTest
- Selenium WebDriver
- Playwright for .NET

## Purpose

This repository explores the difference between:
- validating DOM state
- validating authentic user interaction behavior
- validating semantic business correctness
- validating accessibility interaction contracts

It demonstrates how abstraction layers in automation frameworks can change defect visibility and interaction fidelity.

The experiments focus on:
- keyboard realism
- accessibility behavior
- focus handling
- overlay interception
- stale element behavior
- focus stealing behavior
- hover instability behavior
- hidden mouse dependencies
- accessibility interaction contracts
- keyboard-only user workflows
- semantic interaction drift
- transient UI defects
- DOM re-rendering
- unstable interaction surfaces
- probabilistic locator healing
- direct DOM interaction vs real browser interaction flow
- structural similarity vs business correctness

## Videos

### Typing Bug Experiment
[FidelityTests.webm](https://github.com/user-attachments/assets/b7ca1b0f-f0ae-4afc-bce6-57e03b2c96c4)

### Overlay Bug Experiment
[OverlayBug.webm](https://github.com/user-attachments/assets/69b7c29d-a7af-48fc-b036-61e84ec58357)

### Re-render Bug Experiment
[ReRenderBug.webm](https://github.com/user-attachments/assets/6222ddbb-886e-481a-9c79-e98f2280b942)

### Focus Bug Experiment
[focusbug.webm](https://github.com/user-attachments/assets/622c860b-dded-43e7-b7a7-31d304c677bb)

### Missed Click Bug Experiment
[missedclickbug.webm](https://github.com/user-attachments/assets/11a209fb-0bc8-406f-98a2-74b97d378b62)

### Click Required Accessibility Experiment
[ClickRequiredBug.webm](https://github.com/user-attachments/assets/13d21eab-86b3-47b9-a152-546b3e5035b4)

### AI Semantic Bug Experiment
[AISemanticBug.webm](https://github.com/user-attachments/assets/6bbe70e6-b361-4af5-9383-ec7f152924cd)
