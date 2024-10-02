@echo off
set uiPath=E:\Coding\02Github\yimogit\Admin.Core\ui\zhontai.ui.admin.vue3
set apiPath=E:\Coding\02Github\yimogit\Admin.Core\src\hosts\ZhonTai.Host

echo "Build Api"
cd %apiPath%
dotnet publish -c release -o ./bin/Publish -r win-x64

cd %uiPath%
echo "Build Web"
npm run build

echo "Copy Web Dist"

xcopy %uiPath%\dist %apiPath%\bin\Publish\wwwroot\ /s /e /Q /Y /I

echo "Build Ok"
pause