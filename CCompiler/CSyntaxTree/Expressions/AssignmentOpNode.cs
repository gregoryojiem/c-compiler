using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;

namespace CCompiler.CSyntaxTree.Expressions;

public class AssignmentOpNode : ExpressionNode
{
    private readonly ExpressionNode _leftExpr;
    private readonly ExpressionNode _rightExpr;

    public AssignmentOpNode(ExpressionNode leftExpr, ExpressionNode rightExpr)
    {
        _leftExpr = leftExpr;
        _rightExpr = rightExpr;
    }

    public override TacExpressionNode ConvertToTac(List<StatementNode> statementList)
    {
        throw new NotImplementedException();
    }
}