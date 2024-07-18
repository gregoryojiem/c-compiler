namespace CCompiler.CSyntaxTree.TacStatements;

public class JumpNode : TacStatementNode
{
    private string _target;

    public JumpNode(string target)
    {
        _target = target;
    }

    public override string ToString()
    {
        return "goto " + _target;
    }
}