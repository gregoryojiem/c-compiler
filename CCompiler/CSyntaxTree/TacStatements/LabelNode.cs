namespace CCompiler.CSyntaxTree.TacStatements;

public class LabelNode : TacStatementNode
{
    public readonly string Identifier;

    public LabelNode(string identifier)
    {
        Identifier = identifier;
    }
    
    public override string ToString()
    {
        return Identifier.Replace(".", "_") + ":";
    }
}