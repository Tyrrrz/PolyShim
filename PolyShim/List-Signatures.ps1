#!/usr/bin/env pwsh
param(
    [string]$OutputPath = $PSScriptRoot
)

# If the output path is a directory, append the default filename
if (Test-Path $OutputPath -PathType Container) {
    $OutputPath = Join-Path $OutputPath "Signatures.md"
}

$codeFiles = Get-ChildItem -Path $PSScriptRoot -Filter "*.cs" -Recurse |
    Where-Object { $_.FullName -notmatch '\\obj\\|\\bin\\' }

$signatures = @()

# Converts framework directory name to display name
function Get-FrameworkName {
    param([string]$dirName)

    if ($dirName -match '^Net(\d+)(\d)$') {
        return ".NET $($matches[1]).$($matches[2])"
    }
    elseif ($dirName -match '^NetCore(\d)(\d)$') {
        return ".NET Core $($matches[1]).$($matches[2])"
    }

    return $null
}

# Finds documentation URL in preceding lines before a given position
function Find-DocumentationUrl {
    param([string]$content, [int]$position, [int]$maxLinesToSearch = 20)

    $lines = $content.Substring(0, $position) -split '\r?\n'

    for ($i = $lines.Count - 1; $i -ge 0 -and $i -ge $lines.Count - $maxLinesToSearch; $i--) {
        $trimmedLine = $lines[$i].Trim()

        # Found URL comment
        if ($trimmedLine -match '^//\s*(https://(?:learn\.microsoft\.com|docs\.microsoft\.com)\S+)') {
            return $matches[1]
        }

        # Stop if we hit another declaration or namespace
        if ($trimmedLine -match '^(public|internal|private|protected)\s+(class|struct|enum|interface|record|static|readonly)' -or
            $trimmedLine -match '^namespace\s+') {
            break
        }
    }

    return $null
}

# Finds matching closing brace for a given opening brace position
function Find-ClosingBrace {
    param([string]$content, [int]$startPos)

    $braceCount = 1
    $pos = $startPos

    while ($pos -lt $content.Length -and $braceCount -gt 0) {
        $char = $content[$pos]
        if ($char -eq '{') { $braceCount++ }
        elseif ($char -eq '}') { $braceCount-- }
        $pos++
    }

    return $pos
}

# Splits parameters by comma, respecting nested brackets/parens
function Split-Parameters {
    param([string]$paramList)

    $params = @()
    $current = ''
    $depth = 0

    for ($i = 0; $i -lt $paramList.Length; $i++) {
        $char = $paramList[$i]
        if ($char -eq '(' -or $char -eq '<') {
            $depth++
            $current += $char
        }
        elseif ($char -eq ')' -or $char -eq '>') {
            $depth--
            $current += $char
        }
        elseif ($char -eq ',' -and $depth -eq 0) {
            $params += $current.Trim()
            $current = ''
        }
        else {
            $current += $char
        }
    }

    if ($current.Trim()) {
        $params += $current.Trim()
    }

    return $params
}

# Cleans parameter names from signature
function Clean-Signature {
    param([string]$sig)

    # Remove leading/trailing whitespace within parentheses
    $sig = $sig -replace '\(\s+', '(' -replace '\s+\)', ')'

    # Remove default values (= value)
    $sig = $sig -replace '\s*=\s*[^,)]+', ''

    # Find the parameter list by matching balanced parentheses
    if ($sig -notmatch '(.+?)\((.+)\)(.*)$') {
        return $sig
    }

    $before = $matches[1]
    $paramList = $matches[2]
    $after = $matches[3]

    # Remove attributes [...] but NOT array brackets like byte[]
    while ($paramList -match '\[[A-Z][^\[\]]*\]') {
        $paramList = $paramList -replace '\[[A-Z][^\[\]]*\]\s*', ''
    }

    # Split parameters and remove parameter names
    $params = Split-Parameters $paramList
    $cleanedParams = $params | ForEach-Object {
        # Match everything up to the last word (which is the param name)
        if ($_ -match '^(.+)\s+\w+$') {
            $matches[1].Trim()
        } else {
            $_
        }
    }

    return $before + '(' + ($cleanedParams -join ', ') + ')' + $after
}

# Validates type name extracted from extension block
function Test-TypeName {
    param([string]$typeName, [string]$relativePath, [int]$lineNumber)

    # Check for empty, preprocessor directives, or whitespace outside generics
    $invalid = [string]::IsNullOrWhiteSpace($typeName) -or
                $typeName -match '#if|#else|#endif' -or
                ($typeName -replace '<[^>]+>', '') -match '\s'

    if ($invalid) {
        Write-Warning "Unable to extract type information on line $lineNumber in '$relativePath'."
        return $false
    }

    return $true
}

