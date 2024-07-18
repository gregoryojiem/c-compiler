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
        var pseudoReg = new PseudoRegOp(declarationNode.Variable.Identifier);

        if (expression is TacUnaryOpNode unaryOpNode)
        {
            var unaryOperator = unaryOpNode.UnaryOperator.Type;
            var operandValue = IOperand.ExprToAsmOperand(unaryOpNode.Operand);
            instructions.Add(new MovlNode(operandValue, pseudoReg));
            instructions.Add(new UnaryNode(unaryOperator, pseudoReg));
            return;
        }

        if (expression is not TacBinaryOpNode binaryOpNode) throw new NotImplementedException();

        var binaryOperator = binaryOpNode.BinaryOperator.Type;
        var leftOperandValue = IOperand.ExprToAsmOperand(binaryOpNode.LeftOperand);
        var rightOperandValue = IOperand.ExprToAsmOperand(binaryOpNode.RightOperand);

        if (binaryOperator != TokenType.Divide && binaryOperator != TokenType.Modulo)
        {
            instructions.Add(new MovlNode(leftOperandValue, pseudoReg));
            instructions.Add(new BinaryNode(binaryOperator, rightOperandValue, pseudoReg));
            return;
        }

        instructions.Add(new MovlNode(leftOperandValue, new RegOp(RegOp.Register.Eax)));
        instructions.Add(new CdqNode());
        instructions.Add(new DivlNode(rightOperandValue));
        instructions.Add(binaryOpNode.BinaryOperator.Type == TokenType.Divide
            ? new MovlNode(new RegOp(RegOp.Register.Eax), pseudoReg)
            : new MovlNode(new RegOp(RegOp.Register.Edx), pseudoReg));
    }

    public abstract string ConvertToAsm();
}