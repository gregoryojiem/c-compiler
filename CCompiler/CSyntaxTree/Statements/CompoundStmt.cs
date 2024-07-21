namespace CCompiler.CSyntaxTree.Statements;

public class CompoundStmt : StatementNode
{
    private readonly BlockNode _body;

    public CompoundStmt(TokenList tokens)
    {
        _body = new BlockNode(tokens);
    }

    public override void SemanticPass(SymbolTable symbolTable)
    {
        symbolTable.NewScope();
        _body.Validate(symbolTable);
        symbolTable.ExitScope();
    }

    public override void ConvertToTac(List<BlockItem> blockItems)
    {
        _body.ConvertToTac(blockItems);
    }

    public override string ToString()
    {
        return _body.ToString();
    }
}