# ScreenTask

## 👩🏼‍💻 → [Latest release](https://github.com/mchampanis/ScreenTask/releases)

## Description

Screen sharing made easy! Share your screen across local devices without internet.

Cross-platform Java project (Linux & OSX) available here: [ahmadomar/ScreenTask](https://github.com/ahmadomar/ScreenTask)

### Requirements
- .NET Framework 4.8: [download](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48)
- Look for the `Offline installer / Run apps - runtime` link at the above link
- Should work on Windows Vista, 7, 8, 10, 11
- You might need to build it yourself for your specific Windows install (see below)

### Building
- Download [Visual Studio](https://visualstudio.microsoft.com/downloads/)
- Install above and select the .NET desktop apps component when asked
- Open `ScreenTask.sln` and change to `Release` (instead of `Debug`) and then `Run`

Alternatively, build from the command line using `make` (requires `GNU Make` and `MSBuild` on your PATH):

| Command | Action |
|---|---|
| `make` | Release build |
| `make debug` | Debug build |
| `make clean` | Clean all outputs |
| `make run` | Release build and launch |
| `make format` | Run `dotnet format` |

### Features
- Share your screen on local network
- WebUI so no additional client software needed - access via web browser
- Basic HTTP authentication support
- Unlimited number of clients
- Multiple screens support

### v1.3.3 changes
- worked on fullscreen compatibility across browsers
- moved app config to a more sensible folder
- cleaned up a lot of the code
- set up code formatter for the project

### v1.3.2 changes
- show main window on start instead of just the system tray icon
- fix JS: was halting execution due to legacy code that wasn't removed, fullscreen didn't work because of this

### v1.3.1 changes
- firewall rules created programmatically
- better tray icon logic
- new icon & server on/off status indicated in taskbar
- started cleaning up code
- cleaned up web ui, removed unneeded header and footer
- changed javascript timer to behave better on phone browsers
- memory leak fixed
- fixed saved ip bug

### Forked from / original credit
[EslaMx7/ScreenTask](https://github.com/EslaMx7/ScreenTask) (last version is 1.2)

### License
ScreenTask is released under the GPL v3 (or later) license: http://www.gnu.org/licenses/gpl-3.0.html

