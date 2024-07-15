using CCompiler.AssemblySyntaxTree.Instructions;
using CCompiler.CSyntaxTree;

namespace CCompiler.AssemblySyntaxTree;

public class AsmFunctionNode : IAsmNode
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

    public void DoAllocationPass(Dictionary<string, int> variableMap)
    {
        var stackPos = 0;
        foreach (var instruction in _instructions)
        {
            if (instruction is IAllocatableInstruction allocatableInstruction)
            {
                allocatableInstruction.DoAllocationPass(variableMap, ref stackPos);
            }
        }

        _instructions.Insert(0, new AllocateStackNode(-stackPos));

        var fixedInstructions = new List<AsmInstructionNode>();
        foreach (var instruction in _instructions)
        {
            if (instruction is MovlNode movlNode)
            {
                movlNode.FixDoubleStackOps(fixedInstructions);
            }
            else
            {
                fixedInstructions.Add(instruction);
            }
        }

        _instructions = fixedInstructions;
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