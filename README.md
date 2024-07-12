# ZeroEPUB
A good EPUB reader application for Windows and Linux.

Made with the Avalonia UI framework.

If you stumbled across this, hello. You can clone and help.

Requirements:
* Visual Studio Community 2022
* Avalonia UI Extension
* .NET 8.0. 7.0 might be needed also.

Here are the issues that exist:

* Crashes when loading file
  * Issue has been pointed down to the parser for contents. It's in the `GetContents` function of [`ZeroEPUB/EpubOpener.cs`](https://github.com/HydeZero/ZeroEPUB/blob/main/ZeroEPUB/EpubOpener.cs)

## Currently being made. Releases aren't available.
