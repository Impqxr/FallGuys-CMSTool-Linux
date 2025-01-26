# Fall Guys CMS Tool
<div align="center">
    <img src="https://github.com/floyzi/FallGuys-CMSTool/blob/master/Assets/GithubImages/Screenshot1.png" alt="How it looks">
</div>
<p align="center">An easy to use tool to decrypt and encrypt Fall Guys content files.<br>Support both, content_v1 & content_v2 versions</p>

## Features
- Content decryption in different ways 
- Custom XorKey 
- Encryption in both ways. Content_v1 & content_v2

## Installation
#### Windows
- Extract [latest release](https://github.com/floyzi/FallGuys-CMSTool/releases/latest) into the blank folder and launch the .exe
- Or [compile it](#Windows-1) by yourself
#### Linux (NixOS)
- Installing the package directly in your NixOS configuration with flakes
```nix
{
  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";

    cmstool = {
      url = "github:Impqxr/FallGuys-CMSTool-Linux";
    };
  };

  outputs =
    { nixpkgs, cmstool, ... }:
    {
      nixosConfigurations.nixos = nixpkgs.lib.nixosSystem {
        modules = [
          ./configuration.nix

          (
            { pkgs, ... }:
            {
              environment.systemPackages = [ cmstool.packages.${pkgs.system} ];
            }
          )
        ];
      };
    };
}
```
- Installing the package directly in your NixOS configuration without flakes
```nix
{ pkgs, ... }:
{
  environment.systemPackages = [
    (import (
      builtins.fetchTarball "https://github.com/Impqxr/FallGuys-CMSTool-Linux/archive/linux.tar.gz"
    )).packages.${pkgs.system}
  ];
}
```
#### Linux (NixOS or any distribution with Nix package manager)
- If you have [enabled flakes](https://wiki.nixos.org/wiki/Flakes#Enable_flakes_permanently_in_NixOS), run one of these commands either to run or install it
```shell
# To run it
nix run github:Impqxr/FallGuys-CMSTool-Linux

# To install it
nix profile install github:Impqxr-CMSTool-Linux
```
- If you prefer without flakes, run these commands to install it
```shell
nix-channel --add https://github.com/Impqxr/FallGuys-CMSTool-Linux/archive/linux.tar.gz cmstool
nix-channel --update cmstool

nix-env -iA cmstool
```

#### Linux (without Nix package manager)
- Extract [latest release](https://github.com/floyzi/FallGuys-CMSTool/releases/latest) into the blank folder and launch the executable `CMSTool`
- Or [compile it](#Linux) by yourself

## Compilation
#### Windows
1. Install [Git for Windows](https://git-scm.com/downloads/win) and [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- You can do this easily with one command in PowerShell or manually
```shell
winget install Microsoft.DotNet.SDK.8 Git.Git --source winget
```
2. Now compile it with these commands in PowerShell!
```shell
git clone https://github.com/Impqxr/FallGuys-CMSTool-Linux.git; cd FallGuys-CMSTool-Linux
dotnet publish -c Release-Win-x64 --self-contained -p:PublishSingleFile=true -p:PublishTrimmed=true
```
3. Output files will be located in `.\CMSTool\bin\Release-Win-x64\net8.0\win-x64\publish`
   
#### Linux
1. Install [.NET SDK 8.0](https://learn.microsoft.com/en-us/dotnet/core/install/linux) and Git like this
- Ubuntu
```shell
apt install dotnet-sdk-8.0 git
```
- Fedora
```shell
dnf install dotnet-sdk-8.0 git
```
- Arch Linux
```shell
pacman -S dotnet-sdk-8.0 git
```
2. Now compile it with these commands!
```shell
git clone https://github.com/Impqxr/FallGuys-CMSTool-Linux.git && cd FallGuys-CMSTool-Linux
dotnet publish -c Release-Linux-x64 --self-contained -p:PublishSingleFile=true -p:PublishTrimmed=true
```
3. Output files will be located in `./CMSTool/bin/Release-Linux-x64/net-8.0/linux-x64/publish`

## Usage
### How to decrypt
- Select `content_v2.gdata` or `content_v1` file (can be found in `AppData/LocalLow/Mediatonic/FallGuys_client/`)
- Hit the `"Decrypt"` button and wait for a moment
- Once done check the `"Decrypted_Output"` folder (`Open -> Decoded Output`) for the decrypted content file

### How to encrypt
- Select `.json` file of your content (or `_meta.json` if you decrypted content into parts)
- Hit the `"Encrypt"` button and wait for a moment 
- Once done check the `"Encrypted_Output"` folder (`Open -> Encoded Output`). There you'll find the `content_v2.gdata` file (or `content_v1`, if you selected it in settings)

## Credits 
- Made using [Avalonia UI](https://github.com/AvaloniaUI/Avalonia)
- Some UI code were taken from [this repository](https://github.com/M0n7y5/FenixProFmod)
