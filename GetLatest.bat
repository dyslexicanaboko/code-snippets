ECHO OFF
ECHO.
SET "repoPath=%1"
SET "mainBranch=%2"
SET git="C:\Program Files\Git\cmd\git.exe"

CD /D %repoPath%

%git% checkout %mainBranch%
ECHO.
%git% fetch --all
ECHO.
%git% remote prune origin
ECHO.
%git% pull
ECHO.
%git% branch

PAUSE