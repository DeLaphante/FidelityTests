# Selenium vs Playwright Interaction Fidelity Tests

C#/.NET tests comparing Selenium WebDriver and Playwright interaction behavior. Demonstrates differences in keyboard navigation, tabindex accessibility issues, blocked key events, focus handling, overlay interception behavior, stale element behavior, focus stealing behavior, hover instability behavior, and how direct automation APIs can bypass real user interaction paths and hide accessibility or UX defects.

## Files

The HTML files used for the interaction experiments are located in the root of the repository:

- `TypingBug.html`
- `OverlayBug.html`
- `ReRenderBug.html`
- `FocusBug.html`
- `MissedClickBug.html`

These files intentionally contain accessibility and interaction defects used to compare framework behavior.

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

It demonstrates how abstraction layers in automation frameworks can change defect visibility and interaction fidelity.

The experiments focus on:
- keyboard realism
- accessibility behavior
- focus handling
- overlay interception
- stale element behavior
- focus stealing behavior
- hover instability behavior
- transient UI defects
- DOM re-rendering
- unstable interaction surfaces
- direct DOM interaction vs real browser interaction flow

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