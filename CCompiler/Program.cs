﻿using CCompiler.AssemblySyntaxTree;
using CCompiler.CSyntaxTree;

namespace CCompiler;

public static class Program
{
    private const string HelpMessage = "Arguments should follow the pattern: root-path/test-cases/.c files";

    public static int Main(string[] args)
    {
        if (args.Length != 1)
        {
            throw new ArgumentException("Invalid argument length!\n" + HelpMessage);
        }

        if (!File.Exists(args[0]))
        {
            throw new ArgumentException("File could not be found!\n" + HelpMessage);
        }

        var filePath = args[0];
        var fileName = Path.GetFileName(filePath);
        var newFileName = Path.ChangeExtension(fileName, ".s");
        var outputPath = Path.GetDirectoryName(filePath) + "\\" + newFileName;

        try
        {
            Compile(outputPath, File.ReadAllText(filePath));
        }
        catch (SyntaxException ex)
        {
            Console.WriteLine(ex.Message);
            return -1;
        }
        catch (ParseException ex)
        {
            Console.WriteLine(ex.Message);
            return -2;
        }
        catch (SemanticException ex)
        {
            Console.WriteLine(ex.Message);
            return -3;
        }

        return 1;
    }

    private static void Compile(string outputPath, string inputCode)
    {
        var lexer = new Lexer(inputCode);
        var tokens = lexer.TokenizeCode();
        var cSyntaxTree = new ProgramNode(tokens);
        cSyntaxTree.Validate();
        cSyntaxTree.ConvertToTac();
        var assemblySyntaxTree = new AsmProgramNode(cSyntaxTree);
        assemblySyntaxTree.FinalPass();
        File.WriteAllText(outputPath, assemblySyntaxTree.ToString());
    }
}