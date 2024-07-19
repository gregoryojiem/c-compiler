using CCompiler.CSyntaxTree.Expressions;

namespace CCompiler.CSyntaxTree.Statements;

public class DeclarationStmtNode : StatementNode
{
    private TokenType _type;
    private Token _identifier;
    private ExpressionNode? _expressionNode;

    public DeclarationStmtNode(TokenList tokens)
    {
        _type = tokens.PopExpected(TokenType.IntType).Type;
        _identifier = tokens.PopExpected(TokenType.Identifier);
        if (tokens.Peek().Type == TokenType.Assignment)
        {
            tokens.Pop();
            _expressionNode = ExpressionNode.ParseExpressionNode(tokens, 0);
        }
        tokens.PopExpected(TokenType.Semicolon);
    }

    public override void ConvertToTac(List<StatementNode> statementList)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return Token.GetTypeString(_type) + " " + _identifier + " = " + _expressionNode;
    }
}