COLOR 2

set target_path=D:\workcode\myself\excelCovertXml\temp\excel
set file_path=D:\workcode\myself\excelCovertXml\temp\data
set tools_path=D:\workcode\myself\excelCovertXml\tools

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








