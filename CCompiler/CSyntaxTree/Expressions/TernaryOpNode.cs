﻿using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree.Expressions;

public class TernaryOpNode : ExpressionNode
{
    private readonly ExpressionNode _condition;
    private readonly ExpressionNode _trueResult;
    private readonly ExpressionNode _falseResult;

    public TernaryOpNode(ExpressionNode condition, ExpressionNode trueResult, ExpressionNode falseResult)
    {
        _condition = condition;
        _trueResult = trueResult;
        _falseResult = falseResult;
    }

    public override void VariableResolution(SymbolTable symbolTable)
    {
        _condition.VariableResolution(symbolTable);
        _trueResult.VariableResolution(symbolTable);
        _falseResult.VariableResolution(symbolTable);
    }

    public override Token GetRepresentativeToken()
    {
        return _condition.GetRepresentativeToken();
    }

    public override TacExpressionNode ConvertToTac(List<StatementNode> statementList)
    {
        var finalResultVar = new TacVariableNode("tmp_" + UniqueVariableCounter++);

        //evaluate condition
        var tacCondition = (BaseValueNode)_condition.ConvertToTac(statementList);
        var jumpToFalse = "tern_false" + UniqueLabelCounter++;
        statementList.Add(new JumpIfZeroNode(tacCondition, jumpToFalse, false));

        //handle true case
        var trueResult = _trueResult.ConvertToTac(statementList);
        var jumpToEnd = "tern_end" + UniqueLabelCounter++;
        statementList.Add(new AssignmentNode(finalResultVar, trueResult));
        statementList.Add(new JumpNode(jumpToEnd));

        //handle false case
        statementList.Add(new LabelNode(jumpToFalse));
        var falseResult = _falseResult.ConvertToTac(statementList);
        statementList.Add(new AssignmentNode(finalResultVar, falseResult));
        statementList.Add(new LabelNode(jumpToEnd));
        return finalResultVar;
    }

    public override string ToString()
    {
        return _condition + " ? " + _trueResult + " : " + _falseResult;
    }
}