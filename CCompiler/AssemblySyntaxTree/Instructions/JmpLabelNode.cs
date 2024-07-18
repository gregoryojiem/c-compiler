namespace CCompiler.AssemblySyntaxTree.Instructions;

public class JmpLabelNode : AsmInstructionNode
{
    private string _identifier;

    public JmpLabelNode(string identifier)
    {
        _identifier = identifier;
    }

    public override string ConvertToAsm()
    {
        return "." + _identifier + ":";
    }
}