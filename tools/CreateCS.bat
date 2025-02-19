color 2

@echo off
:: 获取当前目录的完整路径
set currentDir=%CD%

:: 使用 .. 来指向父目录，并通过将路径转换为短文件名的方式来解析实际的父目录路径
for %%i in ("%currentDir%\..") do set project_path=%%~fi

:: 输出父目录的完整路径
echo The parent directory is: %project_path%
set target_path=%project_path%\Document\Protobuf\
set file_path=%project_path%\Document\Protobuf\protobuf
set tools_path=%project_path%\tools
set copy_path=%project_path%\Assets\Script\Config

for /r %file_path% %%i in (*.proto) do (
    set filename=%%i       
    %tools_path%\protoc-23.0\win\bin\protoc.exe --csharp_out=%target_path% --proto_path=%file_path% %%~ni.proto
    copy %target_path%\%%~ni.cs %copy_path%
)

pause