namespace CCompiler.CSyntaxTree;

public abstract class StatementNode
{
    public static StatementNode CreateStatementNode(TokenList tokens)
    {
        if (tokens.PeekExpected(TokenType.Return))
        {
            var returnStmtNode = new ReturnStmtNode(tokens);
            return returnStmtNode;
        }

        var unexpectedToken = tokens.Pop();
        throw new ParseException(unexpectedToken, $"Invalid statement: {unexpectedToken.Value}");
    }
}