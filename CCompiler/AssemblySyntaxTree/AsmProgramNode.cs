using CCompiler.CSyntaxTree;

namespace CCompiler.AssemblySyntaxTree;

public class AsmProgramNode : IAsmNode
{
    private readonly List<AsmFunctionNode> _asmFunctions;

    public AsmProgramNode(ProgramNode programNode)
    {
        _asmFunctions = new List<AsmFunctionNode>();
        foreach (var function in programNode.Functions)
        {
            _asmFunctions.Add(new AsmFunctionNode(function));
        }
    }

    public void FinalPass()
    {
        foreach (var function in _asmFunctions)
        {
            function.FinalPass();
        }
    }
    
    public string ConvertToAsm()
    {
        var outputAsm = "";
        foreach (var function in _asmFunctions)
        {
            outputAsm += function.ConvertToAsm();
        }

        return outputAsm;
    }
}