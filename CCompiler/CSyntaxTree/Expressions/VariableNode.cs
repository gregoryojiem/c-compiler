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

    public override void VariableResolution(Dictionary<string, string> variableMap)
    {
        var variableName = _identifier.Value;
        variableMap.TryGetValue(variableName, out var uniqueName);
        if (uniqueName != null)
            _identifier.Value = uniqueName;
        else
            SemanticException.UndeclaredVariableError(_identifier);
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