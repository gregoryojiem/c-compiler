namespace CCompiler.CSyntaxTree.Statements.Loops;

public class ContinueStmt : StatementNode
{
    public ContinueStmt(TokenList tokens)
    {
        tokens.PopExpected(TokenType.Continue);
        tokens.PopExpected(TokenType.Semicolon);
    }
    
    public override void SemanticPass(SymbolTable symbolTable)
    {
        throw new NotImplementedException();
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