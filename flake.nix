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
        pkgs.buildDotnetModule rec
        {
          # Someone help me to figure out the way to get the version normally or make dotnet see .git directory
          packageVersion = builtins.readFile (pkgs.runCommand "extract-version" { } ''
            grep -oP '<Version>\K[^<]+' ${self}/${projectFile} > $out
          '');

          name = "FallGuys-CMSTool";
          version = "${packageVersion}+${self.rev or self.dirtyRev}";

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

          shellHook = ''
            export LD_LIBRARY_PATH="${pkgs.lib.makeLibraryPath (with pkgs; [ fontconfig xorg.libX11 xorg.libICE xorg.libSM ])}"
          '';
        };
    };
}
