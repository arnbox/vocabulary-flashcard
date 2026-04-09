:: Project source code backup as 7zip
@ECHO OFF

:: Generate timestamp for backup filename
:: Get current date and time as YYYY-MM-DD_HH-MM-SS
FOR /f "delims=" %%i IN ('powershell -command "get-date -format 'yyyy-MM-dd_HH-mm-ss'"') DO SET "DATE_TIME=%%i"

:: Define backup configuration variables
SET PROJECT_NAME=VocabularyFlashCard
SET BACKUP_FOLDER=code-backup
SET TARGET_FILE=%BACKUP_FOLDER%\%PROJECT_NAME%-%DATE_TIME%.7z
SET SOURCE_FOLDER=vocabulary-flashcard
SET PROJECT_ROOT_PATH=..\..\..

:: Specify folders to exclude from backup
:: Set "-xr!+folder_name" for example for "bin" folder: -xr!bin
SET EXCLUDE_FOLDERS=-xr!bin -xr!obj -xr!node_modules -xr!.vs -xr!.angular
SET ZIP_PARAM=a -t7z -mx9 -md1024m -mfb256 -mmt2
SET ZIPPER="c:\Program Files\7-Zip\7z.exe"

:: Navigate to project root directory
:: Set current directory to batch file directory, then go project root folder
CD /D "%~dp0"
CD %PROJECT_ROOT_PATH%
IF ERRORLEVEL 1 (
    powershell -command "Write-Host 'Backup could not be done: failed to change to project root folder \"%PROJECT_ROOT_PATH%\".' -ForegroundColor Red"
    PAUSE
    EXIT /B 1
)

ECHO Current path (project root) is:
CD

:: Verify 7-Zip installation
IF NOT EXIST %ZIPPER% (
    powershell -command "Write-Host 'Backup could not be done: 7-Zip executable not found at \"%ZIPPER%\".' -ForegroundColor Red"
    PAUSE
    EXIT /B 1
)

:: Ensure backup folder exists
IF NOT EXIST "%BACKUP_FOLDER%" (
    MD "%BACKUP_FOLDER%"
    IF ERRORLEVEL 1 (
        powershell -command "Write-Host 'Backup could not be done: failed to create backup folder \"%BACKUP_FOLDER%\".' -ForegroundColor Red"
        PAUSE
        EXIT /B 1
    )
)

:: Execute backup compression
%ZIPPER% %ZIP_PARAM% "%TARGET_FILE%" %SOURCE_FOLDER%\* %EXCLUDE_FOLDERS%
SET "ERROR_CODE=%ERRORLEVEL%"

:: Report backup result
IF %ERROR_CODE% EQU 0 (
    ECHO.
    powershell -command "Write-Host 'Backup done successfully.' -ForegroundColor Green"
    ECHO Backup file name: %PROJECT_NAME%-%DATE_TIME%.7z
    ECHO Backup saved to: "%CD%\%TARGET_FILE%"
) ELSE (
    ECHO.
    powershell -command "Write-Host 'Backup could not be done.' -ForegroundColor Red"
    powershell -command "Write-Host 'Reason: 7-Zip exited with error code %ERROR_CODE%.' -ForegroundColor Red"
)

PAUSE
EXIT /B %ERROR_CODE%
