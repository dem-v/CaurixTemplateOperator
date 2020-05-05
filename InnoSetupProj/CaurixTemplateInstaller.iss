; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Caurix Template Operator"
#define MyAppVersion "1.0"
#define MyAppPublisher "de3452"
#define MyAppExeName "CaurixTemplateOperator.exe"
#define MyAppId "{61B16427-33FD-46CC-9A5B-B26BB642395D}"

[CustomMessages]
english.NewerVersionExists=A newer version of {#MyAppName} is already installed.%n%nInstaller version: {#MyAppVersion}%nCurrent version: 
[Code]
// find current version before installation
function InitializeSetup: Boolean;
var Version: String;
begin
  if RegValueExists(HKEY_LOCAL_MACHINE,'Software\Microsoft\Windows\CurrentVersion\Uninstall\{#MyAppId}_is1', 'DisplayVersion') then
    begin
      RegQueryStringValue(HKEY_LOCAL_MACHINE,'Software\Microsoft\Windows\CurrentVersion\Uninstall\{#MyAppId}_is1', 'DisplayVersion', Version);
      if Version > '{#MyAppVersion}' then
        begin
          MsgBox(ExpandConstant('{cm:NewerVersionExists} '+Version), mbInformation, MB_OK);
          Result := False;
        end
      else
        begin
          Result := True;
        end
    end
  else
    begin
      Result := True;
    end
end;

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{61B16427-33FD-46CC-9A5B-B26BB642395D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
OutputBaseFilename=mysetup
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\CaurixTemplateOperator.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\BouncyCastle.Crypto.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\CaurixTemplateOperator.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\CaurixTemplateOperator.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\CaurixTemplateOperator.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\itextsharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\itextsharp.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\log.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\MailKit.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\MailKit.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\MimeKit.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\MimeKit.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\MySql.Data.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\NetOffice.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\NetOffice.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\Newtonsoft.Json.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\OfficeApi.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\OfficeApi.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\OutlookApi.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\OutlookApi.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\stdole.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\VBIDEApi.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\VBIDEApi.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\template.docx"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\updOutlookRegValues\bin\Release\updOutlookRegValues.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\updOutlookRegValues\bin\Release\updOutlookRegValues.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\NDP461-KB3102438-Web.exe"; DestDir: "{app}\Lib"; Flags: deleteafterinstall  
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\mysql-connector-odbc-5.3.14-win32.msi"; DestDir: "{app}\Lib"; Flags: deleteafterinstall
Source: "C:\Users\Demi\source\repos\CaurixTemplateOperator\CaurixTemplateOperator\bin\Release\mysql-connector-odbc-5.3.14-winx64.msi"; DestDir: "{app}\Lib"; Flags: deleteafterinstall
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\Lib\mysql-connector-odbc-5.3.14-win32.msi"; Description: "Updates for ODBC Driver"; StatusMsg: "Updating ODBC Driver"; Flags: shellexec nowait
Filename: "{app}\Lib\mysql-connector-odbc-5.3.14-winx64.msi"; Description: "Updates for ODBC Driver"; StatusMsg: "Updating ODBC Driver"; Flags: shellexec nowait; Check: IsWin64
Filename: "{app}\Lib\NDP461-KB3102438-Web.exe"; Description: "Updates for .Net Framework"; StatusMsg: "Updating .Net Framework"; Flags: nowait
Filename: "{app}\updOutlookRegValues.exe"; Description: "Registry update for Outlook"; StatusMsg: "Updating Outlook settings"; Flags: nowait 
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
