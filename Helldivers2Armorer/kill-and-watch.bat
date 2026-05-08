@echo off
for /f "tokens=5" %%p in ('netstat -ano ^| findstr "LISTENING" ^| findstr ":5200"') do (
    echo Killing PID %%p on port 5200
    taskkill /F /PID %%p 2>nul
)
dotnet watch run --launch-profile fordotnetwatch
