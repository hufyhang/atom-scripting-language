using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

namespace Atom.core
{
    class core
    {
        private Atom.main.Compiler compiler;

        public core(Atom.main.Compiler compiler)
        {
            this.compiler = compiler;
        }

        public void Condition(String line, VariableLib.VariableLib variables)
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
                this.compiler.translate(temp.Substring(temp.IndexOf(",") + 1), variables);
            }
        }

        protected ArrayList GetLoopInformation(String line, VariableLib.VariableLib variables)
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
                result.Add(temp.Substring(1, temp.IndexOf(",") - 1));
                result.Add(variables.GetInt(temp.Substring(1, temp.IndexOf(",") - 1)));
            }
            else
            {
                result.Add(" ");
                result.Add(temp.Substring(0, temp.IndexOf(",")));
            }
            result.Add(temp.Substring(temp.IndexOf(",") + 1));
            return result;
        }

        public void WhileLoop(String line, VariableLib.VariableLib variables, StreamReader reader)
        {
            ArrayList whileList = new ArrayList();
            line = line.Substring(6);
            String loopName = line.Substring(1, line.IndexOf(",") - 1);
            int loopBool = variables.GetBool(loopName);
            String current = null;
            while ((current = reader.ReadLine()) != "}")
            {
                whileList.Add(current);
            }
            while (this.TranslateBool(loopBool))
            {
                foreach (String cmd in whileList)
                {
                    this.compiler.translate(cmd, variables);
                }
                loopBool = variables.GetBool(loopName);
            }
        }

        protected Boolean TranslateBool(int boolValue)
        {
            if (boolValue == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void InnerAtom(String line, VariableLib.VariableLib variables)
        {
            line = line.Substring(5);
            if (line[0] == '\"')
            {
                line = line.Substring(1, line.Length - 2);
                new Atom.main.Compiler(line).Compile();
            }
            else if (line[0] == '%')
            {
                new Atom.main.Compiler(variables.GetString(line.Substring(1))).Compile();
            }
        }

        public void Loop(String line, Atom.main.InnerLoop baseLoopStatements, Boolean baseLoop, VariableLib.VariableLib variables, StreamReader reader)
        {
            ArrayList information = this.GetLoopInformation(line, variables);

            int begin = int.Parse(information[0].ToString());
            String endName = information[1].ToString();
            int end = int.Parse(information[2].ToString());
            String temp = information[3].ToString();
            if (baseLoop)
            {
                baseLoopStatements = new Atom.main.InnerLoop(begin, end, endName,variables);
                begin = end = 0;
            }

            Atom.main.InnerLoop devideLoopStatements = new Atom.main.InnerLoop(begin, end, endName, variables);

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
                        this.Loop(temp, baseLoopStatements, false, variables, reader);
                    }
                }
            }
            if (baseLoop)
            {
                baseLoopStatements.Execute(variables);
            }
            else
            {
                baseLoopStatements.CombineStatements(devideLoopStatements);
            }
        }
    }
}
