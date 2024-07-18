using CCompiler.AssemblySyntaxTree.Operands;

namespace CCompiler.AssemblySyntaxTree.Instructions;

public class CmpNode : AsmInstructionNode
{
    private IOperand _leftOp;
    private IOperand _rightOp;
    
    public CmpNode(IOperand leftOp, IOperand rightOp)
    {
        this._leftOp = leftOp;
        this._rightOp = rightOp;
    }

    public override string ConvertToAsm()
    {
        throw new NotImplementedException();
    }
}