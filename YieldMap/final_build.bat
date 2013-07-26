@echo off

rem http://stackoverflow.com/questions/12407800/which-comment-style-should-i-use-in-batch-files
echo Structuring project

rem http://ss64.com/nt/setlocal.html
rem http://stackoverflow.com/questions/10166386/arrays-linked-lists-and-other-data-structures-in-cmd-exe-batch-script
rem http://ss64.com/nt/delayedexpansion.html
setlocal EnableDelayedExpansion

rem http://stackoverflow.com/questions/10166386/arrays-linked-lists-and-other-data-structures-in-cmd-exe-batch-script
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

mkdir "Yield Map" 
rem http://stackoverflow.com/questions/138497/batch-scripting-iterating-over-files-in-a-directory
for /r %%i in (*) do (
	echo will copy %%i
	rem http://stackoverflow.com/questions/8797983/can-a-dos-batch-file-determine-its-own-file-name
	for %%f in (%%i) do set fn=%%~nxf
	echo name now is !fn!
	if not %0 EQU !fn! copy !fn! "Yield Map"
)

echo Moving files
mkdir "Yield Map"\lib
for /L %%i in (1,1,100) do (
	if not defined FileNames[%%i] (
		echo file name %%i not defined, will stop
		goto stop
	) else echo file name %%i is defined, will copy it

	move "Yield Map"\!FileNames[%%i]! ./"Yield Map"\lib >> nul
)
:stop
