using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.Expressions;

public class ConstantNode : ExpressionNode
{
    private readonly Token _valueToken;

    public ConstantNode(Token value)
    {
        _valueToken = value;
    }

    public override void VariableResolution(Dictionary<string, string> variableMap)
    {
    }

    public override Token GetRepresentativeToken()
    {
        return _valueToken;
    }

    public override TacExpressionNode ConvertToTac(List<StatementNode> statementList)
    {
        return new TacConstantNode(int.Parse(_valueToken.Value));
    }

    public override string ToString()
    {
        return _valueToken.ToString();
    }
}