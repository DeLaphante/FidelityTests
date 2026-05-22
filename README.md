# Selenium vs Playwright Interaction Fidelity Tests

C#/.NET tests comparing Selenium WebDriver and Playwright interaction behavior. Demonstrates differences in keyboard navigation, tabindex accessibility issues, blocked key events, focus handling, overlay interception behavior, stale element behavior, and how direct automation APIs can bypass real user interaction paths and hide accessibility or UX defects.

## Files

The HTML files used for the interaction experiments are located in the root of the repository:

- `TypingBug.html`
- `OverlayBug.html`
- `ReRenderBug.html`

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
- Selenium immediately throws `ElementClickInterceptedException`
- Playwright waits/retries until the overlay disappears and then passes

This highlights how interaction abstraction and auto-waiting heuristics can mask transient UI defects.

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
- transient UI defects
- DOM re-rendering
- direct DOM interaction vs real browser interaction flow

## Videos

### Typing Bug Experiment
[FidelityTests.webm](https://github.com/user-attachments/assets/b7ca1b0f-f0ae-4afc-bce6-57e03b2c96c4)

### Overlay Bug Experiment
[OverlayBug.webm](https://github.com/user-attachments/assets/69b7c29d-a7af-48fc-b036-61e84ec58357)
