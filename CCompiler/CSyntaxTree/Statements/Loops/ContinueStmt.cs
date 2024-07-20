namespace CCompiler.CSyntaxTree.Statements.Loops;

public class ContinueStmt : LoopStmt
{
    public ContinueStmt(TokenList tokens)
    {
        tokens.PopExpected(TokenType.Continue);
        tokens.PopExpected(TokenType.Semicolon);
    }

    public override void SemanticPass(SymbolTable symbolTable)
    {
        SetLabel(symbolTable);
    }

    public override void ConvertToTac(List<StatementNode> statementList)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return "continue;";
    }
}