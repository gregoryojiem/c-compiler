using CCompiler.CSyntaxTree.Statements;

namespace CCompiler.CSyntaxTree;

public class FunctionNode
{
    private readonly Token _returnType;
    public readonly Token Name;
    public readonly BlockNode Body;

    public FunctionNode(TokenList tokens)
    {
        _returnType = tokens.Pop();
        ParseException.ValidReturnType(_returnType);

        Name = tokens.Pop();
        ParseException.ValidFunctionName(Name);

        //TODO argument handling
        tokens.PopExpected(TokenType.LeftParen);
        while (tokens.Pop().Type != TokenType.RightParen)
        {
        }

        Body = new BlockNode(tokens);
    }

    public void Validate()
    {
        Body.Validate(new SymbolTable());
    }

    public void ConvertToTac()
    {
        Body.ConvertToTac();
        Body.AddBlockItem(new ReturnStmt(0)); // guarantees all functions have an epilogue
    }

    public override string ToString() //TODO argument handling
    {
        return _returnType + " " + Name + "()\n" + Body;
    }
}