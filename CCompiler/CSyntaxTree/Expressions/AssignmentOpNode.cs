using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;

namespace CCompiler.CSyntaxTree.Expressions;

public class AssignmentOpNode : ExpressionNode
{
    private readonly ExpressionNode LeftExpression;
    private readonly ExpressionNode RightExpression;

    public AssignmentOpNode(ExpressionNode leftExpression, ExpressionNode rightExpression)
    {
        LeftExpression = leftExpression;
        RightExpression = rightExpression;
    }

    public override void VariableResolution(Dictionary<string, string> variableMap)
    {
        SemanticException.CheckValidAssignment(LeftExpression);
        LeftExpression.VariableResolution(variableMap);
        RightExpression.VariableResolution(variableMap);
    }

    public override Token GetRepresentativeToken()
    {
        return LeftExpression.GetRepresentativeToken();
    }

    public override TacExpressionNode ConvertToTac(List<StatementNode> statementList)
    {
        throw new NotImplementedException();
    }
}