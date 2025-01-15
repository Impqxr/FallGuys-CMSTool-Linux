{
  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs";
  };
  outputs = { self, nixpkgs, ... }:
    let
      pkgs = nixpkgs.legacyPackages.x86_64-linux;
    in
    {
      packages.x86_64-linux.default =
        pkgs.buildDotnetModule
          {
            name = "FallGuys-CMSTool";

            src = ./.;

            projectFile = "CMSTool/FallGuys-CMSTool.csproj";
            dotnet-sdk = pkgs.dotnetCorePackages.sdk_8_0;
            dotnet-runtime = pkgs.dotnetCorePackages.runtime_8_0;
            nugetDeps = ./deps.json;
            executables = [ "CMSTool" ];
          };

      apps.x86_64-linux.default = {
        type = "app";
        program = "${self.packages.x86_64-linux.default}/bin/CMSTool";
      };

      devShells.x86_64-linux.default =
        pkgs.mkShell {
          packages = with pkgs; [ dotnetCorePackages.sdk_8_0 nuget-to-json ];
        };
    };
}
