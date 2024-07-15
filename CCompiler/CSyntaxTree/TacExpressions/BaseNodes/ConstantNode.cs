namespace CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

public class ConstantNode : BaseValueNode
{
    public readonly int Value;

    public ConstantNode(TokenList tokens)
    {
        Value = int.Parse(tokens.PopExpected(TokenType.IntegerLiteral).Value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}