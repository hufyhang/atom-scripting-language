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
                Console.WriteLine("Doggy: no input file");
            }
            else
            {
                new main.Compiler(args).Compile();
            }
        }
    }
}
