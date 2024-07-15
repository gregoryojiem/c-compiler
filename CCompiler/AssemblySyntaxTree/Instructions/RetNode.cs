namespace CCompiler.AssemblySyntaxTree.Instructions;

public class RetNode : AsmInstructionNode
{
    public override string ConvertToAsm()
    {
        return "ret";
    }
}