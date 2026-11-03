Unicode true
!include "MUI2.nsh"
!include "LogicLib.nsh"
!include "x64.nsh"
!if "${VARIANT}" == "full"
  !define SUFFIX "-full"
!else
  !define SUFFIX ""
!endif
Name "Voltura TextClock"
Caption "Voltura TextClock ${VERSION}"
BrandingText "Voltura AB | Time, in words."
OutFile "${OUTPUT}"
InstallDir "$LOCALAPPDATA\Programs\VolturaTextClock"
RequestExecutionLevel user
ManifestDPIAware true
ManifestSupportedOS all
SetCompressor /SOLID lzma
VIProductVersion "${VERSION}.0"
VIAddVersionKey /LANG=1033 "ProductName" "Voltura TextClock"
VIAddVersionKey /LANG=1033 "CompanyName" "Voltura AB"
VIAddVersionKey /LANG=1033 "FileDescription" "Voltura TextClock Installer"
VIAddVersionKey /LANG=1033 "FileVersion" "${VERSION}.0"
VIAddVersionKey /LANG=1033 "ProductVersion" "${VERSION}"
VIAddVersionKey /LANG=1033 "OriginalFilename" "VolturaTextClock-Setup-${VERSION}-win-x64${SUFFIX}.exe"
VIAddVersionKey /LANG=1033 "InternalName" "VolturaTextClockSetup"
VIAddVersionKey /LANG=1033 "LegalCopyright" "Copyright (c) 2020-2026 Voltura AB"
VIAddVersionKey /LANG=1033 "Comments" "https://textclock.voltura.se/ | Joakim Skoglund"
!define MUI_ICON "..\assets\branding\setup.ico"
!define MUI_UNICON "..\VolturaTextClock\VolturaTextClock.ico"
!define MUI_ABORTWARNING
!define MUI_LANGDLL_ALLLANGUAGES
!define MUI_FINISHPAGE_RUN "$INSTDIR\VolturaTextClock.exe"
!define MUI_FINISHPAGE_REBOOTLATER_DEFAULT
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "..\LICENSE"
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "Swedish"
LangString Core ${LANG_ENGLISH} "Voltura TextClock (required)"
LangString Core ${LANG_SWEDISH} "Voltura TextClock (krävs)"
LangString Desktop ${LANG_ENGLISH} "Desktop shortcut"
LangString Desktop ${LANG_SWEDISH} "Skrivbordsgenväg"
LangString Failed ${LANG_ENGLISH} "Setup could not complete. Close TextClock and try again. Your previous installation is preserved if an upgrade fails."
LangString Failed ${LANG_SWEDISH} "Installationen kunde inte slutföras. Stäng TextClock och försök igen. Din tidigare installation bevaras om en uppgradering misslyckas."
Var PowerShell
Function .onInit
  !insertmacro MUI_LANGDLL_DISPLAY
  ${IfNot} ${RunningX64}
    Abort "Windows x64 is required."
  ${EndIf}
  StrCpy $PowerShell "$WINDIR\Sysnative\WindowsPowerShell\v1.0\powershell.exe"
  System::Call 'kernel32::SetEnvironmentVariableW(w "PSModulePath", w "$WINDIR\System32\WindowsPowerShell\v1.0\Modules") i.r0'
FunctionEnd
Section "$(Core)" CoreSection
  SectionIn RO
  InitPluginsDir
  SetOutPath "$PLUGINSDIR"
  File "maintain.ps1"
  File "prepare-payload.ps1"
  File "prerequisite.ps1"
!if "${VARIANT}" == "standard"
  nsExec::ExecToStack '"$PowerShell" -NoProfile -NonInteractive -ExecutionPolicy Bypass -File "$PLUGINSDIR\prerequisite.ps1"'
  Pop $0
  Pop $1
  DetailPrint "$1"
  ${If} $0 == 3010
    SetRebootFlag true
  ${ElseIf} $0 != 0
    SetErrorLevel 1
    Abort "$(Failed)"
  ${EndIf}
!endif
  SetOutPath "$PLUGINSDIR\payload"
  File /r "${PAYLOAD}\*"
  WriteUninstaller "$PLUGINSDIR\payload\Uninstall.exe"
  nsExec::ExecToStack '"$PowerShell" -NoProfile -NonInteractive -ExecutionPolicy Bypass -File "$PLUGINSDIR\prepare-payload.ps1" -Payload "$PLUGINSDIR\payload"'
  Pop $0
  Pop $1
  ${If} $0 != 0
    SetErrorLevel 1
    Abort "$(Failed)"
  ${EndIf}
  nsExec::ExecToStack '"$PowerShell" -NoProfile -NonInteractive -ExecutionPolicy Bypass -File "$PLUGINSDIR\maintain.ps1" -Payload "$PLUGINSDIR\payload" -Variant "${VARIANT}" -SetupPath "$EXEPATH"'
  Pop $0
  Pop $1
  DetailPrint "$1"
  ${If} $0 != 0
    SetErrorLevel 1
    Abort "$(Failed)"
  ${EndIf}
SectionEnd
Section /o "$(Desktop)" DesktopSection
  SetShellVarContext current
  CreateShortcut "$DESKTOP\Voltura TextClock.lnk" "$INSTDIR\VolturaTextClock.exe"
SectionEnd
Function un.onInit
  !insertmacro MUI_UNGETLANGUAGE
  StrCpy $PowerShell "$WINDIR\Sysnative\WindowsPowerShell\v1.0\powershell.exe"
  System::Call 'kernel32::SetEnvironmentVariableW(w "PSModulePath", w "$WINDIR\System32\WindowsPowerShell\v1.0\Modules") i.r0'
FunctionEnd
Section "Uninstall"
  InitPluginsDir
  SetOutPath "$PLUGINSDIR"
  File "maintain.ps1"
  nsExec::ExecToLog '"$PowerShell" -NoProfile -NonInteractive -ExecutionPolicy Bypass -File "$PLUGINSDIR\maintain.ps1" -Mode Uninstall'
  Pop $0
  ${If} $0 != 0
    SetErrorLevel 1
    Abort "$(Failed)"
  ${EndIf}
SectionEnd
