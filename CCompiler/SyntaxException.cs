namespace CCompiler;

public class SyntaxException : Exception
{
    public SyntaxException(int line, int column, string message)
        : base($"Syntax error! On line: {line}, column {column}: {message}")
    {
    }
}