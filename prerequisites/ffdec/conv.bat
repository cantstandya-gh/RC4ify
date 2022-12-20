@echo off
pushd "%~dp0"
pushd "%~dp0"
set dirr="%~d1%~p1%~n1"
set input="%1"
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