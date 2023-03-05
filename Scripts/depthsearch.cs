using System;
using System.Collections;
using System.Collections.Generic;

namespace Graph
{
    public class Node
    {
        public string data;

        public List<Node> connections;

        public Node(string input)
        {
            data = input;
            connections = new List<Node>();
        }

        public void addConnection(Node node)
        {
            connections.Add(node);
        }
    }
    class Graph
    {
        Node start;
        public Graph(Node node)
        {
            start = node;
        }
        public string depthTraverse()
        {
            return depthTraverse(start, new List<Node>());
        }
        private string depthTraverse(Node node, List<Node> found)
        {
            if (found.Contains(node))
            {
                return ""; //stop checking this node (already found)
            }
            else
            {
                found.Add(node); //add node to list
            }
            if (node.connections.Count == 0)
            {
                return node.data; //stop checking this node (no branches)
            }
            string result = node.data; //get data
            foreach (var i in node.connections)
            {
                result += depthTraverse(i, found); // recursion of nodes down the tree
            }
            return result;
        }
        
        public string breadthTraverse()
        {
            return breadthTraverse(new List<Node>() { start }, new List<Node>() { start }); // put start in list on declaration
        }
        private string breadthTraverse(List<Node> queue, List<Node> found)
        {
            Node node = queue[0];
            queue.RemoveAt(0); //advance queue
            if (node.connections.Count > 0) //if node has branches, add the other (unfound) branches
            {
                foreach (var i in node.connections)
                {
                    if (!found.Contains(i))
                    {
                        queue.Add(i); //add to queue to be searched later
                        found.Add(i);
                    }
                }
            }
            if (queue.Count == 0) //if queue is empty, collect all the results (branch being searched is last node)
            {
                string result = "";
                foreach (var i in found)
                {
                    result += i.data; 
                }
                return result; //turn FOUND list into string
            }
            return breadthTraverse(queue, found); //advance queue (recursion)
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<string> searchedNodes = new List<string>();
            Queue<Node> breadth = new Queue<Node>();
            Stack<Node> depth = new Stack<Node>();

            Node a = new Node("A");
            Node b = new Node("B");
            Node c = new Node("C");
            Node d = new Node("D");
            Node e = new Node("E");
            Node f = new Node("F");
            Node g = new Node("G");
            Node h = new Node("H");

            Graph graph = new Graph(a);
            a.addConnection(b);
            a.addConnection(d);
            a.addConnection(e);

            b.addConnection(c);
            b.addConnection(d);

            c.addConnection(b);
            c.addConnection(f);
            c.addConnection(h);

            d.addConnection(a);
            d.addConnection(b);

            e.addConnection(a);
            e.addConnection(f);

            f.addConnection(c);
            f.addConnection(e);
            f.addConnection(h);

            h.addConnection(c);
            h.addConnection(f);
            h.addConnection(g);

            g.addConnection(h);

            // BREADTH FIRST SEARCH
            Console.WriteLine(graph.depthTraverse());
            Console.WriteLine(graph.breadthTraverse());
            Console.ReadLine();
        }
    }
}