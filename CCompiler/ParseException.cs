namespace CCompiler;

public class ParseException : Exception
{
    public ParseException(int line, int column, string message)
        : base($"Parse error! On line: {line}, column {column}: {message}")
    {
    }
    
    public ParseException(Token token, string message)
    {
        throw new ParseException(token.Line, token.Column, message);
    }
    
    //TODO expected values, help messages
    public static void ValidReturnType(Token returnToken)
    {
        if (!Token.VariableTypeTokens.Contains(returnToken.Type))
        {
            throw new ParseException(returnToken, $"Invalid function return type: {returnToken}");
        }
    }

    public static void ValidFunctionName(Token functionName)
    {
        if (functionName.Type != TokenType.Identifier)
        {
            throw new ParseException(functionName, $"Invalid function name: {functionName}");
        }
    }
}