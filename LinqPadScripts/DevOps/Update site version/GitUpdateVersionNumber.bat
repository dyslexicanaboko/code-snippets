ECHO OFF

setlocal
call :setESC

SET "VersionNumber=9.9.9.9"
SET "RepoPath=C:\Dev\SiteRepo\"
SET "LinqPadScriptPath=C:\Dev\"

WHERE lprun >nul 2>nul
IF %ERRORLEVEL% NEQ 0 (
	ECHO %ESC%[33mlprun.exe%ESC%[0m %ESC%[31mwasn't found!%ESC%[0m %ESC%[4mMake sure%ESC%[0m you have LinqPad installed and it can be found in the PATH variable.
	ECHO.
	PAUSE
	
	EXIT
)

WHERE git >nul 2>nul
IF %ERRORLEVEL% NEQ 0 (
	ECHO %ESC%[33mgit.exe%ESC%[0m %ESC%[31mwasn't found!%ESC%[0m %ESC%[4mMake sure%ESC%[0m you have Git installed and it can be found in the PATH variable.
	ECHO.
	PAUSE
	
	EXIT
)

CD "%RepoPath%"

ECHO.
ECHO Make absolutely sure that the following parameters are correct before you continue:
ECHO.
ECHO     Updating version number to: %ESC%[33m%ESC%[1m%VersionNumber%%ESC%[0m%ESC%[0m
ECHO.
ECHO     Current repository path   : %ESC%[35m%RepoPath%%ESC%[0m
ECHO.
ECHO     Are you pointed to the correct branch (look at next line)?
ECHO.
REM This takes the output of the git command and stores it in a variable. This is the current branch name.
FOR /F "tokens=* USEBACKQ" %%F IN (`git rev-parse --abbrev-ref HEAD`) DO (
SET parentBranch=%%F
)
ECHO %parentBranch%
ECHO.
PAUSE
ECHO %ESC%[7;31mExecution%ESC%[0m
ECHO =====================================================================
ECHO.
ECHO %ESC%[32mCreating a branch off of current branch%ESC%[0m
git checkout -b tasks/updateVersionNumberTo%VersionNumber%
ECHO.
ECHO %ESC%[32mUpdating the version number to %ESC%[33m%ESC%[1m%VersionNumber%%ESC%[0m%ESC%[0m%ESC%[0m
lprun "%LinqPadScriptPath%\Update Site version.linq" %VersionNumber% %RepoPath%
ECHO.

ECHO %ESC%[32mStaging changes%ESC%[0m
ECHO.
git add -A
ECHO %ESC%[32mCommitting changes%ESC%[0m
ECHO.
git commit -m "Updating Site version number to %VersionNumber% for parent branch %parentBranch%"
ECHO %ESC%[32mPushing changes%ESC%[0m
ECHO.
git push --set-upstream origin tasks/updateVersionNumberTo%VersionNumber%
REM ECHO %ESC%[32mCreating pull request%ESC%[0m
REM ECHO.
REM "git request-pull" does not create pull requests https://git-scm.com/docs/git-request-pull
REM It seems like if you need to create a PR, there is no way to do it via Git Bash really

ECHO %ESC%[7;37mExecution is complete%ESC%[0m
ECHO.
ECHO %ESC%[7;31mManual steps%ESC%[0m
ECHO =====================================================================
ECHO 1. Create a work item
ECHO 2. Create a pull request
ECHO 3. Don't forget to get rid of the local branch when you are finished
ECHO.
PAUSE

:setESC
for /F "tokens=1,2 delims=#" %%a in ('"prompt #$H#$E# & echo on & for %%b in (1) do rem"') do (
  set ESC=%%b
  exit /B 0
)
exit /B 0