@echo off
setlocal EnableDelayedExpansion

set tests_passed=0
set total_tests=0

for /f "delims=" %%a in ('dir /b /s "c-examples\valid\*.*"') do (
  if "%%~xa"==".c" (
    :: Test the custom compiler on each .c file in the given subdirectory
    set /a total_tests+=1
    echo Compiling: %%a
    .\CCompiler\bin\Debug\net7.0\CCompiler.exe "%%a"
    if !errorlevel! lss 0 (
        echo Test result: FAIL, compilation unsuccessful.
    ) else (
        :: Compilation was successful, so we assemble, link, and then run the .exe created
        gcc -c "%%~dpna.s" -o "%%~dpna.o" 
        gcc "%%~dpna.o" -o "%%~dpna" 
        call "%%~dpna"
        set ccompiler_return=!errorlevel!
        echo Return value: !ccompiler_return!
        
        :: Test the result of custom compiler against the result of gcc compiler
        gcc "%%~dpna.c" -o "%%~dpna_gcc.exe" 
        call "%%~dpna_gcc.exe"
        set gcc_return=!errorlevel!
        echo gcc return value: !gcc_return!
        if !ccompiler_return! == !gcc_return! (
          echo Test result: PASS, return value matches gcc.
          set /a tests_passed+=1
        ) else (
          echo Test result: FAIL, return value differs from gcc.
        )
        
        :: Clean up the files produced
        del "%%~dpna.s"
        del "%%~dpna.o"
        del "%%~dpna.exe"
        del "%%~dpna_gcc.exe"
    )
    echo.
  )
)

echo Total tests passed: %tests_passed%/%total_tests%

endlocal
pause
