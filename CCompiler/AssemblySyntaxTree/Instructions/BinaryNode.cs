using CCompiler.AssemblySyntaxTree.Operands;

namespace CCompiler.AssemblySyntaxTree.Instructions;

public class BinaryNode : AsmInstructionNode, IAllocatableInstruction
{
    private readonly TokenType _binaryOp;
    private IOperand _src;
    private IOperand _dst;

    public BinaryNode(TokenType binaryOp, IOperand src, IOperand dst)
    {
        _binaryOp = binaryOp;
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
        if (_binaryOp is TokenType.Multiply && _dst is StackOp)
        {
            var originalDst = _dst;
            var tempRegister = new RegOp(RegOp.Register.R11d);
            _dst = tempRegister;
            instructions.Add(new MovlNode(originalDst, tempRegister));
            instructions.Add(this);
            instructions.Add(new MovlNode(tempRegister, originalDst));
            return;
        }

        if (_binaryOp is TokenType.Multiply)
        {
            instructions.Add(this);
            return;
        }

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

    public override string ToString()
    {
        return IOperand.TokenTypeToBinaryOp(_binaryOp) + "\t" + _src + ", " + _dst;
    }
}