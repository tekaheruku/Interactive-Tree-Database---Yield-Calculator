<#
.SYNOPSIS
    Writes every source file of the solution into one text file (docs\SOURCE_CODE.txt),
    the "soft copy" of the complete code required for submission.

.DESCRIPTION
    Run it again after any code change so the soft copy always matches the code:

        powershell -File tools\Export-SourceCode.ps1

    (If Windows says that running scripts is disabled, use
        powershell -ExecutionPolicy Bypass -File tools\Export-SourceCode.ps1 )

    Included: the .sln, .vbproj, .vb (including *.Designer.vb), .sql, .config, .manifest,
    .myapp and .settings files of the Visual Basic solution in "Interactive Tree Database\".
    Build output (bin, obj, .vs) is skipped. The .resx files only contain the standard
    Visual Studio header and the .mdb database is a binary file, so they are listed but
    not printed.
#>

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$output = Join-Path $root 'docs\SOURCE_CODE.txt'

$extensions = '.sln', '.vbproj', '.vb', '.sql', '.config', '.manifest', '.myapp', '.settings'
$sourceRoot = Join-Path $root 'Interactive Tree Database'

$files = @(Get-ChildItem -Path $sourceRoot -Recurse -File |
    Where-Object { $extensions -contains $_.Extension.ToLowerInvariant() } |
    Where-Object { $_.FullName -notmatch '\\(bin|obj|\.vs)\\' } |
    Where-Object { $_.Name -notmatch '\.vshost\.' } |
    Sort-Object { $_.FullName.Substring($root.Length) })

$listedOnly = @(Get-ChildItem -Path $sourceRoot -Recurse -File -Include *.resx, *.mdb, *.ico |
    Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' } |
    Sort-Object FullName)

$builder = New-Object System.Text.StringBuilder
$rule = '=' * 100

[void]$builder.AppendLine($rule)
[void]$builder.AppendLine('INTERACTIVE TREE DATABASE & YIELD CALCULATOR - COMPLETE SOURCE CODE (SOFT COPY)')
[void]$builder.AppendLine('Visual Basic .NET Windows Forms (.NET Framework 4.5.2) + Microsoft Access database (.mdb) - Visual Studio 2015 solution')
[void]$builder.AppendLine('Generated: ' + (Get-Date -Format 'yyyy-MM-dd HH:mm'))
[void]$builder.AppendLine($rule)
[void]$builder.AppendLine()
[void]$builder.AppendLine('FILE INDEX')
$number = 0
$totalLines = 0
foreach ($file in $files) {
    $number++
    $lines = (Get-Content -LiteralPath $file.FullName).Count
    $totalLines += $lines
    [void]$builder.AppendLine(('{0,3}. {1}  ({2} lines)' -f $number, $file.FullName.Substring($root.Length + 1), $lines))
}
[void]$builder.AppendLine()
[void]$builder.AppendLine("Total: $number files, $totalLines lines.")
[void]$builder.AppendLine()
[void]$builder.AppendLine('Other project files (designer resources, icon and the Access database; not printed):')
foreach ($file in $listedOnly) {
    [void]$builder.AppendLine('     ' + $file.FullName.Substring($root.Length + 1))
}
[void]$builder.AppendLine()

$number = 0
foreach ($file in $files) {
    $number++
    [void]$builder.AppendLine($rule)
    [void]$builder.AppendLine(('FILE {0} of {1}: {2}' -f $number, $files.Count, $file.FullName.Substring($root.Length + 1)))
    [void]$builder.AppendLine($rule)
    [void]$builder.AppendLine([System.IO.File]::ReadAllText($file.FullName).TrimEnd())
    [void]$builder.AppendLine()
    [void]$builder.AppendLine()
}

[System.IO.File]::WriteAllText($output, $builder.ToString(), (New-Object System.Text.UTF8Encoding($true)))
Write-Host "Wrote $output ($($files.Count) files, $totalLines lines)."
