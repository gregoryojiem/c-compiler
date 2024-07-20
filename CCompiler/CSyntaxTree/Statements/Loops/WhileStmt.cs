using CCompiler.CSyntaxTree.Expressions;

namespace CCompiler.CSyntaxTree.Statements.Loops;

public class WhileStmt : StatementNode
{
    private readonly ExpressionNode _condition;
    private readonly StatementNode _body;

    public WhileStmt(TokenList tokens)
    {
        tokens.PopExpected(TokenType.While);
        tokens.PopExpected(TokenType.LeftParen);
        _condition = ExpressionNode.ParseExpression(tokens);
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
        var output = "while (" + _condition + ")" + indent + _body;
        BlockNode.DecreaseIndent(_body);
        return output;
    }
}