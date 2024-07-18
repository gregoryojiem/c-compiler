namespace CCompiler.CSyntaxTree.TacStatements;

public class LabelNode : TacStatementNode
{
    private string _identifier;

    public LabelNode(string identifier)
    {
        _identifier = identifier;
    }
    
    public override string ToString()
    {
        return _identifier + ":";
    }
}