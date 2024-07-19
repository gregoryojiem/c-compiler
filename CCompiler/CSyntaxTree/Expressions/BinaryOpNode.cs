﻿using CCompiler.CSyntaxTree.Statements;
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

    public override void VariableResolution(Dictionary<string, string> variableMap)
    {
        LeftExpression.VariableResolution(variableMap);
        RightExpression.VariableResolution(variableMap);
    }

    public override Token GetRepresentativeToken()
    {
        return LeftExpression.GetRepresentativeToken();
    }

    public override TacExpressionNode ConvertToTac(List<StatementNode> statementList)
    {
        var leftExprValue = (BaseValueNode)LeftExpression.ConvertToTac(statementList);
        var shortCircuit = BinaryOperator.Type is TokenType.And or TokenType.Or;
        var jumpCondInverted = BinaryOperator.Type is TokenType.Or;
        var shortCircuitType = BinaryOperator.Type is TokenType.And ? "and_false_" : "or_true_";
        var shortCircuitId = shortCircuitType + UniqueLabelCounter;

        if (shortCircuit)
        {
            statementList.Add(new JumpIfZeroNode(leftExprValue, shortCircuitId, jumpCondInverted));
        }

        var rightExprValue = (BaseValueNode)RightExpression.ConvertToTac(statementList);
        var resultNode = new TacVariableNode("tmp_" + UniqueVariableCounter++);
        if (shortCircuit)
        {
            var endIdentifier = "cond_result_" + UniqueLabelCounter;
            var shortCircuitConst = BinaryOperator.Type is TokenType.And ? 0 : 1;
            statementList.Add(new JumpIfZeroNode(rightExprValue, shortCircuitId, jumpCondInverted));
            statementList.Add(new AssignmentNode(resultNode, new TacConstantNode(shortCircuitConst ^ 1)));
            statementList.Add(new JumpNode(endIdentifier));
            statementList.Add(new LabelNode(shortCircuitId));
            statementList.Add(new AssignmentNode(resultNode, new TacConstantNode(shortCircuitConst)));
            statementList.Add(new LabelNode(endIdentifier));
            UniqueLabelCounter++;
            return resultNode;
        }

        var tacNode = new TacBinaryOpNode(BinaryOperator.Type, leftExprValue, rightExprValue);
        statementList.Add(new AssignmentNode(resultNode, tacNode));
        return resultNode;
    }

    public override string ToString()
    {
        return "(" + LeftExpression + " " + BinaryOperator.Value + " " + RightExpression + ")";
    }
}