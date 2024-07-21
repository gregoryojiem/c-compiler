namespace CCompiler.AssemblySyntaxTree.Operands;

public class PseudoRegOp : IOperand
{
    public readonly string Identifier;

    public PseudoRegOp(string identifier)
    {
        Identifier = identifier;
    }
}