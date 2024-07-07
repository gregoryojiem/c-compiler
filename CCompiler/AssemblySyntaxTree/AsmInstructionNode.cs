using CCompiler.CSyntaxTree;

namespace CCompiler.AssemblySyntaxTree;

public abstract class AsmInstructionNode : AsmNode
{
    public static void ConvertCToAsmInstructions(List<AsmInstructionNode> instructions, StatementNode cStatement)
    {
        if (cStatement is ReturnStmtNode returnStmtNode)
        {
            instructions.Add(new MovlNode(returnStmtNode.ReturnValue));
            instructions.Add(new RetNode());
        }
    }

    public abstract string ConvertToAsm();
}