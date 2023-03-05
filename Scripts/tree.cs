using System;
using System.Collections.Generic;
using System.Linq;

namespace Tree
{
    class Tree
    {
        public List<int> values = new List<int>();
        public List<int> leftPointer = new List<int>();
        public List<int> rightPointer = new List<int>();
        
        public void setRoot(int data)
        {
            values.Add(data);
        }
        public void addValue(int value)
        {
            Console.WriteLine("Adding: "+value);
            int index = 0;
            bool searching = true;
            while (searching)
            {
                if (value >= values[index])
                {
                    if (rightPointer.Count() < index)
                    {
                        Console.WriteLine(index);
                        index = rightPointer[index];
                    }
                    else
                    {
                        rightPointer.Add(values.Count());
                        values.Add(value);
                        Console.WriteLine("RIGHT NODE ADDED");
                        break;
                    }
                }
                else
                {
                    if (rightPointer.Count() < index)
                    {
                        index = leftPointer[index];
                    }
                    else
                    {
                        leftPointer.Add(values.Count());
                        values.Add(value);
                        Console.WriteLine("LEFT NODE ADDED");
                        break;
                    }
                }
                if (index >= values.Count)
                {
                    searching = false;
                    break;
                }
            }
        }
        public void OUTPUT()
        {
            values.ForEach(p => Console.Write(p));
            Console.WriteLine();
            rightPointer.ForEach(p => Console.Write(p));
            Console.WriteLine();
            leftPointer.ForEach(p => Console.Write(p));
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Tree tree = new Tree();
            tree.setRoot(5);
            tree.addValue(8);
            tree.addValue(3);
            tree.addValue(11);
            tree.OUTPUT();
        }
    }
}
