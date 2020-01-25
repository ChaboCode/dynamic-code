using System;
using System.Collections.Generic;
using System.Text;
 
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using System.Reflection;
 
namespace DynaCode
{
    class Program
    {
        static string[] code = {
            "using System;"+
            "namespace DynaCore"+
            "{"+
            "   public class DynaCore"+
            "   {"+
            "       static public int Main()"+
            "       {"+
            "           {0}"+
            "       }"+
            "   }"+
            "}"};
 
        static void Main(string[] args)
        {
Console.WriteLine(code[0]);
	    string op = Console.ReadLine();
	    string ret = "return " + op;
Console.WriteLine(ret);
Console.WriteLine("a{0}", ret);
	    code[0] = string.Format(code[0], ret);
Console.WriteLine(code);
Console.ReadKey();
            CompileAndRun(code);
 
            Console.ReadKey();
        }
 
        static void CompileAndRun(string[] code)
        {
            CompilerParameters CompilerParams = new CompilerParameters();
            string outputDirectory = Directory.GetCurrentDirectory();
 
            CompilerParams.GenerateInMemory = true;
            CompilerParams.TreatWarningsAsErrors = false;
            CompilerParams.GenerateExecutable = false;
            CompilerParams.CompilerOptions = "/optimize";
 
            string[] references = { "System.dll" };
            CompilerParams.ReferencedAssemblies.AddRange(references);
 
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerResults compile = provider.CompileAssemblyFromSource(CompilerParams, code);
 
            if (compile.Errors.HasErrors)
            {
                string text = "Compile error: ";
                foreach (CompilerError ce in compile.Errors)
                {
                    text += "rn" + ce.ToString();
                }
                throw new Exception(text);
            }
 
            //ExpoloreAssembly(compile.CompiledAssembly);
 
            Module module = compile.CompiledAssembly.GetModules()[0];
            Type mt = null;
            MethodInfo methInfo = null;
 
            if (module != null)
            {
                mt = module.GetType("DynaCore.DynaCore");
            }
 
            if (mt != null)
            {
                methInfo = mt.GetMethod("Main");
            }
 
            if (methInfo != null)
            {
                Console.WriteLine(methInfo.Invoke(null, new object[] {}));
            }
        }
 
        static void ExpoloreAssembly(Assembly assembly)
        {
            Console.WriteLine("Modules in the assembly:");
            foreach (Module m in assembly.GetModules())
            {
                Console.WriteLine("{0}", m);
 
                foreach (Type t in m.GetTypes())
                {
                    Console.WriteLine("t{0}", t.Name);
 
                    foreach (MethodInfo mi in t.GetMethods())
                    {
                        Console.WriteLine("tt{0}", mi.Name);
                    }
                }
            }
        }
    }
}