call settings.bat

if not exist "%SUBMODULE_DIR%" (
  mkdir "%SUBMODULE_DIR%"
)

git submodule init
if not "%ERRORLEVEL%" == "0" ( exit /b )

pull-all.bat