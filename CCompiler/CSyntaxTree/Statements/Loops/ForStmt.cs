using CCompiler.CSyntaxTree.Expressions;

namespace CCompiler.CSyntaxTree.Statements.Loops;

public class ForStmt : StatementNode
{
    private readonly IForInitialClause _initialClause;
    private readonly ExpressionNode? _condition;
    private readonly ExpressionNode? _post;
    private readonly StatementNode _body;

    public ForStmt(TokenList tokens)
    {
        tokens.PopExpected(TokenType.For);
        tokens.PopExpected(TokenType.LeftParen);
        _initialClause = IForInitialClause.Parse(tokens);
        if (tokens.Peek().Type != TokenType.Semicolon)
        {
            _condition = ExpressionNode.ParseExpression(tokens);
        }

        tokens.PopExpected(TokenType.Semicolon);
        if (tokens.Peek().Type != TokenType.RightParen)
        {
            _post = ExpressionNode.ParseExpression(tokens);
        }

        tokens.PopExpected(TokenType.RightParen);
        _body = ParseStatementNode(tokens);
    }

    public override void SemanticPass(SymbolTable symbolTable)
    {
        throw new NotImplementedException();
    }

    public override void ConvertToTac(List<StatementNode> statementList)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {

        var indent = BlockNode.GetIndent(_body);
        BlockNode.IncreaseIndent(_body);
        var output = "for (" + _initialClause + " " + _condition + "; " + _post + ")" + indent + _body;
        BlockNode.DecreaseIndent(_body);
        return output;
    }
}