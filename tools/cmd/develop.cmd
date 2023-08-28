@echo off
:: Purpose of this batch file is to open development environment

:: set variables
REM SET FRONT_END_PATH="c:\\arnData\\projects\\VocabularyFlashCard\\VocabularyFlashCard\\VocabularyFlashCard.Web\\ClientApp\\"
SET FRONT_END_PATH=../../src/VocabularyFlashCard.Web/ClientApp/
REM SET BACK_END_PATH="c:\\arnData\\projects\\VocabularyFlashCard\\VocabularyFlashCard\\VocabularyFlashCard.Web\\"
SET BACK_END_PATH=../../src/VocabularyFlashCard.Web/
SET APP_NAME="(VocabularyFlashCard)"


REM SET VS_CODE="C:\\Users\\AlirezaNasrollahzade\\AppData\\Local\\Programs\\Microsoft VS Code\\Code.exe"
SET VS_CODE=CODE
REM SET VISUAL_STUDIO="C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\Common7\\IDE\\devenv.exe"
SET VISUAL_STUDIO=devenv
REM SET SOLUTION_PATH="c:\\arnData\\projects\\VocabularyFlashCard\\VocabularyFlashCard\\VocabularyFlashCard.sln"
SET SOLUTION_PATH=../../VocabularyFlashCard.sln


:: Open Windows Terminal for front-end and back-end
@REM START "" WT -d %FRONT_END_PATH% --tabColor #32CD32 --title "Front-end %APP_NAME%"; new-tab -d %BACK_END_PATH% --tabColor #1e90ff --title "Back-end %APP_NAME%"; focus-tab -t 0
START "" WT -d %FRONT_END_PATH% --tabColor #2088cd --title "Front-end %APP_NAME%"; split-pane -d %BACK_END_PATH% --tabColor #63228f --title "Back-end %APP_NAME%"

:: Open solution in Visual Studio
START "" %VISUAL_STUDIO% %SOLUTION_PATH%

:: Open VS Code for front-end
CODE -n %FRONT_END_PATH% | EXIT
