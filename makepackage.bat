SET MSBUILD="C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe"

msbuild Metacoder.sln

cd deploy
mkdir lib
mkdir lib\net45
mkdir tools
mkdir build

copy ..\Metacoder.Interfaces\bin\Debug\*.* lib\net45
copy ..\Metacoder\bin\Debug\*.* tools
copy ..\metacoder.targets build

nuget pack Metacoder.nuspec

rmdir lib /S /Q
rmdir tools /S /Q
rmdir build /S /Q

cd ..