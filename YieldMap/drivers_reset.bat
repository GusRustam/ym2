@echo off
SETLOCAL
set FLD=Thomson Reuters\TRD 6\Program
if exist "%USERPROFILE%\Local Settings\Application Data\%FLD%" (
	set THPATH=%USERPROFILE%\Local Settings\Application Data\%FLD%
	
) else if exist "%LOCALAPPDATA%\%FLD%" (
	set THPATH=%LOCALAPPDATA%\%FLD%

) else if exist "%CommonProgramFiles%\%FLD%" (
	set THPATH=%CommonProgramFiles%\%FLD%
	
) else if exist "%ProgramFiles%\%FLD%" (
	set THPATH=%ProgramFiles%\%FLD%
	
) else if exist "%ProgramFiles(x86)%\%FLD%" (
	set "THPATH=%ProgramFiles(x86)%\%FLD%"
	
) else if exist "%COMMONPROGRAMFILES(x86)%\%FLD%" (
	set "THPATH=%COMMONPROGRAMFILES(x86)%\%FLD%"
	
) else (
	echo Failed to find Thomson Reuters Eikon home folder
	goto :ext
	
)

if /I [%1] EQU [/u] ( 
	set ACTION=unregister
) else  (
	set ACTION=register
)

echo --- %ACTION%ing Eikon libraries located at %THPATH%

(regsvr32 /s %1 "%THPATH%\Adxfo.dll" && (echo AdFin Functions %ACTION%ed successfully)) || echo Failed to %ACTION% AdFin Functions
(regsvr32 /s %1 "%THPATH%\rtx.dll"  && (echo AdFin RealTime %ACTION%ed successfully)) || echo Failed to %ACTION% AdFin RealTime
(regsvr32 /s %1 "%THPATH%\Dex2.dll" && (echo Dex2 %ACTION%ed successfully)) || echo Failed to %ACTION% Dex2
(regsvr32 /s %1 "%THPATH%\EikonDesktopDataAPI.dll" && (echo Desktop SDK %ACTION%ed successfully))  || echo Failed to %ACTION% Desktop SDK

:ext
ENDLOCAL
pause
