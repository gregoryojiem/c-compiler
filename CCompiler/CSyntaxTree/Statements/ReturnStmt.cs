using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.Statements;

public class ReturnStmt : StatementNode
{
    public ExpressionNode ReturnValue;

    public ReturnStmt(TokenList tokens)
    {
        tokens.PopExpected(TokenType.Return);
        ReturnValue = ExpressionNode.ParseExpression(tokens);
        tokens.PopExpected(TokenType.Semicolon);
    }

    public ReturnStmt(int returnValue)
    {
        ReturnValue = new TacConstantNode(returnValue);
    }

    public override void SemanticPass(SymbolTable symbolTable)
    {
        ReturnValue.VariableResolution(symbolTable);
    }

    public override void ConvertToTac(List<BlockItem> blockItems)
    {
        ReturnValue = ReturnValue.ConvertToTac(blockItems);
        blockItems.Add(this);
    }

    public override string ToString()
    {
        return "return " + ReturnValue + ";";
    }
}