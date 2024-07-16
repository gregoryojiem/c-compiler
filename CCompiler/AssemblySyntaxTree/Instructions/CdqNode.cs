﻿namespace CCompiler.AssemblySyntaxTree.Instructions;

public class CdqNode : AsmInstructionNode
{
    public override string ConvertToAsm()
    {
        return "cdq";
    }
}