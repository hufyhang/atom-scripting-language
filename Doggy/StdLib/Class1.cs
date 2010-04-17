using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace StdLib
{
    public class StdLib
    {
        private String sourceFile = null;
        private String line = null;
        private VariableLib.VariableLib variables;

        public StdLib(String line, VariableLib.VariableLib variables, String sourceFile)
        {
            this.sourceFile = sourceFile;
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
                else if (line[0] == '%')
                {
                    line = line.Substring(1);
                    Console.Write(this.variables.GetString(line));
                }
                else if (line[0] == '&')
                {
                    line = line.Substring(1);
                    Console.Write(this.variables.GetBool(line));
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
                else if (line[0] == '%')
                {
                    line = line.Substring(1);
                    Console.WriteLine(this.variables.GetString(line));
                }
                else if (line[0] == '&')
                {
                    line = line.Substring(1);
                    Console.WriteLine(this.variables.GetBool(line));
                }
            }
        }

        public void GetInput()
        {
            String input = Console.ReadLine();
            String temp = line.Substring(4);
            if (temp[0] == '$')
            {
                variables.setInt(temp.Substring(1), int.Parse(input));
            }
            else if(temp[0] == '%')
            {
                variables.setString(temp.Substring(1), input);
            }
        }

        public void Read()
        {
            String temp = line.Substring(5);
            if (temp[0] == '%')
            {
                temp = variables.GetString(temp.Substring(1));
            }
            else
            {
                temp = temp.Substring(1, temp.Length - 2);
            }
            try
            {
                StreamReader reader = new StreamReader(temp);
                Console.WriteLine(reader.ReadToEnd());
                reader.Close();
            }
            catch (Exception)
            {
                Console.Title = "Atom -- ERROR";
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nOops! Atom has detected an error occured in " + this.sourceFile + "!");
                Console.WriteLine("> FILESTREAM ERROR: " + line + " <");
                Console.ForegroundColor = ConsoleColor.Gray;
                Environment.Exit(0);
            }
        }
    }
}
;