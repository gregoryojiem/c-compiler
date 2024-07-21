using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree.Statements.Loops;

public class BreakStmt : LoopStmt
{
    public BreakStmt(TokenList tokens)
    {
        tokens.PopExpected(TokenType.Break);
        tokens.PopExpected(TokenType.Semicolon);
    }

    public override void SemanticPass(SymbolTable symbolTable)
    {
        SetLabel(symbolTable);
    }

    public override void ConvertToTac(List<BlockItem> blockItems)
    {
        blockItems.Add(new JumpNode("break_" + GetLabel()));
    }

    public override string ToString()
    {
        return "break;";
    }
}