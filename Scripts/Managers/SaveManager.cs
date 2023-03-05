using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace saveManagerScript
{
    public class SaveManager
    {
        public SaveManager()
        {
            SaveData data = new SaveData("save.bin");
            DebugStats.AddLog("Initiated SaveManager");
            //data.addString("Hello");
            //data.addInt(12);
            //data.addString("Goodbye!");
            //data.addDouble(22.35);

            //private double[] position = { 0, 0 }; //placeholder

            //data.saveFile();

            //foreach (var i in data.readFile("save.bin"))
            //{
            //    Console.WriteLine(i);
            //}

            //Console.WriteLine(data.format[0]);
        }

        public class SaveData
        {
            string filePath;
            const string tempPath = "temp.bin";
            const int templatePadding = 30;
            const char seperator = ';';
            const int encryptionOffset = 5;
            Stream stream;
            BinaryWriter binaryWriter;
            public List<int> format = new List<int>();

            public SaveData(string path)
            {
                filePath = path;
                if (!File.Exists(tempPath))
                {
                    File.Create(tempPath).Close(); // create the file if it doesn't exist
                }
                stream = File.OpenWrite(tempPath);
                binaryWriter = new BinaryWriter(stream, Encoding.UTF8, false); // open writer for whole class
            }

            public void resetToAdd()
            {
                try
                {
                    stream.Dispose();
                    binaryWriter.Dispose();
                }
                catch { }
                if (!File.Exists(tempPath))
                {
                    File.Create(tempPath).Close(); // create the file if it doesn't exist
                }
                stream = File.OpenWrite(tempPath);
                binaryWriter = new BinaryWriter(stream, Encoding.UTF8, false); // open writer for whole class
            }

            public void addString(string toAdd)
            {
                binaryWriter.Write(toAdd + seperator);
                format.Add(0);
            }
            public void addInt(int toAdd)
            {
                binaryWriter.Write(toAdd.ToString() + seperator);
                format.Add(1);
            }
            public void addDouble(double toAdd)
            {
                binaryWriter.Write(toAdd.ToString() + seperator);
                format.Add(2);
            }

            public void saveFile()
            {
                try
                {
                    //close all binarywriter functions
                    stream.Dispose();
                    binaryWriter.Dispose();

                    //pad out file
                    for (int i = format.Count; i < templatePadding; i++)
                    {
                        format.Add(9);
                    }

                    // write format to another file
                    byte[] template = Encoding.UTF8.GetBytes(String.Join("", format) + seperator);

                    if (!File.Exists(filePath)) File.Create(filePath).Close(); // create the file if it doesn't exist
                    File.WriteAllText(filePath, string.Empty);

                    stream = File.OpenWrite(filePath);
                    using (var binaryWriter = new BinaryWriter(stream, Encoding.UTF8, false)) // open writer for whole class
                    {
                        binaryWriter.Write(template);
                        using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(tempPath)))
                        {
                            byte[] buffer = new byte[1028]; //read in chunks to avoid large memory usage
                            int bytesRead;
                            while ((bytesRead = binaryReader.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                binaryWriter.Write(buffer, 0, bytesRead);
                            }
                        }
                    }

                    stream.Dispose();
                    binaryWriter.Dispose();

                    File.Delete(tempPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            public List<string> readFile(string path)
            {
                filePath = path;
                stream = File.OpenRead(filePath);
                var binaryReader = new BinaryReader(stream, Encoding.UTF8, false);

                format.Clear();

                for (int i = 0; i < templatePadding; i++)
                {
                    try
                    {
                        char currentRead = (char)binaryReader.Read();
                        if (currentRead == seperator) //seperator -- some error
                        {
                            Console.WriteLine("Save format loop is too short, check length.");
                            break;
                        }
                        format.Add(int.Parse(currentRead.ToString()));
                    }
                    catch
                    {
                        Console.WriteLine("Save format loop is too long, check length.");
                        break;
                    }
                }

                //align binaryReader to actual save data
                if (binaryReader.Read() == seperator)
                {
                    binaryReader.Read();
                }

                List<string> data = new List<string>();

                for (int i = 0; i < templatePadding; i++)
                {
                    try
                    {
                        string result = getNextReadPart(ref binaryReader).Trim();
                        if (result != String.Empty)
                        {
                            data.Add(result);
                            binaryReader.Read();
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Error reading part of the save file!");
                    }
                }

                //temporary, but would return the relevant files ?
                stream.Dispose();
                binaryWriter.Dispose();

                return data;
            }

            private string getNextReadPart(ref BinaryReader binaryReader)
            {
                bool validString = false;
                string builder = String.Empty;
                while (true)
                {
                    var currentRead = binaryReader.Read();
                    if (currentRead == -1) break; //end of file but without a seperator so invalid
                    if ((char)currentRead == seperator) //end of part (seperator) so valid
                    {
                        validString = true;
                        break;
                    }
                    builder += ((char)currentRead).ToString();
                }
                return validString ? builder : String.Empty;
            }
        }
    }
}