# Extracts extension members from an extension block
function Get-ExtensionMembers {
    param(
        [string]$block,
        [string]$typeName,
        [string]$framework,
        [string]$relativePath
    )

    $members = @()
    $currentSignature = ''
    $currentUrl = $null
    $inSignature = $false

    $lines = $block -split '\r?\n'
    for ($lineIndex = 0; $lineIndex -lt $lines.Count; $lineIndex++) {
        $line = $lines[$lineIndex]

        # Check if this is a public declaration (extensions must be public)
        if ($line -match '^\s*public\s+') {
            # Save previous signature if exists
            if ($currentSignature) {
                $cleanedSig = Clean-Signature ($currentSignature.Trim() -replace '\s+', ' ')

                if (-not $currentUrl) {
                    Write-Warning "Missing documentation URL for '$typeName.$cleanedSig' in '$relativePath'."
                }

                $members += [PSCustomObject]@{
                    Type = $typeName
                    Member = $cleanedSig
                    Kind = 'Extension'
                    Framework = $framework
                    Url = $currentUrl
                }
            }

            # Find documentation URL for this member (search backwards from current line)
            $currentUrl = $null
            for ($i = $lineIndex - 1; $i -ge 0 -and $i -ge $lineIndex - 5; $i--) {
                if ($lines[$i] -match '^\s*//\s*(https://(?:learn\.microsoft\.com|docs\.microsoft\.com)\S+)') {
                    $currentUrl = $matches[1]
                    break
                }
            }

            # Start new signature
            $currentSignature = $line -replace '^\s*public\s+(?:static\s+)?(?:readonly\s+)?(?:async\s+)?', ''
            $inSignature = $true

            # Check if signature ends on same line
            if ($currentSignature -match '^(.+?)(?:\s*\{|\s*=>)') {
                $currentSignature = $matches[1]
                $inSignature = $false
            }
        }
        elseif ($inSignature) {
            $trimmedLine = $line.Trim()

            # Check if signature ends
            if ($trimmedLine -match '^[\{\}]' -or $trimmedLine -match '^=>') {
                $inSignature = $false
            }
            elseif ($trimmedLine -match '^(.+?)(?:\s*\{|\s*=>)') {
                $currentSignature += ' ' + $matches[1]
                $inSignature = $false
            }
            elseif ($trimmedLine) {
                $currentSignature += ' ' + $trimmedLine
            }
        }
    }

    # Save last signature
    if ($currentSignature) {
        $cleanedSig = Clean-Signature ($currentSignature.Trim() -replace '\s+', ' ')

        if (-not $currentUrl) {
            Write-Warning "Missing documentation URL for '$typeName.$cleanedSig' in '$relativePath'."
        }

        $members += [PSCustomObject]@{
            Type = $typeName
            Member = $cleanedSig
            Kind = 'Extension'
            Framework = $framework
            Url = $currentUrl
        }
    }

    return $members
}

# Extracts type name from extension declaration
function Get-ExtensionTypeName {
    param([string]$typeNameRaw)

    # Remove receiver variable name: Type<...> variableName or Type variableName
    if ($typeNameRaw -match '^(.+?[>}])\s+\w+$' -or $typeNameRaw -match '^(.+?)\s+\w+$') {
        return $matches[1]
    }

    return $typeNameRaw
}

# Deduplicates type definitions, keeping the one with URL if available
function Remove-DuplicateSignatures {
    param([array]$signatures)

    $deduplicated = @()
    $seenTypes = @{}

    foreach ($item in $signatures | Sort-Object -Property Type, Member) {
        # Extensions are never deduplicated
        if ($item.Kind -eq 'Extension') {
            $deduplicated += $item
            continue
        }

        # For type definitions, keep first or replace with one that has URL
        $key = "$($item.Type)|$($item.Kind)"
        if (-not $seenTypes.ContainsKey($key)) {
            $seenTypes[$key] = $item
            $deduplicated += $item
        }
        elseif ($item.Url -and -not $seenTypes[$key].Url) {
            # Replace if new one has URL and old one doesn't
            $existingIndex = $deduplicated.IndexOf($seenTypes[$key])
            $deduplicated[$existingIndex] = $item
            $seenTypes[$key] = $item
        }
        elseif (-not $item.Url -and $seenTypes[$key].Url) {
            # Current item has no URL but existing one does, update FilePath to track all locations
            # Keep the one with URL
            continue
        }
    }

    return $deduplicated
}

