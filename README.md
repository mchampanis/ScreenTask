# ScreenTask
------------------------------
## v1.3.2 release: [download here](https://github.com/mchampanis/ScreenTask/releases)

Screen sharing made easy! Share your screen across local devices without internet.

**Cross-platform Java project for (Linux & OSX):** [**GitHub page**](https://github.com/ahmadomar/ScreenTask)

### Requirements
- .NET Framework 4.8 required: [download here](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48)
- Use the `Offline installer` -> `Run apps - runtime` link for the above
- Should work on Windows Vista, 7, 8, 10, 11 (v1.3.2 tested only on Windows 11 in a VM)
- You might need to build it yourself for your specific Windows install (see below)

### Building
- Download [Visual Studio](https://visualstudio.microsoft.com/downloads/)
- Install above and select the .NET desktop apps component when asked
- Open `ScreenTask.sln` and change to `Release` (instead of `Debug`) and then `Run`

### Features
- Share your screen on local network
- WebUI so no additional client software needed - access via web browser
- Basic HTTP authentication support
- Unlimited number clients
- Multiple screens support

### v1.3.2 changes
- show main window on start instead of just the system tray icon
- fix JS; was halting execution due to legacy code that wasn't removed, fullscreen didn't work because of this

### v1.3.1 changes
- firewall rules created programmatically
- better tray icon logic
- new icon & server on/off status indicated in taskbar
- started cleaning up code
- cleaned up web ui, removed unneeded header and footer
- changed javascript timer to behave better on phone browsers
- memory leak fixed
- fixed saved ip bug

### License
Screen Task is released under the GPL v3 (or later) license: http://www.gnu.org/licenses/gpl-3.0.html

### Original project
https://github.com/EslaMx7/ScreenTask
