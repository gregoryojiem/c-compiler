namespace CCompiler.CSyntaxTree.Statements;

public abstract class StatementNode
{
    public static StatementNode CreateStatementNode(TokenList tokens)
    {
        if (tokens.Peek().Type == TokenType.Return)
        {
            var returnStmtNode = new ReturnStmtNode(tokens);
            return returnStmtNode;
        }

        var unexpectedToken = tokens.Pop();
        throw new ParseException(unexpectedToken, $"Invalid statement: {unexpectedToken.Value}");
    }

    public abstract void ConvertToTac(List<StatementNode> statementList);
}