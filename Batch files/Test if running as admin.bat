ECHO OFF

NET SESSION >nul 2>&1
IF %ERRORLEVEL% EQU 0 (
    ECHO.
) ELSE (
    ECHO You need to run this as Administrator

    PAUSE
    EXIT /B 1
)

PAUSE
