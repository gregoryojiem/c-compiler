namespace CCompiler.CSyntaxTree.Statements;

public class NullStmtNode : StatementNode
{
    public NullStmtNode(TokenList tokens)
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