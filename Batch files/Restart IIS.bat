ECHO OFF

NET SESSION >nul 2>&1
IF %ERRORLEVEL% EQU 0 (
    ECHO.
) ELSE (
    ECHO You need to run this as Administrator

    PAUSE
    EXIT /B 1
)

REM Sometimes even if you run this as Admin you can get an error back that complains you aren't running it as admin
REM When that happens, ignore and try again. Usually works the second time.
REM If it still doesn't work then explicitly run this in an Administrator CMD session
iisreset /restart

PAUSE