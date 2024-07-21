using CCompiler.AssemblySyntaxTree.Operands;
using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.AssemblySyntaxTree.Instructions;

public abstract class AsmInstructionNode : IAsmNode
{
    public static void ConvertCToAsmInstructions(List<AsmInstructionNode> instructions, StatementNode cStatement)
    {
        switch (cStatement)
        {
            case AssignmentNode assignmentNode:
                ConvertAssignmentToAsm(instructions, assignmentNode);
                break;
            case JumpIfZeroNode jumpIfZeroNode:
            {
                var condCode = jumpIfZeroNode.Inverted ? TokenType.Neq : TokenType.Eq;
                var condition = IOperand.ExprToAsmOperand(jumpIfZeroNode.Condition);
                instructions.Add(new CmpNode(new ImmOp(0), condition));
                instructions.Add(new JmpCcNode(condCode, jumpIfZeroNode.Target));
                break;
            }
            case JumpNode jumpNode:
            {
                instructions.Add(new JmpNode(jumpNode.Target));
                break;
            }
            case LabelNode labelNode:
            {
                instructions.Add(new JmpLabelNode(labelNode.Identifier));
                break;
            }
            case ReturnStmt returnStmt:
            {
                var operandValue = IOperand.ExprToAsmOperand(returnStmt.ReturnValue);
                instructions.Add(new MovlNode(operandValue, new RegOp(RegOp.Register.Eax)));
                instructions.Add(new RetNode());
                break;
            }
            default:
                throw new NotImplementedException();
        }
    }

    private static void ConvertAssignmentToAsm(List<AsmInstructionNode> instructions, AssignmentNode assignmentNode)
    {
        var expression = assignmentNode.ExpressionNode;
        var pseudoRegId = assignmentNode.TacVariable.Identifier;

        switch (expression)
        {
            case BaseValueNode:
                var valToCopy = IOperand.ExprToAsmOperand(expression);
                instructions.Add(new MovlNode(valToCopy, new PseudoRegOp(pseudoRegId)));
                break;
            case TacUnaryOpNode { UnaryOperator: TokenType.Not } relOpNode:
                var binaryNode = new TacBinaryOpNode(TokenType.Eq, relOpNode.Operand, new TacConstantNode(0));
                ConvertRelOp(instructions, binaryNode, pseudoRegId);
                break;
            case TacBinaryOpNode
            {
                BinaryOperator: TokenType.Eq or TokenType.Neq or
                TokenType.Lt or TokenType.Gt or TokenType.LtOrEq or TokenType.GtOrEq
            } relOpNode:
                ConvertRelOp(instructions, relOpNode, pseudoRegId);
                break;
            case TacUnaryOpNode unaryOpNode:
                ConvertUnaryOp(instructions, unaryOpNode, pseudoRegId);
                break;
            case TacBinaryOpNode binaryOpNode:
                ConvertBinaryOp(instructions, binaryOpNode, pseudoRegId);
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private static void ConvertRelOp(List<AsmInstructionNode> instructions, TacBinaryOpNode binaryNode, string varId)
    {
        var pseudoReg = new PseudoRegOp(varId);
        var leftOp = IOperand.ExprToAsmOperand(binaryNode.LeftOperand);
        var rightOp = IOperand.ExprToAsmOperand(binaryNode.RightOperand);
        instructions.Add(new CmpNode(rightOp, leftOp));
        instructions.Add(new MovlNode(new ImmOp(0), pseudoReg));
        instructions.Add(new SetCcNode(binaryNode.BinaryOperator, pseudoReg));
    }

    private static void ConvertUnaryOp(List<AsmInstructionNode> instructions, TacUnaryOpNode unaryNode, string varId)
    {
        var pseudoReg = new PseudoRegOp(varId);
        var unaryOperator = unaryNode.UnaryOperator;
        var operandValue = IOperand.ExprToAsmOperand(unaryNode.Operand);
        instructions.Add(new MovlNode(operandValue, pseudoReg));
        instructions.Add(new UnaryNode(unaryOperator, pseudoReg));
    }

    private static void ConvertBinaryOp(List<AsmInstructionNode> instructions, TacBinaryOpNode binaryNode, string varId)
    {
        var pseudoReg = new PseudoRegOp(varId);
        var binaryOperator = binaryNode.BinaryOperator;
        var leftOperandValue = IOperand.ExprToAsmOperand(binaryNode.LeftOperand);
        var rightOperandValue = IOperand.ExprToAsmOperand(binaryNode.RightOperand);

        if (binaryOperator != TokenType.Divide && binaryOperator != TokenType.Modulo)
        {
            instructions.Add(new MovlNode(leftOperandValue, pseudoReg));
            instructions.Add(new BinaryNode(binaryOperator, rightOperandValue, pseudoReg));
            return;
        }

        var axRegToUse = RegOp.Register.Eax;
        var dxRegToUse = RegOp.Register.Edx;
        instructions.Add(new MovlNode(leftOperandValue, new RegOp(RegOp.Register.Eax)));
        instructions.Add(new CdqNode());
        instructions.Add(new DivlNode(rightOperandValue));
        instructions.Add(binaryNode.BinaryOperator == TokenType.Divide
            ? new MovlNode(new RegOp(axRegToUse), pseudoReg)
            : new MovlNode(new RegOp(dxRegToUse), pseudoReg));
    }

    public abstract string ToString();
}