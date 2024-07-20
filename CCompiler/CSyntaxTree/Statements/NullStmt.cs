using CCompiler.CSyntaxTree.Statements.Loops;

namespace CCompiler.CSyntaxTree.Statements;

public class NullStmt : ForInitialClause
{
    public NullStmt(TokenList tokens)
    {
        tokens.PopExpected(TokenType.Semicolon);
    }

    public override void SemanticPass(SymbolTable symbolTable)
    {
    }

    public override void ConvertToTac(List<StatementNode> statementList)
    {
    }

    public override string ToString()
    {
        return ";";
    }
}