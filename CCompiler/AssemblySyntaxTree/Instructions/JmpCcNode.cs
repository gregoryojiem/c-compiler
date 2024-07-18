using CCompiler.AssemblySyntaxTree.Operands;

namespace CCompiler.AssemblySyntaxTree.Instructions;

public class JmpCcNode : AsmInstructionNode
{
    private TokenType _condCode;
    private string _identifier;
    
    public JmpCcNode(TokenType condCode, string identifier)
    {
        _condCode = condCode;
        _identifier = identifier;
    }

    public override string ConvertToAsm()
    {
        return "j" + IOperand.GetConditionCode(_condCode) + "\t\t." + _identifier;
    }
}