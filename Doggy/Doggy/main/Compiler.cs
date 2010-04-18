using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using StdLib;
using Formatter;

namespace Atom.main
{
    class Compiler
    {
        private String sourceFile = null;
        private String line = null;
        private StreamReader reader;
        private VariableLib.VariableLib variables = new VariableLib.VariableLib();
        private InnerLoop innerLoop;
        private ArrayList externalData = null;

        public Compiler() { }

        public Compiler(String args, ArrayList externalData)
        {
            this.sourceFile = args;
            this.externalData = externalData;
            new Formatter.Formatter(args).Execute();
            this.innerLoop = new InnerLoop(0, 0, " ", this.variables);
            reader = new StreamReader(args + @".atom");
        }

        public Compiler(string[] args)
        {
            this.sourceFile = args[0];
            new Formatter.Formatter(args[0]).Execute();
            this.innerLoop = new InnerLoop(0, 0, " ", this.variables);
            reader = new StreamReader(args[0] + @".atom");
        }

        public void Compile()
        {
            while ((line = reader.ReadLine()) != null)
            {
                if (line == @"<prog>")
                {
                    while ((line = reader.ReadLine()) != "</prog>")
                    {
                        this.translate(this.line, this.variables);
                    }
                }
            }
            reader.Close();
        }

        public void translate(String line, VariableLib.VariableLib vars)
        {
            try
            {
                if (line[0] != '.' && line.Length != 0)
                {
                    switch (line.Substring(0, line.IndexOf(" ")))
                    {
                        case "show":
                            new StdLib.StdLib(line, vars, this.sourceFile).Show();
                            break;

                        case "showln":
                            new StdLib.StdLib(line, vars, this.sourceFile).Showln();
                            break;

                        case "loop":
                            new Atom.core.core(this).Loop(line, this.innerLoop, true, vars, this.reader);
                            break;

                        case "if":
                            new Atom.core.core(this).Condition(line, vars);
                            break;

                        case "read":
                            new StdLib.StdLib(line, vars, this.sourceFile).Read();
                            break;

                        case "dim":
                            vars.Dim(line);
                            break;

                        case "var":
                            vars.varOpt(line);
                            break;

                        case "bool":
                            vars.boolOpt(line);
                            break;

                        case "get":
                            new StdLib.StdLib(line, vars, this.sourceFile).GetInput();
                            break;

                        case "while":
                            new Atom.core.core(this).WhileLoop(line, vars, this.reader);
                            break;

                        case "atom":
                            new Atom.core.core(this).InnerAtom(line, vars);
                            break;

                        case "external":
                            new Atom.core.core(this).ExternalData(line, vars, this.externalData);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Title = "Atom -- ERROR";
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nOops! Atom has detected an error occured in " + this.sourceFile + "!");
                Console.WriteLine("> ERROR: " + line + " <");
                Console.WriteLine(e.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;
                Environment.Exit(0);
            }
        }
    }

}
