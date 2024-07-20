namespace CCompiler.CSyntaxTree.Statements;

public class CompoundStmtNode : StatementNode
{
    public readonly BlockNode Body;

    public CompoundStmtNode(TokenList tokens)
    {
        Body = new BlockNode(tokens);
    }

    public override void SemanticPass(SymbolTable symbolTable)
    {
        symbolTable.NewScope();
        Body.Validate(symbolTable);
        symbolTable.ExitScope();
    }

    public override void ConvertToTac(List<StatementNode> statementList)
    {
        Body.ConvertToTac(statementList);
    }

    public override string ToString()
    {
        return Body.ToString();
    }
}