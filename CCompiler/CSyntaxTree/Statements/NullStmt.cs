using CCompiler.CSyntaxTree.Statements.Loops;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree.Statements;

public class NullStmt : StatementNode
{
    public NullStmt(TokenList tokens)
    {
        tokens.PopExpected(TokenType.Semicolon);
    }

    public override void SemanticPass(SymbolTable symbolTable)
    {
    }

    public override void ConvertToTac(List<TacStatementNode> tacStatements)
    {
    }

    public override string ToString()
    {
        return ";";
    }
}