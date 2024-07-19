using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree.Expressions;

public class AssignmentOpNode : ExpressionNode
{
    private readonly ExpressionNode _leftExpression;
    private ExpressionNode _rightExpression;

    public AssignmentOpNode(ExpressionNode leftExpression, ExpressionNode rightExpression)
    {
        _leftExpression = leftExpression;
        _rightExpression = rightExpression;
    }

    public override void VariableResolution(Dictionary<string, string> variableMap)
    {
        SemanticException.CheckValidAssignment(_leftExpression);
        _leftExpression.VariableResolution(variableMap);
        _rightExpression.VariableResolution(variableMap);
    }

    public override Token GetRepresentativeToken()
    {
        return _leftExpression.GetRepresentativeToken();
    }

    public override TacExpressionNode ConvertToTac(List<StatementNode> statementList)
    {
        var assignedExpr = _rightExpression.ConvertToTac(statementList);
        var assigneeVariable = (TacVariableNode)_leftExpression.ConvertToTac(statementList);
        statementList.Add(new AssignmentNode(assigneeVariable, assignedExpr));
        return assigneeVariable;
    }

    public override string ToString()
    {
        return _leftExpression + " = " + _rightExpression;
    }
}