using System;
using System.Collections.Generic;
using System.Text;

namespace Doggy
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Title = "Atom -- No input file";
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Doggy: no input file");
                Console.ForegroundColor = ConsoleColor.Gray;
                Environment.Exit(0);
            }
            else
            {
                Console.Title = "Atom -- " + args[0];
                Console.ForegroundColor = ConsoleColor.White;
                new Atom.main.Compiler(args).Compile();
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
