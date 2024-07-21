using CCompiler.CSyntaxTree.Statements;
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

    public override TacExpressionNode ConvertToTac(List<BlockItem> blockItems)
    {
        var finalResultVar = new TacVariableNode("tmp_" + SymbolTable.VariableId++);

        //evaluate condition
        var tacCondition = (BaseValueNode)_condition.ConvertToTac(blockItems);
        var jumpToFalse = "tern_false" + SymbolTable.LabelId++;
        blockItems.Add(new JumpIfZeroNode(tacCondition, jumpToFalse, false));

        //handle true case
        var trueResult = _trueResult.ConvertToTac(blockItems);
        var jumpToEnd = "tern_end" + SymbolTable.LabelId++;
        blockItems.Add(new AssignmentNode(finalResultVar, trueResult));
        blockItems.Add(new JumpNode(jumpToEnd));

        //handle false case
        blockItems.Add(new LabelNode(jumpToFalse));
        var falseResult = _falseResult.ConvertToTac(blockItems);
        blockItems.Add(new AssignmentNode(finalResultVar, falseResult));
        blockItems.Add(new LabelNode(jumpToEnd));
        return finalResultVar;
    }

    public override string ToString()
    {
        return _condition + " ? " + _trueResult + " : " + _falseResult;
    }
}