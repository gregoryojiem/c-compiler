using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.AssemblySyntaxTree.Operands;

public interface IOperand
{
    public static IOperand ExprToAsmOperand(ExpressionNode expression)
    {
        return expression switch
        {
            ConstantNode constantNode => new ImmOp(constantNode.Value),
            VariableNode variableNode => new PseudoRegOp(variableNode.Identifier),
            _ => throw new NotImplementedException()
        };
    }
}