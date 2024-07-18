namespace CCompiler.CSyntaxTree.TacStatements;

public class JumpNode : TacStatementNode
{
    public readonly string Target;

    public JumpNode(string target)
    {
        Target = target;
    }

    public override string ToString()
    {
        return "goto " + Target;
    }
}