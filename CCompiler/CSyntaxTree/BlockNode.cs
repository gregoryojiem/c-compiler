using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree;

public class BlockNode
{
    public static int IndentationLevel = 1;
    private readonly List<BlockItem> _blockItems;
    public List<TacStatementNode> TacBlockItems;
    
    public BlockNode(TokenList tokens)
    {
        _blockItems = new List<BlockItem>();

        tokens.PopExpected(TokenType.LeftBrace);
        while (!tokens.PopIfFound(TokenType.RightBrace))
        {
            _blockItems.Add(BlockItem.ParseBlockItem(tokens));
        }
    }

    public void AddBlockItem(BlockItem blockItem)
    {
        _blockItems.Add(blockItem);
    }

    public void Validate(SymbolTable symbolTable)
    {
        foreach (var blockItem in _blockItems)
        {
            blockItem.SemanticPass(symbolTable);
        }
    }

    public void ConvertToTac()
    {
        var tacBlockItems = new List<TacStatementNode>();
        foreach (var blockItem in _blockItems)
        {
            blockItem.ConvertToTac(tacBlockItems);
        }

        TacBlockItems = tacBlockItems;
    }

    public void ConvertToTac(List<TacStatementNode> tacBlockItems)
    {
        foreach (var blockItem in _blockItems)
        {
            blockItem.ConvertToTac(tacBlockItems);
        }
    }

    public static string GetIndent(BlockItem? blockItem, bool addNewline = true, int modifier = 0)
    {
        var indent = new string('\t', IndentationLevel + modifier);
        if (addNewline)
            indent = "\n" + indent;
        if (blockItem is CompoundStmt)
            indent = " ";
        return indent;
    }

    public static void IncreaseIndent(BlockItem blockItem)
    {
        if (blockItem is not CompoundStmt)
        {
            IndentationLevel++;
        }
    }

    public static void DecreaseIndent(BlockItem blockItem)
    {
        if (blockItem is not CompoundStmt)
        {
            IndentationLevel--;
        }
    }

    public override string ToString()
    {
        var indent = new string('\t', IndentationLevel);
        IndentationLevel++;
        var outputBlock = "";
        var declarations = new List<string>();
        foreach (var blockItem in _blockItems)
        {
            var declaration = "";
            if (blockItem is AssignmentNode assignment && !declarations.Contains(assignment.TacVariable.Identifier))
            {
                declarations.Add(assignment.TacVariable.Identifier);
                declaration = "int ";
            }

            var indentToUse = blockItem is LabelNode ? indent[..^1] : indent;
            outputBlock += indentToUse + declaration + blockItem + "\n";
        }

        IndentationLevel--;
        return "{\n" + outputBlock + new string('\t', IndentationLevel - 1) + "}";
    }
}