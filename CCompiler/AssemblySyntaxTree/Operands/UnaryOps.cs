namespace CCompiler.AssemblySyntaxTree.Operands;

public static class UnaryOps
{
    public static string TokenTypeToUnaryOp(TokenType tokenType)
    {
        return tokenType switch
        {
            TokenType.Negation => "negl",
            TokenType.Complement => "notl",
            _ => throw new NotImplementedException()
        };
    }
}