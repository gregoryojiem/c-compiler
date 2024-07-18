namespace CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

public class VariableNode : BaseValueNode
{
    public readonly string Identifier;

    public VariableNode(string identifier)
    {
        Identifier = identifier;
    }

    public override string ToString()
    {
        return Identifier;
    }
}