set target_path=D:\workcode\unity\framework\Document\Protobuf
set file_path=D:\workcode\unity\framework\Document\Json
set tools_path=D:\workcode\unity\framework\tools

chcp 65001
set sure=1
if "%1"=="" (
	set /p sure="excel to json? 1 Yes   2 No"  
)

COLOR 2
echo %target_path%
echo %file_path%
echo %tools_path %

main.exe -t %target_path% -s %file_path% -l %tools_path% -m 3 -d true
pause