foreach ($file in $codeFiles) {
    $content = Get-Content $file.FullName -Raw
    $relativePath = $file.FullName.Replace($PSScriptRoot, '').TrimStart('\', '/')

    # Extract framework from path
    $framework = $null
    if ($relativePath -match '([^\\]+)\\[^\\]+\.cs$') {
        $framework = Get-FrameworkName $matches[1]
    }

    # Skip files not in framework directories
    if (-not $framework) {
        continue
    }

    # Extract extension members
    $extensionPattern = 'extension(?:<[^>]+>)?\(([^)]+)\)\s*\{'
    $extensionMatches = [regex]::Matches($content, $extensionPattern)

    foreach ($match in $extensionMatches) {
        $typeName = Get-ExtensionTypeName $match.Groups[1].Value.Trim()

        # Calculate line number for error reporting
        $lineNumber = ($content.Substring(0, $match.Index) -split '\r?\n').Count

        # Validate type name
        if (-not (Test-TypeName $typeName $relativePath $lineNumber)) {
            continue
        }

        # Find extension block and extract members
        $endPos = Find-ClosingBrace $content ($match.Index + $match.Length)
        $block = $content.Substring($match.Index + $match.Length, $endPos - $match.Index - $match.Length - 1)

        $signatures += Get-ExtensionMembers $block $typeName $framework $relativePath
    }

    # Extract type declarations (must be internal)
    $typePattern = 'internal\s+(?:readonly\s+|partial\s+|static\s+|sealed\s+)*(class|enum|struct|interface|record)\s+(\w+(?:<[^>]+>)?)'
    $typeMatches = [regex]::Matches($content, $typePattern)

    foreach ($match in $typeMatches) {
        $typeKind = $match.Groups[1].Value
        $typeName = $match.Groups[2].Value

        # Skip PolyfillExtensions classes
        if ($typeName -match '^PolyfillExtensions\d*$') {
            continue
        }

        $typeUrl = Find-DocumentationUrl $content $match.Index

        $signatures += [PSCustomObject]@{
            Type = $typeName
            Member = ''
            Kind = $typeKind
            Framework = $framework
            Url = $typeUrl
            FilePath = $relativePath
        }
    }
}

# Deduplicate signatures
$deduplicatedSignatures = Remove-DuplicateSignatures $signatures

# Check for types missing documentation URLs (after deduplication)
$typesWithoutUrls = $deduplicatedSignatures |
    Where-Object { $_.Kind -ne 'Extension' -and -not $_.Url } |
    Select-Object -Property Type, FilePath

foreach ($type in $typesWithoutUrls) {
    Write-Warning "Missing documentation URL for type '$($type.Type)' in '$($type.FilePath)'."
}

# Calculate statistics
$stats = @{
    Total = $deduplicatedSignatures.Count
    Types = ($deduplicatedSignatures | Where-Object { $_.Kind -ne 'Extension' }).Count
    Members = ($deduplicatedSignatures | Where-Object { $_.Kind -eq 'Extension' }).Count
}

# Generate markdown header
$markdown = @(
    "# Signatures"
    ""
    "- **Total:** $($stats.Total)"
    "- **Types:** $($stats.Types)"
    "- **Members:** $($stats.Members)"
    ""
    "___"
    ""
)

# Generate signature listings
$groupedByType = $deduplicatedSignatures | Group-Object -Property Type | Sort-Object Name

foreach ($typeGroup in $groupedByType) {
    $markdown += "- ``$($typeGroup.Name)``"

    foreach ($item in $typeGroup.Group) {
        $frameworkTag = " <sup><sub>$($item.Framework)</sub></sup>"
        $content = if ($item.Member) { "``$($item.Member)``" } else { "**[$($item.Kind)]**" }

        $markdown += if ($item.Url) {
            "  - [$content]($($item.Url))$frameworkTag"
        } else {
            "  - $content$frameworkTag"
        }
    }
}

# Write output
$markdown | Out-File -FilePath $OutputPath -Encoding UTF8

# Console summary
Write-Host "Generated signature list: $OutputPath" -ForegroundColor Green
Write-Host "Total: $($stats.Total)" -ForegroundColor Cyan
Write-Host "Types: $($stats.Types)" -ForegroundColor Yellow
Write-Host "Members: $($stats.Members)" -ForegroundColor Yellow
