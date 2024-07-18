using CCompiler.AssemblySyntaxTree.Operands;
using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.AssemblySyntaxTree.Instructions;

public abstract class AsmInstructionNode : IAsmNode
{
    public static void ConvertCToAsmInstructions(List<AsmInstructionNode> instructions, StatementNode cStatement)
    {
        if (cStatement is DeclarationNode declarationNode)
        {
            ConvertDeclarationToAsm(instructions, declarationNode);
            return;
        }

        if (cStatement is ReturnStmtNode returnStmtNode)
        {
            var operandValue = IOperand.ExprToAsmOperand(returnStmtNode.ReturnValue);
            instructions.Add(new MovlNode(operandValue, new RegOp(RegOp.Register.Eax)));
            instructions.Add(new RetNode());
            return;
        }

        throw new NotImplementedException();
    }

    private static void ConvertDeclarationToAsm(List<AsmInstructionNode> instructions, DeclarationNode declarationNode)
    {
        var expression = declarationNode.ExpressionNode;
        var pseudoRegId = declarationNode.Variable.Identifier;

        switch (expression)
        {
            case TacUnaryOpNode { UnaryOperator.Type: TokenType.Not } relOpNode:
                ConvertNotOp(instructions, relOpNode, pseudoRegId);
                break;
            case TacBinaryOpNode
            {
                BinaryOperator.Type: TokenType.Eq or TokenType.Neq or
                TokenType.Lt or TokenType.Gt or
                TokenType.LtOrEq or TokenType.GtOrEq
            } relOpNode:
                ConvertRelOp(instructions, relOpNode, pseudoRegId);
                break;
            case TacBinaryOpNode { BinaryOperator.Type: TokenType.And or TokenType.Or } relOpNode:
                ConvertShortCircOp(instructions, relOpNode, pseudoRegId);
                break;
            case TacUnaryOpNode unaryOpNode:
                ConvertUnaryOp(instructions, unaryOpNode, pseudoRegId);
                break;
            case TacBinaryOpNode binaryOpNode:
                ConvertBinaryOp(instructions, binaryOpNode, pseudoRegId);
                break;
        }
    }

    private static void ConvertNotOp(List<AsmInstructionNode> instructions, TacUnaryOpNode unaryNode, string varId)
    {
    }

    private static void ConvertRelOp(List<AsmInstructionNode> instructions, TacBinaryOpNode binaryNode, string varId)
    {
    }

    private static void ConvertShortCircOp(List<AsmInstructionNode> instructions, TacBinaryOpNode ssOp, string varId)
    {
    }

    private static void ConvertUnaryOp(List<AsmInstructionNode> instructions, TacUnaryOpNode unaryNode, string varId)
    {
        var pseudoReg = new PseudoRegOp(varId);
        var unaryOperator = unaryNode.UnaryOperator.Type;
        var operandValue = IOperand.ExprToAsmOperand(unaryNode.Operand);
        instructions.Add(new MovlNode(operandValue, pseudoReg));
        instructions.Add(new UnaryNode(unaryOperator, pseudoReg));
    }

    private static void ConvertBinaryOp(List<AsmInstructionNode> instructions, TacBinaryOpNode binaryNode, string varId)
    {
        var pseudoReg = new PseudoRegOp(varId);
        var binaryOperator = binaryNode.BinaryOperator.Type;
        var leftOperandValue = IOperand.ExprToAsmOperand(binaryNode.LeftOperand);
        var rightOperandValue = IOperand.ExprToAsmOperand(binaryNode.RightOperand);

        if (binaryOperator != TokenType.Divide && binaryOperator != TokenType.Modulo)
        {
            instructions.Add(new MovlNode(leftOperandValue, pseudoReg));
            instructions.Add(new BinaryNode(binaryOperator, rightOperandValue, pseudoReg));
            return;
        }

        instructions.Add(new MovlNode(leftOperandValue, new RegOp(RegOp.Register.Eax)));
        instructions.Add(new CdqNode());
        instructions.Add(new DivlNode(rightOperandValue));
        instructions.Add(binaryNode.BinaryOperator.Type == TokenType.Divide
            ? new MovlNode(new RegOp(RegOp.Register.Eax), pseudoReg)
            : new MovlNode(new RegOp(RegOp.Register.Edx), pseudoReg));
    }

    public abstract string ConvertToAsm();
}