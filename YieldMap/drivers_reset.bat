@echo on

echo Unregistering old Xtra libraries

regsvr32 /u "C:\Program Files\Reuters\Common\adfin\Adxoo.dll"
regsvr32 /u "C:\Program Files\Reuters\Common\adfin\Adxfo.dll"
regsvr32 /u "C:\Program Files\Reuters\Common\adfin\rtx.dll"

if exist "C:\Program Files\Thomson Reuters\TRD 6" goto usePF
set _thPath=%USERPROFILE%\Local Settings\Application Data\Thomson Reuters\TRD 6\Program
goto reg

:usePF
set _thPath=C:\Program Files\Thomson Reuters\TRD 6\Program

:reg
echo Registering Eikon libraries from %_thPath%

regsvr32 "%_thPath%\Adxoo.dll"
regsvr32 "%_thPath%\Adxfo.dll"
regsvr32 "%_thPath%\rtx.dll"
regsvr32 "%_thPath%\EikonDesktopSDK.dll"

pause
