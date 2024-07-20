using CCompiler.CSyntaxTree.Statements.Loops;

namespace CCompiler.CSyntaxTree.Statements;

public abstract class StatementNode
{
    public static StatementNode ParseStatementNode(TokenList tokens)
    {
        var nextToken = tokens.Peek().Type;
        return nextToken switch
        {
            TokenType.IntType => new DeclarationStmt(tokens),
            TokenType.If => new IfStmt(tokens),
            TokenType.While => new WhileStmt(tokens),
            TokenType.Do => new DoWhileStmt(tokens),
            TokenType.For => new ForStmt(tokens),
            TokenType.Break => new BreakStmt(tokens),
            TokenType.Continue => new ContinueStmt(tokens),
            TokenType.LeftBrace => new CompoundStmt(tokens),
            TokenType.Return => new ReturnStmt(tokens),
            TokenType.Semicolon => new NullStmt(tokens),
            _ => HandleUnknownToken(tokens, nextToken)
        };
    }

    private static StatementNode HandleUnknownToken(TokenList tokens, TokenType token)
    {
        if (TokenList.IsExpressionStart(token))
            return new ExpressionStmt(tokens);
        var unexpectedToken = tokens.Pop();
        throw new ParseException(unexpectedToken, $"Invalid statement: {unexpectedToken.Value}");
    }

    public abstract void SemanticPass(SymbolTable symbolTable);

    public abstract void ConvertToTac(List<StatementNode> statementList);
}