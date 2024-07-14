namespace CCompiler.CSyntaxTree;

public class ConstantNode : ExpressionNode
{
    public readonly string ConstantValue;

    public ConstantNode(TokenList tokens)
    {
        ConstantValue = tokens.PopExpected(TokenType.IntegerLiteral).Value;
    }

    public override string ToString()
    {
        return ConstantValue;
    }
}