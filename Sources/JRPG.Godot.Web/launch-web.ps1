param(
	[int]$Port = 8080,
	[string]$Folder = 'AppBundle',
	[ValidateSet('Debug', 'Release')]
	[string]$Configuration = 'Debug'
)

$ErrorActionPreference = 'Stop'
$url = "http://localhost:$Port/"

dotnet publish JRPG.Godot.Web.csproj -c $Configuration
if ($LASTEXITCODE -ne 0) {
	exit $LASTEXITCODE
}

if (-not (Get-Command python -ErrorAction SilentlyContinue)) {
	Write-Error 'Python was not found on PATH. Install Python or add it to PATH.'
	exit 1
}

$chrome = (Get-Command chrome.exe -ErrorAction SilentlyContinue).Source
if (-not $chrome) {
	$candidates = @(
		"$env:ProgramFiles\Google\Chrome\Application\chrome.exe",
		"${env:ProgramFiles(x86)}\Google\Chrome\Application\chrome.exe",
		"$env:LOCALAPPDATA\Google\Chrome\Application\chrome.exe"
	)
	$chrome = $candidates | Where-Object { Test-Path $_ } | Select-Object -First 1
}
if (-not $chrome) {
	Write-Warning 'Chrome was not found. The server will start but the browser will not open.'
}
else {
	Start-Job -ArgumentList $chrome, $url, $Port -ScriptBlock {
		param($chromePath, $targetUrl, $listenPort)
		$deadline = (Get-Date).AddSeconds(15)
		while ((Get-Date) -lt $deadline) {
			$client = New-Object System.Net.Sockets.TcpClient
			try {
				$client.Connect('127.0.0.1', $listenPort)
				$client.Close()
				break
			}
			catch {
				Start-Sleep -Milliseconds 300
			}
			finally {
				$client.Dispose()
			}
		}
		$profileDir = Join-Path $env:TEMP 'jrpg-chrome-profile'
		$prefsDir = Join-Path $profileDir 'Default'
		$prefsFile = Join-Path $prefsDir 'Preferences'
		if (-not (Test-Path $prefsFile)) {
			New-Item -ItemType Directory -Path $prefsDir -Force | Out-Null
			Set-Content -Path $prefsFile -Value '{"devtools":{"preferences":{"panel-selectedTab":"\"console\""}}}' -Encoding ASCII
		}
		Start-Process -FilePath $chromePath -ArgumentList '--incognito', '--start-maximized', '--auto-open-devtools-for-tabs', "--user-data-dir=`"$profileDir`"", '--no-first-run', $targetUrl
	} | Out-Null
}

& .\serve-web.ps1 -Port $Port -Folder $Folder
