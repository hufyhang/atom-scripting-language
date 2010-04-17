using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Doggy.main
{
    class InnerLoop
    {
        private int begin;
        private int end;
        private ArrayList statements;
        private VariableLib.VariableLib variables;

        public InnerLoop(int begin, int end, VariableLib.VariableLib variables)
        {
            this.begin = begin;
            this.end = end;
            this.statements = new ArrayList();
            this.variables = variables;
//            this.AddStatement("@");
        }

        public void combineStatements(InnerLoop devideStatements)
        {
            int combineBegin = devideStatements.GetBegin();
            int combineEnd = devideStatements.GetEnd();

            if (combineBegin <= combineEnd)
            {
                for (int index = combineBegin; index <= combineEnd; ++index)
                {
                    foreach (String str in devideStatements.GetStamentsList())
                    {
                        this.statements.Add(str);
                    }
                }
            }
            else
            {
                for (int index = combineBegin; index <= combineEnd; --index)
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
                for (int index = this.begin; index != this.end; ++index)
                {
                    foreach (String str in this.statements)
                    {
                        new Compiler().translate(str, variables);
                    }
                }
            }
            else
            {
                for (int index = this.begin; index != this.end; --index)
                {
                    foreach (String str in this.statements)
                    {
                        new Compiler().translate(str, variables);
                    }
                }
            }
        }
    }
}
