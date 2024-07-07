namespace CCompiler.AssemblySyntaxTree;

public class RetNode : AsmInstructionNode
{
    public override string ConvertToAsm()
    {
        return "ret";
    }
}