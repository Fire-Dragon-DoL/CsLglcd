!include "LogicLib.nsh"
; Given a .NET version number, this function returns that .NET framework's
; install directory. Returns "" if the given .NET version is not installed.
; Params: [version] (eg. "v2.0")
; Return: [dir] (eg. "C:\WINNT\Microsoft.NET\Framework\v2.0.50727")
!macro MacroGetDotNetDir un
  Function ${un}GetDotNetDir
    Exch $R0 ; Set R0 to .net version major
    Push $R1
    Push $R2
   
    ; set R1 to minor version number of the installed .NET runtime
    EnumRegValue $R1 HKLM \
      "Software\Microsoft\.NetFramework\policy\$R0" 0
    IfErrors getdotnetdir_err
   
    ; set R2 to .NET install dir root
    ReadRegStr $R2 HKLM \
      "Software\Microsoft\.NetFramework" "InstallRoot"
    IfErrors getdotnetdir_err
   
    ; set R0 to the .NET install dir full
    StrCpy $R0 "$R2$R0.$R1"
   
  getdotnetdir_end:
    Pop $R2
    Pop $R1
    Exch $R0 ; return .net install dir full
    Return
   
  getdotnetdir_err:
    StrCpy $R0 ""
    Goto getdotnetdir_end
   
  FunctionEnd
!macroend
!insertmacro MacroGetDotNetDir ""
!insertmacro MacroGetDotNetDir "un."

;NSIS Modern User Interface
;Welcome/Finish Page Example Script
;Written by Joost Verburg

;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"

;--------------------------------
;General

  ;Name and file
  Name "Media Jukebox Plugin Installer"
  OutFile "mediajukebox_plugin.exe"

  ;Default installation folder
  InstallDir "C:\Program Files (x86)\J River\Media Jukebox 14\Plugins\CsLglcd.MediaJukeboxDisplay"

  ;Get installation folder from registry if available
  InstallDirRegKey HKCU "Software\J. River\Media Jukebox 14\Plugins\Interface\CsLglcd.MediaJukeboxDisplay" ""

  ;Request application privileges for Windows Vista
  RequestExecutionLevel user
  
  ShowInstDetails show

;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING

;--------------------------------
;Pages

  !insertmacro MUI_PAGE_WELCOME
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  !insertmacro MUI_PAGE_FINISH

  !insertmacro MUI_UNPAGE_WELCOME
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  !insertmacro MUI_UNPAGE_FINISH

;--------------------------------
;Languages

  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Installer Sections
  Var /GLOBAL PathDotNet
  Var /GLOBAL PLUGINREGKEY
  
Section "Var declarations"
  StrCpy $PLUGINREGKEY "Software\Wow6432Node\J. River\Media Jukebox\Plugins\Interface\CsLglcd.MediaJukeboxDisplay"
SectionEnd

Section "Install"  
  Push "v4.0"
  Call GetDotNetDir
  Pop $R0
  StrCmpS "" $R0 error_dot_net_not_found  
  
  StrCpy $PathDotNet $R0

  SetOutPath "$INSTDIR"

  ;ADD YOUR OWN FILES HERE...
  File "D:\Users\Francesco\Documents\Visual Studio 2010\Projects\CsLglcd\CsLglcd.MediaJukeboxDisplay\bin\Release\CsLglcd.MediaJukeboxDisplay.dll"
  
	nsExec::ExecToStack '$PathDotNet\regasm.exe /codebase "$INSTDIR\CsLglcd.MediaJukeboxDisplay.dll"'
  Pop $R0
  ${If} $R0 <> 0
    Goto error_cant_register
  ${EndIf}
  
  ;Additional Files
  File "D:\Users\Francesco\Documents\Visual Studio 2010\Projects\CsLglcd\CsLglcd.MediaJukeboxDisplay\bin\Release\CsLglcd.dll"
  File "D:\Users\Francesco\Documents\Visual Studio 2010\Projects\CsLglcd\CsLglcd.MediaJukeboxDisplay\bin\Release\CsLglcd.UI.dll"
  File "D:\Users\Francesco\Documents\Visual Studio 2010\Projects\CsLglcd\CsLglcd.MediaJukeboxDisplay\bin\Release\Lglcd_x64.dll"
  File "D:\Users\Francesco\Documents\Visual Studio 2010\Projects\CsLglcd\CsLglcd.MediaJukeboxDisplay\bin\Release\Lglcd_x86.dll"

  ;Store installation folder
  WriteRegStr HKLM "$PLUGINREGKEY" "" $INSTDIR
  
  ; Plugin keys
  WriteRegDWORD HKLM "$PLUGINREGKEY" "IVersion" 1
  WriteRegStr HKLM "$PLUGINREGKEY" "Company" "Francesco Belladonna"
  WriteRegStr HKLM "$PLUGINREGKEY" "Version" "0.0.0.1"
  WriteRegStr HKLM "$PLUGINREGKEY" "URL" "https://github.com/Fire-Dragon-DoL/CsLglcd"
  WriteRegStr HKLM "$PLUGINREGKEY" "Copyright" "Copyright (c) 2012, Francesco Belladonna."
  WriteRegDWORD HKLM "$PLUGINREGKEY" "PluginMode" 1
  WriteRegStr HKLM "$PLUGINREGKEY" "ProdID" "CsLglcd.MediaJukeboxDisplay"

  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"
  
  Return
error_dot_net_not_found:
  Abort ".NET 4.0 Framework not found"
error_cant_register:
  Delete "$INSTDIR\CsLglcd.MediaJukeboxDisplay.dll"
  RMDir "$INSTDIR"
  
  Abort "Failed registering plugin"

SectionEnd

;--------------------------------

;--------------------------------
;Uninstaller Section

Section "Uninstall"
  Push "v4.0"
  Call un.GetDotNetDir
  Pop $R0
  StrCmpS "" $R0 error_dot_net_not_found  
  
  StrCpy $PathDotNet $R0
  
	nsExec::ExecToStack '$PathDotNet\regasm.exe /unregister "$INSTDIR\CsLglcd.MediaJukeboxDisplay.dll"'

  ;ADD YOUR OWN FILES HERE...
  Delete "$INSTDIR\CsLglcd.MediaJukeboxDisplay.dll"
  Delete "$INSTDIR\CsLglcd.dll"
  Delete "$INSTDIR\CsLglcd.UI.dll"
  Delete "$INSTDIR\Lglcd_x64.dll"
  Delete "$INSTDIR\Lglcd_x86.dll"  
  
  Delete "$INSTDIR\Uninstall.exe"

  RMDir "$INSTDIR"

  DeleteRegValue HKLM "$PLUGINREGKEY" ""
  DeleteRegValue HKLM "$PLUGINREGKEY" "IVersion"
  DeleteRegValue HKLM "$PLUGINREGKEY" "Company"
  DeleteRegValue HKLM "$PLUGINREGKEY" "Version"
  DeleteRegValue HKLM "$PLUGINREGKEY" "URL"
  DeleteRegValue HKLM "$PLUGINREGKEY" "Copyright"
  DeleteRegValue HKLM "$PLUGINREGKEY" "PluginMode"
  DeleteRegValue HKLM "$PLUGINREGKEY" "ProdID"
  
  DeleteRegKey /ifempty HKLM "$PLUGINREGKEY"
  
  Return
error_dot_net_not_found:
  Abort ".NET 4.0 Framework not found"

SectionEnd
