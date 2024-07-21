using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree.Expressions;

public class UnaryOpNode : ExpressionNode
{
    public readonly Token UnaryOperator;
    public readonly ExpressionNode Expression;

    public UnaryOpNode(TokenList tokens)
    {
        UnaryOperator = tokens.Pop();
        Expression = ParseExpressionFactor(tokens);
    }

    public override void VariableResolution(SymbolTable symbolTable)
    {
        Expression.VariableResolution(symbolTable);
    }

    public override Token GetRepresentativeToken()
    {
        return UnaryOperator;
    }

    public override TacExpressionNode ConvertToTac(List<BlockItem> blockItems)
    {
        var exprValue = (BaseValueNode)Expression.ConvertToTac(blockItems);
        var tacNode = new TacUnaryOpNode(UnaryOperator.Type, exprValue);
        var tempVar = new TacVariableNode("tmp_" + SymbolTable.VariableId++);
        blockItems.Add(new AssignmentNode(tempVar, tacNode));
        return tempVar;
    }

    public override string ToString()
    {
        return UnaryOperator.Value + "(" + Expression + ")";
    }
}