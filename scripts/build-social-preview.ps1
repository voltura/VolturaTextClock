#Requires -Version 7.0
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing
$root = Split-Path $PSScriptRoot -Parent
$image = [Drawing.Bitmap]::new(1200, 630)
$g = [Drawing.Graphics]::FromImage($image)
$g.Clear([Drawing.Color]::FromArgb(245,242,233))
$g.SmoothingMode = 'AntiAlias'; $g.TextRenderingHint = 'AntiAliasGridFit'
$ink = [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(32,39,48))
$warm = [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(152,113,60))
$muted = [Drawing.SolidBrush]::new([Drawing.Color]::FromArgb(100,105,103))
$label = [Drawing.Font]::new('Segoe UI',18,[Drawing.FontStyle]::Regular,[Drawing.GraphicsUnit]::Pixel)
$heading = [Drawing.Font]::new('Segoe UI',94,[Drawing.FontStyle]::Regular,[Drawing.GraphicsUnit]::Pixel)
$italic = [Drawing.Font]::new('Georgia',100,[Drawing.FontStyle]::Italic,[Drawing.GraphicsUnit]::Pixel)
$icon = [Drawing.Image]::FromFile((Join-Path $root 'assets/branding/textclock.png'))
$clock = [Drawing.Image]::FromFile((Join-Path $root 'site/assets/clock-english.png'))
$g.DrawImage($icon,56,48,48,48)
$g.DrawString('TEXTCLOCK  /  BY VOLTURA',$label,$ink,118,60)
$g.DrawString('Time,',$heading,$ink,50,166)
$g.DrawString('in words.',$italic,$warm,48,264)
$g.DrawString('A little character for your desktop.',$label,$muted,59,435)
$g.DrawString('Free for Windows  •  English & Swedish',$label,$ink,59,514)
$g.FillRectangle($ink,642,48,510,510)
$g.DrawImage($clock,650,56,494,494)
$g.DrawString('textclock.voltura.se',$label,$muted,781,576)
$g.Dispose(); $icon.Dispose(); $clock.Dispose(); $label.Dispose(); $heading.Dispose(); $italic.Dispose(); $ink.Dispose(); $warm.Dispose(); $muted.Dispose()
$image.Save((Join-Path $root 'site/assets/social-preview.png'),[Drawing.Imaging.ImageFormat]::Png)
$image.Dispose()
