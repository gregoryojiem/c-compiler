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
        var variableMap = new Dictionary<string, string>();
        foreach (var function in Functions)
        {
            function.Validate(variableMap);
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