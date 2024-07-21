using CCompiler.AssemblySyntaxTree.Instructions;
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
        AsmParseBlock(functionNode.Body);
    }

    private void AsmParseBlock(BlockNode blockNode)
    {
        foreach (var blockItem in blockNode.TacBlockItems)
        {
            AsmInstructionNode.ConvertCToAsmInstructions(_instructions, blockItem);
        }
    }
    
    public void FinalPass()
    {
        DoAllocationPass();
        DoCorrectionPass();
    }

    private void DoAllocationPass()
    {
        var variableMap = new Dictionary<string, int>();
        var stackPos = 0;
        foreach (var instruction in _instructions)
        {
            if (instruction is IAllocatableInstruction allocatableInstruction)
            {
                allocatableInstruction.DoAllocationPass(variableMap, ref stackPos);
            }
        }

        _instructions.Insert(0, new AllocateStackNode(-stackPos));
    }

    private void DoCorrectionPass()
    {
        var fixedInstructions = new List<AsmInstructionNode>();
        foreach (var instruction in _instructions)
        {
            if (instruction is MovlNode movlNode)
            {
                movlNode.FixOperands(fixedInstructions);
            }
            else if (instruction is CmpNode cmpNode)
            {
                cmpNode.FixOperands(fixedInstructions);
            }
            else if (instruction is BinaryNode binaryNode)
            {
                binaryNode.FixOperands(fixedInstructions);
            }

            else if (instruction is DivlNode divlNode)
            {
                divlNode.FixConstantOps(fixedInstructions);
            }
            else
            {
                fixedInstructions.Add(instruction);
            }
        }

        _instructions = fixedInstructions;
    }

    public override string ToString()
    {
        var outputAsm = "";
        outputAsm += ".globl " + _name + "\n";
        outputAsm += _name + ":\n";
        outputAsm += "\t" + "pushq\t%rbp" + "\n";
        outputAsm += "\t" + "movq\t%rsp, %rbp" + "\n";
        foreach (var instruction in _instructions)
        {
            var addTab = instruction is JmpLabelNode ? "" : "\t";
            outputAsm += addTab + instruction.ToString() + "\n";
        }

        return outputAsm;
    }
}