using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree.Expressions;

public class UnaryOpNode : ExpressionNode
{
    private readonly Token UnaryOperator;
    private readonly ExpressionNode Expression;

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

    public override TacExpressionNode ConvertToTac(List<TacStatementNode> tacStatements)
    {
        var exprValue = (ValueNode)Expression.ConvertToTac(tacStatements);
        var tacNode = new TacUnaryOpNode(UnaryOperator.Type, exprValue);
        var tempVar = new TacVariableNode("tmp_" + SymbolTable.VariableId++);
        tacStatements.Add(new AssignmentNode(tempVar, tacNode));
        return tempVar;
    }

    public override string ToString()
    {
        return UnaryOperator.Value + "(" + Expression + ")";
    }
}