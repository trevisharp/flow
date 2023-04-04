$csproj = gc .\flow.csproj
$versionText = $csproj | % {
    if ($_.Contains("PackageVersion"))
    {
        $_
    }
}

$version = ""
$flag = 0
for ($i = 0; $i -lt $versionText.Length; $i++)
{
    $char = $versionText[$i]

    if ($flag -eq 1)
    {
        if ($char -eq "<")
        {
            break
        }

        $version += $char
    }

    if ($char -eq ">")
    {
        $flag = 1
    }
}

dotnet pack -c Release
$file = ".\bin\Release\flow." + $version + ".nupkg"
cp $file flowing.nupkg

$key = gc .\.env

dotnet nuget push flowing.nupkg --api-key $key --source https://api.nuget.org/v3/index.json
rm .\flowing.nupkg