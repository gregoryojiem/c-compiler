using CCompiler.AssemblySyntaxTree.Instructions;
using CCompiler.CSyntaxTree;

namespace CCompiler.AssemblySyntaxTree;

public class AsmFunctionNode : IAsmNode
{
    private readonly string _name;
    private readonly List<AsmInstructionNode> _instructions;

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

    public void DoAllocationPass(Dictionary<string, int> variableMap)
    {
        var currentStackLocation = 0;
        foreach (var instruction in _instructions)
        {
            if (instruction is IAllocatableInstruction allocatableInstruction)
            {
                allocatableInstruction.DoAllocationPass(variableMap, ref currentStackLocation);
            }
        }
    }
}