namespace CCompiler;

public class SyntaxError : Exception
{
    public SyntaxError(int line, int column, string message)
        : base($"Syntax error! On line: {line}, column {column}: {message}")
    {
    }
}