using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.AssemblySyntaxTree.Operands;

public interface IOperand
{
    public static IOperand ExprToAsmOperand(ExpressionNode expression)
    {
        return expression switch
        {
            TacConstantNode constantNode => new ImmOp(constantNode.Value),
            TacVariableNode variableNode => new PseudoRegOp(variableNode.Identifier),
            _ => throw new NotImplementedException()
        };
    }

    public static string TokenTypeToUnaryOp(TokenType tokenType)
    {
        return tokenType switch
        {
            TokenType.Negate => "negl",
            TokenType.Complement => "notl",
            _ => throw new NotImplementedException()
        };
    }

    public static string TokenTypeToBinaryOp(TokenType tokenType)
    {
        return tokenType switch
        {
            TokenType.Add => "addl",
            TokenType.Negate => "subl",
            TokenType.Multiply => "imull",
            _ => throw new NotImplementedException()
        };
    }

    public static string GetConditionCode(TokenType condCode)
    {
        return condCode switch
        {
            TokenType.Eq => "e",
            TokenType.Neq => "ne",
            TokenType.Lt => "l",
            TokenType.Gt => "g",
            TokenType.LtOrEq => "le",
            TokenType.GtOrEq => "ge",
            _ => throw new NotImplementedException()
        };
    }
}