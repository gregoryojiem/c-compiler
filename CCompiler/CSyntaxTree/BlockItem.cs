using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree;

public abstract class BlockItem
{
    public abstract void SemanticPass(SymbolTable symbolTable);

    public abstract void ConvertToTac(List<TacStatementNode> tacStatements);

    public static BlockItem ParseBlockItem(TokenList tokens)
    {
        if (tokens.Peek().Type == TokenType.IntType)
        {
            return new DeclarationNode(tokens);
        }

        return StatementNode.ParseStatementNode(tokens);
    }
}