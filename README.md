# ItzWarty.MiniSynapse
## Author
Created by [@ItzWarty](http://www.twitter.com/ItzWarty). I probably won't be providing support for this application.

## Purpose
Razer Synapse can be a pretty annoying (and heavy!) bugger at times. However, users need to run Razer Synapse at startup in order to load their Razer device profiles. This application serves as an alternative to Razer Synapse for this purpose: it simply loads your device profiles and exits promptly. If you later desire the full Razer Synapse experience (e.g. for changing your device configuration, or if you plug in a new device), you can then opt to manually start Razer Synapse from your start menu.

## Installation Instructions
Head to https://github.com/ItzWarty/ItzWarty.MiniSynapse/releases for installation instructions.

## License
Let's use GPLv3 for now.

## How it works
Not sure, though it uses the same routines that the real Synapse uses.

## Developers
You'll have to copy `RzCommon.dll`, `RzStorage.dll`, `RzStorageIO.dll`, `RzSynapse.exe`, and `RzUISdk.dll` into your `ItzWarty.MiniSynapse` directory next to `ItzWarty.MiniSynapse.csproj` for this thing to compile.
