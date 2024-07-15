namespace CCompiler.AssemblySyntaxTree.Operands;

public class StackOp : IOperand
{
    private int _offset;

    public StackOp(int offset)
    {
        _offset = offset;
    }

    public override string ToString()
    {
        return _offset + "(%rbp)";
    }
}