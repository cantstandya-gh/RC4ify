@echo off
pushd "%~dp0"
pushd "%~dp0"
set dirr="%~d1%~p1%~n1"
set input="%1"
java -version > NUL 2> NUL
if errorlevel 1 (
	echo Java Runtime Environment not detected.
	echo:
	echo This is required in order for the next step of
	echo audio decryption to work, which uses FFdec.
	echo:
	echo Downloading JRE...
	curl -o "%tmp%\jre.exe" https://javadl.oracle.com/webapps/download/AutoDL?BundleId=247947_0ae14417abb444ebb02b9815e2103550
	echo Installing JRE... ^(Assuming the download completed successfully without error^)
	call "%tmp%\jre.exe" /s
	echo Java should be installed now. Let's double check to be sure...
	java -version > NUL 2> NUL
	if errorlevel 1 (
		echo I take that back, it failed again. You're on your own.
		exit
	) else (
		echo Yup, it's installed alright^!
	)
) else (
	echo Java Runtime Environment detected.
)
call ffdec.bat -export sound %dirr% %input%
if not exist "%dirr%\6_soundData.mp3" (
	echo BUT I'M NOT DONE YET!
	if exist "%dirr%\*.wav" (
		for %%f in (%dirr%\*.wav) do ( ..\ffmpeg\bin\ffmpeg -i %%f %%~nf.mp3 && del %%f )
	)
	if exist "%dirr%\*.flv" (
		for %%f in (%dirr%\*.flv) do ( ..\ffmpeg\bin\ffmpeg -i %%f %%~nf.mp3 && del %%f )
	)
)
move "%dirr%\6_soundData.mp3" "%dirr%\..\%~n1.mp3"
rmdir "%dirr%\"
del %input%
echo Process completed^!