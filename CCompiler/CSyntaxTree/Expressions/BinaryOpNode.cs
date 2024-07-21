using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree.Expressions;

public class BinaryOpNode : ExpressionNode
{
    public readonly TokenType BinaryOperator;
    public readonly ExpressionNode LeftExpression;
    public readonly ExpressionNode RightExpression;

    public BinaryOpNode(TokenType binaryOperator, ExpressionNode leftExpression, ExpressionNode rightExpression)
    {
        BinaryOperator = binaryOperator;
        LeftExpression = leftExpression;
        RightExpression = rightExpression;
    }

    public override void VariableResolution(SymbolTable symbolTable)
    {
        LeftExpression.VariableResolution(symbolTable);
        RightExpression.VariableResolution(symbolTable);
    }

    public override Token GetRepresentativeToken()
    {
        return LeftExpression.GetRepresentativeToken();
    }

    public override TacExpressionNode ConvertToTac(List<BlockItem> blockItems)
    {
        var leftExprValue = (ValueNode)LeftExpression.ConvertToTac(blockItems);
        var shortCircuit = BinaryOperator is TokenType.And or TokenType.Or;
        var jumpCondInverted = BinaryOperator is TokenType.Or;
        var shortCircuitType = BinaryOperator is TokenType.And ? "and_false_" : "or_true_";
        var shortCircuitId = shortCircuitType + SymbolTable.LabelId;

        if (shortCircuit)
        {
            blockItems.Add(new JumpIfZeroNode(leftExprValue, shortCircuitId, jumpCondInverted));
        }

        var rightExprValue = (ValueNode)RightExpression.ConvertToTac(blockItems);
        var resultNode = new TacVariableNode("tmp_" + SymbolTable.VariableId++);
        if (shortCircuit)
        {
            var endIdentifier = "cond_result_" + SymbolTable.LabelId;
            var shortCircuitConst = BinaryOperator is TokenType.And ? 0 : 1;
            blockItems.Add(new JumpIfZeroNode(rightExprValue, shortCircuitId, jumpCondInverted));
            blockItems.Add(new AssignmentNode(resultNode, new TacConstantNode(shortCircuitConst ^ 1)));
            blockItems.Add(new JumpNode(endIdentifier));
            blockItems.Add(new LabelNode(shortCircuitId));
            blockItems.Add(new AssignmentNode(resultNode, new TacConstantNode(shortCircuitConst)));
            blockItems.Add(new LabelNode(endIdentifier));
            SymbolTable.LabelId++;
            return resultNode;
        }

        var tacNode = new TacBinaryOpNode(BinaryOperator, leftExprValue, rightExprValue);
        blockItems.Add(new AssignmentNode(resultNode, tacNode));
        return resultNode;
    }

    public override string ToString()
    {
        return "(" + LeftExpression + " " + Token.GetTypeString(BinaryOperator) + " " + RightExpression + ")";
    }
}