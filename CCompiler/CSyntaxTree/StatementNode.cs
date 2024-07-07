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

        throw new Exception("Statement currently unhandled.");
    }
}