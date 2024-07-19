using CCompiler.CSyntaxTree.Expressions;

namespace CCompiler.CSyntaxTree.Statements;

public class ExpressionStmtNode : StatementNode
{
    private ExpressionNode _expressionNode;

    public ExpressionStmtNode(TokenList tokens)
    {
        _expressionNode = ExpressionNode.ParseExpressionNode(tokens, 0);
    }

    public override void ConvertToTac(List<StatementNode> statementList)
    {
        throw new NotImplementedException();
    }
}