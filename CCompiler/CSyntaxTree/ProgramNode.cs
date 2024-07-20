namespace CCompiler.CSyntaxTree;

public class ProgramNode
{
    public readonly List<FunctionNode> Functions;

    public ProgramNode(TokenList tokens)
    {
        Functions = new List<FunctionNode>();
        while (!tokens.Empty())
        {
            var functionNode = new FunctionNode(tokens);
            Functions.Add(functionNode);
        }
    }

    public void Validate()
    {
        foreach (var function in Functions)
        {
            function.Validate();
        }
    }

    public void ConvertToTac()
    {
        foreach (var function in Functions)
        {
            function.ConvertToTac();
        }
    }
}