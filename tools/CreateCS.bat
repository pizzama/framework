color 2

set target_path=D:\workcode\unity\framework\Document\Protobuf\c#protobuf
set file_path=D:\workcode\unity\framework\Document\Protobuf\protobuf
set tools_path=D:\workcode\unity\framework\tools
set copy_path=D:\workcode\unity\framework\Assets\Script\Config

for /r %file_path% %%i in (*.proto) do (
    set filename=%%i       
    %tools_path%\protoc-23.0\win\bin\protoc.exe --csharp_out=%target_path% --proto_path=%file_path% %%~ni.proto
    copy %target_path%\%%~ni.cs %copy_path%
)
pause
