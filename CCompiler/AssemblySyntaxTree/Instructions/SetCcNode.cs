using CCompiler.AssemblySyntaxTree.Operands;

namespace CCompiler.AssemblySyntaxTree.Instructions;

public class SetCcNode : AsmInstructionNode
{
    private readonly TokenType _relationalOp;
    private readonly IOperand _dst;

    public SetCcNode(TokenType relationalOp, IOperand dst)
    {
        _relationalOp = relationalOp;
        _dst = dst;
    }

    public override string ConvertToAsm()
    {
        throw new NotImplementedException();
    }
}