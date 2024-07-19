using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.Statements;

public class ReturnStmtNode : StatementNode
{
    public ExpressionNode ReturnValue;

    public ReturnStmtNode(TokenList tokens)
    {
        tokens.PopExpected(TokenType.Return);
        ReturnValue = ExpressionNode.ParseExpressionNode(tokens, 0);
        tokens.PopExpected(TokenType.Semicolon);
    }

    public ReturnStmtNode(int returnValue)
    {
        ReturnValue = new TacConstantNode(returnValue);
    }

    public override void SemanticPass(Dictionary<string, string> variableMap)
    {
        ReturnValue.VariableResolution(variableMap);
    }

    public override void ConvertToTac(List<StatementNode> statementList)
    {
        ReturnValue = ReturnValue.ConvertToTac(statementList);
        statementList.Add(this);
    }

    public override string ToString()
    {
        return "return " + ReturnValue;
    }
}