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

    public override void VariableResolution(SymbolTable symbolTable)
    {
        SemanticException.CheckValidAssignment(_leftExpression);
        _leftExpression.VariableResolution(symbolTable);
        _rightExpression.VariableResolution(symbolTable);
    }

    public override Token GetRepresentativeToken()
    {
        return _leftExpression.GetRepresentativeToken();
    }

    public override TacExpressionNode ConvertToTac(List<TacStatementNode> tacStatements)
    {
        var assignedExpr = _rightExpression.ConvertToTac(tacStatements);
        var assigneeVariable = (TacVariableNode)_leftExpression.ConvertToTac(tacStatements);
        tacStatements.Add(new AssignmentNode(assigneeVariable, assignedExpr));
        return assigneeVariable;
    }

    public override string ToString()
    {
        return _leftExpression + " = " + _rightExpression;
    }
}