@echo off
taskkill /F /IM dotnet.exe /T 2>nul && echo Killed dotnet processes || echo No dotnet processes found
