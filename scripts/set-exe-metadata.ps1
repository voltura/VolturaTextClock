#Requires -Version 7.0
param([Parameter(Mandatory)][string]$Path, [Parameter(Mandatory)][version]$Version)
$ErrorActionPreference = 'Stop'
if (-not ('TextClockVersionResource' -as [type])) {
Add-Type -TypeDefinition @'
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
public static class TextClockVersionResource {
    [DllImport("kernel32.dll", CharSet=CharSet.Unicode, SetLastError=true)] static extern IntPtr BeginUpdateResource(string file, bool delete);
    [DllImport("kernel32.dll", SetLastError=true)] static extern bool UpdateResource(IntPtr handle, IntPtr type, IntPtr name, ushort language, byte[] data, uint length);
    [DllImport("kernel32.dll", SetLastError=true)] static extern bool EndUpdateResource(IntPtr handle, bool discard);
    [DllImport("kernel32.dll", CharSet=CharSet.Unicode)] static extern IntPtr LoadLibraryEx(string file, IntPtr reserved, uint flags);
    [DllImport("kernel32.dll")] static extern bool FreeLibrary(IntPtr handle);
    delegate bool LanguageCallback(IntPtr module, IntPtr type, IntPtr name, ushort language, IntPtr argument);
    [DllImport("kernel32.dll")] static extern bool EnumResourceLanguages(IntPtr module, IntPtr type, IntPtr name, LanguageCallback callback, IntPtr argument);
    static byte[] Unicode(string text) => Encoding.Unicode.GetBytes(text + "\0");
    static void Align(BinaryWriter writer) { while (writer.BaseStream.Position % 4 != 0) writer.Write((byte)0); }
    static byte[] Block(string key, ushort type, byte[] value, ushort valueLength, params byte[][] children) {
        using var stream = new MemoryStream(); using var writer = new BinaryWriter(stream);
        writer.Write((ushort)0); writer.Write(valueLength); writer.Write(type); writer.Write(Unicode(key)); Align(writer);
        writer.Write(value);
        foreach (var child in children) { Align(writer); writer.Write(child); }
        var length = checked((ushort)stream.Length); stream.Position=0; writer.Write(length); return stream.ToArray();
    }
    public static void Set(string path, string version) {
        var v = Version.Parse(version); uint ms=(uint)(v.Major << 16 | v.Minor), ls=(uint)(v.Build << 16);
        using var fixedStream = new MemoryStream(); using(var w=new BinaryWriter(fixedStream,Encoding.UTF8,true)) {
            foreach(var n in new uint[] {0xfeef04bd,0x10000,ms,ls,ms,ls,0x3f,0,0x40004,1,0,0,0}) w.Write(n);
        }
        var values=new Dictionary<string,string> {
            ["CompanyName"]="Voltura AB",["ProductName"]="Voltura TextClock",["FileDescription"]="Voltura TextClock — time, in words",
            ["FileVersion"]=version+".0",["ProductVersion"]=version,["OriginalFilename"]="VolturaTextClock.exe",
            ["InternalName"]="VolturaTextClock",["LegalCopyright"]="Copyright © 2020–2026 Voltura AB",
            ["Comments"]="https://textclock.voltura.se/",["LegalTrademarks"]=""
        };
        var strings=new List<byte[]>(); foreach(var pair in values) strings.Add(Block(pair.Key,1,Unicode(pair.Value),(ushort)(pair.Value.Length+1)));
        var table=Block("040904b0",1,Array.Empty<byte>(),0,strings.ToArray());
        var translation=Block("Translation",0,new byte[] {9,4,176,4},4);
        var data=Block("VS_VERSION_INFO",0,fixedStream.ToArray(),52,Block("StringFileInfo",1,Array.Empty<byte>(),0,table),Block("VarFileInfo",1,Array.Empty<byte>(),0,translation));
        var languages=new List<ushort>(); var module=LoadLibraryEx(path,IntPtr.Zero,2);
        if(module!=IntPtr.Zero) { EnumResourceLanguages(module,(IntPtr)16,(IntPtr)1,(m,t,n,l,a)=> { languages.Add(l); return true; },IntPtr.Zero); FreeLibrary(module); }
        if(languages.Count==0) languages.Add(0);
        var handle=BeginUpdateResource(path,false); if(handle==IntPtr.Zero) throw new System.ComponentModel.Win32Exception();
        bool success=false;
        try { foreach(var language in languages) if(!UpdateResource(handle,(IntPtr)16,(IntPtr)1,language,data,(uint)data.Length)) throw new System.ComponentModel.Win32Exception(); success=true; }
        finally { if(!EndUpdateResource(handle,!success)) throw new System.ComponentModel.Win32Exception(); }
    }
}
'@
}
$file = (Resolve-Path -LiteralPath $Path).Path
if ((Get-AuthenticodeSignature -LiteralPath $file).Status -ne 'NotSigned') { throw 'Set metadata before signing.' }
[TextClockVersionResource]::Set($file, $Version.ToString(3))
