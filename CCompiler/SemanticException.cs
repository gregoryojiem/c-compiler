using CCompiler.CSyntaxTree.Expressions;

namespace CCompiler;

public class SemanticException : Exception
{
    private SemanticException(int line, int column, string message)
        : base($"Semantic error! On line: {line}, column {column}: {message}")
    {
    }

    private SemanticException(Token token, string message)
    {
        throw new SemanticException(token.Line, token.Column, message);
    }

    public static void UndeclaredVariableError(Token variable)
    {
        throw new SemanticException(variable, "The variable '" + variable + "' has not been declared");
    }

    public static void CheckDuplicateDeclaration(Dictionary<string, string> variableMap, Token variable)
    {
        if (variableMap.ContainsKey(variable.Value))
        {
            throw new SemanticException(variable, "The variable '" + variable + "' has already been declared");
        }
    }
    
    public static void CheckValidAssignment(ExpressionNode leftExpression)
    {
        if (leftExpression is VariableNode) return;
        var expressionToken = leftExpression.GetRepresentativeToken();
        throw new SemanticException(expressionToken, $"Cannot a value to {leftExpression}");
    }
}