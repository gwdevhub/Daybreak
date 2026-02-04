# Daybreak

<img width="754" height="716" alt="image" src="https://github.com/user-attachments/assets/3eb74d22-a3ac-4463-8d26-8142cea3d237" />

Custom launcher for Guildwars.
Requires webview2 runtime <https://go.microsoft.com/fwlink/p/?LinkId=2124703>.

## Download

- Go to [Releases](https://github.com/AlexMacocian/Daybreak/releases/latest)
- Download daybreakv[VERSION].zip, where [VERSION] is the version of the release

## Features

Please check the [wiki](https://github.com/AlexMacocian/Daybreak/wiki) for
project description and features

## Build requirements

## Windows

- .NET 10 SDK
- ASP.NET Core Runtime
- WebView2 Runtime (<https://go.microsoft.com/fwlink/p/?LinkId=2124703>)

## Linux (Arch)

```bash
# Install .NET SDK and ASP.NET Core runtime
sudo pacman -S dotnet-sdk aspnet-runtime
```

## Linux (Ubuntu/Debian)

```bash
# Add Microsoft package repository
wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb \
  -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Install .NET SDK and ASP.NET Core runtime
sudo apt update
sudo apt install -y dotnet-sdk-10.0 aspnetcore-runtime-10.0
```

## Linux (Fedora)

```bash
sudo dnf install dotnet-sdk-10.0 aspnetcore-runtime-10.0
```

After installation, verify with:

```bash
dotnet --list-sdks
dotnet --list-runtimes
```

## Credits

- Daybreak project is distributed under [MIT license](https://mit-license.org/)
- Tango icons - [LordBiro](https://wiki.guildwars.com/wiki/User:LordBiro)
  - Icons `Daybreak/wwwroot/img/tango` are distributed under [GFDL license](https://en.wikipedia.org/wiki/GNU_Free_Documentation_License)
