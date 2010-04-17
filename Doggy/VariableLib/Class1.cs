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
        private ArrayList boolName;
        private ArrayList boolList;

        public VariableLib()
        {
            this.intName = new ArrayList();
            this.intList = new ArrayList();
            this.StringName = new ArrayList();
            this.StringList = new ArrayList();
            this.boolName = new ArrayList();
            this.boolList = new ArrayList();
        }

        private String TranslateArg(String source)
        {
            String result = null;
            if (source[0] == '$')
            {
                result = this.GetInt(source.Substring(1)).ToString();
            }
            else if (source[0] == '%')
            {
                result = this.GetString(source.Substring(1));
            }
            else
            {
                result = source;
            }
            return result;
        }

        public void boolOpt(String line)
        {
            String cmd = line.Substring(5);
            char type = cmd[0];
            cmd = cmd.Substring(1);
            String name = cmd.Substring(0, cmd.IndexOf(","));
            cmd = cmd.Substring(cmd.IndexOf(",") + 1);
            String first = cmd.Substring(0, cmd.IndexOf(","));
            cmd = cmd.Substring(cmd.IndexOf(",") + 1);
            String opt = cmd.Substring(0, cmd.IndexOf(","));
            cmd = cmd.Substring(cmd.IndexOf(",") + 1);
            String secound = cmd.Substring(0);

            String firstValue = this.TranslateArg(first);
            String secoundValue = this.TranslateArg(secound);
            this.boolList[this.boolName.IndexOf(name)] = this.boolVarOpt(opt, firstValue, secoundValue);
        }

        protected int boolVarOpt(String optType, String first, String secound)
        {
            int result = 0;

            switch (optType)
            {
                case "<":
                    if (int.Parse(first) < int.Parse(secound))
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                    break;

                case ">":
                    if (int.Parse(first) > int.Parse(secound))
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                    break;

                case "==":
                    if (int.Parse(first) == int.Parse(secound))
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                    break;

                case "!=":
                    if (int.Parse(first) != int.Parse(secound))
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                    break;
            }
            
            return result;
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
            String target = this.TranslateArg(tar);

            switch (type)
            {
                case '$':
                    this.varMathOpt(name, opt, target);
                    break;

                case '%':
                    this.varStringOpt(name, opt, target);
                    break;

                default:
                    break;
            }
        }

        public void setInt(String name, int value)
        {
            this.intList[this.intName.IndexOf(name)] = value;
        }

        public void setString(String name, String value)
        {
            this.StringList[this.StringName.IndexOf(name)] = value;
        }

        protected void varStringOpt(String name, char opt, String target)
        {
            switch (opt)
            {
                case '+':
                    this.StringList[this.StringName.IndexOf(name)] = this.StringList[this.StringName.IndexOf(name)].ToString() + target;
                    break;

                case '-':
                    int tar = int.Parse(target);
                    try
                    {
                        if (tar >= 0)
                        {
                            this.StringList[this.StringName.IndexOf(name)] = this.StringList[this.StringName.IndexOf(name)].ToString().Substring(0,
                                                                                                    this.StringList[this.StringName.IndexOf(name)].ToString().Length - tar);
                        }
                        else
                        {
                            tar *= -1;
                            this.StringList[this.StringName.IndexOf(name)] = this.StringList[this.StringName.IndexOf(name)].ToString().Substring(tar);
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        this.StringList[this.StringName.IndexOf(name)] = "";
                    }
                    break;

                case '*':
                    String temp = this.StringList[this.StringName.IndexOf(name)].ToString();
                    String result = "";
                    for (int index = 0; index != int.Parse(target); ++index)
                    {
                        result += temp;
                    }
                    this.StringList[this.StringName.IndexOf(name)] = result;
                    break;

                case '=':
                    this.StringList[this.StringName.IndexOf(name)] = target;
                    break;
            }
        }

        protected void varMathOpt(String name, char opt, String tar)
        {
            int target = int.Parse(tar);
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

                case "bool":
                    this.DimBool(name, value);
                    break;
            }
        }

        protected void DimBool(String name, String value)
        {
            int boolValue;
            if (value.ToUpper() == "TRUE")
            {
                boolValue = 1;
            }
            else
            {
                boolValue = 0;
            }

            this.boolName.Add(name);
            this.boolList.Add(boolValue);
        }

        public int GetBool(String name)
        {
            return int.Parse(this.boolList[this.boolName.IndexOf(name)].ToString());
        }

        protected void DimInt(String name, String value)
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

        protected void DimString(String name, String value)
        {
            this.StringName.Add(name);
            this.StringList.Add(value);
        }
    }
}
