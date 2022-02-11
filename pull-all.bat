@echo off

call settings.bat

git pull --recurse-submodules
if not "%ERRORLEVEL%" == "0" ( exit /b )

cd "%SUBMODULE_DIR%/"
if not "%ERRORLEVEL%" == "0" ( exit /b )
