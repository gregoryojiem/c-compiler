using CCompiler.AssemblySyntaxTree.Operands;

namespace CCompiler.AssemblySyntaxTree.Instructions;

public interface IAllocatableInstruction
{
    public void DoAllocationPass(Dictionary<string, int> variableMap, ref int stackPos);

    public static IOperand HandleAllocation(Dictionary<string, int> variableMap, ref int stackPos, PseudoRegOp operand)
    {
        if (variableMap.TryGetValue(operand.Identifier, out var value))
        {
            return new StackOp(value);
        }

        stackPos -= 4;
        variableMap[operand.Identifier] = stackPos;
        return new StackOp(stackPos);
    }
}