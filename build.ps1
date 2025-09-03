
# Check if dotnet is installed
$dotnetPath = Get-Command dotnet -ErrorAction SilentlyContinue
if (-not $dotnetPath) {
    Write-Host "dotnet is not installed or not found. Please install dotnet and try again."
    break
}

# Check if npm is installed
$npmPath = Get-Command npm -ErrorAction SilentlyContinue
if (-not $npmPath) {
    Write-Host "npm is not installed or not found. Please install npm and try again."
    break
}


# Globals
$currentDirectory = Get-Location
$frontendDirectory = Join-Path -Path $currentDirectory -ChildPath "frontend"
$solutionFileName = "Language-Service.sln"
$dotnetBuildResultPath = "src\app\API\bin\Release\net6.0"
$frontendBuildResultPath = "frontend"
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
if (-not (Test-Path $outputFrontendRootPath -PathType Container)) {
    New-Item -Path $outputFrontendRootPath -ItemType Directory
}

# Build backend
dotnet clean $solutionPath

dotnet build $solutionPath /p:Configuration=Release

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