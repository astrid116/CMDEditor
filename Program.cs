using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using static System.Net.Mime.MediaTypeNames;

namespace CMDEditor
{
    internal class Program
    {
        private static readonly string configFile = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\cmdeditorconfig\\config";
        static void Main(string[] args)
        {
            string path;
            string filename;
            string configDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\cmdeditorconfig";

            if (!File.Exists(configFile))
            {
                Directory.CreateDirectory(configDir);
                DirectoryInfo _ = new (configDir)
                {
                    Attributes = FileAttributes.Hidden
                };
                File.Create(configFile);
                Console.WriteLine("Successfully initialized config file!");
            }


            while (true)
            {
                while (true)
                {
                    Console.Title = "CMDEditor | Made By Astrid";
                    Console.Write("Enter file path (hit enter to use current directory): ");
                    path = Console.ReadLine();
                    Console.Write("Enter the file name: ");
                    filename = Console.ReadLine();
                    if (path == "")
                    {
                        if (!File.Exists(Directory.GetCurrentDirectory() + "\\" + filename))
                        {
                            Console.WriteLine("The specified file doesn't exists. ");
                        }
                        else
                        {
                            path = Directory.GetCurrentDirectory() + "\\" + filename;
                            break;
                        }
                    }
                    else
                    {
                        if (!File.Exists(@path + "\\" + filename))
                        {
                            Console.WriteLine("The specified file doesn't exists. ");
                        }
                        else
                        {
                            path = @path + "\\" + filename;
                            break;
                        }
                    }
                }


                Console.Clear();
                Console.Title = "CMDEditor | Editing: " + path;
                string lineInput;
                string line;
                int lineIndex;
                while (true)
                {
                    Console.Write(">");
                    lineInput = Console.ReadLine();
                    string[] arr = lineInput.Split();
                    if (lineInput == ":q")
                    {
                        Console.Clear();
                        LastModifiedSave(path);
                        break;
                    }

                    if (lineInput == "")
                    {
                        int i = 1;
                        var sr = new StreamReader(path);
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line != "")
                            {
                                if (i < 10)
                                {
                                    Console.WriteLine(i + "    ~  " + line);
                                }
                                else if (i < 100)
                                {
                                    Console.WriteLine(i + "   ~  " + line);
                                }
                                else if (i < 1000)
                                {
                                    Console.WriteLine(i + "  ~  " + line);
                                }
                                else if (i < 10000)
                                {
                                    Console.WriteLine(i + " ~  " + line);
                                }
                            }
                            i++;
                        }
                        sr.Close();
                    }
                    else if (int.TryParse(arr[0], out lineIndex))
                    {
                        string tempFile = Path.GetTempFileName();
                        var sr = new StreamReader(path);
                        var sw = new StreamWriter(tempFile);
                        for (int i = 0; i < lineIndex - 1; i++)
                        {
                            line = sr.ReadLine();
                            if (line == null || line == "")
                            {
                                sw.WriteLine("");
                            }
                            else
                            {
                                sw.WriteLine(line);
                            }
                        }
                        try
                        {
                            lineInput = string.Join(" ", lineInput.Split(' ').Skip(1));
                            sw.WriteLine(lineInput);
                            line = sr.ReadLine();
                            while ((line = sr.ReadLine()) != null)
                            {
                                sw.WriteLine(line);
                            }
                            sr.Close();
                            sw.Close();
                            File.Delete(path);
                            File.Move(tempFile, path);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            line = sr.ReadLine();
                            sw.WriteLine("");
                            while ((line = sr.ReadLine()) != null)
                            {
                                sw.WriteLine(line);
                            }
                            sr.Close();
                            sw.Close();
                            File.Delete(path);
                            File.Move(tempFile, path);
                        }
                    }
                    else if (arr[0] == "+")
                    {
                        if (int.TryParse(arr[1], out lineIndex))
                        {
                            string tempFile = Path.GetTempFileName();
                            var sr = new StreamReader(path);
                            var sw = new StreamWriter(tempFile);
                            for (int i = 0; i < lineIndex; i++)
                            {
                                line = sr.ReadLine();
                                if (line == null || line == "")
                                {
                                    sw.WriteLine("");
                                }
                                else
                                {
                                    sw.WriteLine(line);
                                }
                            }
                            try
                            {
                                lineInput = string.Join(" ", lineInput.Split(' ').Skip(2));
                                sw.WriteLine(lineInput);
                                line = sr.ReadLine();
                                sw.WriteLine(line);
                                while ((line = sr.ReadLine()) != null)
                                {
                                    sw.WriteLine(line);
                                }
                                sr.Close();
                                sw.Close();
                                File.Delete(path);
                                File.Move(tempFile, path);
                            }
                            catch (IndexOutOfRangeException)
                            {
                                line = sr.ReadLine();
                                sw.WriteLine("");
                                sw.WriteLine(line);
                                while ((line = sr.ReadLine()) != null)
                                {
                                    sw.WriteLine(line);
                                }
                                sr.Close();
                                sw.Close();
                                File.Delete(path);
                                File.Move(tempFile, path);
                            }
                        }
                    }
                    else if (lineInput == ":clear")
                    {
                        Console.Clear();
                    }
                    else if (lineInput == ":clearfile")
                    {
                        File.WriteAllText(path, "");
                    }
                    else if (lineInput == ":removeemptylines")
                    {
                        string tempFile = Path.GetTempFileName();
                        var sr = new StreamReader(path);
                        var sw = new StreamWriter(tempFile);

                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line != "")
                            {
                                sw.WriteLine(line);
                            }
                        }
                        sr.Close();
                        sw.Close();
                        File.Delete(path);
                        File.Move(tempFile, path);
                    }
                    else if (lineInput == ":lastmodified")
                    {
                        ListLastModified(path);
                    }
                    else
                    {
                        Console.WriteLine("Invalid usage.");
                    }
                }
            }
        }

        static void LastModifiedSave(string filePath)
        {
            DateTime currentDateTime = DateTime.Now;
            string path = filePath;
            string line;
            bool writePath = true;

            string tempFile = Path.GetTempFileName();
            var sr = new StreamReader(configFile);
            var sw = new StreamWriter(tempFile);

            while ((line = sr.ReadLine()) != null)
            {
                string[] arr = line.Split();
                if (arr[0] == path)
                {
                    line = sr.ReadLine();
                    line = sr.ReadLine();
                    sw.WriteLine(path + "\nLast Modified: " + currentDateTime + "\n");
                    writePath = false;
                }
                else
                {
                    sw.WriteLine(line);
                }
            }
            if (writePath == true)
            {
                sw.WriteLine(path + "\nLast Modified: " + currentDateTime + "\n");
            }

            sr.Close();
            sw.Close();
            File.Delete(configFile);
            File.Move(tempFile, configFile);
        }

        static void ListLastModified(string filePath)
        {
            string path = filePath;
            string line;
            var sr = new StreamReader(configFile);

            while ((line = sr.ReadLine()) != null)
            {
                string[] arr = line.Split();
                if (arr[0] == path)
                {
                    for (int i = 0; i <= 2; i++)
                    {
                        Console.WriteLine(line);
                        line = sr.ReadLine();
                    }
                }
            }
            sr.Close();
        }
    }
}