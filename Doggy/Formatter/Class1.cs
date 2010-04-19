using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace Formatter
{
    public class Formatter
    {
        private FileInfo fileInfo;
        private ArrayList content;
        private String file;
        private StreamReader reader;
        private StreamWriter writer;

        public Formatter(String file)
        {
            this.content = new ArrayList();
            this.file = file;
            this.fileInfo = new FileInfo(this.file + @".atom");
            if (this.fileInfo.Exists)
            {
                this.fileInfo.Delete();
            }
            else
            {
                this.fileInfo.Create();
            }
        }

        protected void readIn()
        {
            try
            {
                this.reader = new StreamReader(this.file);
            }
            catch (Exception)
            {
                Console.Title = "Atom -- ERROR";
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nOops! Atom has detected an error occured in your source file.");
                Console.WriteLine("> ACCESS ERROR: Cannot access \"" + this.file + "\" <");
                Console.ForegroundColor = ConsoleColor.Gray;
                Environment.Exit(0);
            }

            String temp = null;
            while ((temp = this.reader.ReadLine()) != null)
            {
                try
                {
                    while (temp[0] == '\t' || temp[0] == ' ')
                    {
                        temp = temp.Remove(0, 1);
                    }
                    this.content.Add(temp);
                }
                catch (IndexOutOfRangeException)
                {
                }
            }
        }

        protected void output()
        {
            this.writer = new StreamWriter(this.file + @".atom");
            foreach (String str in this.content)
            {
                this.writer.WriteLine(str);
            }
        }

        public void Execute()
        {
            this.readIn();
            this.reader.Close();
            this.output();
            this.writer.Close();
        }
    }
}
