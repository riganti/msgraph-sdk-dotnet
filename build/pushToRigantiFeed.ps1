param([String]$version, [String]$apiKey = "LasVegasBlvd", [String]$server = "https://d41d8cd98f0-nuget.riganti.cz/nuget")

function SetVersion($directory, $version) {
    $filePath = "../src/$($directory)/$($directory).csproj"
    echo $filePath
    $file = [System.IO.File]::ReadAllText($filePath, [System.Text.Encoding]::UTF8)
    $file = [System.Text.RegularExpressions.Regex]::Replace($file, "\<VersionPrefix\>([^<]+)\</VersionPrefix\>", "<VersionPrefix>" + $version + "</VersionPrefix>")
    $file = [System.Text.RegularExpressions.Regex]::Replace($file, "\<PackageVersion\>([^<]+)\</PackageVersion\>", "<PackageVersion>" + $version + "</PackageVersion>")
    [System.IO.File]::WriteAllText($filePath, $file, [System.Text.Encoding]::UTF8)
}

function PackAndPublish($directory, $version) {
    cd ../src/$directory
    & dotnet pack | Out-Host
    & dotnet nuget push ./bin/debug/$directory.Beta.$version.nupkg -s $server -k $apiKey | Out-Host
    cd ../../build
}

SetVersion "Microsoft.Graph.Core" $version
PackAndPublish "Microsoft.Graph.Core" $version

SetVersion "Microsoft.Graph" $version
PackAndPublish "Microsoft.Graph" $version





