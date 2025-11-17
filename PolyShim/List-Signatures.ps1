#!/usr/bin/env pwsh
param(
    [string]$OutputFile = "$PSScriptRoot/Signatures.md"
)

$csFiles = Get-ChildItem -Path $PSScriptRoot -Filter "*.cs" -Recurse |
    Where-Object { $_.FullName -notmatch '\\obj\\|\\bin\\' }

$polyfills = @()

# Converts framework directory name to display name
function Get-FrameworkName {
    param([string]$dirName)

    # Pattern: NetXY or NetXYZ -> .NET major.minor (minor is always last digit)
    # Pattern: NetCoreXY -> .NET Core X.Y
    if ($dirName -match '^Net(\d+)(\d)$') {
        # NetXY format: last digit is minor, everything before is major
        # Examples: Net60 -> .NET 6.0, Net100 -> .NET 10.0
        $major = $matches[1]
        $minor = $matches[2]
        return ".NET $major.$minor"
    }
    elseif ($dirName -match '^NetCore(\d)(\d)$') {
        # NetCoreXY format (e.g., NetCore21 -> .NET Core 2.1)
        $major = $matches[1]
        $minor = $matches[2]
        return ".NET Core $major.$minor"
    }

    return $null
}

# Cleans parameter names from signature
function Clean-Signature {
    param([string]$sig)

    # Remove leading/trailing whitespace within parentheses
    $sig = $sig -replace '\(\s+', '(' -replace '\s+\)', ')'

    # Remove default values first (= value)
    $sig = $sig -replace '\s*=\s*[^,)]+', ''

    # Find the parameter list by matching balanced parentheses
    if ($sig -match '(.+?)\((.+)\)(.*)$') {
        $before = $matches[1]
        $paramList = $matches[2]
        $after = $matches[3]

        # Remove all attributes [...] - handle nested parens inside brackets
        # But NOT array brackets like byte[]
        # Attributes always have content and usually start with uppercase letter
        while ($paramList -match '\[[A-Z][^\[\]]*\]') {
            $paramList = $paramList -replace '\[[A-Z][^\[\]]*\]\s*', ''
        }

        # Smart split by comma (avoid commas inside parens/angles)
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

        # Clean each parameter - remove the last word (parameter name)
        $cleanedParams = $params | ForEach-Object {
            $param = $_
            # Match everything up to the last word (which is the param name)
            if ($param -match '^(.+)\s+\w+$') {
                $matches[1].Trim()
            } else {
                $param
            }
        }

        $sig = $before + '(' + ($cleanedParams -join ', ') + ')' + $after
    }

    return $sig
}

