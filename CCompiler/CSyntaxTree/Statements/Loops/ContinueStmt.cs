using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree.Statements.Loops;

public class ContinueStmt : LoopStmt
{
    private readonly Token _continueToken;

    public ContinueStmt(TokenList tokens)
    {
        _continueToken = tokens.PopExpected(TokenType.Continue);
        tokens.PopExpected(TokenType.Semicolon);
    }

    public override void SemanticPass(SymbolTable symbolTable)
    {
        SetLabel(symbolTable, _continueToken);
    }

    public override void ConvertToTac(List<TacStatementNode> tacStatements)
    {
        tacStatements.Add(new JumpNode("continue_" + GetLabel()));
    }

    public override string ToString()
    {
        return "continue;";
    }
}