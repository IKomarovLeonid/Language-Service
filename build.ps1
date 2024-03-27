# Globals
$currentDirectory = Get-Location
$frontendDirectory = Join-Path -Path $currentDirectory -ChildPath "frontend"
$solutionFileName = "Language-Service.sln"
$dotnetBuildResultPath = "src\app\API\bin\Debug\net5.0"
$frontendBuildResultPath = "frontend\dist\language-Service"
$outputFrontendRootPath = "output\wwwroot"

# solution path
$solutionPath = Join-Path -Path $currentDirectory -ChildPath $solutionFileName

# Output folder path
$outputFolder = Join-Path -Path $currentDirectory -ChildPath "output"

# create output if not exists
if (-not (Test-Path $outputFolder -PathType Container)) {
    New-Item -Path $outputFolder -ItemType Directory
}
else {
    # Clear output folder every time
    Remove-Item -Path $outputFolder\* -Recurse -Force
}

# Build backend
dotnet clean $solutionPath

dotnet build $solutionPath 

# Build frontend
cd $frontendDirectory
npm install
ng build

cd $currentDirectory

Write-Host "Copy to output directory backend build result..."

$buildOutputFolder = Join-Path -Path $currentDirectory -ChildPath $dotnetBuildResultPath
Copy-Item -Path "$buildOutputFolder\*" -Destination $outputFolder -Recurse -Force

Write-Host "Done"

Write-Host "Copy to output directory frontend build result..."

$buildOutputFolder = Join-Path -Path $currentDirectory -ChildPath $frontendBuildResultPath
Copy-Item -Path "$buildOutputFolder\*" -Destination $outputFrontendRootPath -Recurse -Force

Write-Host "Done"