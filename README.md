# PartyOrderOnNameplate

PartyOrderOnNameplate is a FFXIV plugin that displays each party member's current party order number beside their in-game nameplate, making teammates easier to identify at a glance for targeting.

## Install

This plugin is not currently distributed through the official Dalamud plugin repository. To install a GitHub release:

1. Download the latest release ZIP from the [Releases page](https://github.com/floumitou/XIVPartyNumberNameplate/releases).
2. Extract the ZIP.
3. In XIVLauncher, open Dalamud settings and enable **Experimental** or **Developer** mode.
4. Under **Dev Plugin Locations**, add the extracted folder, then save the settings.
5. Open `/xlplugins`, find PartyOrderOnNameplate under **Installed Dev Plugins**, and enable it.

To update the plugin, download the newer release, replace the extracted files, and restart or reload the plugin through the Dalamud plugin installer.

## Features

- Choose where the party number appears: left, right, top, or bottom.
- Disable number display for your own character.
- Adjust how often the plugin checks the party HUD for party order changes.
- Enable or disable plugin functionality from the settings window or with `/poon`.
- Open the settings window with `/poonconf`.

## Known Issues
Because of current plugin limitations, Top and Bottom placement use the nameplate title line. Therefore the player title is hidden in those modes.

## Settings

Open the settings window with `/poonconf` or through Dalamud's plugin interface. Use **Enabled** to toggle party numbers, **Disable for self** to hide your own number, **Number Position** to select a placement, and **Party List Update Interval** to configure how often party order is checked.

> **AI disclosure:** This README and also some code snippets were generated with the help of AI tools. However, I reviewed everything thoroughly myself.
