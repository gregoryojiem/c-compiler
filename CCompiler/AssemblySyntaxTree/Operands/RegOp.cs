namespace CCompiler.AssemblySyntaxTree.Operands;

public class RegOp : IOperand
{
    public enum Register
    {
        Eax,
        Edx,
        R10d,
        R11d
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
            Register.Edx => "%edx",
            Register.R10d => "%r10d",
            Register.R11d => "%r11d",
            _ => throw new ArgumentException("Invalid register type.")
        };
    }
}