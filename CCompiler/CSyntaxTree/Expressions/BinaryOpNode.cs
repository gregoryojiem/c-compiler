using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;
using CCompiler.CSyntaxTree.TacStatements;

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
        var shortCircuit = BinaryOperator.Type is TokenType.And or TokenType.Or;
        var jumpCondInverted = BinaryOperator.Type is TokenType.Or;
        var shortCircuitType = BinaryOperator.Type is TokenType.And ? "and_false_" : "or_true_";
        var shortCircuitId = shortCircuitType + TempLabelCounter;

        if (shortCircuit)
        {
            statementList.Add(new JumpIfZeroNode(leftExprValue, shortCircuitId, jumpCondInverted));
        }

        var rightExprValue = (BaseValueNode)RightExpression.ConvertToTac(statementList);
        var resultNode = new VariableNode("tmp_" + TempVariableCounter++);
        if (shortCircuit)
        {
            var endIdentifier = "cond_result_" + TempLabelCounter;
            var shortCircuitConst = BinaryOperator.Type is TokenType.And ? 0 : 1;
            statementList.Add(new JumpIfZeroNode(rightExprValue, shortCircuitId, jumpCondInverted));
            statementList.Add(new DeclarationNode(resultNode, new ConstantNode(shortCircuitConst ^ 1)));
            statementList.Add(new JumpNode(endIdentifier));
            statementList.Add(new LabelNode(shortCircuitId));
            statementList.Add(new DeclarationNode(resultNode, new ConstantNode(shortCircuitConst)));
            statementList.Add(new LabelNode(endIdentifier));
            TempLabelCounter++;
            return resultNode;
        }

        var tacNode = new TacBinaryOpNode(BinaryOperator.Type, leftExprValue, rightExprValue);
        statementList.Add(new DeclarationNode(resultNode, tacNode));
        return resultNode;
    }

    public override string ToString()
    {
        return "(" + LeftExpression + " " + BinaryOperator.Value + " " + RightExpression + ")";
    }
}