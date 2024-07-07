using CCompiler.CSyntaxTree;

namespace CCompiler.AssemblySyntaxTree;

public class AsmFunctionNode : AsmNode
{
    private readonly string _name;
    private List<AsmInstructionNode> _instructions;

    public AsmFunctionNode(FunctionNode functionNode)
    {
        _name = functionNode.Name.Value;
        _instructions = new List<AsmInstructionNode>();
        foreach (var statement in functionNode.Body)
        {
            AsmInstructionNode.ConvertCToAsmInstructions(_instructions, statement);
        }
    }

    public string ConvertToAsm()
    {
        var outputAsm = "";
        outputAsm += ".globl " + _name + "\n";
        outputAsm += _name + ":\n";
        foreach (var instruction in _instructions)
        {
            outputAsm += "\t" + instruction.ConvertToAsm() + "\n";
        }

        return outputAsm;
    }
}