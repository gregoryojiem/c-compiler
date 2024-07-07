namespace CCompiler;

public class SyntaxError : Exception
{
    public SyntaxError(int line, int column, string message)
        : base($"Error! On line: {line}, column {column}: {message}")
    {
    }
    
    public SyntaxError(Token token, string message)
    {
        throw new SyntaxError(token.Line, token.Column, message);
    }
    
    //todo expected values, help messages
    public static void ValidReturnType(Token returnToken)
    {
        if (!Token.VariableTypeTokens.Contains(returnToken.Type))
        {
            throw new SyntaxError(returnToken, $"Invalid function return type: {returnToken}");
        }
    }

    public static void ValidFunctionName(Token functionName)
    {
        if (functionName.Type != TokenType.Identifier)
        {
            throw new SyntaxError(functionName, $"Invalid function name: {functionName}");
        }
    }
}