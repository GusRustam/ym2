@echo off

echo Unregistering old Xtra libraries

regsvr32 /s /u "C:\Program Files\Reuters\Common\adfin\Adxoo.dll"
regsvr32 /s /u "C:\Program Files\Reuters\Common\adfin\Adxfo.dll"
regsvr32 /s /u "C:\Program Files\Reuters\Common\adfin\rtx.dll"
"C:\Program Files\Reuters\Common\Dex\dex.exe" /UnRegServer

if exist "C:\Program Files\Thomson Reuters\TRD 6" goto usePF
set _thPath=%USERPROFILE%\Local Settings\Application Data\Thomson Reuters\TRD 6\Program
goto reg

:usePF
set _thPath=C:\Program Files\Thomson Reuters\TRD 6\Program

:reg
echo Registering Eikon libraries from %_thPath%

regsvr32 /s "%_thPath%\Adxoo.dll"
regsvr32 /s "%_thPath%\Adxfo.dll"
regsvr32 /s "%_thPath%\rtx.dll"
regsvr32 /s "%_thPath%\Dex2.dll"
regsvr32 /s "%_thPath%\EikonDesktopSDK.dll"

pause
