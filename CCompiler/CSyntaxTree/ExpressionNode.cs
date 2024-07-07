namespace CCompiler.CSyntaxTree;

public abstract class ExpressionNode
{
    public static ExpressionNode CreateExpressionNode(TokenList tokens)
    {
        var constantNode = new ConstantNode(tokens);
        return constantNode;
    }
}