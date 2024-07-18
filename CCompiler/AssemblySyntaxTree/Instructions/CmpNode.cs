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
        if (_rightOp is not ImmOp)
        {
            return;
        }

        var originalDst = _rightOp;
        var tempRegister = new RegOp(RegOp.Register.R11d);
        _rightOp = tempRegister;
        instructions.Add(new MovlNode(originalDst, tempRegister));
        instructions.Add(this);
    }


    public override string ConvertToAsm()
    {
        return "cmpl\t" + _leftOp + ", " + _rightOp;
    }
}