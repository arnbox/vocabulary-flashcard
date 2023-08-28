@echo off
:: Backup as 7zip file for source codes

:: Get current date and time
:: timestamp YYYY-MM-DD_HH-MM-SS
FOR /f "delims=" %%a in ('wmic OS Get localdatetime  ^| FIND "."') do SET dt=%%a
SET dt=%dt:~0,4%-%dt:~4,2%-%dt:~6,2%_%dt:~8,2%-%dt:~10,2%-%dt:~12,2%
:: echo %dt%

:: Set current path to .cmd file path
CD %~dp0

:: set variables
SET PROJECT_NAME=VocabularyFlashCard
SET BACKUP_FOLDER=code-backup\
SET TARGET_FILE="%BACKUP_FOLDER%%PROJECT_NAME%-%dt%.7z"

SET SOURCE_FOLDER=VocabularyFlashCard
SET WORKING_FOLDER=..\..\..\
:: add "-xr!+folder_name" for example for "bin" folder: -xr!bin
SET EXCLUDE_FOLDERS=-xr!bin -xr!obj -xr!node_modules -xr!.vs -xr!dist
SET ZIP_PARAM=a -t7z -mx9 -md1024m -mfb256 -mmt2

SET ZIPPER="c:\Program Files\7-Zip\7z.exe"

:: TIMEOUT 5
CD %WORKING_FOLDER%

%ZIPPER% %ZIP_PARAM% %TARGET_FILE% "%SOURCE_FOLDER%\*" %EXCLUDE_FOLDERS%

REM PAUSE