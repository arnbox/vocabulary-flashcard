@echo off
:: Open development environment

:: variables
SET FRONT_END_PATH=../../src/VocabularyFlashCard.Web/ClientApp/
SET BACK_END_PATH=../../src/VocabularyFlashCard.Web/
SET PROJECT_PATH=../../
SET APP_NAME="(VocabularyFlashCard)"

SET VS_CODE=CODE
SET VISUAL_STUDIO=devenv
SET SOLUTION_PATH=../../VocabularyFlashCard.sln

:: Open Windows Terminal for git, front-end and back-end
START WT ^
	nt -d %PROJECT_PATH% --title "Root for git %APP_NAME%" --tabColor #f34c26; ^
	split-pane -d %BACK_END_PATH% --title "Back-end %APP_NAME%" --tabColor #63228f; ^
	split-pane -d %FRONT_END_PATH% --title "Front-end %APP_NAME%" --tabColor #2088cd

:: Open solution in Visual Studio
START "" %VISUAL_STUDIO% %SOLUTION_PATH%

:: Open VS Code for front-end
CODE -n %FRONT_END_PATH% | EXIT
