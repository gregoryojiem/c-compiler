namespace CCompiler.AssemblySyntaxTree.Operands;

public class ImmOp : IOperand
{
    private readonly int _value;

    public ImmOp(int constantValue)
    {
        _value = constantValue;
    }

    public override string ToString()
    {
        return "$" + _value;
    }
}