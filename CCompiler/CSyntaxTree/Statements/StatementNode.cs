namespace CCompiler.CSyntaxTree.Statements;

public abstract class StatementNode
{
    public static StatementNode CreateStatementNode(TokenList tokens)
    {
        var nextToken = tokens.Peek().Type;
        if (nextToken == TokenType.Semicolon)
        {
            return new NullStmtNode(tokens);
        }

        if (nextToken == TokenType.IntType)
        {
            return new DeclarationStmtNode(tokens);
        }

        if (TokenList.IsExpressionStart(nextToken))
        {
            return new ExpressionStmtNode(tokens);
        }

        if (nextToken == TokenType.Return)
        {
            return new ReturnStmtNode(tokens);
        }

        var unexpectedToken = tokens.Pop();
        throw new ParseException(unexpectedToken, $"Invalid statement: {unexpectedToken.Value}");
    }

    public abstract void SemanticPass(Dictionary<string, string> variableMap);

    public abstract void ConvertToTac(List<StatementNode> statementList);
}