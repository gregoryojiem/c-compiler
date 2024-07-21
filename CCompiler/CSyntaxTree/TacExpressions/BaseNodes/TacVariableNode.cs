namespace CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

public class TacVariableNode : ValueNode
{
    public readonly string Identifier;

    public TacVariableNode(string identifier)
    {
        Identifier = identifier;
    }

    public override string ToString()
    {
        return Identifier.Replace(".", "_");
    }
}