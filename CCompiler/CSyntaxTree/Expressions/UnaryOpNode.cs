using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

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
    
    public override TacExpressionNode ConvertToTac(List<StatementNode> statementList)
    {
        var exprValue = (BaseValueNode)Expression.ConvertToTac(statementList);
        var tacNode = new TacUnaryOpNode(UnaryOperator, exprValue);
        var tempVar = new VariableNode("tmp" + TempVariableCounter++);
        statementList.Add(new DeclarationNode(tempVar, tacNode));
        return tempVar;
    }
    
    public override string ToString()
    {
        return UnaryOperator.Value + "(" + Expression + ")";
    }
}