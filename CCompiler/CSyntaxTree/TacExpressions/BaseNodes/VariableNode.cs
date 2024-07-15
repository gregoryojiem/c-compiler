namespace CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

public class VariableNode : BaseValueNode
{
    public string Identifier;

    public VariableNode(string identifier)
    {
        Identifier = identifier;
    }
}