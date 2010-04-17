using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using StdLib;
using Formatter;

namespace Doggy.main
{
    class Compiler
    {
        private String line = null;
        private StreamReader reader;
        private VariableLib.VariableLib variables = new VariableLib.VariableLib();
        private InnerLoop innerLoop;

        public Compiler() { }

        public Compiler(string[] args)
        {
            new Formatter.Formatter(args[0]).Execute();
            this.innerLoop = new InnerLoop(0, 0, this.variables);
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

        protected void condition(String line)
        {
            int TURE = 1;
            int FALES = 0;
            Boolean cond = false;
            String temp = line.Substring(3);
            int index = temp.IndexOf(',');

            String conditionStatement = temp.Substring(0, temp.IndexOf(","));
            int condition = 0;
            if (conditionStatement[0] == '&')
            {
                condition = variables.GetBool(conditionStatement.Substring(1));
            }
            else
            {
                condition = int.Parse(conditionStatement);
            }
            
            if (condition == TURE)
            {
                cond = true;
            }
            else if (condition == FALES)
            {
                cond = false;
            }
            if (cond)
            {
                this.translate(temp.Substring(temp.IndexOf(",") + 1), variables);
            }
        }

        protected ArrayList getLoopInformation(String line)
        {
            ArrayList result = new ArrayList();
            String temp = line.Substring(5);
            if (temp[0] == '$')
            {
                result.Add(variables.GetInt(temp.Substring(1, temp.IndexOf(",") - 1)));
            }
            else
            {
                result.Add(temp.Substring(0, temp.IndexOf(",")));
            }
            temp = temp.Substring(temp.IndexOf(",") + 1);
            if (temp[0] == '$')
            {
                result.Add(variables.GetInt(temp.Substring(1, temp.IndexOf(",") - 1)));
            }
            else
            {
                result.Add(temp.Substring(0, temp.IndexOf(",")));
            }
            result.Add(temp.Substring(temp.IndexOf(",") + 1));
            return result;
        }

        protected void Loop(String line, InnerLoop baseLoopStatements, Boolean baseLoop, VariableLib.VariableLib variables)
        {
            ArrayList information = this.getLoopInformation(line);

            int begin = int.Parse(information[0].ToString());
            int end = int.Parse(information[1].ToString());
            String temp = information[2].ToString();
            if (baseLoop)
            {
                baseLoopStatements = new InnerLoop(begin, end, variables);
                begin = end = 0;
            }

            InnerLoop devideLoopStatements = new InnerLoop(begin, end, variables);

            if (temp[0] != '{')
            {
                baseLoopStatements.AddStatement(temp);
            }
            else
            {
                while ((temp = reader.ReadLine()) != "}")
                {
                    if (temp.Substring(0, line.IndexOf(" ")) != "loop")
                    {
                        if (baseLoop)
                        {
                            baseLoopStatements.AddStatement(temp);
                        }
                        else
                        {
                            devideLoopStatements.AddStatement(temp);
                        }
                    }
                    else
                    {
                        this.Loop(temp, baseLoopStatements, false, variables);
                    }
                }
            }
            if (baseLoop)
            {
                baseLoopStatements.Execute(variables);
            }
            else
            {
                baseLoopStatements.combineStatements(devideLoopStatements);
            }
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
                            new StdLib.StdLib(line, vars).Show();
                            break;

                        case "showln":
                            new StdLib.StdLib(line, vars).Showln();
                            break;

                        case "loop":
                            this.Loop(line, this.innerLoop, true, vars);
                            break;

                        case "if":
                            this.condition(line);
                            break;

                        case "read":
                            new StdLib.StdLib(line, vars).Read();
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
                            new StdLib.StdLib(line, vars).GetInput();
                            break;
                    }
                }
            }
            catch (Exception)
            {
                Console.Title = "Atom -- ERROR";
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nOops! Atom has detected an error occured in your source file.");
                Console.WriteLine("> ERROR: " + line + " <");
                Console.ForegroundColor = ConsoleColor.Gray;
                Environment.Exit(0);
            }
        }
    }
}
