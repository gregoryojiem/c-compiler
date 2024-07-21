namespace CCompiler.AssemblySyntaxTree.Instructions;

public class JmpLabelNode : AsmInstructionNode
{
    private readonly string _identifier;

    public JmpLabelNode(string identifier)
    {
        _identifier = identifier;
    }

    public override string ToString()
    {
        return "." + _identifier + ":";
    }
}