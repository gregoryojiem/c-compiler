namespace CCompiler.AssemblySyntaxTree.Operands;

public class PseudoRegOp : IOperand
{
    public string Identifier;

    public PseudoRegOp(string identifier)
    {
        Identifier = identifier;
    }
}