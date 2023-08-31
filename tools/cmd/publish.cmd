@echo off
set PUBLISH_FOLDER=d:\project\aspcore\VocabularyFlashCard All\publishfolder
set APP_FOLDER="d:\project\aspcore\VocabularyFlashCard All\vocabulary-flashcard\src\VocabularyFlashCard.Web"
set ZIP_FILE_NAME="vocabularyflashcard"
set ZIPPER="c:\Program Files\7-Zip\7z.exe"

set SECRECTS_FOLDER="d:\project\aspcore\VocabularyFlashCard All\secrets-settings\production"
set FRONTEND_PATH="d:\project\aspcore\VocabularyFlashCard All\vocabulary-flashcard\src\VocabularyFlashCard.Web\ClientApp"
set DIST_PATH="d:\project\aspcore\VocabularyFlashCard All\vocabulary-flashcard\src\VocabularyFlashCard.Web\ClientApp\dist"
set WWWROOT_PATH="d:\project\aspcore\VocabularyFlashCard All\publishfolder\wwwroot\"
D:

:: Clean Publish folder
ECHO Delete all file from publish folder ...
CD "%PUBLISH_FOLDER%"
del * /q
for /D %%i in (*) do rd /s /q "%%i"

:: Publish application
CD %APP_FOLDER%
@REM dotnet publish -p:PublishProfile=FolderProfile -c Release --self-contained true -o "%PUBLISH_FOLDER%" -r win-x86 -p:PublishTrimmed=false
dotnet publish -p:PublishProfile=FolderProfile -c Release -o "%PUBLISH_FOLDER%" -r win-x86 /p:DebugType=None /p:DebugSymbols=false

:: Secure app settings
CD %SECRECTS_FOLDER%
node .\key-replacer.js

:: Build front-end
CD %FRONTEND_PATH%
CMD /c "npm run build"
MD %WWWROOT_PATH%
XCOPY %DIST_PATH% %WWWROOT_PATH% /E

:: Get current date and time
:: timestamp YYYY-MM-DD_HH-MM-SS
for /f "delims=" %%a in ('wmic OS Get localdatetime  ^| find "."') do set dt=%%a
set dt=%dt:~0,4%-%dt:~4,2%-%dt:~6,2%_%dt:~8,2%-%dt:~10,2%-%dt:~12,2%
:: echo %dt%


:: ZIP Publish
:: TIMEOUT 5
CD %PUBLISH_FOLDER%
CD..
%ZIPPER% a -tzip "c:\Users\nasro\Desktop\%ZIP_FILE_NAME%-%dt%.zip" "%PUBLISH_FOLDER%\*"

PAUSE