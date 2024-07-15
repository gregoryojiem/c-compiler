using CCompiler.AssemblySyntaxTree.Operands;
using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;

namespace CCompiler.AssemblySyntaxTree.Instructions;

public abstract class AsmInstructionNode : IAsmNode
{
    public static void ConvertCToAsmInstructions(List<AsmInstructionNode> instructions, StatementNode cStatement)
    {
        if (cStatement is DeclarationNode declarationNode) //TODO currently only represents unary operators
        {
            var unaryOpNode = (TacUnaryOpNode)declarationNode.ExpressionNode;
            var unaryOperator = unaryOpNode.UnaryOperator.Type;
            var operandValue = IOperand.ExprToAsmOperand(unaryOpNode.Operand);
            var pseudoReg = new PseudoRegOp(declarationNode.Variable.Identifier);
            instructions.Add(new MovlNode(operandValue, pseudoReg));
            instructions.Add(new UnaryNode(unaryOperator, pseudoReg));
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

    public abstract string ConvertToAsm();
}