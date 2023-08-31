@echo off
:: Purpose of this batch file is to open development environment

:: set variables
SET FRONT_END_PATH=../../src/VocabularyFlashCard.Web/ClientApp/
SET BACK_END_PATH=../../src/VocabularyFlashCard.Web/
SET APP_NAME="(VocabularyFlashCard)"


SET VS_CODE=CODE
SET VISUAL_STUDIO=devenv
SET SOLUTION_PATH=../../VocabularyFlashCard.sln


:: Open Windows Terminal for front-end and back-end
START "" WT -d %FRONT_END_PATH% --tabColor #2088cd --title "Front-end %APP_NAME%"; split-pane -d %BACK_END_PATH% --tabColor #63228f --title "Back-end %APP_NAME%"

:: Open solution in Visual Studio
START "" %VISUAL_STUDIO% %SOLUTION_PATH%

:: Open VS Code for front-end
CODE -n %FRONT_END_PATH% | EXIT
