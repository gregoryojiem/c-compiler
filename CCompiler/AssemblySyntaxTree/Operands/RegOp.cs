namespace CCompiler.AssemblySyntaxTree.Operands;

public class RegOp : IOperand
{
    public enum Register
    {
        Eax,
        R10d
    }

    private readonly Register _register;

    public RegOp(Register register)
    {
        _register = register;
    }

    public override string ToString()
    {
        return _register switch
        {
            Register.Eax => "%eax",
            Register.R10d => "%R10D",
            _ => throw new ArgumentException("Invalid register type.")
        };
    }
}