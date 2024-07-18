using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.Expressions;

public class BinaryOpNode : ExpressionNode
{
    public readonly Token BinaryOperator;
    public readonly ExpressionNode LeftExpression;
    public readonly ExpressionNode RightExpression;

    public BinaryOpNode(Token binaryOperator, ExpressionNode leftExpression, ExpressionNode rightExpression)
    {
        BinaryOperator = binaryOperator;
        LeftExpression = leftExpression;
        RightExpression = rightExpression;
    }

    public override TacExpressionNode ConvertToTac(List<StatementNode> statementList)
    {
        var leftExprValue = (BaseValueNode)LeftExpression.ConvertToTac(statementList);
        var rightExprValue = (BaseValueNode)RightExpression.ConvertToTac(statementList);
        var tacNode = new TacBinaryOpNode(BinaryOperator, leftExprValue, rightExprValue);
        var tempVar = new VariableNode("tmp" + TempVariableCounter++);
        statementList.Add(new DeclarationNode(tempVar, tacNode));
        return tempVar;
    }

    public override string ToString()
    {
        return "(" + LeftExpression + " " + BinaryOperator.Value + " " + RightExpression + ")";
    }
}