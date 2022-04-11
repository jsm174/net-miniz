# MiniZ for .NET

[![CI status (x64 Linux, macOS and Windows)](https://github.com/jsm174/net-miniz/actions/workflows/build.yml/badge.svg?branch=master)](https://github.com/jsm174/net-miniz/actions) 
[![NuGet](https://img.shields.io/nuget/vpre/NetMiniZ.svg)](https://www.nuget.org/packages/NetMiniZ)

*Add MiniZ support to any .NET application*

This NuGet package provides a .NET binding for [MiniZ](https://github.com/richgel999/miniz).

## Supported Platforms

- .NET Core (.NETStandard 2.1 and higher on Windows, Linux and macOS)
- Mono

## Setup

The native wrapper is a different package and contains pre-compiled binaries of libminiz.

|                       | NuGet Package                                                       |
|-----------------------|:-------------------------------------------------------------------:|
| **Windows 64-bit**    | [![NetMiniZ.Native.win-x64-badge]][NetMiniZ.Native.win-x64-nuget]     |
| **Windows 32-bit**    | [![NetMiniZ.Native.win-x86-badge]][NetMiniZ.Native.win-x86-nuget]     |
| **macOS x64**         | [![NetMiniZ.Native.osx-x64-badge]][NetMiniZ.Native.osx-x64-nuget]     |
| **macOS arm64**       | [![NetMiniZ.Native.osx-arm64-badge]][NetMiniZ.Native.osx-arm64-nuget] |
| **macOS (x64/arm64**) | [![NetMiniZ.Native.osx-badge]][NetMiniZ.Native.osx-nuget] |
| **Linux x64**         | [![NetMiniZ.Native.linux-x64-badge]][NetMiniZ.Native.linux-x64-nuget] |

[NetMiniZ.Native.win-x64-badge]: https://img.shields.io/nuget/vpre/NetMiniZ.Native.win-x64.svg
[NetMiniZ.Native.win-x64-nuget]: https://www.nuget.org/packages/NetMiniZ.Native.win-x64
[NetMiniZ.Native.win-x86-badge]: https://img.shields.io/nuget/vpre/NetMiniZ.Native.win-x86.svg
[NetMiniZ.Native.win-x86-nuget]: https://www.nuget.org/packages/NetMiniZ.Native.win-x86
[NetMiniZ.Native.osx-x64-badge]: https://img.shields.io/nuget/vpre/NetMiniZ.Native.osx-x64.svg
[NetMiniZ.Native.osx-x64-nuget]: https://www.nuget.org/packages/NetMiniZ.Native.osx-x64
[NetMiniZ.Native.osx-arm64-badge]: https://img.shields.io/nuget/vpre/NetMiniZ.Native.osx-arm64.svg
[NetMiniZ.Native.osx-arm64-nuget]: https://www.nuget.org/packages/NetMiniZ.Native.osx-arm64
[NetMiniZ.Native.osx-badge]: https://img.shields.io/nuget/vpre/NetMiniZ.Native.osx.svg
[NetMiniZ.Native.osx-nuget]: https://www.nuget.org/packages/NetMiniZ.Native.osx
[NetMiniZ.Native.linux-x64-badge]: https://img.shields.io/nuget/vpre/NetMiniZ.Native.linux-x64.svg
[NetMiniZ.Native.linux-x64-nuget]: https://www.nuget.org/packages/NetMiniZ.Native.linux-x64

To install this package with the native dependency of your current platform, run:

```
Install-Package NetMiniZ
Install-Package NetMiniZ-Native
```

## License

[MIT](LICENSE.txt)

