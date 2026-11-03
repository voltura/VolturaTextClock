#Requires -Version 7.0
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing
$root = Split-Path $PSScriptRoot -Parent
$brand = Join-Path $root 'assets/branding'
New-Item -ItemType Directory -Force $brand | Out-Null

function Render-Icon([int]$size, [bool]$setup = $false) {
    $image = [Drawing.Bitmap]::new($size, $size)
    $g = [Drawing.Graphics]::FromImage($image)
    $g.SmoothingMode = 'AntiAlias'
    $g.TextRenderingHint = 'AntiAliasGridFit'
    $g.ScaleTransform($size / 512.0, $size / 512.0)
    $shape = [Drawing.Drawing2D.GraphicsPath]::new()
    foreach ($arc in @(@(8,8,104,104,180,90),@(400,8,104,104,270,90),@(400,400,104,104,0,90),@(8,400,104,104,90,90))) {
        $shape.AddArc($arc[0],$arc[1],$arc[2],$arc[3],$arc[4],$arc[5])
    }
    $shape.CloseFigure()
    $fill = [Drawing.Drawing2D.LinearGradientBrush]::new([Drawing.Point]::new(0,0),[Drawing.Point]::new(512,512),[Drawing.Color]::FromArgb(38,44,52),[Drawing.Color]::FromArgb(12,16,22))
    $g.FillPath($fill,$shape)
    $border = [Drawing.Pen]::new([Drawing.Color]::FromArgb(73,80,88),3)
    $g.DrawPath($border,$shape)
    $format = [Drawing.StringFormat]::new(); $format.Alignment = 'Center'; $format.LineAlignment = 'Center'
    if ($size -le 32) { $rows = @('IT','IS'); $fontSize = 174; $startY = 56; $stepY = 202; $stepX = 176; $startX = 80 }
    else { $rows = @('ITIS','HALF','PAST','TIME'); $fontSize = 76; $startY = 47; $stepY = 105; $stepX = 102; $startX = 51 }
    $font = [Drawing.Font]::new('Consolas',$fontSize,[Drawing.FontStyle]::Bold,[Drawing.GraphicsUnit]::Pixel)
    $lit = [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(255,225,166))
    $dim = [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(87,98,110))
    for ($row = 0; $row -lt $rows.Count; $row++) {
        for ($column = 0; $column -lt $rows[$row].Length; $column++) {
            $brush = if ($row -eq 0 -or $row -eq $rows.Count - 1) { $lit } else { $dim }
            $rect = [Drawing.RectangleF]::new($startX + $column * $stepX,$startY + $row * $stepY,$stepX,$stepY)
            $g.DrawString([string]$rows[$row][$column],$font,$brush,$rect,$format)
        }
    }
    if ($setup) {
        $g.FillEllipse($lit,328,328,172,172)
        $arrow = [Drawing.Pen]::new([Drawing.Color]::FromArgb(20,25,32),15)
        $g.DrawLine($arrow,414,361,414,446); $g.DrawLine($arrow,380,415,414,449); $g.DrawLine($arrow,448,415,414,449)
        $arrow.Dispose()
    }
    $g.Dispose(); $shape.Dispose(); $fill.Dispose(); $border.Dispose(); $format.Dispose(); $font.Dispose(); $lit.Dispose(); $dim.Dispose()
    return ,$image
}
function Write-Ico([string]$path, [bool]$setup) {
    $frames = @(16,20,24,32,40,48,64,128,256) | ForEach-Object {
        $bitmap = Render-Icon $_ $setup
        $stream = [IO.MemoryStream]::new(); $bitmap.Save($stream,[Drawing.Imaging.ImageFormat]::Png)
        [pscustomobject]@{ Size = $_; Bytes = $stream.ToArray() }
        $bitmap.Dispose(); $stream.Dispose()
    }
    $stream = [IO.File]::Create($path); $writer = [IO.BinaryWriter]::new($stream)
    $writer.Write([uint16]0); $writer.Write([uint16]1); $writer.Write([uint16]$frames.Count)
    $offset = 6 + 16 * $frames.Count
    foreach ($frame in $frames) {
        $dimension = if ($frame.Size -eq 256) { 0 } else { $frame.Size }
        $writer.Write([byte]$dimension); $writer.Write([byte]$dimension); $writer.Write([uint16]0)
        $writer.Write([uint16]1); $writer.Write([uint16]32); $writer.Write([uint32]$frame.Bytes.Length); $writer.Write([uint32]$offset)
        $offset += $frame.Bytes.Length
    }
    foreach ($frame in $frames) { $writer.Write([byte[]]$frame.Bytes) }
    $writer.Dispose(); $stream.Dispose()
}
Write-Ico (Join-Path $root 'VolturaTextClock/VolturaTextClock.ico') $false
Write-Ico (Join-Path $brand 'setup.ico') $true
$image = Render-Icon 512
$image.Save((Join-Path $brand 'textclock.png'),[Drawing.Imaging.ImageFormat]::Png); $image.Dispose()
Copy-Item -LiteralPath (Join-Path $root 'VolturaTextClock/VolturaTextClock.ico') -Destination (Join-Path $root 'site/assets/favicon.ico')
Copy-Item -LiteralPath (Join-Path $brand 'textclock.png') -Destination (Join-Path $root 'site/assets/icon.png')
