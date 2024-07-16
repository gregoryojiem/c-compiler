using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.TacExpressions;

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

    public override void ConvertToTac(List<StatementNode> statementList)
    {
        ReturnValue = ExpressionNode.ConvertToTac(statementList, ReturnValue);
        statementList.Add(this);
    }
}