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

    public void FixOperands(List<AsmInstructionNode> instructions)
    {
        if (_src is not StackOp srcStackOp || _dst is not StackOp)
        {
            instructions.Add(this);
            return;
        }

        var scratchRegister = new RegOp(RegOp.Register.R10d);
        var tempVariable = new MovlNode(srcStackOp, scratchRegister);
        _src = scratchRegister;
        instructions.Add(tempVariable);
        instructions.Add(this);
    }

    public override string ConvertToAsm()
    {
        return "movl\t" + _src + ", " + _dst;
    }
}