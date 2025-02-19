color 2

@echo off
:: 获取当前目录的完整路径
set currentDir=%CD%

:: 使用 .. 来指向父目录，并通过将路径转换为短文件名的方式来解析实际的父目录路径
for %%i in ("%currentDir%\..") do set project_path=%%~fi

:: 输出父目录的完整路径
echo The parent directory is: %project_path%
set target_path=%project_path%\Document\Protobuf
set file_path=%project_path%\Document\Json
set tools_path=%project_path%\tools
set unity_path=%project_path%\Assets

chcp 65001
set sure=1
if "%1"=="" (
	set /p sure="excel to json? 1 Yes   2 No"
)

COLOR 2
echo %target_path%
echo %file_path%
echo %tools_path%

main.exe -t %target_path% -s %file_path% -l %tools_path% -m 3 -d true

xcopy %target_path%\bytes %unity_path%\StreamingAssets\bytes /s /e
xcopy %target_path%\csharp %unity_path%\Script\Config /s /e

echo "copy over"
pause