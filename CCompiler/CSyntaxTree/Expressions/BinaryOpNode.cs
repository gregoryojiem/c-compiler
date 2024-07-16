namespace CCompiler.CSyntaxTree.Expressions;

public class BinaryOpNode : ExpressionNode
{
    public readonly Token BinaryOperator;
    public readonly ExpressionNode LeftExpression;
    public readonly ExpressionNode RightExpression;

    public BinaryOpNode(Token binaryOperator, ExpressionNode leftExpression, ExpressionNode rightExpression)
    {
        BinaryOperator = binaryOperator;
        LeftExpression = leftExpression;
        RightExpression = rightExpression;
    }

    public override string ToString()
    {
        return "(" + LeftExpression + " " + BinaryOperator.Value + " " + RightExpression + ")";
    }
}