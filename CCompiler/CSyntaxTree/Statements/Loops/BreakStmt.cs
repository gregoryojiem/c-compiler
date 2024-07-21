using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree.Statements.Loops;

public class BreakStmt : LoopStmt
{
    private readonly Token _breakToken;

    public BreakStmt(TokenList tokens)
    {
        _breakToken = tokens.PopExpected(TokenType.Break);
        tokens.PopExpected(TokenType.Semicolon);
    }

    public override void SemanticPass(SymbolTable symbolTable)
    {
        SetLabel(symbolTable, _breakToken);
    }

    public override void ConvertToTac(List<TacStatementNode> tacStatements)
    {
        tacStatements.Add(new JumpNode("break_" + GetLabel()));
    }

    public override string ToString()
    {
        return "break;";
    }
}