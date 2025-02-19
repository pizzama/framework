color 2

@echo off
:: 获取当前目录的完整路径
set currentDir=%CD%

:: 使用 .. 来指向父目录，并通过将路径转换为短文件名的方式来解析实际的父目录路径
for %%i in ("%currentDir%\..") do set project_path=%%~fi

:: 输出父目录的完整路径
echo The parent directory is: %project_path%
set target_path=%project_path%\Document\excel
set file_path=%project_path%\Document\data
set tools_path=%project_path%\tools

chcp 65001
set sure=1
if "%1"=="" (
	set /p sure="json to excel? 1 Yes   2 No"  
)

echo %sure%

if %sure%==1 (

echo %target_path%
echo %file_path%
echo %tools_path %

main.exe -t %target_path% -s %file_path% -m 2 -d true
if "%1"=="" (
	pause
)

)








