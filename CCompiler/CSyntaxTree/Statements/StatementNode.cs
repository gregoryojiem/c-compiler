namespace CCompiler.CSyntaxTree.Statements;

public abstract class StatementNode
{
    public static StatementNode CreateStatementNode(TokenList tokens)
    {
        var nextToken = tokens.Peek().Type;
        switch (nextToken)
        {
            case TokenType.IntType:
                return new DeclarationStmtNode(tokens);
            case TokenType.If:
                return new IfStmtNode(tokens);
            case TokenType.LeftBrace:
                return new CompoundStmtNode(tokens);
            case TokenType.Return:
                return new ReturnStmtNode(tokens);
            case TokenType.Semicolon:
                return new NullStmtNode(tokens);
            default:
                if (TokenList.IsExpressionStart(nextToken))
                    return new ExpressionStmtNode(tokens);
                break;
        }

        var unexpectedToken = tokens.Pop();
        throw new ParseException(unexpectedToken, $"Invalid statement: {unexpectedToken.Value}");
    }

    public abstract void SemanticPass(SymbolTable symbolTable);

    public abstract void ConvertToTac(List<StatementNode> statementList);
}