﻿namespace CCompiler.AssemblySyntaxTree.Instructions;

public class JmpNode : AsmInstructionNode
{
    private string _identifier;

    public JmpNode(string identifier)
    {
        _identifier = identifier;
    }

    public override string ToString()
    {
        return "jmp\t\t." + _identifier;
    }
}