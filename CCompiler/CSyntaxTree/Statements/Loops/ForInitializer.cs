namespace CCompiler.CSyntaxTree.Statements.Loops;

public abstract class ForInitialClause : StatementNode
{
    public static ForInitialClause Parse(TokenList tokens)
    {
        if (tokens.NextTokenGroup(TokenList.Group.DataType))
        {
            return new DeclarationStmt(tokens);
        }

        if (TokenList.IsExpressionStart(tokens.Peek().Type))
        {
            return new ExpressionStmt(tokens);
        }

        if (tokens.Peek().Type == TokenType.Semicolon)
        {
            return new NullStmt(tokens);
        }

        throw new ParseException(tokens.Peek(), $"Invalid for loop initializer {tokens.Peek()}," +
                                                " expected expression, declaration, or semicolon");
    }
}