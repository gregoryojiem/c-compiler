namespace CCompiler.AssemblySyntaxTree.Instructions;

public class RetNode : AsmInstructionNode
{
    public override string ConvertToAsm()
    {
        var outputAsm = "";
        outputAsm += "movq\t%rbp, %rsp" +"\n";
        outputAsm += "\t" + "popq\t%rbp" +"\n";
        return outputAsm + "\tret";
    }
}