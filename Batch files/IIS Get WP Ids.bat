ECHO OFF

NET SESSION >nul 2>&1
IF %ERRORLEVEL% EQU 0 (
    ECHO.
) ELSE (
    REM If you don't run this as Admin you get this error: ERROR ( message:The WAS service is not available - try starting the service first. )
    ECHO You need to run this as Administrator

    PAUSE
    EXIT /B 1
)

CD C:\Windows\System32\inetsrv

REM This list could actually show as empty if nothing is currently loaded
ECHO Worker Processes:
ECHO.
appcmd list wp
ECHO.
PAUSE