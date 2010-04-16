using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace StdLib
{
    public class StdLib
    {
        private String line = null;
        private VariableLib.VariableLib variables;
/*
        public StdLib(String line)
        {
            this.line = line;
        }
*/
        public StdLib(String line, VariableLib.VariableLib variables)
        {
            this.line = line;
            this.variables = variables;
        }

        public void Show()
        {
            if (this.line.Contains("\""))
            {
                int index = this.line.IndexOf("\"") + 1;
                Console.Write(this.line.Substring(index, this.line.Length - index - 1));
            }
            else
            {
                line = line.Replace("show ", "");
                if (line[0] == '$')
                {
                    line = line.Substring(1);
                    Console.Write(this.variables.GetInt(line));
                }
                else
                {
                    line = line.Substring(1);
                    Console.Write(this.variables.GetString(line));
                }
            }
        }

        public void Showln()
        {
            if (this.line.Contains("\""))
            {
                int index = this.line.IndexOf("\"") + 1;
                Console.WriteLine(this.line.Substring(index, this.line.Length - index - 1));
            }
            else
            {
                line = line.Replace("showln ", "");
                if (line[0] == '$')
                {
                    line = line.Substring(1);
                    Console.WriteLine(this.variables.GetInt(line));
                }
                else
                {
                    line = line.Substring(1);
                    Console.WriteLine(this.variables.GetString(line));
                }
            }
        }

        public void Read()
        {
            String temp = line.Substring(6);
            temp = temp.Substring(0, temp.Length - 1);
            StreamReader reader = new StreamReader(temp);
            Console.WriteLine(reader.ReadToEnd());
            reader.Close();
        }
    }
}
