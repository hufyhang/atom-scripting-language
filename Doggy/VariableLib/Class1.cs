using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace VariableLib
{
    public class VariableLib
    {
        private ArrayList intName;
        private ArrayList intList;
        private ArrayList StringName;
        private ArrayList StringList;

        public VariableLib()
        {
            this.intName = new ArrayList();
            this.intList = new ArrayList();
            this.StringName = new ArrayList();
            this.StringList = new ArrayList();
        }

        public void varOpt(String line)
        {
            String temp = line.Substring(4);
            char type = temp[0];
            temp = temp.Substring(1);
            String name = temp.Substring(0, temp.IndexOf(","));
            temp = temp.Substring(temp.IndexOf(",") + 1);
            char opt = temp[0];
            String tar = temp.Substring(2);
            int target = 0;
            String strTarget = null;
            if (tar[0] == '$')
            {
                target = this.GetInt(tar.Substring(1));
            }
            else if (tar[0] == '%')
            {
                strTarget = this.GetString(tar.Substring(1));
            }
            else
            {
                strTarget = tar.Substring(0);
                try
                {
                    target = int.Parse(strTarget);
                }
                catch (FormatException)
                {
                }
            }

            switch (type)
            {
                case '$':
                    this.varMathOpt(name, opt, target);
                    break;

                case '%':
                    this.varStringOpt(name, opt, strTarget);
                    break;

                default:
                    break;
            }
        }

        public void setInt(String name, int value)
        {
            this.intList[this.intName.IndexOf(name)] = value;
        }

        protected void varStringOpt(String name, char opt, String target)
        {
            switch (opt)
            {
                case '+':
                    this.StringList[this.StringName.IndexOf(name)] = this.StringList[this.StringName.IndexOf(name)].ToString() + target;
                    break;

                case '=':
                    this.StringList[this.StringName.IndexOf(name)] = target;
                    break;
            }
        }


        protected void varMathOpt(String name, char opt, int target)
        {
            switch (opt)
            {
                case '+':
                    this.intList[this.intName.IndexOf(name)] = int.Parse(this.intList[this.intName.IndexOf(name)].ToString()) + target;
                    break;

                case '-':
                    this.intList[this.intName.IndexOf(name)] = int.Parse(this.intList[this.intName.IndexOf(name)].ToString()) - target;
                    break;

                case '*':
                    this.intList[this.intName.IndexOf(name)] = int.Parse(this.intList[this.intName.IndexOf(name)].ToString()) * target;
                    break;

                case '/':
                    this.intList[this.intName.IndexOf(name)] = int.Parse(this.intList[this.intName.IndexOf(name)].ToString()) / target;
                    break;

                case '=':
                    this.intList[this.intName.IndexOf(name)] = target;
                    break;
            }
        }

        public void Dim(String line)
        {
            String temp = line.Substring(4);
            String type = temp.Substring(0, temp.IndexOf(" "));
            temp = temp.Replace(type + " ", "");
            String name = temp.Substring(0, temp.IndexOf(","));
            String value = temp.Replace(name + ",", "");

            switch (type)
            {
                case "int":
                    this.DimInt(name, value);
                    break;

                case "String":
                    this.DimString(name, value);
                    break;
            }
        }

        public void DimInt(String name, String value)
        {
            this.intName.Add(name);
            this.intList.Add(int.Parse(value));
        }

        public int GetInt(String name)
        {
            return int.Parse(this.intList[this.intName.IndexOf(name)].ToString());
        }

        public String GetString(String name)
        {
            return this.StringList[this.StringName.IndexOf(name)].ToString();
        }

        public void DimString(String name, String value)
        {
            this.StringName.Add(name);
            this.StringList.Add(value);
        }
    }
}
