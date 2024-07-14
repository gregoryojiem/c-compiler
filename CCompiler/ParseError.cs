namespace CCompiler;

public class ParseError : Exception
{
    public ParseError(int line, int column, string message)
        : base($"Parse error! On line: {line}, column {column}: {message}")
    {
    }
    
    public ParseError(Token token, string message)
    {
        throw new ParseError(token.Line, token.Column, message);
    }
    
    //TODO expected values, help messages
    public static void ValidReturnType(Token returnToken)
    {
        if (!Token.VariableTypeTokens.Contains(returnToken.Type))
        {
            throw new ParseError(returnToken, $"Invalid function return type: {returnToken}");
        }
    }

    public static void ValidFunctionName(Token functionName)
    {
        if (functionName.Type != TokenType.Identifier)
        {
            throw new ParseError(functionName, $"Invalid function name: {functionName}");
        }
    }
}