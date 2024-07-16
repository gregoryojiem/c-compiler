namespace CCompiler.CSyntaxTree.Expressions;

public class UnaryOpNode : ExpressionNode
{
    public readonly Token UnaryOperator;
    public readonly ExpressionNode Expression;

    public UnaryOpNode(TokenList tokens)
    {
        UnaryOperator = tokens.Pop();
        Expression = ParseExpressionFactor(tokens);
    }
}