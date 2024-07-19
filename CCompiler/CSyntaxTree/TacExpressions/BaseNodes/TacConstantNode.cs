namespace CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

public class TacConstantNode : BaseValueNode
{
    public readonly int Value;

    public TacConstantNode(int value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}