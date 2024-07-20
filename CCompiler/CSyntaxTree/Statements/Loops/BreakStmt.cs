namespace CCompiler.CSyntaxTree.Statements.Loops;

public class BreakStmt : StatementNode
{
    public BreakStmt(TokenList tokens)
    {
        tokens.PopExpected(TokenType.Break);
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
        return "break;";
    }
}