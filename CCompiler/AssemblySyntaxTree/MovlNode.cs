using CCompiler.CSyntaxTree;

namespace CCompiler.AssemblySyntaxTree;

public class MovlNode : AsmInstructionNode
{
    private readonly string _src;
    private readonly string _dst;
    
    public MovlNode(ExpressionNode expression)
    {
        if (expression is ConstantNode constantNode)
        {
            _src = "$" + constantNode._constantValue.Value; 
            _dst = "%eax";
        } 
    }

    public override string ConvertToAsm()
    {
        return "movl\t" + _src + ", " + _dst;
    }
}