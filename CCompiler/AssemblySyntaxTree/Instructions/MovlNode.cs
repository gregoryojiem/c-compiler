using CCompiler.AssemblySyntaxTree.Operands;

namespace CCompiler.AssemblySyntaxTree.Instructions;

public class MovlNode : AsmInstructionNode, IAllocatableInstruction
{
    private IOperand _src;
    private IOperand _dst;

    public MovlNode(IOperand src, IOperand dst)
    {
        _src = src;
        _dst = dst;
    }

    public override string ConvertToAsm()
    {
        return "movl" + _src + ", " + _dst;
    }

    public void DoAllocationPass(Dictionary<string, int> variableMap, ref int stackPos)
    {
        if (_src is PseudoRegOp srcPseudo)
        {
            _src = IAllocatableInstruction.HandleAllocation(variableMap, ref stackPos, srcPseudo);
        }
        if (_dst is PseudoRegOp dstPseudo)
        {
            _dst = IAllocatableInstruction.HandleAllocation(variableMap, ref stackPos, dstPseudo);
        }
    }
}