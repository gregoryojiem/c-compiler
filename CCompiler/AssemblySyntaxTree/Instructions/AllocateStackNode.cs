namespace CCompiler.AssemblySyntaxTree.Instructions;

public class AllocateStackNode : AsmInstructionNode
{
    private static int _allocationAmount;

    public AllocateStackNode(int allocationAmount)
    {
        _allocationAmount = allocationAmount;
    }

    public override string ToString()
    {
        return "subq\t" + "$" + _allocationAmount + ", " + "%rsp";
    }
}