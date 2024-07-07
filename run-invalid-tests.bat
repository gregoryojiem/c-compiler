@echo off
setlocal EnableDelayedExpansion

set tests_passed=0
set total_tests=0

for /f "delims=" %%a in ('dir /b /s "CCompiler\examples\invalid\*.*"') do (
  if "%%~xa"==".c" (
    echo Compiling: %%a
    .\CCompiler\bin\Debug\net7.0\CCompiler.exe "%%a" 
    
    if !errorlevel! lss 0 (
      echo Test result: PASS, program correctly detected an error.
      set /a tests_passed+=1
    ) else (
      echo Test result: FAIL, program failed to detect error.
      del "%%~dpna.s"
    )
    
    echo.
    set /a total_tests+=1
  )
)

echo Total tests passed: %tests_passed%/%total_tests%

pause
endlocal
