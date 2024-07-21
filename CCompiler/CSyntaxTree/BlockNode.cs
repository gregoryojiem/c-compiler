using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree;

public class BlockNode
{
    public static int IndentationLevel = 1;
    public List<BlockItem> BlockItems;

    public BlockNode(TokenList tokens)
    {
        BlockItems = new List<BlockItem>();

        tokens.PopExpected(TokenType.LeftBrace);
        while (!tokens.PopIfFound(TokenType.RightBrace))
        {
            BlockItems.Add(BlockItem.ParseBlockItem(tokens));
        }
    }

    public void AddBlockItem(BlockItem blockItem)
    {
        BlockItems.Add(blockItem);
    }

    public void Validate(SymbolTable symbolTable)
    {
        foreach (var blockItem in BlockItems)
        {
            blockItem.SemanticPass(symbolTable);
        }
    }

    public void ConvertToTac()
    {
        var tacBlockItems = new List<BlockItem>();
        foreach (var blockItem in BlockItems)
        {
            blockItem.ConvertToTac(tacBlockItems);
        }

        BlockItems = tacBlockItems;
    }

    public void ConvertToTac(List<BlockItem> tacBlockItems)
    {
        foreach (var blockItem in BlockItems)
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
        foreach (var blockItem in BlockItems)
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