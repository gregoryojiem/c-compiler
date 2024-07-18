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
    private bool _useByteRegister;
    
    public RegOp(Register register)
    {
        _register = register;
    }

    public void SetByteRegister()
    {
        _useByteRegister = true;
    }
    
    public override string ToString()
    {
        if (_useByteRegister)
        {
            return ToStringByte();
        }
        
        return _register switch
        {
            Register.Eax => "%eax",
            Register.Edx => "%edx",
            Register.R10d => "%r10d",
            Register.R11d => "%r11d",
            _ => throw new ArgumentException("Invalid register type.")
        };
    }

    private string ToStringByte()
    {
        return _register switch
        {
            Register.Eax => "%al",
            Register.Edx => "%dl",
            Register.R10d => "%r10b",
            Register.R11d => "%r11b",
            _ => throw new ArgumentException("Invalid register type.")
        };
    }
}