foreach ($file in $csFiles) {
    $content = Get-Content $file.FullName -Raw
    $relativePath = $file.FullName.Replace($PSScriptRoot, '').TrimStart('\', '/')

    # Extract framework from path
    $framework = $null
    if ($relativePath -match '([^\\]+)\\[^\\]+\.cs$') {
        $dirName = $matches[1]
        $framework = Get-FrameworkName $dirName
    }

    # Skip files not in framework directories
    if (-not $framework) {
        continue
    }

    # Extract extension members (extension(TypeName) blocks)
    $extensionStartPattern = 'extension(?:<[^>]+>)?\(([^)]+)\)\s*\{'
    $extensionMatches = [regex]::Matches($content, $extensionStartPattern)

    foreach ($match in $extensionMatches) {
        $typeNameRaw = $match.Groups[1].Value.Trim()

        # Remove receiver variable name
        # Pattern: Type<...> variableName or Type variableName
        # Try generic case first (Type<...> variableName)
        if ($typeNameRaw -match '^(.+>)\s+(\w+)$') {
            $typeName = $matches[1]
        } elseif ($typeNameRaw -match '^(.+?)\s+(\w+)$') {
            # Simple case: Type variableName (no angle brackets)
            $typeName = $matches[1]
        } else {
            $typeName = $typeNameRaw
        }

        # Validate extracted type name
        # Skip if type name is empty or contains preprocessor directives
        # Allow whitespace inside generic parameters (e.g., KeyValuePair<TKey, TValue>)
        if ([string]::IsNullOrWhiteSpace($typeName) -or $typeName -match '#if|#else|#endif') {
            $relativeFilePath = $relativePath.TrimStart('\', '/')
            Write-Warning "Skipped signature in '$relativeFilePath'. Unable to extract type information."
            continue
        }

        # Additional check: if type has whitespace outside angle brackets, it's invalid
        $typeWithoutGenerics = $typeName -replace '<[^>]+>', ''
        if ($typeWithoutGenerics -match '\s') {
            $relativeFilePath = $relativePath.TrimStart('\', '/')
            Write-Warning "Skipped signature in '$relativeFilePath'. Unable to extract type information."
            continue
        }

        # Find the matching closing brace
        $startPos = $match.Index + $match.Length
        $braceCount = 1
        $endPos = $startPos

        while ($endPos -lt $content.Length -and $braceCount -gt 0) {
            $char = $content[$endPos]
            if ($char -eq '{') { $braceCount++ }
            elseif ($char -eq '}') { $braceCount-- }
            $endPos++
        }

        $block = $content.Substring($startPos, $endPos - $startPos - 1)

        # Extract method/property signatures
        $lines = $block -split '\r?\n'
        $currentSignature = ''
        $currentUrl = $null
        $inSignature = $false

        for ($i = 0; $i -lt $lines.Count; $i++) {
            $line = $lines[$i]

            # Check for documentation URL comment
            if ($line -match '//\s*(https://(?:learn\.microsoft\.com|docs\.microsoft\.com)[^\s]+)') {
                $currentUrl = $matches[1]
            }

            if ($line -match '^\s*(public|private|protected|internal)\s+') {
                # Save previous signature
                if ($currentSignature) {
                    $sig = $currentSignature.Trim() -replace '\s+', ' '
                    $sig = Clean-Signature $sig
                    $polyfills += [PSCustomObject]@{
                        Type = $typeName
                        Member = $sig
                        Kind = 'Extension'
                        Framework = $framework
                        Url = $currentUrl
                    }
                }

                # Start new signature - extract everything after modifiers
                $currentSignature = $line -replace '^\s*(public|private|protected|internal)\s+(?:static\s+)?(?:readonly\s+)?(?:async\s+)?', ''
                $inSignature = $true

                # Check if signature ends on this line
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
            $sig = $currentSignature.Trim() -replace '\s+', ' '
            $sig = Clean-Signature $sig
            $polyfills += [PSCustomObject]@{
                Type = $typeName
                Member = $sig
                Kind = 'Extension'
                Framework = $framework
                Url = $currentUrl
            }
        }
    }

    # Extract type declarations
    # Pattern captures: class/struct/etc followed by type name (with optional generics)
    $typePattern = '(?:public|internal)\s+(?:readonly\s+|partial\s+|static\s+|sealed\s+)*(class|enum|struct|interface|record)\s+(\w+(?:<[^>]+>)?)'
    $typeMatches = [regex]::Matches($content, $typePattern)

    foreach ($match in $typeMatches) {
        $typeKind = $match.Groups[1].Value
        $typeName = $match.Groups[2].Value

        # Skip PolyfillExtensions classes
        if ($typeName -match '^PolyfillExtensions\d*$') {
            continue
        }

        # Look for documentation URL comment before type declaration
        # Search backwards through lines, skipping attributes and other non-comment lines
        $lines = $content.Substring(0, $match.Index) -split '\r?\n'
        $typeUrl = $null
        for ($i = $lines.Count - 1; $i -ge 0 -and $i -ge $lines.Count - 20; $i--) {
            $trimmedLine = $lines[$i].Trim()
            # Found URL comment
            if ($trimmedLine -match '^//\s*(https://[^\s]+)') {
                $typeUrl = $matches[1]
                break
            }
            # Stop if we hit another type/method declaration or namespace
            if ($trimmedLine -match '^(public|internal|private|protected)\s+(class|struct|enum|interface|record|static|readonly)' -or
                $trimmedLine -match '^namespace\s+') {
                break
            }
        }

        $polyfills += [PSCustomObject]@{
            Type = $typeName
            Member = ''
            Kind = $typeKind
            Framework = $framework
            Url = $typeUrl
        }
    }
}

# Deduplicate type definitions
$deduplicatedPolyfills = @()
$seenTypeDefinitions = @{}

foreach ($item in $polyfills | Sort-Object -Property Type, Member) {
    if ($item.Kind -eq 'Extension') {
        $deduplicatedPolyfills += $item
    } else {
        $key = "$($item.Type)|$($item.Kind)"
        if (-not $seenTypeDefinitions.ContainsKey($key)) {
            $seenTypeDefinitions[$key] = $item
            $deduplicatedPolyfills += $item
        } elseif ($item.Url -and -not $seenTypeDefinitions[$key].Url) {
            # Replace existing entry if current one has URL and existing doesn't
            $existingIndex = $deduplicatedPolyfills.IndexOf($seenTypeDefinitions[$key])
            $deduplicatedPolyfills[$existingIndex] = $item
            $seenTypeDefinitions[$key] = $item
        }
    }
}

# Generate markdown
$markdown = @()
$markdown += "# PolyShim Signatures"
$markdown += ""
$markdown += "- **Total:** $($deduplicatedPolyfills.Count)"
$markdown += "- **Types:** $(($deduplicatedPolyfills | Where-Object { $_.Kind -ne 'Extension' }).Count)"
$markdown += "- **Members:** $(($deduplicatedPolyfills | Where-Object { $_.Kind -eq 'Extension' }).Count)"
$markdown += ""
$markdown += "## Signatures"
$markdown += ""

$groupedByType = $deduplicatedPolyfills | Group-Object -Property Type | Sort-Object Name

foreach ($typeGroup in $groupedByType) {
    $typeName = $typeGroup.Name

    # Type name is always plain text (not a link)
    $markdown += "- ``$typeName``"

    foreach ($item in $typeGroup.Group) {
        if ($item.Member) {
            # Extension method/property
            if ($item.Url) {
                $markdown += "  - [``$($item.Member)``]($($item.Url)) <sup><sub>$($item.Framework)</sub></sup>"
            } else {
                $markdown += "  - ``$($item.Member)`` <sup><sub>$($item.Framework)</sub></sup>"
            }
        } else {
            # Type definition marker
            if ($item.Url) {
                $markdown += "  - [**[$($item.Kind)]**]($($item.Url)) <sup><sub>$($item.Framework)</sub></sup>"
            } else {
                $markdown += "  - **[$($item.Kind)]** <sup><sub>$($item.Framework)</sub></sup>"
            }
        }
    }
}

# Write to file
$markdown | Out-File -FilePath $OutputFile -Encoding UTF8

# Console summary
Write-Host "Generated signature list: $OutputFile" -ForegroundColor Green
Write-Host "Total: $($deduplicatedPolyfills.Count)" -ForegroundColor Cyan
Write-Host "Types: $(($deduplicatedPolyfills | Where-Object { $_.Kind -ne 'Extension' }).Count)" -ForegroundColor Yellow
Write-Host "Members: $(($deduplicatedPolyfills | Where-Object { $_.Kind -eq 'Extension' }).Count)" -ForegroundColor Yellow
