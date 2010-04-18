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
            ArrayList data = new ArrayList();
            String[] args = line.Split(',');
            String atom = null;

            if (args[0][0] == '\"')
            {
                atom = args[0].Substring(1,args[0].Length - 2);
            }
            else if (args[0][0] == '%')
            {
                atom = variables.GetString(args[0].Substring(1));
            }

            for (int index = 1; index != args.Length; ++index)
            {
                switch (args[index][0])
                {
                    case '$':
                        data.Add(variables.GetInt(args[index].Substring(1)));
                        break;

                    case '%':
                        data.Add(variables.GetString(args[index].Substring(1)));
                        break;

                    default:
                        data.Add(args[index]);
                        break;
                }
            }

            new Atom.main.Compiler(atom, data).Compile();
        }

        public void ExternalData(String line, VariableLib.VariableLib variables, ArrayList externalData)
        {
            line = line.Substring(9);
            String[] vars = line.Split(',');
            int index = 0;
            foreach (String var in vars)
            {
                switch (var[0])
                {
                    case '$':
                        variables.setInt(var.Substring(1), int.Parse(externalData[index].ToString()));
                        break;

                    case '%':
                        variables.setString(var.Substring(1), externalData[index].ToString());
                        break;
                }
                ++index;
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
