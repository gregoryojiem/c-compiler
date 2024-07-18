using CCompiler.AssemblySyntaxTree.Operands;

namespace CCompiler.AssemblySyntaxTree.Instructions;

public class CmpNode : AsmInstructionNode, IAllocatableInstruction
{
    private IOperand _leftOp;
    private IOperand _rightOp;

    public CmpNode(IOperand leftOp, IOperand rightOp)
    {
        _leftOp = leftOp;
        _rightOp = rightOp;
    }

    public void DoAllocationPass(Dictionary<string, int> variableMap, ref int stackPos)
    {
        if (_leftOp is PseudoRegOp srcPseudo)
        {
            _leftOp = IAllocatableInstruction.HandleAllocation(variableMap, ref stackPos, srcPseudo);
        }

        if (_rightOp is PseudoRegOp dstPseudo)
        {
            _rightOp = IAllocatableInstruction.HandleAllocation(variableMap, ref stackPos, dstPseudo);
        }
    }

    public void FixOperands(List<AsmInstructionNode> instructions)
    {
        var secondOpNotConst = _rightOp is not ImmOp;
        var singleMemoryAddress = _leftOp is not StackOp || _rightOp is not StackOp;
        if (secondOpNotConst && singleMemoryAddress)
        {
            instructions.Add(this);
            return;
        }

        if (!secondOpNotConst)
        {
            var originalDst = _rightOp;
            var tempRegister = new RegOp(RegOp.Register.R11d);
            _rightOp = tempRegister;
            instructions.Add(new MovlNode(originalDst, tempRegister));
            instructions.Add(this);
            return;
        }
        
        var scratchRegister = new RegOp(RegOp.Register.R10d);
        var tempVariable = new MovlNode(_leftOp, scratchRegister);
        _leftOp = scratchRegister;
        instructions.Add(tempVariable);
        instructions.Add(this);
    }


    public override string ConvertToAsm()
    {
        return "cmpl\t" + _leftOp + ", " + _rightOp;
    }
}