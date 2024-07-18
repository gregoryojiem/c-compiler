namespace CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

public class ConstantNode : BaseValueNode
{
    public readonly int Value;

    public ConstantNode(int value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}