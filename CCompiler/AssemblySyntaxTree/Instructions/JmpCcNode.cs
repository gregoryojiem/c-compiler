namespace CCompiler.AssemblySyntaxTree.Instructions;

public class JmpCcNode : AsmInstructionNode
{
    private TokenType _equalityType;
    private string _identifier;
    
    public JmpCcNode(TokenType equalityType, string identifier)
    {
        _equalityType = equalityType;
        _identifier = identifier;
    }

    public override string ConvertToAsm()
    {
        throw new NotImplementedException();
    }
}