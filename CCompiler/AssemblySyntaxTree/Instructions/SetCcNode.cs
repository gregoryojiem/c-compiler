using CCompiler.AssemblySyntaxTree.Operands;

namespace CCompiler.AssemblySyntaxTree.Instructions;

public class SetCcNode : AsmInstructionNode, IAllocatableInstruction
{
    private readonly TokenType _relationalOp;
    private IOperand _dst;

    public SetCcNode(TokenType relationalOp, IOperand dst)
    {
        _relationalOp = relationalOp;
        _dst = dst;
    }

    public void DoAllocationPass(Dictionary<string, int> variableMap, ref int stackPos)
    {
        if (_dst is PseudoRegOp dstPseudo)
        {
            _dst = IAllocatableInstruction.HandleAllocation(variableMap, ref stackPos, dstPseudo);
        }
    }

    public override string ToString()
    {
        if (_dst is RegOp dstRegOp)
        {
            dstRegOp.SetByteRegister();
        }

        return "set" + IOperand.GetConditionCode(_relationalOp) + "\t" + _dst;
    }
}