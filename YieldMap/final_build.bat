@echo off

rem http://stackoverflow.com/questions/12407800/which-comment-style-should-i-use-in-batch-files
echo Structuring project

rem http://ss64.com/nt/setlocal.html
rem http://stackoverflow.com/questions/10166386/arrays-linked-lists-and-other-data-structures-in-cmd-exe-batch-script
rem http://ss64.com/nt/delayedexpansion.html
setlocal EnableDelayedExpansion

rem http://stackoverflow.com/questions/10166386/arrays-linked-lists-and-other-data-structures-in-cmd-exe-batch-script
set fldrs[1]=debug
set fldrs[2]=release
set fldrs[3]=yield map v2

set FileNames[1]=CommonController.*
set FileNames[2]=DbManager.*
set FileNames[3]=DotNumerics.*
set FileNames[4]=Ionic.Zip.*
set FileNames[5]=Logging.*
set FileNames[6]=MathNet.Numerics.*
set FileNames[7]=MathNet.Numerics.IO.*
set FileNames[8]=NLog.*
set FileNames[9]=ReutersData.*
set FileNames[10]=Settings.*
set FileNames[11]=YieldMap.vshost.*

rem http://superuser.com/questions/160702/get-current-folder-name-by-a-dos-command
for %%* in (.) do set CurrDirName=%%~n*
echo Current folder is !CurrDirName!


rem 100 must be enough
for /L %%i in (1,1,100) do (
	if not defined fldrs[%%i] (
		echo Nothing foung, quitting
		goto stop
	) else echo Searching myself in folder %%i: !fldrs[%%i]!
	
	rem http://stackoverflow.com/questions/8438511/if-or-if-in-a-windows-batch-file
	rem http://ss64.com/nt/if.html
	if /I !CurrDirName! EQU !fldrs[%%i]! (
	 	echo ... found myself in !fldrs[%%i]! folder
	 	goto enough
	) else echo ... didn't find myself in !fldrs[%%i]! folder
	
	echo ---
)
goto stop

:enough
echo Moving files
mkdir lib
for /L %%i in (1,1,100) do (
	if not defined FileNames[%%i] (
		echo file name %%i not defined, will stop
		goto stop
	) else echo file name %%i is defined, will copy it

	move !FileNames[%%i]! .\lib >> nul
)
:stop
