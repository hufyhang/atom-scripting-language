using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Atom.main
{
    class InnerLoop
    {
        private int begin;
        private int end;
        private String endName;
        private ArrayList statements;
        private VariableLib.VariableLib variables;

        public InnerLoop(int begin, int end, String endName, VariableLib.VariableLib variables)
        {
            this.begin = begin;
            this.end = end;
            this.endName = endName;
            this.statements = new ArrayList();
            this.variables = variables;
        }

        public void CombineStatements(InnerLoop devideStatements)
        {
            int combineBegin = devideStatements.GetBegin();
            int combineEnd = devideStatements.GetEnd();

            if (combineBegin <= combineEnd)
            {
                for (int index = combineBegin; index != combineEnd; ++index)
                {
                    foreach (String str in devideStatements.GetStamentsList())
                    {
                        this.statements.Add(str);
                    }
                }
            }
            else
            {
                for (int index = combineBegin; index != combineEnd; --index)
                {
                    foreach (String str in devideStatements.GetStamentsList())
                    {
                        this.statements.Add(str);
                    }
                }
            }
        }

        public void AddStatement(String line)
        {
            this.statements.Add(line);
        }

        public int GetBegin()
        {
            return this.begin;
        }

        public int GetEnd()
        {
            return this.end;
        }

        public ArrayList GetStamentsList()
        {
            return this.statements;
        }

        public int GetLength()
        {
            return this.statements.Count;
        }

        public void Execute(VariableLib.VariableLib variables)
        {
            if (this.begin <= this.end)
            {
                int index = this.begin;
                while (index != this.end)
                {
                    foreach (String str in this.statements)
                    {
                        new Compiler().translate(str, variables);
                    }
                    ++index;
                    if (this.endName != " ")
                    {
                        this.end = variables.GetInt(this.endName);
                    }
                }
            }
            else
            {
                int index = this.begin;
                while (index != this.end)
                {
                    foreach (String str in this.statements)
                    {
                        new Compiler().translate(str, variables);
                    }
                    --index;
                    if (this.endName != " ")
                    {
                        this.end = variables.GetInt(this.endName);
                    }
                }
            }
        }
    }
}
