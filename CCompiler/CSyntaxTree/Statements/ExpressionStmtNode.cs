using CCompiler.CSyntaxTree.Expressions;

namespace CCompiler.CSyntaxTree.Statements;

public class ExpressionStmtNode : StatementNode
{
    private ExpressionNode _expression;

    public ExpressionStmtNode(TokenList tokens)
    {
        _expression = ExpressionNode.ParseExpressionNode(tokens, 0);
    }

    public override void SemanticPass(Dictionary<string, string> variableMap)
    {
        _expression.VariableResolution(variableMap);
    }

    public override void ConvertToTac(List<StatementNode> statementList)
    {
        _expression.ConvertToTac(statementList);
    }
}