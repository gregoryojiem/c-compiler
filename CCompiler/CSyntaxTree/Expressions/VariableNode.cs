using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.Expressions;

public class VariableNode : ExpressionNode
{
    private readonly Token _identifier;

    public VariableNode(Token identifier)
    {
        _identifier = identifier;
    }

    public override void VariableResolution(SymbolTable symbolTable)
    {
        _identifier.Value = symbolTable.GetUniqueId(_identifier);
    }

    public override Token GetRepresentativeToken()
    {
        return _identifier;
    }

    public override TacExpressionNode ConvertToTac(List<StatementNode> statementList)
    {
        return new TacVariableNode(_identifier.Value);
    }

    public override string ToString()
    {
        return _identifier.ToString();
    }
}