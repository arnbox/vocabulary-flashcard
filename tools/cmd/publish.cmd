@echo off
SET PUBLISH_FOLDER=d:\project\aspcore\VocabularyFlashCard All\publishfolder
SET APP_FOLDER="d:\project\aspcore\VocabularyFlashCard All\vocabulary-flashcard\src\VocabularyFlashCard.Web"
SET ZIP_FILE_NAME="vocabularyflashcard"

SET ZIPPER="c:\Program Files\7-Zip\7z.exe"
SET SECRECTS_FOLDER="d:\project\aspcore\VocabularyFlashCard All\secrets-settings\production"
SET FRONTEND_PATH="d:\project\aspcore\VocabularyFlashCard All\vocabulary-flashcard\src\VocabularyFlashCard.Web\ClientApp"
SET DIST_PATH="d:\project\aspcore\VocabularyFlashCard All\vocabulary-flashcard\src\VocabularyFlashCard.Web\ClientApp\dist"
SET WWWROOT_PATH="d:\project\aspcore\VocabularyFlashCard All\publishfolder\wwwroot\"
D:

:: Clean Publish folder
ECHO Delete all file from publish folder ...
CD "%PUBLISH_FOLDER%"
DEL * /q
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
FOR /f "delims=" %%a IN ('powershell -command "Get-Date -Format 'yyyy-MM-dd_HH-mm-ss'"') DO SET dt=%%a
:: echo %dt%


:: ZIP Publish
:: TIMEOUT 5
CD %PUBLISH_FOLDER%
CD..
%ZIPPER% a -tzip "%USERPROFILE%\Desktop\%ZIP_FILE_NAME%-%dt%.zip" "%PUBLISH_FOLDER%\*"

PAUSE