# Selenium vs Playwright Interaction Fidelity Tests

C#/.NET tests comparing Selenium WebDriver and Playwright interaction behavior. Demonstrates differences in keyboard navigation, tabindex accessibility issues, blocked key events, focus handling, and how direct automation APIs can bypass real user interaction paths and hide accessibility or UX defects.

## Files

The HTML file used for the interaction experiments are located in the root of the repository:

- `TypingBug.html`

This file intentionally contain accessibility and interaction issues used to compare framework behavior.

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

It demonstrates how Playwright can successfully interact with elements using the .fill() method even when real keyboard users would fail while Selenium's .SendKeys() method doesn't have this problem.

[FidelityTests.webm](https://github.com/user-attachments/assets/b7ca1b0f-f0ae-4afc-bce6-57e03b2c96c4)

