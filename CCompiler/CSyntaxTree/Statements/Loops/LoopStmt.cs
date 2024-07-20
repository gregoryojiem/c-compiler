namespace CCompiler.CSyntaxTree.Statements.Loops;

public abstract class LoopStmt : StatementNode
{
    private string? _label;

    protected void SetLabel(SymbolTable symbolTable)
    {
        _label = symbolTable.GetCurrentLoopId();
    }

    public string GetLabel()
    {
        if (_label == null)
        {
            throw new Exception("GetLabel called before label has been generated");
        }
        return _label;
    }
}