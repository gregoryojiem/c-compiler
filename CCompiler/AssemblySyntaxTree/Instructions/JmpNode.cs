namespace CCompiler.AssemblySyntaxTree.Instructions;

public class JmpNode : AsmInstructionNode
{
    private string _identifier;

    public JmpNode(string identifier)
    {
        _identifier = identifier;
    }

    public override string ConvertToAsm()
    {
        throw new NotImplementedException();
    }
}