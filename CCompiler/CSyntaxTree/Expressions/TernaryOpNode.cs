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

    public override TacExpressionNode ConvertToTac(List<TacStatementNode> tacStatements)
    {
        var finalResultVar = new TacVariableNode("tmp_" + SymbolTable.VariableId++);

        //evaluate condition
        var tacCondition = (ValueNode)_condition.ConvertToTac(tacStatements);
        var jumpToFalse = "tern_false" + SymbolTable.LabelId++;
        tacStatements.Add(new JumpIfZeroNode(tacCondition, jumpToFalse, false));

        //handle true case
        var trueResult = _trueResult.ConvertToTac(tacStatements);
        var jumpToEnd = "tern_end" + SymbolTable.LabelId++;
        tacStatements.Add(new AssignmentNode(finalResultVar, trueResult));
        tacStatements.Add(new JumpNode(jumpToEnd));

        //handle false case
        tacStatements.Add(new LabelNode(jumpToFalse));
        var falseResult = _falseResult.ConvertToTac(tacStatements);
        tacStatements.Add(new AssignmentNode(finalResultVar, falseResult));
        tacStatements.Add(new LabelNode(jumpToEnd));
        return finalResultVar;
    }

    public override string ToString()
    {
        return _condition + " ? " + _trueResult + " : " + _falseResult;
    }
}