@echo off
setlocal EnableDelayedExpansion

set tests_passed=0
set total_tests=0

for /f "delims=" %%a in ('dir /b /s "CCompiler\examples\valid\*.*"') do (
  if "%%~xa"==".c" (
    :: Use custom compiler, assemble and link with gcc, then run and capture the return value of main
    echo Compiling: %%a
    .\CCompiler\bin\Debug\net7.0\CCompiler.exe "%%a" 
    echo Assembling: "%%~dpna.s"
    gcc -c "%%~dpna.s" -o "%%~dpna.o" 
    echo Linking: "%%~dpna.o"
    gcc "%%~dpna.o" -o "%%~dpna" 
    echo Running: "%%~dpna.exe"
    call "%%~dpna.exe"
    set ccompiler_return=!errorlevel!
    echo Return value: !ccompiler_return!
    
    :: Test the result of custom compiler against the result of gcc compiler
    gcc "%%~dpna.c" -o "%%~dpna_gcc.exe" 
    call "%%~dpna_gcc.exe"
    set gcc_return=!errorlevel!
    if !errorlevel! == !gcc_return! (
      echo Test result: PASS, return values match.
      set /a tests_passed+=1
    ) else (
      echo Test result: FAIL, return values differ.
    )
    
    :: Clean up the files produced
    echo.
    del "%%~dpna.s"
    del "%%~dpna.o"
    del "%%~dpna.exe"
    del "%%~dpna_gcc.exe"
    set /a total_tests+=1
  )
)

echo Total tests passed: %tests_passed%/%total_tests%

pause
endlocal
