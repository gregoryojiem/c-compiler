using CCompiler.AssemblySyntaxTree.Operands;

namespace CCompiler.AssemblySyntaxTree.Instructions;

public class DivlNode : AsmInstructionNode, IAllocatableInstruction
{
    private IOperand _divisor;

    public DivlNode(IOperand divisor)
    {
        _divisor = divisor;
    }

    public void DoAllocationPass(Dictionary<string, int> variableMap, ref int stackPos)
    {
        if (_divisor is PseudoRegOp divisor)
        {
            _divisor = IAllocatableInstruction.HandleAllocation(variableMap, ref stackPos, divisor);
        }
    }

    public void FixConstantOps(List<AsmInstructionNode> fixedInstructions)
    {
        if (_divisor is ImmOp)
        {
            var scratchRegister = new RegOp(RegOp.Register.R10d);
            fixedInstructions.Add(new MovlNode(_divisor, scratchRegister));
            _divisor = scratchRegister;
        }

        fixedInstructions.Add(this);
    }

    public override string ConvertToAsm()
    {
        return "idivl\t" + _divisor; 
    }
}