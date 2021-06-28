ECHO OFF

REM start or stop
SET "operation=%1"

iisreset /%operation%
net %operation% ServiceName1
net %operation% ServiceName2
net %operation% ServiceName3

REM Exact service names can be found by looking them up in the Services console. You cannot use the display name.

REM PAUSE