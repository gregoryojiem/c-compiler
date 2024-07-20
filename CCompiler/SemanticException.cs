using CCompiler.CSyntaxTree;
using CCompiler.CSyntaxTree.Expressions;

namespace CCompiler;

public class SemanticException : Exception
{
    private SemanticException(int line, int column, string message)
        : base($"Semantic error! On line: {line}, column {column}: {message}")
    {
    }

    public SemanticException(Token token, string message)
    {
        throw new SemanticException(token.Line, token.Column, message);
    }

    public static void CheckDuplicateDeclaration(SymbolTable symbolTable, Token variable)
    {
        if (symbolTable.ExistsInCurrentScope(variable.Value))
        {
            throw new SemanticException(variable, "The variable '" + variable + "' has already been declared");
        }
    }
    
    public static void CheckValidAssignment(ExpressionNode leftExpression)
    {
        if (leftExpression is VariableNode) return;
        var expressionToken = leftExpression.GetRepresentativeToken();
        throw new SemanticException(expressionToken, $"Cannot assign a value to {leftExpression}");
    }
}