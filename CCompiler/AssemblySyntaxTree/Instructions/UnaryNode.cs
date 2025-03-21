﻿using CCompiler.AssemblySyntaxTree.Operands;

namespace CCompiler.AssemblySyntaxTree.Instructions;

public class UnaryNode : AsmInstructionNode, IAllocatableInstruction
{
    private readonly TokenType _unaryOp;
    private IOperand _dst;

    public UnaryNode(TokenType unaryOp, IOperand dst)
    {
        _unaryOp = unaryOp;
        _dst = dst;
    }

    public void DoAllocationPass(Dictionary<string, int> variableMap, ref int stackPos)
    {
        if (_dst is PseudoRegOp dstPseudo)
        {
            _dst = IAllocatableInstruction.HandleAllocation(variableMap, ref stackPos, dstPseudo);
        }
    }

    public override string ToString()
    {
        return IOperand.TokenTypeToUnaryOp(_unaryOp) + "\t" + _dst;
    }
}