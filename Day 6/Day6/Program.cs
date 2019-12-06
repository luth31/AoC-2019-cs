using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day6 {
    class Program {
        static void Main(string[] args) {
            StreamReader Reader = new StreamReader(@"/Work/AoC/Day 6/day6.in");
            StreamWriter Writer = new StreamWriter(@"/Work/AoC/Day 6/day6.out");
            string[] Lines = Reader.ReadToEnd().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            List<string> Child = new List<string>();
            List<string> Parent = new List<string>();
            foreach(string Line in Lines) {
                string[] Col = Line.Split(")");
                Parent.Add(Col[0]);
                Child.Add(Col[1]);
            }
            Tree Tree = new Tree();
            Tree.InitNodes(Child);
            Tree.InitNodes(Parent);
            Tree.UpdateParents(Parent, Child);

            int Orbits = Tree.CountOrbits();
            Console.WriteLine(Orbits);
            Writer.WriteLine(Orbits);

            Tree.Node Begin = Tree.FindNodeByName("YOU")._Parent;
            Tree.Node End = Tree.FindNodeByName("SAN")._Parent;
            int Distance = Tree.MinDistanceFromTo(Begin, End);
            Console.WriteLine(Distance);
            Writer.WriteLine(Distance);

            Writer.Close();
            Reader.Close();
        }


    }

    public class Tree {
        public class Node {
            public Node(string Name, Tree ParentTree) {
                _Tree = ParentTree;
                _Name = Name;
                _Parent = null;
                Adjacent = new List<Node>();
            }
            public void AddAdjacent(Node Node) {
                Adjacent.Add(Node);
            }

            public int CountToRoot() {
                Node Travel = this;
                int Count = 0;
                while(Travel._Parent != null) {
                    Travel = Travel._Parent;
                    ++Count;
                }
                return Count;
            }

            public void SetParent(Node Parent) {
                _Parent = Parent;
            }

            public string _Name;
            public Node _Parent;
            public Tree _Tree;
            public List<Node> Adjacent;
        }
        public Tree() {
            Nodes = new List<Node>();
        }

        public Node FindNodeByName(string Name) {
            return Nodes.FirstOrDefault(e => e._Name == Name);
        }

        public Node CreateNode(string Name) {
            Node NewNode = new Node(Name, this);
            return NewNode;
        }

        // Step 1: Create all nodes and populate Nodes list
        public void InitNodes(List<string> NameArray) {
            foreach (string Name in NameArray) {
                if (FindNodeByName(Name) != null)
                    continue;
                Nodes.Add(CreateNode(Name));
            }
        }

        // Step 2: Set all nodes' parent
        public void UpdateParents(List<string> Parents, List<string> Children) {
            for(int i = 0; i < Parents.Count() && i < Children.Count(); ++i) {
                Node Parent = FindNodeByName(Parents[i]);
                Node Child = FindNodeByName(Children[i]);
                Child.SetParent(Parent);
                Parent.AddAdjacent(Child);
                Child.AddAdjacent(Parent);
            }
        }

        // Step 3: Sum the path to root element of each Node
        public int CountOrbits() {
            int Orbits = 0;
            foreach(Node N in Nodes) {
                Orbits += N.CountToRoot();
            }
            return Orbits;
        }

        // Step 4: Find min distance between two nodes using BFS
        public int MinDistanceFromTo(Node From, Node To) {
            List<Node> Visited = new List<Node>();
            Queue<Node> Queue = new Queue<Node>();
            Dictionary<Node, int> Distance = new Dictionary<Node, int>();

            Node TempNode = From; // Parent of YOU

            Queue.Enqueue(TempNode);
            Visited.Add(TempNode);
            Distance.Add(TempNode, 0);

            while (Queue.Count > 0) {
                TempNode = Queue.Dequeue();
                foreach (Node Adj in TempNode.Adjacent) {
                    if (Visited.Contains(Adj))
                        continue;
                    Distance[Adj] = Distance[TempNode] + 1;
                    Queue.Enqueue(Adj);
                    Visited.Add(Adj);
                }
            }
            return Distance[To];
        }

        List<Node> Nodes;
    }
}
