using CCompiler.AssemblySyntaxTree.Operands;

namespace CCompiler.AssemblySyntaxTree.Instructions;

public class JmpCcNode : AsmInstructionNode
{
    private readonly TokenType _condCode;
    private readonly string _identifier;

    public JmpCcNode(TokenType condCode, string identifier)
    {
        _condCode = condCode;
        _identifier = identifier;
    }

    public override string ToString()
    {
        return "j" + IOperand.GetConditionCode(_condCode) + "\t\t." + _identifier;
    }
}